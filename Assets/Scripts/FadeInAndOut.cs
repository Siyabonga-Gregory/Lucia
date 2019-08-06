using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInAndOut : MonoBehaviour {

	public GameObject fadeCanvas;

	public IEnumerator BloodySplash()
	{
		fadeCanvas.SetActive (true);
		yield return new WaitForSeconds (0.02f);
		fadeCanvas.SetActive (false);
		yield return null;
	}


}
