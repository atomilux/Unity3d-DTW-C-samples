using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameDataObj {

	public int boneBank = 0;
	public int bonesSpent = 0;

	public bool introViewed = false;

	public List<string> cutscenesObjAll = new List<string> ();
	public List<string> cutscenesObj = new List<string>();

	public List<string> countriesObjAll = new List<string>();
	public List<string> countriesObj = new List<string>();

	public List<string> paidAccessObjAll = new List<string>();
	public List<string> paidAccessObj = new List<string> ();


	//-------- GAME MISSIONS ---------
	public List<string> gameMissionListObjAll = new List<string>();
	public List<string> gameMissionListActiveObj = new List<string> ();
	public List<string> gameMissionListCompleteObj = new List<string> ();
	public List<string> gameMissionListTitlesObjAll = new List<string>();



	//-------- COUCHISTSAN ---------

	public List<string> couchistanMissionDukePhotosObjAll = new List<string> ();
	public List<string> couchistanMissionDukePhotosObj = new List<string>();

	public List<string> couchistanMissionPaidAreasObjAll = new List<string> ();
	public List<string> couchistanMissionPaidAreasObj = new List<string> ();

	public List<string> couchistanMissionInventionsObjAll = new List<string> ();
	public List<string> couchistanMissionInventionsObj = new List<string>();

	public List<string> couchistanMissionCollectiblesObjAll = new List<string>();
	public List<string> couchistanMissionCollectiblesObj = new List<string>();



	public bool checkCutsceneViewed(string key) {

		switch (key) {
			
		case "mercurial":
			return isMissionComplete ("mercurial", "cutscenes");
			break;

		default:
			return false;
		}

	}


	public int lookupObjCompletion(string key) {

		switch (key) {


		/* ---- COUCHISTAN ---- */

		case "couchistanMissionDukePhotos":
			return calcPercent(couchistanMissionDukePhotosObjAll,couchistanMissionDukePhotosObj);
			break;

		case "couchistanMissionPaidAreas":
			return calcPercent (couchistanMissionPaidAreasObjAll, couchistanMissionPaidAreasObj);
			break;

		case "couchistanMissionInventions":
			return calcPercent(couchistanMissionInventionsObjAll,couchistanMissionInventionsObj);
			break;

		case "couchistanMissionCollectibles":
			return calcPercent (couchistanMissionCollectiblesObjAll, couchistanMissionCollectiblesObj);
			break;

		case "gameMissions":
			return calcPercent (gameMissionListObjAll,gameMissionListCompleteObj);
			break;

		}

		return 0;

	}

	public int calcPercent (List<string> all, List<string> some) {

		float tot = (float)all.Count;
		float act = (float)some.Count;

		if (tot == 0 || act == 0) {
			return 0;
		}

		float raw = (float)act/(float)tot * 100;

		float tmp = Mathf.Round(raw);
		int tmp2 = (int)Mathf.Ceil (tmp);

		//GameManager.log ("calcPercent() - tmp2: " + tmp2);

		return tmp2;
	}

	public bool completeMission(string item, string key) {

		switch (key) {

			case "cutscenes":

				if (cutscenesObjAll.Contains (item)) {
					cutscenesObj.Add (item);
				}

				break;
	
			case "countries":
				if (countriesObjAll.Contains (item)) {
					countriesObj.Add (item);
					return true;
				}
				break;

			case "gameMissions":
				if (gameMissionListObjAll.Contains(item)) {
					gameMissionListCompleteObj.Add (item);
					gameMissionListActiveObj.Remove (item);
					return true;
				}
				break;


			/* ----- COUCHISTAN ------ */

			case "couchistanMissionDukePhotos":
				if (couchistanMissionDukePhotosObjAll.Contains (item)) {
					couchistanMissionDukePhotosObj.Add (item);
					return true;
				}
				break;


			case "couchistanMissionPaidAreas":
				if (couchistanMissionPaidAreasObjAll.Contains (item)) {
					couchistanMissionPaidAreasObj.Add (item);
					return true;
				}
				break;


			case "couchistanMissionInventions":
				if (couchistanMissionInventionsObjAll.Contains (item)) {
					couchistanMissionInventionsObj.Add (item);
					return true;
				}
				break;


			case "couchistanMissionCollectibles":
				if (couchistanMissionCollectiblesObjAll.Contains (item)) {
					couchistanMissionCollectiblesObj.Add (item);
					return true;
				}
				break;

		}

		return false;
	}


	public bool isMissionActive(string item, string key) {

		switch (key) {

			case "gameMissions":

			if (gameMissionListActiveObj.Contains(item)) {
				return true;
			}

			break;

		}

		return false;

	}


	public void setMissionActive(string item, string key) {

		switch (key) {

			case "gameMissions":
			if (gameMissionListObjAll.Contains (item)) {
				gameMissionListActiveObj.Add (item);
			}
			break;
		}

	}


	public bool isMissionComplete(string item, string key) {

		//GameManager.log ("checkForMissionItemCompletion() - item: " + item + " - key: " + key);

		switch (key) {

			case "cutscenes":
				if (cutscenesObj.Contains (item)) {
					return true;
				}

			break;

			case "countries":
				if (countriesObj.Contains (item)) {
					return true;
				}
			break;

			case "gameMissions":
				if (gameMissionListCompleteObj.Contains(item)) {
					return true;
				}
			break;


			/* ----- COUCHISTAN ---- */

			case "couchistanMissionDukePhotos":
				if (couchistanMissionDukePhotosObj.Contains(item)) {
					return true;
				}
			break;

			case "couchistanMissionPaidAreas":
				if (couchistanMissionPaidAreasObj.Contains(item)) {
					return true;
				}
			break;

			case "couchistanMissionInventions":
				if (couchistanMissionInventionsObj.Contains(item)) {
					return true;
				}
			break;

			case "couchistanMissionCollectibles":
				if (couchistanMissionCollectiblesObj.Contains(item)) {
					return true;
				}
			break;
				
		}

		return false;

	}


	public void defaultAchievementsMisssions() {

		gameMissionListObjAll = new List<string>{
			"mission_defeatGeneral",
			"mission_defeatMercurial"
		};

		gameMissionListTitlesObjAll = new List<string> {
			"Defeat Maow's Cat General",
			"Defeat Comrade Mercurial"
		};

		cutscenesObjAll = new List<string>{"intro_game","intro_warroom","intro_windstun","intro_mercurial","intro_maow"};

		countriesObjAll = new List<string>{"couchistan","republicofentertainment", "librarya","bathtubnesia","caabinetbodia","toiletgaria","closetstan","dinnermark"};
		countriesObj = new List<string>{"couchistan"};


		/* -------- COUCHISTAN --------- */
		couchistanMissionDukePhotosObjAll = new List<string>{"L1P1","L1P2"};

		couchistanMissionPaidAreasObjAll = new List<string>{"L1_ATTIC_1","L1_ATTIC_2","L1_OUTSIDE","L1_BASEMENT"};

		couchistanMissionInventionsObjAll = new List<string>{"L1I1"};

		couchistanMissionCollectiblesObjAll = new List<string>{"L1C1","L1C2","L1C3"};

	}

}

