using UnityEngine;
using System.Collections;

public class WorldMapCountry : MonoBehaviour {

	public GameObject worldCam;

	private bool countryLocked = true;

	public string countryKey;

	public GameObject countryCamLoc;
	public GameObject countryMapLoc;
	public GameObject countryPanel;
	public GameObject countryMap;
	public GameObject countryDeployButton;

	public GameObject countryDetails;
	private float countryDetailsLocShow;
	private float countryDetailsLocHidden;


	private Vector3 countryMapOriginalLoc;
	private Vector3 countryMapOriginalRot;
		
	public Material countryMatOn;
	public Material countryMatLocked;

	public string data_key;

	private BasicLevelDataSerializable country_data;

	public int boneUnlockCount;

	public GameObject countryCompletedBones;
	public GameObject countryCompletedMissions;
	public GameObject countryCompletedSecrets;
	public GameObject countryCompletedBosses;

	public GameObject unlockButton;
	public GameObject unlockButtonText;

	private UnityEngine.UI.Text ccBones;
	private UnityEngine.UI.Text ccMissions;
	private UnityEngine.UI.Text ccSecrets;
	private UnityEngine.UI.Text ccBosses;

	public GameObject UI_boneCount;
	private UnityEngine.UI.Text UI_boneCountObj;

	public GameObject purchasePanel;
	private PanelPurchase purchPan;

	public SceneLOADER loadingPanel;

	// Use this for initialization
	void Start () {

		//set references
		countryMapOriginalLoc = countryPanel.transform.position;
		countryMapOriginalRot = countryPanel.transform.eulerAngles;

		countryDeployButton.GetComponent<RectTransform>().transform.localScale = new Vector3 (0, 0, 0);

		countryDetailsLocShow = countryDetails.transform.localPosition.x;
		countryDetailsLocHidden = countryDetailsLocShow + 8500;

		Vector3 tmp = countryDetails.transform.localPosition;
		tmp.x = countryDetailsLocHidden;

		countryDetails.transform.localPosition = tmp;

		ccBones = countryCompletedBones.GetComponent<UnityEngine.UI.Text> ();

		LeanTween.scale (countryDeployButton.GetComponent<RectTransform>(), new Vector3(0,0,0),1.0f);

		//data
		country_data = GameManager.getLevelData(data_key);

		//logic
		countryLocked = !unlockedCheck ();

		ui_setCountryState();

		ui_lockButtonOff();

		//GameManager.log ("country lock: " + countryLocked);

		UI_boneCountObj = UI_boneCount.GetComponent<UnityEngine.UI.Text> ();

		purchPan = purchasePanel.GetComponent<PanelPurchase> ();


	}



	//------------ CONTROLS --------------



	public void tap_openCountry() {

		bool tmp = GameManager.spendBones (boneUnlockCount);

		//GameManager.log ("tap_openCountry() - tmp: " + tmp);

		if (tmp) {

			if (GameManager.gameData.completeMission (countryKey, "countries")) {

				countryLocked = false;
				//ui_unlockCountry ();
				ui_lockButtonOff ();

				UI_boneCountObj.text = GameManager.gameData.boneBank.ToString ();

			} else {
				GameManager.log ("ERRROR");
			
			}

		} else {
			purchPan.buttonLinkClick (ui_setCountryState);
			purchPan.moveToOn ();
		}

	}



	//------------ DATA CHECKS --------------

	private bool unlockedCheck() {

		bool tmp = false;

		if (GameManager.gameData.isMissionComplete (countryKey, "countries")) {
			tmp = true;
		}
		return tmp;
	}

	private bool boneCountEnoughCheck() {
		if (GameManager.gameData.boneBank >= boneUnlockCount) {
			return true;
		}
		return false;
	}

	private int calcBonesNeeded() {

		int tmp = GameManager.gameData.boneBank - boneUnlockCount;

		//if we have enough left for a purchase
		if (tmp > 0) {
			return 0;
		} else {
			
			//we went negatory
			return tmp * -1;//invert for remainder

		}

	}

	//---------- UI ---------

	public void ui_setCountryState() {

		//1. first check for the unlock in the game data and we're done
		if (!countryLocked) {
			//ui_unlockCountry ();
			ui_lockButtonOff ();

			return;
			//done
		}

		//2. if not, and we don't have enough bones, lockERDOWN!
		if (!boneCountEnoughCheck ()) {
			//ui_lockCountry();

			//string c = calcBonesNeeded().ToString ();

			setUnlockButtonText (boneUnlockCount.ToString());

			ui_lockButtonOn ();

		} 

		//3. if not, and WE DO have enough bones but just haven't boubght yet...
		if (boneCountEnoughCheck ()) {
			setUnlockButtonText (boneUnlockCount.ToString());

			ui_lockButtonOn ();
		}

	}



	private void setUnlockButtonText(string a) {
		unlockButtonText.GetComponent<UnityEngine.UI.Text> ().text = a;
	}

	private void ui_lockButtonOn() {
		unlockButton.transform.localScale = new Vector3 (1,1,1);
		countryDeployButton.transform.localScale = new Vector3 (0,0,0);
	}

	private void ui_lockButtonOff() {
		unlockButton.transform.localScale = new Vector3 (0,0,0);
		countryDeployButton.transform.localScale = new Vector3 (1, 1, 1);
	}

	public void setCountryRenderer(bool toggle) {

		countryLocked = toggle;

		if (toggle) {
			countryMap.GetComponent<Renderer>().material = countryMatLocked ;
		} else {
			countryMap.GetComponent<Renderer>().material = countryMatOn;
		}

	}

	public void setCountryActive(bool toggle) {
		//GameManager.log ("countryActive() - toggle: " + toggle);
		
		if (toggle) {

			LeanTween.move(countryPanel,countryMapLoc.transform.position,2.0f).setEase(LeanTweenType.easeOutQuad);
			LeanTween.rotateX(countryPanel,countryMapLoc.transform.eulerAngles.x,2.0f).setEase(LeanTweenType.easeOutQuad);
			
			LeanTween.move(worldCam,countryCamLoc.transform.position,2.0f).setEase(LeanTweenType.easeOutQuad);
			LeanTween.rotateX(worldCam,countryCamLoc.transform.eulerAngles.x,2.0f).setEase(LeanTweenType.easeOutQuad);

			LeanTween.moveLocalX (countryDetails,countryDetailsLocShow,1.0f).setEase (LeanTweenType.easeOutQuad);

			if (countryLocked == false) {
				LeanTween.scale (countryDeployButton.GetComponent<RectTransform>(), new Vector3(1,1,1),1.0f);
			}

		} else {

			LeanTween.move(countryPanel,countryMapOriginalLoc,2.0f).setEase(LeanTweenType.easeOutQuad);
			LeanTween.rotateX(countryPanel,countryMapOriginalRot.x,2.0f).setEase(LeanTweenType.easeOutQuad);

			LeanTween.moveLocalX (countryDetails,countryDetailsLocHidden,1.0f);

			if (countryLocked == false) {
				LeanTween.scale (countryDeployButton.GetComponent<RectTransform>(), new Vector3(0,0,0),1.0f);
			}
			
		}
		
	}

}
