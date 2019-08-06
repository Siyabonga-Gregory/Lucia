using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

	/*public Transform player;
	public float range;
	public float speed;

	void Update()
	{
		float distance=Vector3.Distance(transform.position,player.position);
		Debug.Log ("Distance    " + distance + "    Range   " + range);
		if(distance<range)
		{
			Vector3 target=player.position-transform.position;
			float angle=Mathf.Atan2(target.y,target.x)*Mathf.Rad2Deg-90f;
			Quaternion q=Quaternion.AngleAxis(angle,Vector3.forward);
			transform.rotation=Quaternion.RotateTowards(transform.rotation,q,180);
			transform.Translate(Vector3.up * Time.deltaTime * speed);
		}
	}*/

	public Vector2 offset=new Vector2 (0,2);

	void Start()
	{
		transform.localPosition = offset;
	}
}
