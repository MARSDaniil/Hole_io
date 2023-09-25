using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UDebug = UnityEngine.Debug;
using UnityPlayerSettings = UnityEditor.PlayerSettings;

namespace Devops
{
    public static class Builder
    {
        private enum BuildType
        {
            PRODUCTION,
            DEVELOP,
        }

        private const string OUT_EXECUTABLE_DEFAULT_FILE_NAME = "client_build";

        private const string BUILD_PRODUCTION = "BUILD_PRODUCTION";
        private const string BUILD_DEVELOP = "BUILD_DEVELOP";

        private const string BUILD_ENABLED_LOGS = "DEBUG_ENABLE_LOG";

        private const string ARG_BUILD_PRODUCTION = "-production";
        private const string ARG_BUILD_DEVELOP = "-develop";

        private const string ARG_BUILD_AAB = "-isAab";

        private const string ARG_BUILD_CONNECT_PROFILER = "-connectProfiler";

        private const string ARG_BUILD_LOGS = "-logs";

        private const string ARG_BUILD_NUMBER = "-buildNumber";
        private const string ARG_BUNDLE_VERSION = "-bundleVersion";
        private const string ARG_BUNDLE_ID = "-bundleId";
        private const string ARG_COMPANY_NAME = "-companyName";
        private const string ARG_PRODUCT_NAME = "-productName";
        private const string ARG_FACEBOOK_APP_NAME = "-facebookAppName";
        private const string ARG_FACEBOOK_APP_ID = "-facebookAppId";
        private const string ARG_APP_IOS_ID = "-applicationIosId";
        private const string BACKEND_URL = "-backendUrl";

        private const string ARG_ANDROID_KEY_ALIAS_NAME = "-androidKeyAliasName";
        private const string ARG_ANDROID_KEY_PASSWORD = "-androidKeyPassword";

        private const string ASSET_PATH_TO_ICONS = "Assets/Devops/BuildIcons/";

        private static string GetExecutableFileName()
        {
            string fullAppVersion = GetFullAppVersion();
            return string.IsNullOrEmpty(fullAppVersion) ? OUT_EXECUTABLE_DEFAULT_FILE_NAME : fullAppVersion;
        }

        private static void ClearDirectory(string value)
        {
            if (Directory.Exists(value))
            {
                Directory.Delete(value, true);
            }
        }

        private static string GetBuildNumber()
        {
            return GetArgumentValue(ARG_BUILD_NUMBER);
        }

        private static string GetBundleVersion()
        {
            return GetArgumentValue(ARG_BUNDLE_VERSION);
        }

        private static string GetCompanyName()
        {
            return GetArgumentValue(ARG_COMPANY_NAME);
        }

        private static string GetProductName()
        {
            return GetArgumentValue(ARG_PRODUCT_NAME);
        }

        private static string GetAppBundleIdentifier()
        {
            return GetArgumentValue(ARG_BUNDLE_ID);
        }

        private static string GetFacebookAppName()
        {
            return GetArgumentValue(ARG_FACEBOOK_APP_NAME);
        }

        private static string GetFacebookAppId()
        {
            return GetArgumentValue(ARG_FACEBOOK_APP_ID);
        }

        private static string GetAppIosId()
        {
            return GetArgumentValue(ARG_APP_IOS_ID);
        }

        private static string GetBackendUrl()
        {
            return GetArgumentValue(BACKEND_URL);
        }

        private static string GetFullAppVersion()
        {
            string bundleVersion = GetBundleVersion();
            if (string.IsNullOrEmpty(bundleVersion))
                return string.Empty;
            return $"{bundleVersion}.{GetBuildNumber()}";
        }

        private static bool IsDevelopBuild()
        {
            return HasArgumentsParameter(ARG_BUILD_DEVELOP);
        }

        private static bool IsProductionBuild()
        {
            return HasArgumentsParameter(ARG_BUILD_PRODUCTION);
        }

        private static bool IsAabBuild()
        {
            return HasArgumentsParameter(ARG_BUILD_AAB);
        }

        private static bool IsLogsEnabledBuild()
        {
            return HasArgumentsParameter(ARG_BUILD_LOGS);
        }

