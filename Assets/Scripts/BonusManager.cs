using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour {

	public float speed;
	public GameObject effect;
	public int power;

	void Update()
	{
		transform.Translate(Vector2.left * speed * Time.deltaTime);
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		StartCoroutine (PeformAndDie (col));
	}

	IEnumerator PeformAndDie(Collision2D col)
	{
		if (col.transform.gameObject != null && col.transform.gameObject.tag=="Player" && this.transform.tag=="Bomb")
		{
			GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioManager>().PlayAudioClip(8,this.transform);
			GameObject hit = Instantiate (effect, transform.position, Quaternion.identity);
			yield return new WaitForSeconds (0.05f);
			StartCoroutine (BombAllEnemies ());
		}
		else if (col.transform.gameObject != null && col.transform.gameObject.tag=="Player" && this.transform.tag=="Power")
		{
			GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioManager>().PlayAudioClip(8,this.transform);
			GameObject.FindGameObjectWithTag ("Player").GetComponent<GhostPlayer> ().health += Random.Range(10,30);
			GameObject hit = Instantiate (effect, transform.position, Quaternion.identity);
			yield return new WaitForSeconds (0.05f);
			Destroy (this.gameObject);
		}
	}

	public IEnumerator BombAllEnemies()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		foreach (GameObject enemy in enemies) 
		{
			GameObject hit = Instantiate (enemy.GetComponent<GhostEnemy>().hitEffect, enemy.transform.position, Quaternion.identity);
			GameObject.FindGameObjectWithTag ("Player").GetComponent<GhostPlayer> ().currentScore += 1;
			yield return new WaitForSeconds (0f);
			Destroy (enemy);
		}
		Destroy (this.gameObject);
	}

}