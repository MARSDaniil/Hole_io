using System;
using Unity.Advertisement.IosSupport;
using UnityEngine.iOS;

public static class IosRequest {
    public static void MakeAdTrackRequest(Action<bool> callback) {
        if (!IsiOSVersionAbove(14, 5)) {
            return;
        }

        ATTrackingStatusBinding.RequestAuthorizationTracking();
    }

    public static bool IsiOSVersionAbove(int majorVersion, int minorVersion) {
        //DDebug.Log($"Device.systemVersion: {Device.systemVersion}");
        string[] iOSVersionComponents = Device.systemVersion.Split('.');
        if (iOSVersionComponents.Length >= 2) {
            if (int.TryParse(iOSVersionComponents[0], out int iOSMajorVersion) && int.TryParse(iOSVersionComponents[1], out int iOSMinorVersion)) {
                if (iOSMajorVersion > majorVersion) {
                    return true;
                }
                else if (iOSMajorVersion == majorVersion && iOSMinorVersion >= minorVersion) {
                    return true;
                }
            }
        }

        return false;
    }
}