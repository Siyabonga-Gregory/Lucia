using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageHitPanel : MonoBehaviour {

	void Start()
	{
		StartCoroutine (Kill ());
	}

	IEnumerator Kill()
	{
		yield return new WaitForSeconds (5f);
		this.gameObject.SetActive (false);
	}
}
