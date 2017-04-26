using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Hero_Controller_Basic : MonoBehaviour, IHeroControllable {

	//------- IDE AVAIL CONFIGS -------------
	
	public float acceleration = 1.0f;
	
	public float speed_horiz_max = 10.0f;

	public int jump_limit = 2;

	public int jump_strength = 1;

	public Camera MainCamera;

	public GameObject ching;

	public MissionCompleteUIsplash UIsplash;

	public UIcolorThrob uiThrob;


	//--------- INTERNAL --------------
	
	//stop receiveing all input
	private bool deactivateInputs = false;
	
	//Hero Animation Controller
	GameObject hero_anim_go;
	
	//Hero Firing Obj
	GameObject hero_firing;
	
	//Hero Audio GO
	GameObject hero_audio;
	
	//random
	AudioPlayRandom apr;

	//Hero RigidBody
	Rigidbody2D myRB2D;
	
	//Hero Animator
	Animator hero_anim;

	Vector3 og_pos;

	int jump_count = 0;

	bool isDead = false;
	bool isRight = true;
	bool isOnGround = false;

	bool deathStateOn = false;

	bool damageStateOn = false;

	string tag_platforms = "PLATFORM";
	string tag_hazards = "HAZARD";
	string tag_scene_reset = "RESET";
	string tag_treats = "TREAT";
	//string tag_scene_cutscene_intro = "TAG_CUTSCENE_INTRO";
	//string tag_scene_cutscene_window = "TAG_CUTSCENE_WINDOW";
	//string tag_scene_cutscene_attic = "TAG_CUTSCENE_ATTIC";


	//track current speed	
	float speedX = 0f;

	float x_translation;

	bool keyRightDown = false;
	bool keyLeftDown = false;
	bool keyJumpDown = false;
	bool keyFireDown = false;

	bool applyYforce = false;

	float axis_h = 0;
	float axis_v = 0;


	//--------bones
	//UI - bone count 
	public Text boneCountText;
	public Image boneCountBar;
	public Image boneCountBarBG;

	int countBones = 0;
	int totalBones = 0;

	TreatLogic tmpTreat;


	//---------health
	int health = 5;

	public GameObject health1;
	public GameObject health2;
	public GameObject health3;
	public GameObject health4;
	public GameObject health5;




	//------------------------------------------------------------ 
	
	//						INIT
	
	//------------------------------------------------------------

	void Start () {

		hero_anim_go = transform.Find("Hero_Animation").gameObject;
		hero_firing = transform.Find("Hero_Attack").gameObject;

		myRB2D = gameObject.GetComponent<Rigidbody2D> ();
		hero_anim = hero_anim_go.GetComponent<Animator> ();
		Physics2D.IgnoreLayerCollision (11,14,true);//ignore attack collisions
		Physics2D.IgnoreLayerCollision (11,22,true);//ignore trap platform colliders

		og_pos = gameObject.transform.position;

		hero_audio = transform.Find("Hero_Audio").gameObject;
		apr = hero_audio.GetComponent<AudioPlayRandom>();

		addScore(1);

	}


	//----- set camera to HERO Vec3 -----
	public void setCameraToHero() {
		Vector3 tmpV3 = transform.position;
		tmpV3.z = -10;
		MainCamera.transform.position = tmpV3;
	}

	public void reportingProxy(string arg) {
		Reporting.sendMsg(arg);
	}


	//------------------------------------------------------------ 
	
	//					CONTINOUS LOOP
	
	//------------------------------------------------------------

	
	// Update is called once per frame
	void FixedUpdate () {

		//run damage handler
		if (damageStateOn == true) {
			damageState();
		}

		//run death handler
		if (deathStateOn == true) {
			deathState();
		}

		//STOP until hero collides with reset
		if (isDead == true) {
			return;
		}

		setCameraToHero();

		//update speed
		speedX = getCurrentSpeedX();

		//keys,buttons pushed
		keyboard_input_detection();

		ui_button_down_controller();

		//---- check to allow jump through platforms -------
		eval_platform_colliders();

		setAnimX();

		//flip graphics for left motion
		graphics_mirroring();

	}




	//------------------------------------------------------------ 
	
	//					HERO BEHAVIORS
	
	//------------------------------------------------------------

	public void uiCompleteMission() {
		
		UIsplash.showThenFadeMe ();
		uiThrob.setThrob ();
		return;

	}

	public void external_death() {
		deathStateOn = true;
		isDead = true;
	}

	public void external_damage() {
		damageStateOn = true;
	}

	IEnumerator colorDamage(GameObject obj) {

		//red to white
		LeanTween.color (obj,new Color(255,0,0,255),0.2f);

		yield return new WaitForSeconds (0.2f);

		LeanTween.color (obj,new Color(255,255,255,255),0.2f);

		yield return new WaitForSeconds (0.2f);


		//red to white
		LeanTween.color (obj,new Color(255,0,0,255),0.2f);
		
		yield return new WaitForSeconds (0.2f);
		
		LeanTween.color (obj,new Color(255,255,255,255),0.2f);
		
		yield return new WaitForSeconds (0.2f);

		yield break;

	}

	void damageState() {

		Debug.Log ("damageState()");

		if (deathStateOn == true) {
			return;
		}


		//ching

		ching.GetComponent<Animator> ().SetBool ("playMe", true);

		StartCoroutine (colorDamage (hero_anim_go));
	
		//inflict damage
		health--;

		//check for a pulse
		if (health == 0) {
			LeanTween.alpha (health5.GetComponent<RectTransform>(),0.2f,0.5f);
			deathStateOn = true;
			damageStateOn = false;
			isDead = true;
			return;
		}

		//throw in air
		myRB2D.AddForce (transform.up * 200);
		myRB2D.AddForce (transform.right * -100);

		apr.playDeathAudio ();
		//apr.playHit ();


		switch (health) {

			case 4: 
				LeanTween.alpha (health1.GetComponent<RectTransform>(),0.2f,0.5f);
			break;

			case 3: 
				LeanTween.alpha (health2.GetComponent<RectTransform>(),0.2f,0.5f);
			break;

			case 2: 
				LeanTween.alpha (health3.GetComponent<RectTransform>(),0.2f,0.5f);
			break;

			case 1: 
				LeanTween.alpha (health4.GetComponent<RectTransform>(),0.2f,0.5f);
			break;

			default:
			break;
		
		}

		damageStateOn = false;

	}
		

	void addHealth() {

		switch (health) {

		case 5: 
			LeanTween.alpha (health1.GetComponent<RectTransform>(),1f,0.5f);
			LeanTween.alpha (health2.GetComponent<RectTransform>(),1f,0.5f);
			LeanTween.alpha (health3.GetComponent<RectTransform>(),1f,0.5f);
			LeanTween.alpha (health4.GetComponent<RectTransform>(),1f,0.5f);
			LeanTween.alpha (health5.GetComponent<RectTransform>(),1f,0.5f);
			break;

		case 4: 
			LeanTween.alpha (health2.GetComponent<RectTransform>(),1f,0.5f);
			LeanTween.alpha (health3.GetComponent<RectTransform>(),1f,0.5f);
			LeanTween.alpha (health4.GetComponent<RectTransform>(),1f,0.5f);
			LeanTween.alpha (health5.GetComponent<RectTransform>(),1f,0.5f);
			break;

		case 3: 
			LeanTween.alpha (health3.GetComponent<RectTransform>(),1f,0.5f);
			LeanTween.alpha (health4.GetComponent<RectTransform>(),1f,0.5f);
			LeanTween.alpha (health5.GetComponent<RectTransform>(),1f,0.5f);
			break;

		case 2: 
			LeanTween.alpha (health4.GetComponent<RectTransform>(),1f,0.5f);
			LeanTween.alpha (health5.GetComponent<RectTransform>(),1f,0.5f);
			break;

		case 1: 
			LeanTween.alpha (health5.GetComponent<RectTransform>(),1f,0.5f);
			break;

		default:
			break;

		}
	}

	void deathState() {

		Debug.Log ("deathState()");

		//turn Duke Red
		hero_anim.GetComponent<SpriteRenderer> ().color = new Color(255,0,0,255);

		//throw in air
		myRB2D.AddForce (transform.up * 100);

		//turn off hazard collision
		Physics2D.IgnoreLayerCollision (15,11,true);

		// and turn off platform collision
		Physics2D.IgnoreLayerCollision (10,11,true);

		//and turn off barriers to keep Duke honest
		Physics2D.IgnoreLayerCollision (24,11,true);

		//turn off enemy collision
		Physics2D.IgnoreLayerCollision (13,11,true);

		//turn off treat collision
		Physics2D.IgnoreLayerCollision (17,11,true);

		deathStateOn = false;

	}

	void lazarusState() {

		Debug.Log ("lazarusState()");

		//turn platforms back on
		Physics2D.IgnoreLayerCollision (10,11,false);
		Physics2D.IgnoreLayerCollision (15,11,false);
		Physics2D.IgnoreLayerCollision (24,11,false);
		Physics2D.IgnoreLayerCollision (13,11,false);
		Physics2D.IgnoreLayerCollision (17,11,false);

		//turn red off
		hero_anim.GetComponent<SpriteRenderer> ().color = new Color(255,255,255,255);

		//respawn;
		gameObject.transform.position = og_pos;

		//reset health
		LeanTween.alpha (health1.GetComponent<RectTransform>(),1.0f,0.2f);
		LeanTween.alpha (health2.GetComponent<RectTransform>(),1.0f,0.2f);
		LeanTween.alpha (health3.GetComponent<RectTransform>(),1.0f,0.2f);
		LeanTween.alpha (health4.GetComponent<RectTransform>(),1.0f,0.2f);
		LeanTween.alpha (health5.GetComponent<RectTransform>(),1.0f,0.2f);

		health = 5;

		isDead = false;

	}



	public void attack() {
		
		Debug.Log ("HERO - attack()");
		
		Vector3 tmp = gameObject.transform.position;
		tmp.y += 0.5f;
		
		hero_firing.GetComponent<IHeroAttack>().hero_attack(tmp, isRight);

		hero_anim.SetBool ("Attack", true);

		Reporting.sendMsg ("hero_attack");
		
	}



	void getTreat(GameObject col) {


		TreatLogic treatObj = col.GetComponent<TreatLogic> ();
		tmpTreat = treatObj;

		treatObj.playAudio();
		audio_playEatAudio();
		treatObj.animateMe();//calls destroyMe() on complete
		//treatObj.destroyMe ();

		int tmp = tmpTreat.getHealth();
		int boneTmp = tmpTreat.getTreatCount ();

		if (tmp > 0) {

			if (health < 5) {
				health += tmp;
			}

			addHealth ();
			Reporting.sendMsg ("hero_get_bacon");

		} else {
			Reporting.sendMsg ("hero_get_bone");
		}


		GameManager.gotBone(tmpTreat.name);

		addScore (boneTmp);

		//Invoke("finalGetTreat", 0.1f);

	}

	//give animations/sound time to play
	void finalGetTreat() {

		int tmp = tmpTreat.getHealth();
		int boneTmp = tmpTreat.getTreatCount ();

		if (tmp > 0) {

			if (health < 5) {
				health += tmp;
			}

			addHealth ();
			Reporting.sendMsg ("hero_get_bacon");

		} else {
			Reporting.sendMsg ("hero_get_bone");
		}


		GameManager.gotBone(tmpTreat.name);

		addScore (boneTmp);

	}


	void addScore(int amount) {
		
		//countBones = GameManager.currentLevelData.boneDataObj.Count;

		GameManager.gameData.boneBank += amount;

		countBones = GameManager.gameData.boneBank;

		boneCountText.text = countBones.ToString();

		float tmpW = boneCountBarBG.GetComponent<RectTransform> ().rect.width;
		float tmpIncrement = tmpW / totalBones;
		Rect tmpR = boneCountBarBG.GetComponent<RectTransform> ().rect;

		tmpR.width = tmpIncrement * countBones;

		//calc the width
		boneCountBar.GetComponent<RectTransform> ().sizeDelta = new Vector2 (tmpR.width, tmpR.height);

		
	}

	void audio_playEatAudio() {
		apr.playEatingAudio();
	}

	void audio_playDeathAudio() {
		apr.playDeathAudio();
	}

	void audio_playEnemyKillAudio() {
		apr.playEnemyKillAudio ();
	}
	


	//------------------------------------------------------------ 
	
	//					COLLISIONS
	
	//------------------------------------------------------------

	void OnTriggerEnter2D(Collider2D col) {

		if (col.gameObject.CompareTag (tag_treats)) {
			
			//Debug.Log ("HERO ON TREAT - " + col.gameObject.layer);
			
			getTreat(col.gameObject);
			
		}

	}

	void OnCollisionEnter2D(Collision2D col) {
		
		Debug.Log ("HERO COLLISION DETECTED - " + col.gameObject.name);
		
		//Debug.Log ("OnCollisionEnter2D() - tag_platforms: " + tag_platforms);
		//Debug.Log ("OnCollisionEnter2D() - tag_hazards: " + tag_hazards);
		//Debug.Log ("OnCollisionEnter2D() - tag_scene_reset: " + tag_scene_reset);

		//collisions with platforms
		if (col.gameObject.CompareTag (tag_platforms)) {
			
			//Debug.Log ("HERO ON A PLATFORM! - " + col.collider.gameObject.layer);
			
			isOnGround = true;
			keyJumpDown = false;
			jump_count = 0;
			hero_anim.SetBool ("OnGround",true);
			
		}
		
		
		//collisions with hazards
		if (col.gameObject.CompareTag (tag_hazards)) {
			
			//Debug.Log ("HERO ON HAZARD - " + col.collider.gameObject.layer);

			damageStateOn = true;

			Reporting.sendMsg ("hero_damage_hazard");

			//set death flag and death handler flag
			//deathStateOn = true;
			//isDead = true;

			
		}
		
		//collisions with reset
		if (col.gameObject.CompareTag (tag_scene_reset)) {
			
			//Debug.Log ("HERO ON RESET - " + col.collider.gameObject.layer);

			Reporting.sendMsg ("hero_lazarus");

			lazarusState();
			
		}
		
	}
	
	
	
	//----- jump through platforms logic -----
	void eval_platform_colliders() {
		
		//jumping up
		if (myRB2D.velocity.y > 0) {
			Physics2D.IgnoreLayerCollision(10,11,true);
		} else {
			//falling
			Physics2D.IgnoreLayerCollision(10,11,false);
		}
	}






	//------------------------------------------------------------ 
	
	//					PHYSICS 
	
	//------------------------------------------------------------

	//get current speed
	float getCurrentSpeedX() {
		return Mathf.Round(myRB2D.velocity.x * 1000.0f) /1000.0f;
	}
	
	
	void setAnimX() {
		
		if (speedX < 0 && speedX < -0.1f) {
			hero_anim.SetBool ("Walk",true);
		}  
		
		if (speedX > 0.1f){
			hero_anim.SetBool ("Walk",true);
		}
		
		if (speedX < 0.1f && speedX > -0.1f) {
			hero_anim.SetBool ("Walk",false);
		}
		
	}

	void speedAdjustX() {
		
		//Debug.Log ("speedAdjustX() - speedX: " + speedX);

		myRB2D.AddForce ((transform.right * axis_h) * acceleration);
		
		speedX = getCurrentSpeedX ();
		
		//if we need to accelerate
		if (speedX < -speed_horiz_max || speedX > speed_horiz_max) {
			//Debug.Log ("speedAdjustX() - TOO FAST!");
			Vector2 tmpV2 = myRB2D.velocity;
			tmpV2.x = speed_horiz_max*axis_h;
			myRB2D.velocity = tmpV2;
		}
		
	}


	void speedAdjustY() {
		
		//Debug.Log ("speedAdjustY() - Y force");
		
		myRB2D.AddForce (transform.up * jump_strength);
		
		hero_anim.SetBool ("OnGround",false);
		
		applyYforce = false;
		
	}


	//------ flip hero sprites based on x-axis direction -------
	void graphics_mirroring() {
		
		//manipulate graphics flip for left or right velocity
		Vector3 tmpRscale = transform.localScale;
		Vector3 tmpUIscale = UIsplash.transform.localScale;
		
		if (isRight == false) {
			
			tmpRscale.x = -1;
			tmpUIscale.x = -1;
			
			//left
			transform.localScale = tmpRscale;
			UIsplash.transform.localScale = tmpUIscale;
			
		} else {
			
			tmpRscale.x = 1;
			tmpUIscale.x = 1;
			
			//right (normal)
			transform.localScale = tmpRscale;
			UIsplash.transform.localScale = tmpUIscale;
		}
		
	}



	//------------------------------------------------------------ 

	//					INPUTS & CONTROLS 

	//------------------------------------------------------------

	public void inputsOff() {
		//Debug.Log ("inputsOff()");
		deactivateInputs = true;
	}

	public void inputsOn() {
		//Debug.Log ("inputsOn()");
		deactivateInputs = false;
	}


	public void keyboard_input_detection() {
		
		//detect keys pressed - set values
		if (Input.GetKeyDown("right")) {
			keyRightDown = true;
			isRight = true;
		}
		
		if (Input.GetKeyUp ("right")) {
			keyRightDown = false;
		}
		
		if (Input.GetKeyDown("left")) {
			isRight = false;
			keyLeftDown = true;
		}
		
		if (Input.GetKeyUp ("left")) {
			keyLeftDown = false;
		}
		
		if (Input.GetKeyDown ("space")) {
			keyJumpDown = true;
			Reporting.sendMsg ("hero_jump");
		}
		
		if (Input.GetKeyUp ("space")) {
			keyJumpDown = false;
		}
		
	}


	public void ui_button_down_controller() {

		if (deactivateInputs == true) {
			return;
		}

		
		if (keyJumpDown == false) {
			axis_v = 0;
		}
		
		//if keys down - apply axis values
		if (keyRightDown == true) {

			if (axis_h < 1) {
				axis_h += 0.5f;
			}

			//axis_h = 1;
			isRight = true;
		}
		
		if (keyLeftDown == true) {

			if (axis_h > -1) {
				axis_h -= 0.5f;
			}

			//axis_h = -1;
			isRight = false;
		}
		
		if (keyJumpDown == true) {
			axis_v = 1;
		}
		
		if (keyRightDown == true || keyLeftDown == true) {
			
			//Debug.Log ("movement_controller() - left/right");
			
			speedAdjustX();
			
		}
		
		if (keyJumpDown == true) {
			
			if (jump_count < jump_limit) {

				isOnGround = false;
				
				speedAdjustY();
				
				keyJumpDown = false;
				
				jump_count++;

				Reporting.sendMsg("jump");
			}
			
		}
		
		if (keyFireDown == true) {
			attack ();

			keyFireDown = false;

			Reporting.sendMsg("attack");
		} 
	}


	public void input_external_left_down() {
		keyLeftDown = true;
		Debug.Log ("input_external_left_down()");
	}
	
	public void input_external_left_up() {
		keyLeftDown = false;
	}
	
	public void input_external_right_down() {
		keyRightDown = true;
	}
	
	public void input_external_right_up() {
		keyRightDown = false;
	}
	
	public void input_external_jump_down() {
		keyJumpDown = true;
		Reporting.sendMsg ("hero_jump");
	}
	
	public void input_external_jump_up() {
		keyJumpDown = false;
	}
	
	public void input_external_attack_down() {
		keyFireDown = true;
		Reporting.sendMsg ("hero_attack");
	}
	
	public void input_external_attack_up() {
		keyFireDown = false;
	}


}//end class
