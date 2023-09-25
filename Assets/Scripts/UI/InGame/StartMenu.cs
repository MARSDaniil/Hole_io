using UnityEngine;
using UnityEngine.UI;
using UI.Common;
using TMPro;
namespace MainGame.UI {
    public class StartMenu :MenuWindow {
        [SerializeField] MenuManager menuManager;
        [Space]
        [SerializeField] Button startGame;
        private TextMeshProUGUI bestText;

        public override void Init(bool isOpen = false) {
            base.Init(isOpen);
            startGame.onClick.AddListener(StartGame);
          //  SetBest();
        }
        private void StartGame() {
            menuManager.StartGame();
        }

        private void SetBest() {
            var best = PlayerPrefs.GetInt("best",0);
            if (best > 0) {
                bestText.text = "BEST " + best.ToString();
            }
            else {
                bestText.gameObject.SetActive(false);
            }
        }
    }
}