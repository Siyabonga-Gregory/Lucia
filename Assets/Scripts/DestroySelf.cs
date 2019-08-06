using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour {

	void OnEnable()
	{
		StartCoroutine (DestroyItSelft ());
	}

	IEnumerator DestroyItSelft()
	{
		yield return new WaitForSeconds (3f);
		Destroy (this.transform.gameObject);
	}
}
