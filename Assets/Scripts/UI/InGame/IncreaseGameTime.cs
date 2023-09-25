using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UI.Common;
using TMPro;
namespace MainGame.UI {
    public class IncreaseGameTime :MenuWindow {
        [SerializeField] MenuManager menuManager;
        [Space]
        [SerializeField] Button startRotateButton;
        private bool canTurn = true;
        private float speed;
        private int numberOfTurn;
        private int whatWeWin;
        [SerializeField] GameObject wheel;


        public void Awake() {
            Init(true);
        }
        public override void Init(bool isOpen = false) {
            base.Init(isOpen);
            startRotateButton.onClick.AddListener(StartRotate);
            canTurn = true;
        }

        private void StartRotate() {
            if (canTurn) StartCoroutine(RotateWheel());

        }

        private IEnumerator RotateWheel() {
            canTurn = false;

            numberOfTurn = Random.Range(40, 60);
            speed = 0.1f;
            for (int i = 0; i < numberOfTurn; i++) {
                wheel.transform.Rotate(0, 0, 22.5f);

                if (i > Mathf.RoundToInt(numberOfTurn * 0.5f)) {
                    speed = 0.02f;
                }
                if (i > Mathf.RoundToInt(numberOfTurn * 0.7f)) {
                    speed = 0.03f;
                }
                if (i > Mathf.RoundToInt(numberOfTurn * 0.9f)) {
                    speed = 0.04f;
                }
                yield return new WaitForSeconds(speed);
            }
            if(Mathf.RoundToInt(wheel.transform.eulerAngles.z)%45 != 0) {
                wheel.transform.Rotate(0, 0, 22.5f);
            }
            whatWeWin = Mathf.RoundToInt(wheel.transform.eulerAngles.z);
            switch (whatWeWin) {
                case 0:
                StartCoroutine(WaitTimeInscrease(5));
                break;
                case 45:
                StartCoroutine(FinalGameOver(0));
                break;
                case 90:
                StartCoroutine(WaitTimeInscrease(10));
                break;
                case 135:
                StartCoroutine(WaitTimeInscrease(15));
                break;
                case 180:
                StartCoroutine(FinalGameOver(0));
                break;
                case 225:
                StartCoroutine(WaitTimeInscrease(5));
                break;
                case 270:
                StartCoroutine(WaitTimeInscrease(15));
                break;
                case 315:
                StartCoroutine(FinalGameOver(0));
                break;
            }
            

           
        }

        

        private IEnumerator WaitTimeInscrease(int timePlus) {
            yield return new WaitForSeconds(2f);
            menuManager.InscreaseTime(timePlus);
        }
        private IEnumerator FinalGameOver(int timePlus) {
            yield return new WaitForSeconds(2f);
            menuManager.GameOver();
        }

    }
}