#if UNITY_IOS
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace NakusiBuilder
{
    public static class PostProcess
    {
        [PostProcessBuild(1)]
        public static void ChangeBuildManifest(BuildTarget buildTarget, string pathToBuiltProject)
        {
            // paths
            string xCodeProjFolderPath = $"{pathToBuiltProject}/Unity-iPhone.xcodeproj";
            string xcSettingsPath = $"{xCodeProjFolderPath}/project.xcworkspace/xcshareddata/WorkspaceSettings.xcsettings";
            Debug.Log($"xCodeProjFolderPath: {xCodeProjFolderPath}");
            Debug.Log($"xcSettingsPath: {xcSettingsPath}");
            // change the xcode project to use the new build system, without doing this can not compile and get an error in xcode, plus the legacy build system is now deprecated
            PlistDocument xcSettingsDoc = new PlistDocument();
            xcSettingsDoc.ReadFromString(File.ReadAllText(xcSettingsPath));
            PlistElementDict xcSettingsDict = xcSettingsDoc.root;
            IDictionary<string, PlistElement> xcSettingsValues = xcSettingsDict.values;
            string buildSystemTypeKey = "BuildSystemType";
            if (xcSettingsValues.ContainsKey(buildSystemTypeKey))
            {
                xcSettingsValues.Remove(buildSystemTypeKey); // the removal of this key/value pair <key>BuildSystemType</key><string>Original</string> allows xcode to use the default new build system setting
            }
            File.WriteAllText(xcSettingsPath, xcSettingsDoc.WriteToString());
        }
    }
}

#endif