using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

	public GameObject enemy;

	// Use this for initialization
	void Start () {
		Instantiate (enemy, transform.position, Quaternion.identity);
	}

}
