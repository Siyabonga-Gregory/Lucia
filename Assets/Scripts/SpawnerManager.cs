using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour {

	public GameObject[] enemies;

	float timeBtwSpawn;
	public float startTimeBtwSpawn;
	public float decreaseTime;
	public float minTime=0.65f;
	public bool basedOnScore = false;
	public int scoreToTrigger;
	bool spawned=false;
	public GameObject disable;


	void Update()
	{
		if (this.gameObject.activeSelf) 
		{
			if(timeBtwSpawn<=0 && !basedOnScore) /*&& GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().health>0*/
			{
				int rand=Random.Range(0,enemies.Length);
				Instantiate(enemies[rand],transform.position,Quaternion.identity);
				timeBtwSpawn=startTimeBtwSpawn;
				if(startTimeBtwSpawn > minTime){
					startTimeBtwSpawn-=decreaseTime;
				} 
				startTimeBtwSpawn = timeBtwSpawn;
			}
			else
			{
				timeBtwSpawn-=Time.deltaTime;
			}

			if (basedOnScore) { /*&& GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().health>0*/
				if (GameObject.FindGameObjectWithTag ("Player").GetComponent<GhostPlayer> ().currentScore == scoreToTrigger) /*|| GameObject.FindGameObjectWithTag ("Player").GetComponent<GhostPlayer> ().currentScore - scoreToTrigger==0)*/ {
					if (!spawned) 
					{
						int rand = Random.Range (0, enemies.Length);
						Instantiate (enemies [rand], transform.position, Quaternion.identity);
						spawned = true;
						if (disable != null)
						{
							disable.SetActive (false);
						}
					}
				}

			}
		}
	}
}
