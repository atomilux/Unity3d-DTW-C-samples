using UnityEngine;
using System.Collections;
using CinemaDirector;

public class BossMercurial : MonoBehaviour {

	public GameObject eye;
	public GameObject snowflake_attack;
	public GameObject leftArm;
	public GameObject rightArm;

	public GameObject body_attack;
	public Cutscene cs;

	public GameObject ExplosionObj;
	private ExplosionCluster ec;

	private bool leftArmDead = false;
	private bool rightArmDead = false;

	private bool alreadyDead = false;

	public MissionStatusPanel MissionStatusPan;

	// Use this for initialization
	void Start () {

		ec = ExplosionObj.GetComponent<ExplosionCluster>();
	
		Invoke("checkDead",2f);

	}

	private void checkDead() {
		//did we kill him already?
		if (GameManager.gameData.isMissionComplete("boss_mercurial","bosses")) {
			bossDead ();
			alreadyDead = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
		if (alreadyDead) {
			return;
		}

		//is right and left arm dead?
		if (leftArmDead && rightArmDead) {

			GameManager.gameData.completeMission ("boss_mercurial", "bosses");
			GameManager.gameData.completeMission ("mission_defeatMercurial","gameMissions");
			//bossDead ();
			cs.Play();
			alreadyDead = true;
		}

	}

	public void armKilled(string key) {
		if (key == "leftArm") {
			leftArmKilled ();
		}

		if (key == "rightArm") {
			rightArmKilled ();
		}
	}

	private void leftArmKilled() {
		leftArmDead = true;
	}

	private void rightArmKilled() {
		rightArmDead = true;
	}

	public void bossDead() {

		ec.runExplosions ();

		//remove eye
		Destroy(eye);

		//remove tentacles
		Destroy(leftArm);
		Destroy(rightArm);

		//remove snowflake attacker
		Destroy(snowflake_attack);

		Destroy(body_attack);

		MissionStatusPan.completeMissionUIbyId ("mission_defeatMercurial");

		Reporting.sendMsg ("boss_killed_mercurial");

	}

}
