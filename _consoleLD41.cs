using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventoryItem {

	public string nameItem;
	public Sprite spriteItem;
	public bool isWeapon = false;
	//if it is weapon
	public Material weaponTexture;
	public bool isContainer = false;
}

[System.Serializable]
public class BackpackSlot {

	public int itemID = 0;
	public GameObject itemPhysicalObjectTracker;

}

public class _consoleLD41 : MonoBehaviour {

	//MOOD SETTING
	public AudioSource audioPlayer_calm;
	public AudioSource audioPlayer_EDM;
	public AudioSource audioDamage;
	private bool allowTransition = false;
	private bool perma = false;
	//ACTUAL MECHANIC
	public List<InventoryItem> allItems = new List<InventoryItem>();
	public List<BackpackSlot> backpackSlots = new List<BackpackSlot> ();
	public List<GuestAI> everySingleGuest = new List<GuestAI> ();
	public List<BodyguardAI> everySingleBodyguard = new List<BodyguardAI> ();
	//USER INTERFACE
	public List<BackpackSlotUI> myInventorySlot = new List<BackpackSlotUI> ();
	public GameObject tipsDoor;
	public GameObject tipsFood;
	public GameObject tipsCooking;
	public GameObject tipsCrafting;
	public GameObject tipsWashing;
	public GameObject tipsBananaWeapon;
	public GameObject craftingResult;
	public GameObject pauseMenu;
	public GameObject victoryScreen;
	public Text result;
	//REALWORLD
	public GameObject explosion;
	public GameObject prefabExplosion;
	public Transform craftingAreaPoint;
	public Transform washingAreaPoint;
	public List<GameObject> specialItems = new List<GameObject>();
	//COOKING
	public GameObject camera;
	public GameObject mainPlayer;

	public static _consoleLD41 mainConsole;
	private Vector3 originalPlaceholderPosition;
	public CookingCamera cc;
	private PlayerFPSCookingGame pfcg;
	private int enemiesLeft = 0;

	private void Start() {
		 pfcg = FindObjectOfType<PlayerFPSCookingGame> ();
		GuestAI[] guests = FindObjectsOfType<GuestAI> ();
		BodyguardAI[] bodyguards = FindObjectsOfType<BodyguardAI> ();
		originalPlaceholderPosition = pfcg.placeholderPhysicalObject.transform.localPosition;

		mainConsole = this;
		for (int x = 0; x < backpackSlots.Count; x++) {
			myInventorySlot [x].backpackSlot = x;
		}

		foreach (GuestAI gai in guests) {
			everySingleGuest.Add (gai);
		}

		foreach (BodyguardAI bd in bodyguards) {
			everySingleBodyguard.Add (bd);
		}

		enemiesLeft = everySingleBodyguard.Count + everySingleGuest.Count;

		RefreshAllInventory ();
	}


	public void OnePersonIsDead() {
		enemiesLeft -= 1;
	}

	private void RefreshAllInventory() {
		for (int x = 0; x < backpackSlots.Count; x++) {
			int theID = backpackSlots [x].itemID;
			myInventorySlot [x].nameSlot.text = allItems [theID].nameItem;
			myInventorySlot [x].backpackLogo.sprite = allItems [theID].spriteItem;
		}

	}

	public void CookingMode() {
		camera.SetActive (true);
		mainPlayer.SetActive (false);
		pfcg.placeholderPhysicalObject.transform.position = cc.positionPlaceholder.transform.position;
		pfcg.placeholderPhysicalObject.transform.parent = null;
	}

	public void ExitCookingMode() {
		camera.SetActive (false);
		mainPlayer.SetActive (true);

		pfcg.placeholderPhysicalObject.transform.parent = pfcg.playerCamera.transform;
		pfcg.placeholderPhysicalObject.transform.localPosition = originalPlaceholderPosition;
	}

	public BackpackSlot FindingAllPossibleEmptySlot() {
		for (int x = 0; x < backpackSlots.Count; x++) {
			if (backpackSlots [x].itemID == 0) {
				return backpackSlots [x];
			}
		}

		return null;
	}

