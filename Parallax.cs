using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {

	public GameObject[] parallaxObjs;

	float parallaxMultiplier = -0.01f;
	
	void FixedUpdate() {
		setParallax(gameObject.GetComponent<Rigidbody2D>().velocity.x);
	}

	//int direct is either 1,0 or -1
	public void setParallax(float velocityX) {

		//Debug.Log ("setParallax() - velocityX: " + velocityX);

		if (parallaxObjs.Length == 0) {
			return;
		}

		float parallaxMultFinal = parallaxMultiplier;

		if (velocityX == 0) {
			return;
		}

		//invert if negative
		if (velocityX < 0) {
			parallaxMultFinal = -parallaxMultiplier;
			//Debug.Log ("setParallax() - INVERTED: " + parallaxMultFinal);
		}

		for (int i=0; i<parallaxObjs.Length; i++) {

			float mover = (i+1)*parallaxMultFinal;
			
			//Debug.Log ("setParallax() - mover: " + mover);
			
			//get v3 of BG
			Vector3 tmp = parallaxObjs[i].transform.position;
			
			//Debug.Log ("setParallax() - original parallax X: " + tmp.x);
			
			tmp.x += mover;
			
			//Debug.Log ("setParallax() - new parallax X: " + tmp.x);
			
			//reassign V3 and update loc
			parallaxObjs[i].transform.position = tmp;

		}

	}

}
