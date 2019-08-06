using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public Vector2 targetPos;
	public float Yincrement;

	public float speed;
	public float maxHeight;
	public float minHeight;
	public int health=3;
	public Sprite[]plane;

	public Text txtScore;
	public Text txtLevel;
	public Text txtLives;
	int currentScore=0;
	public int scoreBtwUpdate=52;
	public GameObject gameOver;
	public GameObject backgroundMusic;
	public Text txtFinalScore;


	void Start()
	{
		//StartCoroutine (FylPlane ());
	}

	public void TakeDamage(int damage)
	{
		if (health > 0) {
			GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioManager>().PlayAudioClip(0,this.transform);
			health -= damage;
		}

	}

	void Update()
	{
		if (health <= 0) {
			StartCoroutine (GameOver ());
		}

		transform.position=Vector2.MoveTowards(transform.position,targetPos,speed * Time.deltaTime);

		if(Input.GetKeyDown(KeyCode.UpArrow) && transform.position.y<maxHeight)
		{
			targetPos= new Vector2(transform.position.x,transform.position.y + Yincrement);
		}
		else if(Input.GetKeyDown(KeyCode.DownArrow) && transform.position.y>minHeight)
		{
			targetPos=new Vector2(transform.position.x,transform.position.y- Yincrement);
		}

		txtLives.text = "LIVES : " +health.ToString ();
		if (health > 0) {
			scoreBtwUpdate = scoreBtwUpdate - 1;
		}

		if (health > 0 && scoreBtwUpdate==0) {
			currentScore += 1;
			txtScore.text = "SCORE :  "+ currentScore.ToString ();

			scoreBtwUpdate = 52;
		}
	}

	IEnumerator FylPlane()
	{
		GetComponent<SpriteRenderer> ().sprite = plane [0];
		yield return new WaitForSeconds (0.1f);
		GetComponent<SpriteRenderer> ().sprite = plane [1];
		yield return new WaitForSeconds (0.1f);

		StartCoroutine (FylPlane ());
	}

	public void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		backgroundMusic.SetActive (true);
	}

	public void Exit()
	{
		Application.Quit ();
	}

	IEnumerator GameOver(){
		yield return new WaitForSeconds (0.9f);
		backgroundMusic.SetActive (false);
		txtFinalScore.text = currentScore.ToString ();
		gameOver.SetActive (true);
	}
}
