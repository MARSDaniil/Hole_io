using UnityEditor;

namespace Editor
{
	namespace Custom
	{
		internal static class OnProjectOpen
		{
			[InitializeOnLoadMethod]
			private static void InitializeOnLoadMethod()
			{
#if UNITY_EDITOR
				string[] symbols =
				{
					"DEBUG_ENABLE_LOG"
				};
				PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, symbols);
				PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, symbols);
				PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, symbols);

				// Debug.Log("scripting symbols was set");
#endif
			}
		}
	}
}