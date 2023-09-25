#if UNITY_IOS
using UnityEditor.Callbacks;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using System.IO;

namespace NakusiBuilder
{
  public static class PostProcessXCodeComplianceFix
  {
    [PostProcessBuild(4)]
    public static void ChangeXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
    {
      // UpdateInfoPlist(pathToBuiltProject);
      UpdateDependecies(pathToBuiltProject);
      ModifyFrameworks(pathToBuiltProject);
    }

    private static void UpdateInfoPlist(string pathToBuiltProject)
    {
      // Get plist
      string plistPath = pathToBuiltProject + "/Info.plist";
      PlistDocument plist = new PlistDocument();
      plist.ReadFromString(File.ReadAllText(plistPath));
      // Get root
      PlistElementDict rootDict = plist.root;
      string buildKey2 = "ITSAppUsesNonExemptEncryption";
      rootDict.SetString(buildKey2, "false");

      rootDict.SetString("NSLocationAlwaysUsageDescription", "Uses background location");
      rootDict.SetString("NSLocationWhenInUseUsageDescription", "Do you allow the app to use your location?");
      rootDict.SetString("NSPhotoLibraryUsageDescription", "Taking selfies");
      rootDict.SetString("NSCameraUsageDescription", "Taking selfies");
      rootDict.SetString("NSMotionUsageDescription ", "Interactive ad controls");
 
      // Write to file
      File.WriteAllText(plistPath, plist.WriteToString());
    }

    private static void UpdateDependecies(string pathToBuiltProject)
    {
      string projectPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
      PBXProject project = new PBXProject();
      project.ReadFromString(File.ReadAllText(projectPath));
      string targetGuid = project.GetUnityFrameworkTargetGuid();
      project.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-ObjC");
      project.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "$(inherited)");
      project.WriteToFile(projectPath);
    }
  
    private static void ModifyFrameworks(string pathToBuiltProject)
    {
      string projPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
      PBXProject project = new PBXProject();
      project.ReadFromFile(projPath);
 
      string mainTargetGuid = project.GetUnityMainTargetGuid();
      foreach (string targetGuid in new[] { mainTargetGuid, project.GetUnityFrameworkTargetGuid() })
      {
        project.SetBuildProperty(targetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");
      }
      project.SetBuildProperty(mainTargetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");
      project.WriteToFile(projPath);
    }
  }
}

#endif