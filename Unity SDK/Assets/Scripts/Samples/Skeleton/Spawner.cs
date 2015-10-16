using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Timers;

public class Spawner : MonoBehaviour 
{
	public float MinX;
	public float MaxX;
	
	public int Delay = 2000;
	
	Timer timer = new Timer ();
	Timer gameTimer = new Timer (1000);

	Timer restartTimer = new Timer (2000);
	bool gameRestart = false;

	bool isGameobjectCreated = false;
	bool gameOver = false;
	[HideInInspector]
	public int Score= 0;
	int Time = 30;
	
	public Text ScoreText;
	public Text TimeText;
	public Text StatusText;
	public GameObject Ball;
	public GameObject GameOverPanel;
	// Use this for initialization
	void Start () 
	{
		Initialize ();
	}

	public void Initialize()
	{
		isGameobjectCreated = false;
		
		Score = 0;
		Time = 30;
		
		timer.Interval = Delay;
		
		timer.Elapsed += HandleElapsed;
		timer.Start ();
		
		gameTimer.Elapsed += HandleElapsed1;
		gameTimer.Start ();

		MMClient.ConnectionClosed += HandleConnectionClosed;
	}

	void HandleConnectionClosed (object sender, System.EventArgs e)
	{
	}

	void HandleElapsed (object sender, ElapsedEventArgs e)
	{
		isGameobjectCreated = false;
	}

	void HandleElapsed1 (object sender, ElapsedEventArgs e)
	{
		Time--;
		if (Time == 0) 
		{
			GameOverStep1();
		}
	}

	void HandleElapsed2 (object sender, ElapsedEventArgs e)
	{
		restartTimer.Stop ();
		gameRestart = true;
	}

	void GameOverStep1()
	{
		gameTimer.Stop ();
		timer.Stop();
		timer.Elapsed -= HandleElapsed;
		gameTimer.Elapsed -= HandleElapsed1;
		MMClient.ConnectionClosed -= HandleConnectionClosed;
		restartTimer.Elapsed += HandleElapsed2;
		restartTimer.Start ();
		gameOver = true;
	}

	void GameOverStep2()
	{
		ToggleGameobjectVisibility (GameOverPanel);
		gameOver = false;
	}

	// Update is called once per frame
	void Update () 
	{
		ScoreText.text = "Score : " + Score.ToString ();
		TimeText.text = "Remaining Time : " + Time.ToString () + " seconds";
		StatusText.text = MMData.Status;

		if (gameOver) 
		{
			GameOverStep2();
		}
		if (gameRestart) 
		{
			ToggleGameobjectVisibility(GameOverPanel);
			Initialize();
			restartTimer.Elapsed -= HandleElapsed2;
			gameRestart = false;
		}
	}

	void FixedUpdate () 
	{
		if (!isGameobjectCreated) 
		{
			GameObject.Instantiate (Ball, new Vector3 (Random.Range (MinX, MaxX), Ball.transform.position.y, Ball.transform.position.z), Quaternion.identity);
			isGameobjectCreated = true;
		}
	}
	
	public void ToggleGameobjectVisibility(GameObject Go)
	{
		Go.SetActive(!Go.activeSelf);
	}
}