        private static bool IsConnectProfiler()
        {
            return HasArgumentsParameter(ARG_BUILD_CONNECT_PROFILER);
        }

        private static bool HasArgumentsParameter(string paramName)
        {
            string[] args = Environment.GetCommandLineArgs();
            return args.Any(t => t == paramName);
        }

        private static string GetArgumentValue(string argName)
        {
            string[] args = Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == argName)
                {
                    return args[i + 1];
                }
            }

            return string.Empty;
        }

        private static void UpdateDefines(ICollection<string> allCurrentDefines, bool enabled, string define)
        {
            if (enabled && !allCurrentDefines.Contains(define))
                allCurrentDefines.Add(define);
            else if (!enabled && allCurrentDefines.Contains(define))
                allCurrentDefines.Remove(define);
        }

        private static string ConfigureDirectory(string buildTargetName)
        {
            string buildPath = Path.Combine(Application.dataPath, "../Build");
            ClearDirectory(buildPath);
            string buildTargetPath = Path.Combine(buildPath, buildTargetName);
            ClearDirectory(buildTargetPath);
            return buildTargetPath;
        }

        private static void ConfigureDefines(BuildTargetGroup buildTargetGroup)
        {
            bool isProductionBuild = IsProductionBuild();
            bool isDevelopBuild = IsDevelopBuild();
            bool isLogEnabled = IsLogsEnabledBuild();
            UDebug.Log(
                $"Define settings: isProductionBuild: {isProductionBuild}, isDevelopBuild: {isDevelopBuild}, isLogEnabled: {isLogEnabled}");
            string definesString = UnityPlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            UDebug.Log($"Defines from editor: '{definesString}'");
#if ENABLE_CONSOLE
      if (isLogEnabled)
      {
        LunarConsoleEditorInternal.Installer.EnablePlugin();
        Debug.Log("Lunar console is enabled");
      }
      else
      {
        LunarConsoleEditorInternal.Installer.DisablePlugin();
        Debug.Log("Lunar console is disabled");
      }
#endif
            List<string> allCurrentDefines = definesString.Split(';').ToList();
            UpdateDefines(allCurrentDefines, isProductionBuild, BUILD_PRODUCTION);
            UpdateDefines(allCurrentDefines, isDevelopBuild, BUILD_DEVELOP);
            UpdateDefines(allCurrentDefines, isLogEnabled, BUILD_ENABLED_LOGS);
            string resultDefines = string.Join(";", allCurrentDefines.ToArray());
            UDebug.Log($"Result defines to set: '{resultDefines}'");
            UnityPlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, resultDefines);
        }

        private static void BuildExecutor(BuildTarget buildTarget, string buildTargetName)
        {
            UDebug.Log($"Command line args: {string.Join(";", Environment.GetCommandLineArgs())}");
            BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
            string buildTargetPath = ConfigureDirectory(buildTargetName);
            UDebug.Log($"Build directory: {buildTargetPath}");

            ConfigureDefines(buildTargetGroup);

            if (Application.HasProLicense())
            {
                UDebug.Log("UNITY is pro/plus");
                UnityPlayerSettings.SplashScreen.showUnityLogo = false;
            }
            else
            {
                UDebug.Log("Unity is not pro/plus");
            }

            UnityPlayerSettings.companyName = GetCompanyName();
            UnityPlayerSettings.productName = GetProductName();
            UDebug.Log(
                $"Build: PlayerSettings.companyName = {UnityPlayerSettings.companyName}, PlayerSettings.productName = {UnityPlayerSettings.productName}");

            UnityPlayerSettings.SplashScreen.show = true;
            UnityPlayerSettings.SetScriptingBackend(buildTargetGroup, ScriptingImplementation.IL2CPP);
            BuildType? buildType = null;
            BuildOptions buildOptions = BuildOptions.None;

            if (IsProductionBuild())
            {
                buildType = BuildType.PRODUCTION;
                buildOptions = BuildOptions.None;
            }

            if (IsDevelopBuild())
            {
                buildType = BuildType.DEVELOP;
                if (IsLogsEnabledBuild())
                {
                    buildOptions = BuildOptions.Development | BuildOptions.AllowDebugging;

                    if (IsConnectProfiler())
                    {
                        buildOptions |= BuildOptions.ConnectWithProfiler;
                    }
                }
            }

            if (buildType == null)
            {
                throw new Exception("Error, try to build not accepted build type");
            }

            ChangeIcon(buildTargetGroup, buildType.Value);

            // just get all scenes already added in build
            string[] scenes = EditorBuildSettings.scenes.Where(_ => _.path != string.Empty).Select(_ => _.path)
                .ToArray();
            UDebug.Log($"Scenes count: {scenes.Length}. scene names: {string.Join(",", scenes)} ");
            if (scenes.Length == 0)
            {
                scenes = null;
            }

            // https://forum.unity.com/threads/custom-build-script-with-enabled-profiling.523120/
            AssetDatabase.Refresh();

            int result = 0;
            BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = buildTargetPath,
                target = buildTarget,
                targetGroup = buildTargetGroup,
                options = buildOptions,
            });
            UDebug.Log($"Build result is: {report.summary.result}");
            result = report.summary.result == BuildResult.Succeeded ? 0 : 1;
            if (result == 0)
            {
                PrintReport(report);
            }

            EditorApplication.Exit(result);
        }

        [MenuItem("AutoBuild/Build iOS")]
        private static void BuildIos()
        {
            try
            {
                string buildNumber = GetBuildNumber();
                string bundleVersion = GetFullAppVersion();
                UnityPlayerSettings.iOS.buildNumber = buildNumber;
                if (!string.IsNullOrEmpty(bundleVersion))
                    UnityPlayerSettings.bundleVersion = bundleVersion;
                UDebug.Log(
                    $"Build iOS: PlayerSettings.iOS.buildNumber = {buildNumber}, PlayerSettings.bundleVersion = {bundleVersion}");
                string bundleId = GetAppBundleIdentifier();
                if (!string.IsNullOrEmpty(bundleId))
                {
                    UnityPlayerSettings.applicationIdentifier = bundleId;
                }

                UDebug.Log($"Build iOS: PlayerSettings.applicationIdentifier = {bundleId}");
                BuildTarget buildTarget = BuildTarget.iOS;

                UnityPlayerSettings.iOS.appleEnableAutomaticSigning = false;

                BuildExecutor(buildTarget, buildTarget.ToString());
            }
            catch (Exception e)
            {
                UDebug.LogError(e);
                EditorApplication.Exit(1);
            }
        }

        [MenuItem("AutoBuild/Build Android")]
        private static void BuildAndroid()
        {
            try
            {
                string buildNumber = GetBuildNumber();
                string bundleVersion = GetFullAppVersion();
                string executableFileName = GetExecutableFileName();
                int.TryParse(buildNumber, out int bundleVersionCode);
                UnityPlayerSettings.Android.bundleVersionCode = bundleVersionCode;
                if (!string.IsNullOrEmpty(bundleVersion))
                    UnityPlayerSettings.bundleVersion = bundleVersion;
                UnityPlayerSettings.Android.useCustomKeystore = true;
                // В новых Unity - баг, что путь к keyStore берется не от корня проекта, а от папки Temp/gradleOut
                UnityPlayerSettings.Android.keystoreName = $"{Application.dataPath}/../user.keystore";
                UnityPlayerSettings.Android.keystorePass = GetArgumentValue(ARG_ANDROID_KEY_PASSWORD);
                UnityPlayerSettings.Android.keyaliasName = GetArgumentValue(ARG_ANDROID_KEY_ALIAS_NAME);
                UnityPlayerSettings.Android.keyaliasPass = GetArgumentValue(ARG_ANDROID_KEY_PASSWORD);

#if UNITY_2021_2_OR_NEWER
                UnityPlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel22;
#else
                UnityPlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel21;
#endif


                UnityPlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
                UnityPlayerSettings.Android.targetArchitectures = AndroidArchitecture.All;
                UDebug.Log(
                    $"Build android: Use Custom Keystore = {UnityPlayerSettings.Android.useCustomKeystore}, keystore Name = {UnityPlayerSettings.Android.keystoreName}, keystore Pass = {UnityPlayerSettings.Android.keystorePass}, " +
                    $"key alias Name = {UnityPlayerSettings.Android.keyaliasName}, key alias Pass = {UnityPlayerSettings.Android.keyaliasPass}");
                if (IsProductionBuild() || IsAabBuild())
                {
                    EditorUserBuildSettings.buildAppBundle = true;
                    EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
                    UDebug.Log($"Build android: aab build is set to true");
                }

                UDebug.Log(
                    $"Build android: PlayerSettings.Android.bundleVersionCode = {bundleVersionCode}, application path = {Application.dataPath}," +
                    $"executable file name = {executableFileName}");
                string buildTargetName =
                    (IsProductionBuild() || IsAabBuild()) ? $"{executableFileName}.aab" : $"{executableFileName}.apk";
                BuildExecutor(BuildTarget.Android, buildTargetName);
            }
            catch (Exception e)
            {
                UDebug.LogError(e);
                EditorApplication.Exit(1);
            }
        }

        [MenuItem("AutoBuild/Build Win64")]
        private static void BuildWin64()
        {
            try
            {
#if UNITY_ADDRESSABLES
                BuildAddressables();
#endif
                BuildExecutor(BuildTarget.StandaloneWindows64, $"{GetExecutableFileName()}.exe");
            }
            catch (Exception e)
            {
                UDebug.LogError(e);
                EditorApplication.Exit(1);
            }
        }

        private static void PrintReport(BuildReport report)
        {
            string files = string.Join(";", report.files.Select(_ => _.ToString()).ToArray());
            UDebug.Log($"Build result files: {files}");
            string steps = string.Join(";", report.steps.Select(_ => _.ToString()).ToArray());
            UDebug.Log($"Build result steps: {steps}");
            UDebug.Log($"Build result summary: {JsonUtility.ToJson(report.summary)}");
            if (report.strippingInfo != null)
            {
                List<string> reasons = new List<string>();
                foreach (string moduleName in report.strippingInfo.includedModules)
                {
                    reasons.Add(string.Join(",", report.strippingInfo.GetReasonsForIncluding(moduleName)));
                }

                string modules = string.Join(";", reasons);
                UDebug.Log($"Build result modules: {modules}");
            }
        }

        private static void ChangeIcon(BuildTargetGroup buildTargetGroup, BuildType buildType)
        {
            Texture2D appIcon = LoadIcon(buildType, buildTargetGroup);
            if (appIcon == null)
                return;
            int[] iconSizeArray = UnityPlayerSettings.GetIconSizesForTargetGroup(buildTargetGroup);
            Texture2D[] iconArray = new Texture2D[iconSizeArray.Length];
            for (int i = 0; i < iconArray.Length; i++)
            {
                iconArray[i] = appIcon;
            }

            UnityPlayerSettings.SetIconsForTargetGroup(buildTargetGroup, iconArray);
            if (buildTargetGroup == BuildTargetGroup.Android)
            {
                void SetIconsForGroup(PlatformIconKind kind, params Texture2D[] textures)
                {
                    PlatformIcon[] icons = UnityPlayerSettings.GetPlatformIcons(buildTargetGroup, kind);
                    foreach (PlatformIcon icon in icons)
                    {
                        icon.SetTextures(textures);
                    }

                    UnityPlayerSettings.SetPlatformIcons(buildTargetGroup, kind, icons);
                }
#if UNITY_ANDROID
        SetIconsForGroup(UnityEditor.Android.AndroidPlatformIconKind.Round, appIcon);
        SetIconsForGroup(UnityEditor.Android.AndroidPlatformIconKind.Adaptive, appIcon, appIcon);
#endif
            }
        }

        private static Texture2D LoadIcon(BuildType buildType, BuildTargetGroup buildTargetGroup)
        {
            string iconName = $"app_icon_{buildTargetGroup.ToString().ToLower()}_{buildType.ToString().ToLower()}.png";
            string fullPath = $"{ASSET_PATH_TO_ICONS}{iconName}";
            Texture2D spriteIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(fullPath);
            if (spriteIcon == null)
                UDebug.LogError($"spriteIcon == null at path: {fullPath}");
            return spriteIcon;
        }
    }
}