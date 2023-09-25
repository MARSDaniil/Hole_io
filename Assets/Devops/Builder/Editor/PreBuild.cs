using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class PreBuild : IPreprocessBuildWithReport
{
	public int callbackOrder
		=> 1;
    
	public void OnPreprocessBuild(BuildReport report)
	{
		PlayerSettings.SplashScreen.showUnityLogo = false;
		PlayerSettings.Android.splashScreenScale = AndroidSplashScreenScale.ScaleToFill;
		PlayerSettings.SplashScreen.animationMode = PlayerSettings.SplashScreen.AnimationMode.Static;
		PlayerSettings.Android.renderOutsideSafeArea = false;
		PlayerSettings.allowedAutorotateToPortrait = true;
		PlayerSettings.allowedAutorotateToLandscapeRight = true;
		PlayerSettings.allowedAutorotateToLandscapeLeft = true;
		PlayerSettings.allowedAutorotateToPortraitUpsideDown = true;
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, Array.Empty<string>());
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, Array.Empty<string>());
	}
}