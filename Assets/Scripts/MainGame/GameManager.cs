using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager gm;
	public PlayerControler pc;
	public CameraController cam;
	public MenuManager ui;

	public bool started;
	public bool gameOver;

	public float gravity = 10f;
	public float time = 120; // Total game time in seconds

	private void Awake()
	{
		if (!gm)
		{
			gm = this;
			//DontDestroyOnLoad(this);
		}
	}

	public void Play()
	{
		started = true;
	}

	public void GameOver()
	{
		gameOver = true;
		ui.GameOver(pc.score, PlayerPrefs.GetInt("best"));
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.R))
		{
			RestartScene();
		}

		if (!started || gameOver)
		{
			return;
		}

		time -= Time.deltaTime;

		if (time <= 1)
		{
			GameOver();
			ui.SetTime(00, 00);
			return;
		}

		var minutes = Mathf.FloorToInt(time / 60f);
		var seconds = Mathf.FloorToInt(time - minutes * 60f);
		ui.SetTime(minutes, seconds);
		//ui.timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
	}

	public void InscreaseTime(int timePlus) {
		gameOver = false;
		started = true;
		time = timePlus;
		ui.StartGame();
	}
	public void RestartScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void RestartGame(int level = 0) => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + level);
}