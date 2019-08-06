using System.Collections;
using UnityEngine;

public class backgroundScroller : MonoBehaviour {

	public float scrollSpeed=0f;

	void Update()
	{
		GetComponent<Renderer> ().material.mainTextureOffset = new Vector2 (Time.time * scrollSpeed, 0f);
	}

	/*public float scrollSpeed=-5f;
	public Vector2 startPos;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		float newPos = Mathf.Repeat (Time.time * scrollSpeed, 20);
		transform.position=startPos + Vector2.right * newPos;
	}*/
}
