using UnityEngine;
using UnityEngine.UI;
using UI.Common;
using TMPro;
namespace MainGame.UI {
    public class GamePlay :MenuWindow {
        [SerializeField] MenuManager menuManager;
        [Space]
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI timerText;
        public override void Init(bool isOpen = false) {
            base.Init(isOpen);
        }
        public void SetTime(float minutes, float seconds) {
            timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }
        public void SetScore(int score) => scoreText.text = score.ToString();
    }
}