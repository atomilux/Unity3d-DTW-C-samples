using UnityEngine;
using UnityEngine.Analytics;
using System.Collections.Generic;

public static class Reporting {


// Use this for initialization
public static void sendMsg(string key) {

	if (key != "bones_purchased_100" &&
		key != "bones_purchased_300" &&
		key != "bones_purchased_700" &&
		key != "bones_purchased_1000" &&
		key != "bones_ad_10" &&

		key != "hero_get_bone" &&
		key != "hero_get_bacon" &&
		key != "hero_jump" &&
		key != "hero_attack" &&
		key != "hero_lazarus" &&

		key != "hero_damage_cat" &&
		key != "hero_damage_bat" &&
		key != "hero_damage_ant" &&
		key != "hero_damage_hazard" &&
		key != "hero_damage_boss" &&

		key != "gate_open_attic" &&
		key != "gate_open_basement" &&
		key != "gate_open_window" &&

		key != "enemy_killed_cat" &&
		key != "enemy_killed_ant" &&
		key != "enemy_killed_bat" &&

		key != "boss_killed_mercurial" &&
		key != "boss_killed_gencat" &&

		key != "cutscene_warroom_intro" &&
		key != "cutscene_couchistan_intro" &&
		key != "cutscene_couchistan_boss_mercurial_intro" &&
		key != "cutscene_couchistan_boss_mercurial_death" &&
		key != "cutscene_couchistan_boss_gencat_intro" &&
		key != "cutscene_couchistan_boss_gencat_death" &&
		key != "cutscene_couchistan_complete" &&
		key != "attack" && 
		key != "jump"

	) {
		Debug.Log("Analytics ERROR - invalid key: " + key);
		//GameManager.log("Analytics ERROR - invalid key: " + key);
		return;
	}

	Debug.Log ("Analytics - PROCESSING key: " + key);

	int tmpV = 1;

	switch (key) {

		case "bones_purchased_100":
			Analytics.CustomEvent("bones_purchased_100", new Dictionary<string,object> {{"bones_purchased_100",tmpV}});
			break;

		case "bones_purchased_300":
			Analytics.CustomEvent("bones_purchased_300", new Dictionary<string,object> {{"bones_purchased_300",tmpV}});
			break;

		case "bones_purchased_700":
			Analytics.CustomEvent("bones_purchased_700", new Dictionary<string,object> {{"bones_purchased_700",tmpV}});
			break;

		case "bones_purchased_1000":
			Analytics.CustomEvent("bones_purchased_1000", new Dictionary<string,object> {{"bones_purchased_1000",tmpV}});
			break;

		case "bones_ad_10":
			Analytics.CustomEvent ("bones_ad_10", new Dictionary<string,object> { { "bones_ad_10",tmpV } });
			break;



		case "hero_get_bone":
			Analytics.CustomEvent("hero_get_bone", new Dictionary<string,object> {{"hero_get_bone",tmpV}});
			break;

		case "hero_get_bacon":
			Analytics.CustomEvent("hero_get_bacon", new Dictionary<string,object> {{"hero_get_bacon",tmpV}});
			break;

		case "hero_jump":
			Analytics.CustomEvent("hero_jump", new Dictionary<string,object> {{"hero_jump",tmpV}});
			break;

		case "hero_attack":
			Analytics.CustomEvent("hero_attack", new Dictionary<string,object> {{"hero_attack",tmpV}});
			break;

		case "hero_lazarus":
			Analytics.CustomEvent("hero_lazarus", new Dictionary<string,object> {{"hero_lazarus",tmpV}});
			break;



		case "hero_damage_cat":
			Analytics.CustomEvent("hero_damage_cat", new Dictionary<string,object> {{"hero_damage_cat",tmpV}});
			break;

		case "hero_damage_bat":
			Analytics.CustomEvent("hero_damage_bat", new Dictionary<string,object> {{"hero_damage_bat",tmpV}});
			break;

		case "hero_damage_ant":
			Analytics.CustomEvent("hero_damage_ant", new Dictionary<string,object> {{"hero_damage_ant",tmpV}});
			break;

		case "hero_damage_hazard":
			Analytics.CustomEvent("hero_damage_hazard", new Dictionary<string,object> {{"hero_damage_hazard",tmpV}});
			break;

		case "hero_damage_boss":
			Analytics.CustomEvent("hero_damage_boss", new Dictionary<string,object> {{"hero_damage_boss",tmpV}});
			break;



		case "gate_open_attic":
			Analytics.CustomEvent("gate_open_attic", new Dictionary<string,object> {{"gate_open_attic",tmpV}});
			break;

		case "gate_open_basement":
			Analytics.CustomEvent("gate_open_basement", new Dictionary<string,object> {{"gate_open_basement",tmpV}});
			break;

		case "gate_open_window":
			Analytics.CustomEvent("gate_open_window", new Dictionary<string,object> {{"gate_open_window",tmpV}});
			break;



		case "enemy_killed_cat":
			Analytics.CustomEvent("enemy_killed_cat", new Dictionary<string,object> {{"enemy_killed_cat",tmpV}});
			break;

		case "enemy_killed_ant":
			Analytics.CustomEvent("enemy_killed_ant", new Dictionary<string,object> {{"enemy_killed_ant",tmpV}});
			break;

		case "enemy_killed_bat":
			Analytics.CustomEvent("enemy_killed_bat", new Dictionary<string,object> {{"enemy_killed_bat",tmpV}});
			break;



		case "boss_killed_mercurial":
			Analytics.CustomEvent("boss_killed_mercurial", new Dictionary<string,object> {{"boss_killed_mercurial",tmpV}});
			break;

		case "boss_killed_gencat":
			Analytics.CustomEvent("boss_killed_gencat", new Dictionary<string,object> {{"boss_killed_gencat",tmpV}});
			break;




		case "cutscene_warroom_intro":
			Analytics.CustomEvent("cutscene_warroom_intro", new Dictionary<string,object> {{"cutscene_warroom_intro",tmpV}});
			break;

		case "cutscene_couchistan_intro":
			Analytics.CustomEvent("cutscene_couchistan_intro", new Dictionary<string,object> {{"cutscene_couchistan_intro",tmpV}});
			break;

		case "cutscene_couchistan_boss_mercurial_intro":
			Analytics.CustomEvent("cutscene_couchistan_boss_mercurial_intro", new Dictionary<string,object> {{"cutscene_couchistan_boss_mercurial_intro",tmpV}});
			break;

		case "cutscene_couchistan_boss_mercurial_death":
			Analytics.CustomEvent("cutscene_couchistan_boss_mercurial_death", new Dictionary<string,object> {{"cutscene_couchistan_boss_mercurial_death",tmpV}});
			break;

		case "cutscene_couchistan_boss_gencat_intro":
			Analytics.CustomEvent("cutscene_couchistan_boss_gencat_intro", new Dictionary<string,object> {{"cutscene_couchistan_boss_gencat_intro",tmpV}});
			break;

		case "cutscene_couchistan_boss_gencat_death":
			Analytics.CustomEvent("cutscene_couchistan_boss_gencat_death", new Dictionary<string,object> {{"cutscene_couchistan_boss_gencat_death",tmpV}});
			break;

		case "cutscene_couchistan_complete":
			Analytics.CustomEvent("cutscene_couchistan_complete", new Dictionary<string,object> {{"cutscene_couchistan_complete",tmpV}});
			break;


		default:
			break;

		}//end switch

	}//end f

}//end class
