using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : MonoBehaviour {

	void Start()
	{
		StartCoroutine (Kill ());
	}

	IEnumerator Kill()
	{
		yield return new WaitForSeconds (5f);
		Destroy (this.gameObject);
	}
}
