using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using Random = UnityEngine.Random;

public static class PostBuild
{
    [PostProcessBuild(2)]
    public static void ChangeXcodePlist(BuildTarget buildTarget, string path)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            // Get plist
            string plistPath = Path.Combine(path, "Info.plist");
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            // Get root
            PlistElementDict rootDict = plist.root;

            // set properies
            rootDict.SetBoolean("ITSAppUsesNonExemptEncryption", false);

            // photo
            rootDict.SetString("NSCameraUsageDescription", "For selfies");
            rootDict.SetString("NSPhotoLibraryUsageDescription", "For selfies");

            // location
            string[] locOptions =
            {
                "Allow access to your location while you use the app",
                "We use your location to provide customized content and services",
                "Your location is needed to provide better advertisment",
                "We do not need your location",
                "We use your location to improve the overall user experience of the app"
            };
            rootDict.SetString("NSLocationWhenInUseUsageDescription", locOptions[Random.Range(0, locOptions.Length)]);
            rootDict.SetString("NSLocationAlwaysUsageDescription", locOptions[Random.Range(0, locOptions.Length)]);

            // ATT 
            string[] trackOptions =
            {
                "to target advertising and marketing efforts",
                "to provide a better and personalized ad experience",
                "to measure product performance",
                "to analyze and report usage metrics and trends",
                "to track user behavior and usage patterns"
            };
            string trackOption = trackOptions[Random.Range(0, trackOptions.Length)];
            rootDict.SetString("NSUserTrackingUsageDescription", $"Your data will be used {trackOption}");
            Debug.Log($"NSUserTrackingUsageDescription : {trackOption}");

            // advert
            rootDict.SetString("NSAdvertisingAttributionReportEndpoint", "https://appsflyer-skadnetwork.com/");

            // motion
            rootDict.SetString("NSMotionUsageDescription ", "Interactive ad controls");
/*
      // configure SKAdNetworkItems
      const string skadname = "SKAdNetworkItems";
      PlistElementArray skAdNetworkItems = null;
      if (rootDict.values.ContainsKey(skadname))
        try
        {
          skAdNetworkItems = rootDict.values[skadname] as PlistElementArray;
        }
        catch (Exception e)
        {
          Debug.LogWarning($"Could not obtain SKAdNetworkItems PlistElementArray: {e.Message}");
        }

      skAdNetworkItems ??= rootDict.CreateArray(skadname);
      PlistElementDict dict1 = skAdNetworkItems.AddDict();
      dict1["SKAdNetworkIdentifier"] = new PlistElementString("n38lu8286q.skadnetwork");
      PlistElementDict dict2 = skAdNetworkItems.AddDict();
      dict2["SKAdNetworkIdentifier"] = new PlistElementString("v9wttpbfk9.skadnetwork");
*/

            // Write to file
            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }


    [PostProcessBuild(999)]
    public static void ChangeXcodeFrameWork(BuildTarget target, string path)
    {
        if (target == BuildTarget.iOS)
        {
            string projectPath = PBXProject.GetPBXProjectPath(path);
            PBXProject project = new PBXProject();
            project.ReadFromString(File.ReadAllText(projectPath));
            string mainTargetGuid = project.GetUnityMainTargetGuid();
            string frameworkTargetGuid = project.GetUnityFrameworkTargetGuid();

            project.SetBuildProperty(mainTargetGuid, "ENABLE_BITCODE", "NO");
            project.SetBuildProperty(frameworkTargetGuid, "ENABLE_BITCODE", "NO");

            // configure frameworks
            project.AddFrameworkToProject(frameworkTargetGuid, "UserNotifications.framework", false);

            // NOTE: ProjectCapabilityManager's 4th constructor param requires Unity 2019.3+
            string entitlementsFile = "project.entitlements";
            ProjectCapabilityManager projCapability = new ProjectCapabilityManager(projectPath,
                entitlementsFile, null, mainTargetGuid);
            projCapability.AddPushNotifications(true);


            project.AddFile(entitlementsFile, entitlementsFile);
            project.AddBuildProperty(mainTargetGuid, "CODE_SIGN_ENTITLEMENTS", entitlementsFile);

            projCapability.WriteToFile();

            string plistPath = $"{path}/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
            PlistElementDict rootDict = plist.root;
            PlistElementArray arr = rootDict.CreateArray("UIBackgroundModes");
            arr.AddString("remote-notification");
            arr.AddString("fetch");
            File.WriteAllText(plistPath, plist.WriteToString());

            File.WriteAllText(projectPath, project.WriteToString());

            UnityEngine.Debug.Log($"ChangeXcodeFrameWork, path: {path}, project path: {projectPath}");
        }
    }
}