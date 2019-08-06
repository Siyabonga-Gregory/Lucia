using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GhostPlayer : MonoBehaviour {

	public float speed;
	public float jump;
	float moveInput;
	Rigidbody2D rb;
	public bool isFacingRight=true;
	public bool isGrounded;
	public Transform groundCheck;
	public float checkRadius;
	public LayerMask whatIsGround;
	public int extraJump;
	public GameObject barrel;
	public GameObject bulletPrefab;
	public int health;
	public Image healthBar;
	public Image healthBarAttached;
	public Text txtScore;
	public Text txtLevel;
	public Text txtLives;
	public int currentScore=0;
	bool stillAlive=true;
	public GameObject gameOverUI;
	public GameObject nextLevelUI;
	public bool canMove = true;
	public Joystick joystick;
	/*
	//Mobile 
	public SimpleTouchController leftController;
	public SimpleTouchController rightController;
	public bool continuousRightController = true;
	private Vector2 prevRightTouchPos;



	void Awake()
	{
		rightController.TouchEvent += RightController_TouchEvent;
		rightController.TouchStateEvent += RightController_TouchStateEvent;
	}

	public bool ContinuousRightController
	{
		set{continuousRightController = value;}
	}

	void RightController_TouchStateEvent (bool touchPresent)
	{
		if(!continuousRightController)
		{
			prevRightTouchPos = Vector2.zero;
		}
	}

	void RightController_TouchEvent (Vector2 value)
	{
		if(!continuousRightController)
		{
			Vector2 deltaValues = value - prevRightTouchPos;
			prevRightTouchPos = value;

		}
	}*/


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		healthBar = healthBar.GetComponent<Image> ();
		healthBar.fillAmount = health;
		healthBarAttached = healthBarAttached.GetComponent<Image> ();
		healthBarAttached.fillAmount = health;

	}

	public void TakeDamage(int damage)
	{
		if (health > 0) {
			//GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioManager>().PlayAudioClip(0,this.transform);
			health -= damage;
			//if (health < 50) {
				StartCoroutine(GetComponent<FadeInAndOut> ().BloodySplash ());
				GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioManager>().PlayAudioClip(1,this.transform);
				//txtLives.text = "LIVES : " +health.ToString ();
				txtScore.text = currentScore.ToString ();
				GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager> ().txtPopUpUIScore.text = currentScore.ToString ();
				GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager> ().txtGameOverPopUpUIScore.text = currentScore.ToString ();
				if (health <= 0) 
				{
					StartCoroutine (GameOver ());
				}
			//}

		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		/*
		isGrounded = Physics2D.OverlapCircle (groundCheck.position, checkRadius, whatIsGround);

		moveInput = Input.GetAxisRaw ("LeftHorizontal");

		rb.velocity = new Vector2 (moveInput * 
		, rb.velocity.y);

		if (isFacingRight == false && moveInput > 0) {
			FlipPlayer ();
		} else if (isFacingRight == true && moveInput < 0) {
			FlipPlayer ();
		}*/
	}

	void FlipPlayer()
	{
		isFacingRight =! isFacingRight;
		Vector3 scaler = transform.localScale;
		scaler.x *= -1;
		transform.localScale = scaler;
	}

	void Update()
	{

		if (canMove)
		{
			/*if(continuousRightController)
			{
				isGrounded = Physics2D.OverlapCircle (groundCheck.position, checkRadius, whatIsGround);
				moveInput = Input.GetAxisRaw ("LeftHorizontal");
				rb.velocity = new Vector2 (moveInput * speed , rb.velocity.y);
			}*/
			
			
			  
        Vector3 moveVector = (Vector3.right * joystick.Horizontal + Vector3.up * joystick.Vertical);

        if (moveVector != Vector3.zero)
        { 
            //transform.rotation = Quaternion.LookRotation(Vector3.forward, moveVector);
           // transform.Translate(moveVector * moveSpeed * Time.deltaTime, Space.World);

        }
		
  

			isGrounded = Physics2D.OverlapCircle (groundCheck.position, checkRadius, whatIsGround);

			moveInput = Input.GetAxisRaw ("LeftHorizontal");

			rb.velocity = new Vector2 (moveInput * speed , rb.velocity.y);

			if (isFacingRight == false && moveInput > 0) {
				FlipPlayer ();
			} else if (isFacingRight == true && moveInput < 0) {
				FlipPlayer ();
			}


			healthBar.fillAmount = health / 100f;
			healthBarAttached.fillAmount = health / 100f;

			RaycastHit2D groundInfo = Physics2D.Raycast (groundCheck.position, Vector2.down, 1000);
			Debug.DrawRay (groundCheck.position, Vector2.down, Color.red);
			if (groundInfo.collider == false) {
				stillAlive = false;
				StartCoroutine (GameOver ());
			}
			else 
			{
				if (health > 0) {
					stillAlive = true;
				}
			}


			if (health == 0) {
				stillAlive = false;
				StartCoroutine (GameOver ());
			}
			if (health > 0) {
				//txtLives.text = "LIVES : " +health.ToString ();
				txtScore.text = currentScore.ToString ();
				GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager> ().txtPopUpUIScore.text = currentScore.ToString ();
				GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager> ().txtGameOverPopUpUIScore.text = currentScore.ToString ();
			}

			if (isGrounded) {
				extraJump = 2;
			}

			if (Input.GetButtonDown("Jump") && extraJump > 0) {
				//GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioManager>().PlayAudioClip(3,this.transform);
				rb.velocity = Vector2.up * jump;
				extraJump--;
			} else if (Input.GetButtonDown("Jump") && extraJump == 0 && isGrounded) {
				rb.velocity = Vector2.up * jump;
				GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioManager>().PlayAudioClip(3,this.transform);
			}

			if (Input.GetButtonDown("Fire1")){
				var bullet = (GameObject)Instantiate (bulletPrefab, barrel.transform.position,Quaternion.identity);
				GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioManager>().PlayAudioClip(6,this.transform);
				if (isFacingRight) {
					bullet.GetComponent<Rigidbody2D> ().velocity = Vector2.right * 30f;
				} else {
					bullet.GetComponent<Rigidbody2D> ().velocity = Vector2.left * 30f;
				}

				Destroy (bullet, 3.0f);
			}
		}
	}

	IEnumerator GameOver(){
		

		if (!stillAlive) 
		{
			GameObject.FindGameObjectWithTag ("Background").GetComponent<backgroundScroller> ().enabled = false;
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().enabled=false;
			GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioManager>().PlayAudioClip(14,this.transform);
			GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioManager>().canPlayAudio=false;
			GetComponent<BoxCollider2D> ().enabled = false;
			GetComponent<MeshRenderer> ().enabled = false;
			yield return new WaitForSeconds (0.4f);
			canMove = false;
			GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager> ().enableSpawPoints [GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager> ().currentLevel].SetActive (false);
			//StartCoroutine (GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager> ().BombAllEnemies());
			//GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioManager>().PlayAudioClip(14,this.transform);
			GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioManager>().canPlayAudio=false;
			//GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager> ().txtGameOverPopUpUIScore.text=currentScore.ToString();
			GameObject.FindGameObjectWithTag ("UIManager").GetComponent<UIManager> ().ButtonMethod ("CarShopBtn");
			yield return new WaitForSeconds (0.4f);
		}
	}
}