	public BackpackSlot FindingAllOccupiedSlots() {
		for (int x = 0; x < backpackSlots.Count; x++) {
			if (backpackSlots [x].itemID != 0) {
				return backpackSlots [x];
			}
		}

		return null;
	}

	public void specialTrigger() {
		ExitCookingMode ();
		explosion.SetActive (true);
	}

	public void RegisterItem(BackpackSlot whatBS, int whichID, GameObject theObject) {
		whatBS.itemID = whichID;
		whatBS.itemPhysicalObjectTracker = theObject;
		RefreshAllInventory ();
	}

	public void RemoveItem(BackpackSlot whatBS) {

		if (whatBS.itemID != 0) {
			whatBS.itemPhysicalObjectTracker.transform.parent = null;
			whatBS.itemPhysicalObjectTracker.gameObject.SetActive (true);
			whatBS.itemPhysicalObjectTracker = null;
		}
		whatBS.itemID = 0;
		RefreshAllInventory ();
	}

	public void DeletingItemFromExistance(BackpackSlot whatBS) {
		whatBS.itemID = 0;
		RefreshAllInventory ();
	}

	public void EverySinglePeoplePanic() {
		for (int x = 0; x < everySingleGuest.Count; x++) {
			everySingleGuest [x].MakeGuestPanic ();
		}
		for (int x = 0; x < everySingleBodyguard.Count; x++) {
			everySingleBodyguard [x].Alerted ();
		}
		if (!audioPlayer_EDM.isPlaying) { 
			audioPlayer_EDM.Play ();
		}
		allowTransition = true;

	}


	public void CraftingResult() {
		//CRAFT ITEM 1: gun with banana
		//2: molotov
		//3: bowl with bananas
		int gun = 0;
		int spice = 0;
		int cookedBanana = 0;
		int bottles = 0;
		int jerrycan = 0;
		int bowl = 0;
		int cookedSpice = 0;
		int tastelessBowl = 0;

		for (int x = 0; x < backpackSlots.Count; x++) {
			if (backpackSlots [x].itemID == 1) {
				gun++;
			}
		}

		for (int x = 0; x < backpackSlots.Count; x++) {
			if (backpackSlots [x].itemID == 3) {
				spice++;
			}
		}

		for (int x = 0; x < backpackSlots.Count; x++) {
			if (backpackSlots [x].itemID == 4) {
				cookedBanana++;
			}
		}

		for (int x = 0; x < backpackSlots.Count; x++) {
			if (backpackSlots [x].itemID == 6) {
				bottles++;
			}
		}

		for (int x = 0; x < backpackSlots.Count; x++) {
			if (backpackSlots [x].itemID == 7) {
				jerrycan++;
			}
		}

		for (int x = 0; x < backpackSlots.Count; x++) {
			if (backpackSlots [x].itemID == 9) {
				bowl++;
			}
		}

		for (int x = 0; x < backpackSlots.Count; x++) {
			if (backpackSlots [x].itemID == 14) {
				tastelessBowl++;
			}
		}

		for (int x = 0; x < backpackSlots.Count; x++) {
			if (backpackSlots [x].itemID == 15) {
				cookedSpice++;
			}
		}

		craftingResult.SetActive (true);
		result.text = "Fail!";
		if (gun == 1 && spice == 1 && cookedBanana == 1) {
			
			for (int x = 0; x < backpackSlots.Count; x++) {
				if (backpackSlots [x].itemID == 1) {
					DeletingItemFromExistance (backpackSlots [x]);
					break;
				}
			}

			for (int x = 0; x < backpackSlots.Count; x++) {
				if (backpackSlots [x].itemID == 3) {
					DeletingItemFromExistance (backpackSlots [x]);
					break;
				}
			}

			for (int x = 0; x < backpackSlots.Count; x++) {
				if (backpackSlots [x].itemID == 4) {
					DeletingItemFromExistance (backpackSlots [x]);
					break;
				}
			}
			GameObject o = specialItems [0];
			Instantiate (o, craftingAreaPoint.position, craftingAreaPoint.rotation);
			result.text = "Gun with Banana!";
		}

		if (bottles == 1 && jerrycan == 1) {

			for (int x = 0; x < backpackSlots.Count; x++) {
				if (backpackSlots [x].itemID == 6) {
					DeletingItemFromExistance (backpackSlots [x]);
					break;
				}
			}

			for (int x = 0; x < backpackSlots.Count; x++) {
				if (backpackSlots [x].itemID == 7) {
					DeletingItemFromExistance (backpackSlots [x]);
					break;
				}
			}


			GameObject o = specialItems [1];
			Instantiate (o, craftingAreaPoint.position, craftingAreaPoint.rotation);
			result.text = "Molotov Cocktail!";
		}

		if (bowl == 1 && cookedBanana == 1) {

			for (int x = 0; x < backpackSlots.Count; x++) {
				if (backpackSlots [x].itemID == 4) {
					DeletingItemFromExistance (backpackSlots [x]);
					break;
				}
			}

			for (int x = 0; x < backpackSlots.Count; x++) {
				if (backpackSlots [x].itemID == 9) {
					DeletingItemFromExistance (backpackSlots [x]);
					break;
				}
			}


			GameObject o = specialItems [2];
			Instantiate (o, craftingAreaPoint.position, craftingAreaPoint.rotation);
			result.text = "Bowl with Bananas!";
		}

		if (bowl == 1 && jerrycan == 1) {

			for (int x = 0; x < backpackSlots.Count; x++) {
				if (backpackSlots [x].itemID == 7) {
					DeletingItemFromExistance (backpackSlots [x]);
					break;
				}
			}

			for (int x = 0; x < backpackSlots.Count; x++) {
				if (backpackSlots [x].itemID == 9) {
					DeletingItemFromExistance (backpackSlots [x]);
					break;
				}
			}


			GameObject o = specialItems [3];
			Instantiate (o, craftingAreaPoint.position, craftingAreaPoint.rotation);
			result.text = "Jerry Can Soup!";
		}

		if (tastelessBowl == 1 && cookedSpice == 1) {

			for (int x = 0; x < backpackSlots.Count; x++) {
				if (backpackSlots [x].itemID == 14) {
					DeletingItemFromExistance (backpackSlots [x]);
					break;
				}
			}

			for (int x = 0; x < backpackSlots.Count; x++) {
				if (backpackSlots [x].itemID == 15) {
					DeletingItemFromExistance (backpackSlots [x]);
					break;
				}
			}


			GameObject o = specialItems [4];
			Instantiate (o, craftingAreaPoint.position, craftingAreaPoint.rotation);
			result.text = "Curry Soup!";
		}

	}

