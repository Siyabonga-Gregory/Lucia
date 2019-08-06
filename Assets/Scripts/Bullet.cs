using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	public GameObject hitEffect;

	void Start()
	{
		StartCoroutine (DestroyItSelft ());
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.collider.tag.ToString()!="") {
			if (col.collider.transform.tag=="Player" && this.transform.tag.Equals ("EnemyBullet"))
			{
				GameObject hit = Instantiate (hitEffect, transform.position, Quaternion.identity);
				col.collider.transform.GetComponent<GhostPlayer> ().TakeDamage (1);
				GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioManager>().PlayAudioClip(1,col.collider.transform);
				if (col.collider.transform.GetComponent<GhostPlayer> ().health == 5) 
				{
					//StartCoroutine (GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraFollow> ().Shake (0.1f, 0.5f));
					///Debug.Log ("Shaked");
				}

			} 
			else if (this.transform.tag=="PlayerBullet") 
			{
				if (col.collider.transform.tag.Equals ("Enemy")) 
				{
					GameObject hit = Instantiate (hitEffect, transform.position, Quaternion.identity);
					col.collider.transform.GetComponent<GhostEnemy> ().TakeDamage (1);
				} 
				else if (col.collider.transform.tag.Equals ("BossEnemy")) 
				{
					GameObject hit = Instantiate (hitEffect, transform.position, Quaternion.identity);
					col.collider.transform.GetComponent<BossEnemy> ().TakeDamage (2);
				}

			}
		}

		Destroy (this.transform.gameObject);
	}

	IEnumerator DestroyItSelft()
	{
		yield return new WaitForSeconds (5f);
		Destroy (this.transform.gameObject);
	}
}
