using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MainGame.UI;
public class MenuManager :MonoBehaviour {


	[SerializeField] StartMenu startMenuUI;
	[SerializeField] GamePlay gamePlayUI;
	[SerializeField] GameOver gameOverUI;
	[SerializeField] IncreaseGameTime increaseGameTime;
	public void Awake() {
		GameManager.gm.ui = this;
		startMenuUI.Init(true);
		gamePlayUI.Init(false);
		gameOverUI.Init(false);
		increaseGameTime.Init(false);
	}

	public void StartGame() {
		startMenuUI.Close();
		gamePlayUI.Open();
		gameOverUI.Close();
		increaseGameTime.Close();
		GameManager.gm.Play();
	}

	public void IncreaseScore() {
		startMenuUI.Close();
		gamePlayUI.Close();
		gameOverUI.Close();
		increaseGameTime.Open();
	}


	public void SetTime(float minutes, float seconds) {
		gamePlayUI.SetTime(minutes, seconds);
	}

	public void RestartGame() {
		GameManager.gm.started = false;
		GameManager.gm.gameOver = false;
		GameManager.gm.RestartGame();
	}
	
	public void GameOver(int score, int record) {
		startMenuUI.Close();
		gamePlayUI.Close();
		gameOverUI.Open();
		gameOverUI.SetScore(score, record);
		increaseGameTime.Close();
	}
	public void GameOver() {
		startMenuUI.Close();
		gamePlayUI.Close();
		gameOverUI.Open();
		increaseGameTime.Close();
	}

	public void SetScore(int value) => gamePlayUI.SetScore(value);
	public void InscreaseTime(int value) => GameManager.gm.InscreaseTime(value);
	public void GoToMenu(int value = 0) => GameManager.gm.RestartGame(value);
}