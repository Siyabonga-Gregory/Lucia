using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public int damage=1;
	public float speed;
	public GameObject hitEffect;
	public GameObject hitPanelEffect;


	void Update()
	{
		transform.Translate(Vector2.right * speed * Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Player")){
			other.GetComponent<Player> ().TakeDamage (1);

			GameObject hit = Instantiate (hitEffect, transform.position, Quaternion.identity);
			GameObject hitPanel = Instantiate (hitPanelEffect, transform.position, Quaternion.identity);
			hitPanelEffect.SetActive	(true);
			Destroy(gameObject);
		}
	}
}
