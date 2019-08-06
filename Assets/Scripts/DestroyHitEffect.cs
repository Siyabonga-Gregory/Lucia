using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyHitEffect : MonoBehaviour {

	void Start()
	{
		StartCoroutine (Destroy ());
	}

	IEnumerator Destroy()
	{
		yield return new WaitForSeconds (0.4f);
		Destroy (this.gameObject);
	}
}
