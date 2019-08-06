using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform player;
	public float vecticalOffSet;
	public float lowerLimit;
	public float speed;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		transform.parent = null;
	}

	void Update()
	{
		if (player.position.y > lowerLimit) {
			transform.localPosition = Vector3.Lerp (new Vector3 (transform.localPosition.x, transform.localPosition.y, transform.localPosition.z), new Vector3 (player.position.x, player.position.y + vecticalOffSet, transform.localPosition.z), speed);
		} else {
			transform.localPosition = Vector3.Lerp (new Vector3 (transform.localPosition.x, transform.localPosition.y, transform.localPosition.z), new Vector3 (player.position.x, lowerLimit+vecticalOffSet, transform.localPosition.z), speed);
		}
	}
	
	public IEnumerator Shake(float duration,float magnitude)
	{
	  Vector3 originalPos=transform.localPosition;
	  float elapse=0.0f;
	  
	  while(elapse<duration)
	  {
		float x = Random.Range(-1f,1f) * magnitude;
		float y = Random.Range(-1f,1f) * magnitude;
		
		transform.localPosition= new Vector3(x,y,originalPos.z);
		elapse+=Time.deltaTime;
		yield return null;
	  }
		transform.localPosition=originalPos;
	}
}
