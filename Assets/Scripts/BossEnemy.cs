using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossEnemy : MonoBehaviour {

	public GameObject[]barrel;
	public GameObject[] bulletPrefab;
	public int damage=1;
	public float speed;
	public float jump;
	public float checkRadius;
	Rigidbody2D rb;
	bool isGrounded;
	public GameObject hitEffect;
	public LayerMask whatIsGround;
	public bool moveToLeft;
	public int shotBtw=2;
	int shotBtwOrg;
	public int health=3;
	public Transform groundCheck;
	public Transform wheel;

	public bool []BulletToUse;
	public int[] weaponSoundsIndex;
	public int[] bulletDuration;

	int currentBulletIndex=0;
	int currentSoundIndex=0;
	int currentBulletDuration=0;
	public GameObject player;
	public float ChassingRange;
	public float minChassingRange;
	public bool movingTowardsPlayer;
	public Image healthBar;
	public GameObject[] enemies;
	public bool hasSpawned=false;
	public GameObject spawnPos;

	void ManageWeaponSettings()
	{
		int weaponToBeUse=Random.Range(0,BulletToUse.Length);
		for (int i = 0; i < BulletToUse.Length; i++) {
			if (i == weaponToBeUse)
			{
				BulletToUse [i] = true;
				currentBulletIndex = i;
			}
			else
			{
				BulletToUse [i] = false;	
			}
		}
		currentSoundIndex   = weaponSoundsIndex[weaponToBeUse];
		currentBulletDuration = bulletDuration[weaponToBeUse];
	}


	void OnEnable()
	{
		GameObject.FindGameObjectWithTag ("Manager").GetComponent<AudioManager> ().PlayAudioClip (13, GameObject.FindGameObjectWithTag ("Manager").gameObject.transform);
		//GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().walls[GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().currentLevel].SetActive(true);
		StartCoroutine (DestroyItSelft ());
		StartCoroutine (Shoot ());
	}

	void Start()
	{
		healthBar = healthBar.GetComponent<Image> ();
		healthBar.fillAmount = health;

		rb = GetComponent<Rigidbody2D> ();
		shotBtwOrg = shotBtw;
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	void FixedUpdate () {
		isGrounded = Physics2D.OverlapCircle (wheel.position, checkRadius, whatIsGround);
	}

	void Update()
	{
		healthBar.fillAmount = health / 100f;

		if (health <= 0) {
			StartCoroutine (Die ());
		} else if (health == 80) {
			if (!hasSpawned) {
				StartCoroutine (SpawnMoreEnemies ());
				hasSpawned = true;
			}
		}
		else if (health == 50) 
		{
			if (hasSpawned) {
				StartCoroutine (SpawnMoreEnemies ());
				hasSpawned = false;
			}
		}

		transform.Translate(Vector2.right * speed * Time.deltaTime);

		RaycastHit2D hitRayCast;
		RaycastHit2D platformRayCast;
		int jumpOrTurn=Random.Range(0,2);

		platformRayCast = Physics2D.Raycast (groundCheck.position, Vector2.down, 100f);
		Debug.DrawRay (groundCheck.position, Vector2.down, Color.red);

		if (platformRayCast.collider != true)
		{
			if (!moveToLeft && isGrounded) {
				transform.eulerAngles = new Vector3 (0, -180, 0);
				moveToLeft = true;
			}
			else if (moveToLeft && isGrounded) 
			{
				transform.eulerAngles = new Vector3 (0, 0, 0);
				moveToLeft = false;
			}
		}

		if (!movingTowardsPlayer) 
		{
			ChassingRangeController();
		}

		if (!moveToLeft)
		{
			hitRayCast = Physics2D.Raycast (groundCheck.position, Vector2.right, 0.2f);
			Debug.DrawRay (groundCheck.position, Vector2.right, Color.red);
		} 
		else 
		{
			hitRayCast = Physics2D.Raycast (groundCheck.position, Vector2.left, 0.2f);
			Debug.DrawRay (groundCheck.position, Vector2.left, Color.red);
		}

		if (hitRayCast.collider != false && !moveToLeft) {

			if( hitRayCast.collider.tag == "Enemy" || hitRayCast.collider.tag == "BossEnemy" || hitRayCast.collider.tag == "Dirt")
			{
				if (!movingTowardsPlayer) 
				{
					if (jumpOrTurn == 0 && isGrounded) 
					{
						transform.eulerAngles = new Vector3 (0, -180, 0);
						moveToLeft = true;
					} 
					else if (isGrounded) 
					{
						rb.velocity = Vector2.up * jump;
					}
				} 
				else
				{
					if (isGrounded) 
					{
						rb.velocity = Vector2.up * jump;
					}
				}
			}
		} else if (hitRayCast.collider != false && moveToLeft) {
			{
				if( hitRayCast.collider.tag == "Enemy" || hitRayCast.collider.tag == "BossEnemy" || hitRayCast.collider.tag == "Dirt")
				{
					if (!movingTowardsPlayer)
					{
						if (jumpOrTurn == 0 && isGrounded) 
						{
							transform.eulerAngles = new Vector3 (0, 0, 0);
							moveToLeft = false;
						} 
						else if (isGrounded)
						{
							rb.velocity = Vector2.up * jump;
						}
					}
					else
					{
						rb.velocity = Vector2.up * jump;
					}
				}
			}
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
		yield return new WaitForSeconds (20f);
		//GameObject hit = Instantiate (hitEffect, transform.position, Quaternion.identity);
		//Destroy (this.transform.gameObject);
	}

	IEnumerator Shoot()
	{
		if (shotBtw <= 0) {

			if (currentBulletDuration == 0) 
			{
				ManageWeaponSettings ();
			}

			var bullet_a = (GameObject)Instantiate (bulletPrefab[currentBulletIndex], barrel[0].transform.position,Quaternion.identity);
			var bullet_b = (GameObject)Instantiate (bulletPrefab[currentBulletIndex], barrel[1].transform.position,Quaternion.identity);
			var bullet_c = (GameObject)Instantiate (bulletPrefab[currentBulletIndex], barrel[2].transform.position,Quaternion.identity);
			var bullet_d = (GameObject)Instantiate (bulletPrefab[currentBulletIndex], barrel[3].transform.position,Quaternion.identity);

			GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioManager>().PlayAudioClip(currentSoundIndex,this.transform);

			if (!moveToLeft) {
				bullet_a.GetComponent<Rigidbody2D> ().velocity = Vector2.right * 30f;
				bullet_b.GetComponent<Rigidbody2D> ().velocity = Vector2.right * 30f;
				bullet_c.GetComponent<Rigidbody2D> ().velocity = Vector2.right * 30f;
				bullet_d.GetComponent<Rigidbody2D> ().velocity = Vector2.right * 30f;
			} else {
				bullet_a.GetComponent<Rigidbody2D> ().velocity = Vector2.left * 30f;
				bullet_b.GetComponent<Rigidbody2D> ().velocity = Vector2.left * 30f;
				bullet_c.GetComponent<Rigidbody2D> ().velocity = Vector2.left * 30f;
				bullet_d.GetComponent<Rigidbody2D> ().velocity = Vector2.left * 30f;
			}

			Destroy (bullet_a, 3.0f);
			Destroy (bullet_b, 3.0f);
			Destroy (bullet_c, 3.0f);
			Destroy (bullet_d, 3.0f);
			shotBtw = shotBtwOrg;
			currentBulletDuration--;
	
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
		GetComponent<BoxCollider2D> ().enabled = false;
		GetComponent<MeshRenderer> ().enabled = false;
		GameObject.FindGameObjectWithTag ("Background").GetComponent<backgroundScroller> ().enabled = false;

		GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager> ().enableSpawPoints [GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager> ().currentLevel].SetActive (false);
		StartCoroutine (GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager> ().BombAllEnemies());
		GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioManager>().PlayAudioClip(14,this.transform);
		GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioManager>().canPlayAudio=false;
		//yield return new WaitForSeconds (1f);
		//GameObject.FindGameObjectWithTag ("Player").GetComponent<GhostPlayer> ().nextLevelUI.SetActive(true);
		//GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager> ().txtPopUpUIScore.text=GameObject.FindGameObjectWithTag ("Player").GetComponent<GhostPlayer>().currentScore.ToString();
		GameObject.FindGameObjectWithTag ("UIManager").GetComponent<UIManager> ().ButtonMethod ("PowerShopBtn");
		//StartCoroutine(GameObject.FindGameObjectWithTag("BonusManager").GetComponent<BonusManager>().BombAllEnemies());
		yield return new WaitForSeconds (0.2f);
		GameObject.FindGameObjectWithTag ("Player").GetComponent<GhostPlayer> ().canMove = false;
		Destroy (this.gameObject);
	}


	void ChassingRangeController()
	{
		if (Vector2.Distance (transform.position, player.transform.position) > ChassingRange) {
			speed = 9;
			GameObject.FindGameObjectWithTag ("Player").GetComponent<GhostPlayer> ().speed=8;

			if (moveToLeft) {
				transform.eulerAngles = new Vector3 (0, 0, 0);
				moveToLeft = false;
				movingTowardsPlayer = true;
			} else {
				transform.eulerAngles = new Vector3 (0, -180, 0);
				moveToLeft = true;
				movingTowardsPlayer = true;
			}
	
			StartCoroutine (MoveTowardsPlayer ());
		}
		else if (Vector2.Distance (transform.position, player.transform.position) <= minChassingRange || Vector2.Distance (transform.position, player.transform.position) <= ChassingRange)
		{
			speed = 9;
			GameObject.FindGameObjectWithTag ("Player").GetComponent<GhostPlayer> ().speed=8;
			movingTowardsPlayer = false;
		}

		/*float maxRange=Vector3.Distance(transform.position,player.transform.position);
		Debug.Log ("Max Range    " + maxRange + "    Chassing Range   " + ChassingRange);
		if (maxRange > ChassingRange && !movingTowardsPlayer) {
			if (moveToLeft) {
				transform.eulerAngles = new Vector3 (0, 0, 0);
				moveToLeft = false;
				movingTowardsPlayer = true;
			} else {
				transform.eulerAngles = new Vector3 (0, -180, 0);
				moveToLeft = true;
				movingTowardsPlayer = true;
			}
		}
		else if (maxRange > 30 && movingTowardsPlayer) 
		{
			movingTowardsPlayer = false;
			//movingTowardsPlayer = true;
		}
		else if (maxRange <= minChassingRange)
		{
			movingTowardsPlayer = false;
		}*/
	}

	IEnumerator MoveTowardsPlayer()
	{
		transform.position = Vector2.MoveTowards (transform.position, player.transform.position, speed * Time.deltaTime);
		if (Vector2.Distance (transform.position, player.transform.position) > minChassingRange || Vector2.Distance (transform.position, player.transform.position) > ChassingRange) {
			movingTowardsPlayer = true;
			GameObject.FindGameObjectWithTag ("Player").GetComponent<GhostPlayer> ().speed=10;
			StartCoroutine (MoveTowardsPlayer ());
		}
		else
		{
			movingTowardsPlayer = false;
			GameObject.FindGameObjectWithTag ("Player").GetComponent<GhostPlayer> ().speed=15;
			yield return new WaitForSeconds (0f);
		}
	}

	IEnumerator SpawnMoreEnemies()
	{
		for (int i = 0; i < enemies.Length; i++)
		{
			enemies [i].GetComponent<GhostEnemy> ().speed[GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager> ().currentLevel] = 7f;
			Instantiate(enemies[i],spawnPos.transform.position,Quaternion.identity);
			yield return new WaitForSeconds (0f);
			Debug.Log ("Spawning Enemy Number    " + i + " Enemy Name    " + enemies [i].gameObject.transform.name);
		}
	}
}