	public void FillWaterInsideContainer() {
		for (int x = 0; x < backpackSlots.Count; x++) {
			if (backpackSlots [x].itemID == 6) {
				DeletingItemFromExistance (backpackSlots [x]);
				GameObject o = specialItems [5];
				Instantiate (o, washingAreaPoint.position, washingAreaPoint.rotation);
			}

			if (backpackSlots [x].itemID == 9) {
				DeletingItemFromExistance (backpackSlots [x]);
				GameObject o = specialItems [6];
				Instantiate (o, washingAreaPoint.position, washingAreaPoint.rotation);
			}
		}
	}

	private bool isPause = false;

	private void Update() {
		if (Input.GetKeyUp (KeyCode.Escape)) {
			isPause = !isPause;
		}

		if (enemiesLeft < 1) {
			victoryScreen.SetActive (true);
		}

		if (allowTransition) {
			transitionSong ();
		}

		if (isPause) {
			pauseMenu.SetActive (true);
			Time.timeScale = 0;
			Screen.lockCursor = false;
		} else {
			pauseMenu.SetActive (false);
			Time.timeScale = 1;
			Screen.lockCursor = true;
		}
	}



	private void transitionSong() {
		if (!perma) {
			if (audioPlayer_calm.volume > 0) {
				audioPlayer_calm.volume -= 0.1f * Time.deltaTime;
			}

			if (audioPlayer_EDM.volume < 0.6f) {
				audioPlayer_EDM.volume += 0.1f * Time.deltaTime;
			}

			if (audioPlayer_EDM.volume > 0.6f) {
				audioPlayer_EDM.volume = 0.6f;
			}
			if (audioPlayer_calm.volume < 0.1f) {
				perma = true;
			}
			allowTransition = false;
		}
	}
}
