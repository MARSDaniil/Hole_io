using UnityEngine;
using UnityEngine.UI;
using UI.Common;
using TMPro;
namespace MainGame.UI {
    public class GameOver :MenuWindow {
        [SerializeField] MenuManager menuManager;
        [Space]
        public TextMeshProUGUI finalScoreText;
        public TextMeshProUGUI finalBestText;

        [SerializeField] Button restartButton;
        [SerializeField] Button increaseButton;
        [SerializeField] Button goToMenuButton;
        public override void Init(bool isOpen = false) {
            base.Init(isOpen);
            restartButton.onClick.AddListener(RestartGame);
            increaseButton.onClick.AddListener(IncreaseScore);
            goToMenuButton.onClick.AddListener(GoToMenu);
        }

        public void SetScore(int score, int best) {
            finalScoreText.text = score.ToString();
            finalBestText.text = best.ToString();
            
            if (score > best) {
                PlayerPrefs.SetInt("best", score);
                finalBestText.gameObject.SetActive(true);
            }
        }
        public void RestartGame() => menuManager.RestartGame();
        public void IncreaseScore() {
            increaseButton.gameObject.SetActive(false);
            menuManager.IncreaseScore();
        }
        public void GoToMenu() => menuManager.GoToMenu(-1);
    }
}