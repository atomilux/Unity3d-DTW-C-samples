using UnityEngine;
using System.Collections;

public class BossSecondaryItem : MonoBehaviour {

	public string armName;

	public GameObject boss;
	private BossMercurial boss_logic;

	public GameObject explosionCluster;
	private ExplosionCluster ec;

	public int itemMaxHealth;
	private int itemActualHealth;

	public int damageAmount;

	public GameObject ui_hit_main;
	private RectTransform ui_hit;

	public GameObject ui_hit_bg;
	private RectTransform ui_bg;
	private int ui_bg_width;

	private bool isDead = false;

	private AudioSource audioRef;

	void Start() {

		ui_hit = ui_hit_main.GetComponent<RectTransform>();
		ui_bg = ui_hit_bg.GetComponent<RectTransform>();
		ui_bg_width = Mathf.RoundToInt(ui_bg.rect.width);

		itemActualHealth = itemMaxHealth;

		boss_logic = boss.GetComponent<BossMercurial> ();

		ec = explosionCluster.GetComponent<ExplosionCluster> ();

		audioRef = gameObject.GetComponent<AudioSource> ();
	}



	public void registerHit() {

		if (isDead) {
			return;
		}

		itemActualHealth -= damageAmount;

		audioRef.Play ();

		//GameManager.log ("registerHit() - itemActualHealth: " + itemActualHealth);

		//check for dead
		if (itemActualHealth <= 0) {

			ec.setCallback (destroyMe);
			ec.runExplosions ();
			isDead = true;
		}

		updateUI();

	}

	private void updateUI() {

		Vector2 tmp = ui_hit.sizeDelta;

		int units = Mathf.RoundToInt(ui_bg_width / itemMaxHealth);

		ui_hit.sizeDelta = new Vector2 (units*itemActualHealth,tmp.y);

	}

	public void destroyMe() {
		boss_logic.armKilled (armName);
		Destroy (gameObject);
	}

}
