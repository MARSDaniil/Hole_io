using UnityEngine;

public class SendRequest :MonoBehaviour {
    private void Start() {
        IosRequest.MakeAdTrackRequest(null);
    }
}