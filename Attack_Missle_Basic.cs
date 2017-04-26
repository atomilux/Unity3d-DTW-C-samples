using UnityEngine;
using System.Collections;

public class Attack_Missle_Basic : MonoBehaviour, IMissle {

	//------ CONFIGS ---------
	private bool isRight = true;
	private int speed = 300;
	//private string collide_id = "ENEMY";

	private Rigidbody2D myRB;

	public int strength = 1;
	

	void Awake() {
		myRB = gameObject.GetComponent<Rigidbody2D>();
		Physics2D.IgnoreLayerCollision (11, 14, true);
	}

	public void setRight(bool val) { 
		isRight = val;
	}

	public void setPos(Vector3 pos) {
		transform.position = pos;
	}

	public void useTheForce() {
		AddForce ();
	} 

	public int getAttackStrength() {
		return strength;
	}

	public void setAttackStrength(int amt) {
		strength = amt;
	}
	
	public void AddForce() {

		if (isRight == true) {
			myRB.AddForce (transform.right * speed);
		}

		if (isRight == false) {
			myRB.AddForce (-transform.right * speed);

			Vector3 tmpV3 = transform.localScale;
			tmpV3.x = -1;

			gameObject.transform.localScale = tmpV3;
		}

	}


	void OnCollisionEnter2D(Collision2D col) {

		Debug.Log ("Attack Missle Collision");

		Destroy (gameObject.GetComponent<SpriteRenderer>());
		Destroy (gameObject.GetComponent<Rigidbody2D> ());
		Destroy (gameObject.GetComponent<CircleCollider2D>());

		Invoke ("DestroyMe", 1);

	}

	void DestroyMe() {
		Destroy (gameObject);
	}
}
