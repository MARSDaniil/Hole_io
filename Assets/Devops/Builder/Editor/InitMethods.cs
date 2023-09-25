using System;
using UnityEditor;
using UnityEngine;

namespace Custom
{
  internal static class InitMethods
  {
    [InitializeOnLoadMethod]
    private static void SetJavaHome()
    {
#if UNITY_EDITOR
      //Debug.Log(EditorApplication.applicationPath);
      string[] symbols =
      {
        "DEBUG_ENABLE_LOG"
      };
      PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, symbols);
      PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, symbols);

      Debug.Log($"JAVA_HOME in editor was: {Environment.GetEnvironmentVariable("JAVA_HOME")}");

      string newJdkPath =
        EditorApplication.applicationPath.Replace("Unity.app", "PlaybackEngines/AndroidPlayer/OpenJDK");

      if (Environment.GetEnvironmentVariable("JAVA_HOME") != newJdkPath)
      {
        Environment.SetEnvironmentVariable("JAVA_HOME", newJdkPath);
      }

      Debug.Log($"JAVA_HOME in editor set to: {Environment.GetEnvironmentVariable("JAVA_HOME")}");
#endif
    }
  }
}