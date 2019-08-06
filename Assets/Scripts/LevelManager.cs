using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	public bool []levelStatus;
	public int []levelScores;
	public int []levelGenerator;
	public GameObject[] walls;
	public Material[] floor;
	public string[] scenesName;
	public int currentLevel=0;
	private Generator platformGenerator;
	public GameObject[] enableSpawPoints;
	public Text txtPopUpUIScore;
	public Text txtGameOverPopUpUIScore;
	public Material[] backgrounds;

	void Start()
	{
		platformGenerator = GetComponent<Generator>();	
	}

	public void GetCurrentLevel()
	{
		for (int i = 0; i < levelStatus.Length; i++)
		{
			if (levelStatus [i] != true) 
			{
				currentLevel = i;
				Debug.Log ("Return I   " + i);
				i = levelStatus.Length;
			} 
		}
	}

	public void LoadLevel()
	{
		GetCurrentLevel ();
		//platformGenerator.maxX=levelGenerator[currentLevel];
		//platformGenerator.Regenerate();
		ChangeBackground ();
	}

	void EnableSpawnPoints()
	{
	   enableSpawPoints [currentLevel].SetActive (true);
	}

	public void SaveAndLoad(bool replaying)
	{
		levelScores [currentLevel] = GameObject.FindGameObjectWithTag ("Player").GetComponent<GhostPlayer> ().currentScore;
		levelStatus [currentLevel] = true;
		GameObject.FindGameObjectWithTag ("Background").GetComponent<backgroundScroller> ().enabled = true;

		if (!replaying) {
			LoadLevel ();

			if (currentLevel > 0) {
				GameObject.FindGameObjectWithTag ("Player").GetComponent<GhostPlayer> ().txtLevel.text = "LV : " + (currentLevel + 1);
			} else if (currentLevel == 0) {
				GameObject.FindGameObjectWithTag ("Player").GetComponent<GhostPlayer> ().txtLevel.text = "LV : " + 2;
				currentLevel += 1;
			}
		}
		else if (replaying) 
		{
			levelScores [currentLevel] = 0;
			levelStatus [currentLevel] = false;
			GameObject.FindGameObjectWithTag ("Player").GetComponent<GhostPlayer> ().currentScore = 0;
		}

		if(currentLevel>3)
		{
			GameObject.FindGameObjectWithTag ("Player").GetComponent<GhostPlayer> ().health =GameObject.FindGameObjectWithTag ("Player").GetComponent<GhostPlayer> ().health + 100;
		}
		else{
			GameObject.FindGameObjectWithTag ("Player").GetComponent<GhostPlayer> ().health =100;
		}

		EnableSpawnPoints ();
		//GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().walls[GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().currentLevel].SetActive(false);

		GameObject.FindGameObjectWithTag ("Manager").GetComponent<AudioManager> ().canPlayAudio = true;
		GameObject.FindGameObjectWithTag ("Manager").GetComponent<AudioManager> ().PlayAudioClip (15, GameObject.FindGameObjectWithTag ("Manager").gameObject.transform);
		GameObject.FindGameObjectWithTag ("Player").GetComponent<GhostPlayer> ().nextLevelUI.SetActive (false);
		GameObject.FindGameObjectWithTag ("Player").GetComponent<GhostPlayer> ().canMove = true;
	}

	public IEnumerator BombAllEnemies()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		Debug.Log ("Found Enemies  " + enemies.Length);
		foreach (GameObject enemy in enemies) 
		{
			GameObject hit = Instantiate (enemy.GetComponent<GhostEnemy>().hitEffect, enemy.transform.position, Quaternion.identity);
			GameObject.FindGameObjectWithTag ("Player").GetComponent<GhostPlayer> ().currentScore += 1;

			txtPopUpUIScore.text = GameObject.FindGameObjectWithTag ("Player").GetComponent<GhostPlayer>().currentScore.ToString ();
			txtGameOverPopUpUIScore.text =  GameObject.FindGameObjectWithTag ("Player").GetComponent<GhostPlayer>().currentScore.ToString ();

			yield return new WaitForSeconds (0f);
			Destroy (enemy);
		}
	}

	void ChangeBackground()
	{
		GameObject.FindGameObjectWithTag ("Background").GetComponent<MeshRenderer> ().material = backgrounds [currentLevel];
	}

	public void Retry()
	{
		StartCoroutine (GameRetry ());
	}

	IEnumerator GameRetry()
	{
		GameObject.FindGameObjectWithTag ("Background").GetComponent<backgroundScroller> ().enabled = true;
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().enabled=true;
		yield return new WaitForSeconds (0.2f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void Quit()
	{
		Application.Quit ();
	}
}
