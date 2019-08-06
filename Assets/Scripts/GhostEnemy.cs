using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostEnemy : MonoBehaviour {

	public int damage=1;
	public float jump;
	public float checkRadius;
	Rigidbody2D rb;
	public bool isGrounded;
	public GameObject hitEffect;
	public GameObject barrel;
	public GameObject bulletPrefab;
	public LayerMask whatIsGround;
	public bool moveToLeft;
	public int shotBtw=2;
	int shotBtwOrg;
	public int health=3;
	public Transform groundCheck;
	public Transform wheel;
	public GameObject popupTextPrefab;
	public Image healthBar;
	public float[]speed;
	public float lifeSpan=20f;

	void OnEnable()
	{
		StartCoroutine (DestroyItSelft ());
		StartCoroutine (Shoot ());
	}

	void Start()
	{
		healthBar = healthBar.GetComponent<Image> ();
		healthBar.fillAmount = health;
		rb = GetComponent<Rigidbody2D> ();
		shotBtwOrg = shotBtw;

	}

	void FixedUpdate () {
		isGrounded = Physics2D.OverlapCircle (wheel.position, checkRadius, whatIsGround);
	}

	void Update()
	{
		if (GameObject.FindGameObjectWithTag ("Player").GetComponent<GhostPlayer> ().canMove) {
			
			healthBar.fillAmount = health / 3f;

			if (health <= 0) {
				StartCoroutine (Die ());
			}

			transform.Translate (Vector2.right * speed [GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager> ().currentLevel] * Time.deltaTime);

			RaycastHit2D hitRayCast;
			RaycastHit2D platformRayCast;

			int jumpOrTurn = Random.Range (0, 2);

			platformRayCast = Physics2D.Raycast (groundCheck.position, Vector2.down, 100f);
			Debug.DrawRay (groundCheck.position, Vector2.down, Color.red);

			if (platformRayCast.collider != true) {
				if (!moveToLeft && isGrounded) {
					transform.eulerAngles = new Vector3 (0, -180, 0);
					moveToLeft = true;
				} else if (moveToLeft && isGrounded) {
					transform.eulerAngles = new Vector3 (0, 0, 0);
					moveToLeft = false;
				}
			}

			if (!moveToLeft) {
				hitRayCast = Physics2D.Raycast (groundCheck.position, Vector2.right, 0.2f);
				Debug.DrawRay (groundCheck.position, Vector2.right, Color.red);
			} else {
				hitRayCast = Physics2D.Raycast (groundCheck.position, Vector2.left, 0.2f);
				Debug.DrawRay (groundCheck.position, Vector2.left, Color.red);
			}

			if (hitRayCast.collider != false && !moveToLeft) {
				if (hitRayCast.collider.tag == "Enemy" || hitRayCast.collider.tag == "BossEnemy" || hitRayCast.collider.tag == "Dirt") {
					if (jumpOrTurn == 0 && isGrounded) {
						transform.eulerAngles = new Vector3 (0, -180, 0);
						moveToLeft = true;
					} else if (isGrounded) {
						rb.velocity = Vector2.up * jump;
					}
				}

			} else if (hitRayCast.collider != false && moveToLeft) {
				{
					if (hitRayCast.collider.tag == "Enemy" || hitRayCast.collider.tag == "BossEnemy" || hitRayCast.collider.tag == "Dirt") {
						if (jumpOrTurn == 0 && isGrounded) {
							transform.eulerAngles = new Vector3 (0, 0, 0);
							moveToLeft = false;
						} else if (isGrounded) {
							rb.velocity = Vector2.up * jump;
						}
					}
				}
			}

		} 
		else 
		{
			GetComponent<BoxCollider2D> ().enabled = false;
			GetComponent<MeshRenderer> ().enabled = false;
			lifeSpan =2;
			StopCoroutine (DestroyItSelft ());
			StartCoroutine (DestroyItSelft ());
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.transform.gameObject != null && col.transform.gameObject.tag=="Player")
		{
			col.transform.GetComponent<GhostPlayer> ().TakeDamage (1);
			/*var bullet = (GameObject)Instantiate (bulletPrefab, barrel.transform.position,Quaternion.identity);*/
			GameObject hit = Instantiate (hitEffect, transform.position, Quaternion.identity);
			GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioManager>().PlayAudioClip(1,this.transform);
		}
	}

	IEnumerator DestroyItSelft()
	{
		yield return new WaitForSeconds (lifeSpan);
		GameObject hit = Instantiate (hitEffect, transform.position, Quaternion.identity);
		Destroy (this.transform.gameObject);
	}

	IEnumerator Shoot()
	{
		if (shotBtw <= 0) {
			var bullet = (GameObject)Instantiate (bulletPrefab, barrel.transform.position, Quaternion.identity);
			if (moveToLeft) {
				bullet.GetComponent<Rigidbody2D> ().velocity = Vector2.left * 30f;
			} else if (!moveToLeft) {
				bullet.GetComponent<Rigidbody2D> ().velocity = Vector2.right * 30f;
			}
			shotBtw = shotBtwOrg;

		} else {
			yield return new WaitForSeconds (1f);
			shotBtw--;
		}
		StartCoroutine (Shoot ());
	}

	public void TakeDamage(int damage)
	{
		if (health > 0) {
			//GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioManager>().PlayAudioClip(0,this.transform);
			health -= damage;
			ShowPopUpText ();

		}
	}

	void ShowPopUpText()
	{
		if (popupTextPrefab) {
			//Instantiate (popupTextPrefab, transform.position, Quaternion.identity, transform);
		}
	}

	IEnumerator GameOver(){
		yield return new WaitForSeconds (0.9f);
		//backgroundMusic.SetActive (false);
		//txtFinalScore.text = currentScore.ToString ();
		//gameOver.SetActive (true);
	}

	IEnumerator Die()
	{
		yield return new WaitForSeconds (0.01f);
		GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioManager>().PlayAudioClip(5,this.transform);
		GameObject.FindGameObjectWithTag ("Player").GetComponent<GhostPlayer> ().currentScore += 1;
		Destroy (this.gameObject);
	}


}
