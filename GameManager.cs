using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class GameManager {

	//public static Object serializedData;

	public static BasicLevelDataSerializable currentLevelData;
	public static string currentLevelKey;
	public static string currentLevelDataFilePath;

	static List<LevelObj> allLevelsData = new List<LevelObj>();

	static LevelObj l1 = new LevelObj();
	static LevelObj l2 = new LevelObj();
	static LevelObj l3 = new LevelObj();

	static string gameDataFilePath = Application.persistentDataPath + "/gameData.dat";
	public static GameDataObj gameData = new GameDataObj();

	static string gameLog;
	static bool logOutput = true;
	public static GameObject debugger;
	public static GameObject debuggerShell;

	static GameObject destroyObj;

	public static PanelPurchase fuckYouUnityPieceOfShitLIAR;

	//UI refs
	static UnityEngine.UI.Text ui_boneCount;


	//----------------- LEVEL SWITCHER -------------------

	public static void switchLevel(string key) {

		UnityEngine.SceneManagement.SceneManager.LoadScene(key);

	}



	//---------------- DEBUGGER / LOG -------------------




	public static void log(string str) {

		//clear command
		if (str == "+_++_+++") {
			gameLog = "";
			debugger.GetComponent<UnityEngine.UI.Text>().text = "";
			return;
		}

		gameLog += str + "\n";
		Debug.Log (str);

		if (logOutput == true) {

			if (debugger) {
				debugger.GetComponent<UnityEngine.UI.Text>().text = gameLog;
				resizeDebugger();

			}
		}
	}


	public static void setRef_DebuggerRefs(GameObject d, GameObject ds) {
		debugger = d;
		debuggerShell = ds;
		logOutput = true;
	}



	public static void resizeDebugger() {

		Vector2 tmpD = debugger.GetComponent<RectTransform> ().sizeDelta;
		Vector2 tmpOD = debuggerShell.GetComponent<RectTransform> ().sizeDelta;

		tmpOD.y = tmpD.y;

		debuggerShell.GetComponent<RectTransform>().sizeDelta = tmpOD;

	}


	public static void setRef_UIboneCount(GameObject g) {

		ui_boneCount = g.GetComponent<UnityEngine.UI.Text> ();

	}


	//---------- INIT/CONFIGS --------------

	//bootstrap with level meta data configs - 1st in sequence
	public static void startup() {

		initLevelDataDefaults ();

		gameData = new GameDataObj ();
		gameData.defaultAchievementsMisssions ();
		loadGameDataObj ();

		loadAllLevelDataFiles ();

	}

	public static void setLevelDataToDefault() {

		currentLevelData = null;
		currentLevelKey = null;
		currentLevelDataFilePath = null;

		allLevelsData = new List<LevelObj>();;

		l1 = new LevelObj();
		l2 = new LevelObj();
		l3 = new LevelObj();

		gameData = new GameDataObj();
		gameData.defaultAchievementsMisssions ();

		gameLog = "";

		initLevelDataDefaults();

		saveAllLevelDataFiles ();

	}

	private static void initLevelDataDefaults() {

		l1.key = "couchistan";
		l1.filePath = Application.persistentDataPath + "/level1info_couchistan.dat";
		l1.data = new BasicLevelDataSerializable();
		l1.data.boneTmpCount = 233;
		l1.data.enemiesTmpCount = 36;

		allLevelsData.Add (l1);


		l2.key = "roe";
		l2.filePath = Application.persistentDataPath + "/level2info_roe.dat";
		l2.data = new BasicLevelDataSerializable();
		l2.data.boneTmpCount = 100;

		allLevelsData.Add (l2);


		l3.key = "librarya";
		l3.filePath = Application.persistentDataPath + "/level3info_librarya.dat";
		l3.data = new BasicLevelDataSerializable();
		l3.data.boneTmpCount = 100;

		allLevelsData.Add (l3);

		//log ("startup() - allLevelsData.count: " + allLevelsData.Count);
	}
		





	public static void initializeLevelItems() {

		//get a master list of bones
		catalogBasicItems ("TREAT", currentLevelData.boneDataObjAll);

		//remove gotten bone
		removeGottenItems("TREAT",currentLevelData.boneDataObj);


		//get master list of enemies
		catalogBasicItems("ENEMY",currentLevelData.enemiesDataObjAll);

		//remove gotten enemies
		removeGottenItems("ENEMY",currentLevelData.enemiesDataObj);


	}





	//---------- SET CURRENT --------------

	//set outside of this file - usually with a level initializer
	public static void setCurrentLevel(string id) {

		foreach (LevelObj i in allLevelsData) {

			if (id == i.key) {

				currentLevelKey = i.key;
				currentLevelDataFilePath = i.filePath;
				currentLevelData = i.data;

			}

		}

		initializeLevelItems ();

	}

	public static void loadCurrentLevelData() {
		loadLevelDataFile (currentLevelKey);
	}

	public static void saveCurrentLevelData() {
		saveLevelDataFile (currentLevelKey);
	}





	//------------ LOADING DATA ---------------

	public static void loadLevelDataFile(string lvl) {

		foreach (LevelObj i in allLevelsData) {

			if (i.key == lvl) {
				
				if (File.Exists (i.filePath)) {
					BinaryFormatter bf = new BinaryFormatter ();	
					FileStream fs = File.Open (i.filePath, FileMode.Open);

					BasicLevelDataSerializable tmp = (BasicLevelDataSerializable)bf.Deserialize (fs);

					i.data = tmp;

					fs.Close ();

				} else {
					//log ("loadLevelDataFile() - file does not exist for: " + lvl);
				}

			}

		}

	}


	public static void loadAllLevelDataFiles() {

		foreach (LevelObj i in allLevelsData) {

			loadLevelDataFile (i.key);

		}

	}


	public static BasicLevelDataSerializable getLevelData(string key) {

		foreach (LevelObj i in allLevelsData) {

			if (i.key == key) {
				return i.data;
			}

		}

		return null;

	}


	//----------- BONES BUY/SELL ----------------


	public static bool buyBones(string type) {

		switch (type) {

			case "bones_100":
				return api_consumable_BUY(type);
				break;

			case "bones_300":
				return api_consumable_BUY(type);
				break;

			case "bones_700":
				return api_consumable_BUY(type);
				break;

			case "bones_1000":
				return api_consumable_BUY(type);
				break;

			case "ad_watched_skippable":
				return api_consumable_BUY(type);
				break;

			case "ad_watched_unskippable":
				return api_consumable_BUY(type);
				break;

		}

		GameManager.log ("GameManager.buyBones() FAILED - type: " + type);

		return false;

	}

	private static bool api_nonconsumable_BUY(string key) {

		//gadget_periscope
		//gadget_stunner

		//character_windstun
		//character_brill
		//character_shaegis

		return false;
	}


	private static bool api_consumable_BUY(string key) {


		switch (key) {

			case "bones_100":
				gameData.boneBank += 100;
				return true;
			break;

			case "bones_300":
				gameData.boneBank += 300;
				return true;
			break;

			case "bones_700":
				gameData.boneBank += 700;
				return true;
			break;

			case "bones_1000":
				gameData.boneBank += 1000;
				return true;
			break;

			case "ad_watched_skippable":
				gameData.boneBank += 10;
				return true;
			break;

			case "ad_watched_unskippable":
				gameData.boneBank += 10;
				return true;
			break;

		}

		log ("GameManager.apiBonesBuy() FAILED - key: " + key);

		return false;

	}


	public static bool spendBones(int amt) {
		
		if (amt <= gameData.boneBank) {
			gameData.boneBank -= amt;
			gameData.bonesSpent += amt;

			//set UI
			ui_boneCount.text = gameData.boneBank.ToString();
			
			return true;
		
		}

		return false;

	}




	//------------ SAVING DATA ---------------


	public static void saveLevelDataFile(string key) {

		//log("saveLevelDataFile() - key: " + key);

		foreach (LevelObj i in allLevelsData) {

			if (key == i.key) {

				//log("saveLevelDataFile() - i.key: " + i.key);

				BinaryFormatter bf = new BinaryFormatter ();

				FileStream fs = File.Open(i.filePath,FileMode.OpenOrCreate);

				bf.Serialize(fs,i.data);
				fs.Close();

			}

		}


		//log("saveLevelDataFile() - gameData saving");

		BinaryFormatter bf2 = new BinaryFormatter ();

		FileStream fs2 = File.Open(gameDataFilePath,FileMode.OpenOrCreate);

		bf2.Serialize(fs2,gameData);
		fs2.Close();
	
	}


	public static void saveAllLevelDataFiles() {

		log("saveAllLevelDataFiles()");

		foreach (LevelObj i in allLevelsData) {
			saveLevelDataFile (i.key);
		}

	}





	//------------- UPDATING LEVEL DATA -------------

	public static void gotBone(string boneId) {
		basicLevelItemActivator (boneId, currentLevelData.boneDataObj, true);
		//gameData.boneBank += 1;
	}

	public static void gotEnemy(string enemyId) {
		basicLevelItemActivator (enemyId, currentLevelData.enemiesDataObj, true);
	}
		

	
	
	///------------- TOOLS - BASIC LEVEL --------------------


	public static int countTotalBones() {

		int totalBones = 0;

		//add up all the bones
		foreach (LevelObj i in allLevelsData) {

			totalBones += i.data.boneDataObj.Count;

		}
		//log ("countTotalBones() - totalBones: " + totalBones);

		return totalBones;
	}

	public static bool basicLevelItemActivator(string key,List<string> dc, bool onOff) {

		//log("basicLevelItemActivator() -  key: " + key); 

		GameObject obj = GameObject.Find(key);

		//turn on - ungotten
		if (onOff) {

			//log("basicLevelItemActivator() -  GOT"); 

			//make sure the key isn't there arleady
			if (dc.Contains(key) == false) {

				//log("basicLevelItemActivator() - ADDING KEY TO ARRAY"); 

				dc.Add(key);

				Object.Destroy(obj);

				return true;
			} else {
				//already there
				return false;
			}


		} else { //turn off - gotten

			//log("basicLevelItemActivator() - NOT GOT"); 

			//make sure the key is there 
			if (dc.Contains(key)) {

				//log("basicLevelItemActivator() - TAKING KEY OUT OF ARRAY"); 

				dc.Remove (key);
				return true;
			} else {
				//no key there
				return false;
			}

		}

		//nothing picked up - failz
	}


	public static bool basicLevelItemCheckActive(string key, List<string> dc) {

		if (dc.Contains(key)) {
			return true;
		} else {
			return false;
		}

	}
	
	public static int getPercentBasicLevelItems(string key, string type) {

		int tot = 1;
		int act = 1;

		foreach (LevelObj i in allLevelsData) {

			if (key == i.key) {

				if (type == "bones") {

					if (i.data.boneDataObjAll.Count == 0) {
						tot = i.data.boneTmpCount;
					} else {
						tot = i.data.boneDataObjAll.Count;
					}

					act = i.data.boneDataObj.Count;
				}

				if (type == "enemies") {

					if (i.data.enemiesDataObjAll.Count == 0) {
						tot = i.data.enemiesTmpCount;
					} else {
						tot = i.data.enemiesDataObjAll.Count;
					}

					act = i.data.enemiesDataObj.Count;

				}

			}

		}

		if (tot == 0 || act == 0) {
			return 0;
		}

		float raw = (float)act/(float)tot * 100;

		float tmp = Mathf.Round(raw);
		int tmp2 = (int)Mathf.Ceil (tmp);

		return tmp2;
		
	}

	public static int getTotalBoneCount() {

		int tmp = 0;

		foreach (LevelObj i in allLevelsData) {

			tmp += i.data.boneDataObj.Count;
		
		}

		return tmp;
	}
	
	
	
	public static void catalogBasicItems(string tag_name, List<string> itemsFoundList) {

		GameObject[] tmp = GameObject.FindGameObjectsWithTag(tag_name);
		
		foreach (GameObject i in tmp) {

			//ENEMY needs parent names as keys
			if (i.CompareTag ("ENEMY")) {

				if (!itemsFoundList.Contains (i.transform.parent.name)) {
					itemsFoundList.Add (i.transform.parent.name);
				}

			} else {

				//treats, etc
				if (!itemsFoundList.Contains (i.name)) {
					itemsFoundList.Add (i.name);
				}

			}
			
		}
		
	}


	public static void removeGottenItems(string tag_name,List<string> itemsGotten) {

		GameObject[] tmp = GameObject.FindGameObjectsWithTag(tag_name);

		//log("removeGottenItems() - count: " + tmp.Length);

		foreach (GameObject i in tmp) {

			//log("removeGottenItems() - i: " + i.name + " - itemsCurrent: " + itemsCurrent.Contains (i.name));

			//ENEMY structure is different
			if (i.CompareTag ("ENEMY")) {

				if (itemsGotten.Contains (i.transform.parent.name)) { 
					Object.Destroy (i.transform.parent.gameObject);
				}

			} else {

				if (itemsGotten.Contains (i.name)) { 
					Object.Destroy (i);
				}

			}
			
		}

	}





	//------------------------ GAME DATA OBJ ---------------------------

	public static void saveGameDataObj() {

		BinaryFormatter bf = new BinaryFormatter ();

		FileStream fs = File.Open(gameDataFilePath,FileMode.OpenOrCreate);

		bf.Serialize(fs,gameData);
		fs.Close();

	}

	public static void loadGameDataObj() {

		if (File.Exists (gameDataFilePath)) {
			BinaryFormatter bf = new BinaryFormatter ();	
			FileStream fs = File.Open(gameDataFilePath, FileMode.Open);

			GameDataObj tmp = (GameDataObj)bf.Deserialize (fs);

			gameData = tmp;

			fs.Close ();
		}

	}


}//end class

//-------- data structures for levels -------------

[System.Serializable]
public class BasicLevelItem { 

	public bool active;

}


[System.Serializable]
public class LevelObj {

	public string key;
	public string filePath;
	public BasicLevelDataSerializable data;

}



