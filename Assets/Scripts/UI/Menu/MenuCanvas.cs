using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuCanvas :MonoBehaviour {
    [SerializeField] Button startGameButton;
    [SerializeField] Button privacyPoliceButton;
    [SerializeField] string linkToWeb;
    private void Start() {
        startGameButton.onClick.AddListener(StartGame);
        privacyPoliceButton.onClick.AddListener(OpenLink);
    }

    private void StartGame() {
        Debug.Log("u click on start game");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private void OpenLink() {
        Application.OpenURL(linkToWeb);
    }
}
