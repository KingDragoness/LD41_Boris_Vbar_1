using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFPSCookingGame : MonoBehaviour {


	//Post Processing
	public UnityEngine.PostProcessing.PostProcessingBehaviour PPB;
	public UnityEngine.PostProcessing.PostProcessingProfile damagedScreen;
	public UnityEngine.PostProcessing.PostProcessingProfile normalScreen;
	public Camera playerCamera;
	public Camera deadCamera;
	public GameObject placeholderPhysicalObject;
	public Transform projectileOut;
	public MeshRenderer weaponRenderer;
	public int currentWeaponSelection;
	private bool isOkayToSwitchOn = false;
	private bool isOkayToTakeItem = false;
	private bool isOkayToCook = false;
	private bool isOkayToCraft = false;
	private bool isOkayToOpenDoor = false;
	private bool isOkayToGetWaterFromSinkWash = false;
	private DoorOpen at;
	private FoodObject fo;
	public List<GameObject> whatProjectiles = new List<GameObject>();

	private float HP = 50;
	private bool isDead = false;
	private int currentWeaponID = 0;

	//0 null
	// 1 banana gun

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag ("explosion")) {
			TakeDamage (999);

		}

		if (other.CompareTag ("bodyguard")) {
			if (other.gameObject.GetComponent ("BodyguardAI") != null) {
				BodyguardAI bgai = other.gameObject.GetComponent<BodyguardAI> ();
				if (bgai.alertMode && !bgai.isDead) {
					TakeDamage (20);
				}
			}

		}
	}


	private void TakeDamage(int howDamage) {
		HP -= howDamage;
		StartCoroutine (RedScreen ());
		_consoleLD41.mainConsole.audioDamage.Play ();
	}

	void FixedUpdate () {
		if (HP < 0) {
			deadCamera.gameObject.SetActive (true);
			deadCamera.gameObject.transform.position = this.transform.position;
			this.gameObject.SetActive (false);
		}
		RaycastHit hit;
		Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out hit)) {
			GameObject objectHit = hit.collider.gameObject;
			float dist = Vector3.Distance(objectHit.transform.position, this.transform.position);



			if (objectHit.CompareTag("door"))
			{
				if (dist < 5)
				{
					if (objectHit.gameObject.GetComponent ("DoorOpen") != null) { 
						at = objectHit.gameObject.GetComponent<DoorOpen> ();
						_consoleLD41.mainConsole.tipsDoor.gameObject.SetActive (true);
						isOkayToOpenDoor = true;
					}

				}
				else
				{
					_consoleLD41.mainConsole.tipsDoor.gameObject.SetActive(false);
					isOkayToOpenDoor = false;
				}
			} else
			{
				_consoleLD41.mainConsole.tipsDoor.gameObject.SetActive(false);
				isOkayToOpenDoor = false;
			}

			if (objectHit.CompareTag("food"))
			{
				if (dist < 2)
				{
					if (objectHit.gameObject.GetComponent ("FoodObject") != null) { 
						fo = objectHit.gameObject.GetComponent<FoodObject> ();
						isOkayToTakeItem = true;
						_consoleLD41.mainConsole.tipsFood.gameObject.SetActive (true);
					}
					
				}
				else
				{
					isOkayToTakeItem = false;
					_consoleLD41.mainConsole.tipsFood.gameObject.SetActive(false);

				}
			} else
			{
				isOkayToTakeItem = false;
				_consoleLD41.mainConsole.tipsFood.gameObject.SetActive(false);

			}


			if (objectHit.CompareTag("cook"))
			{
				if (dist < 4)
				{
						
						_consoleLD41.mainConsole.tipsCooking.gameObject.SetActive (true);
					isOkayToCook = true;
				}
				else
				{
					_consoleLD41.mainConsole.tipsCooking.gameObject.SetActive(false);
					isOkayToCook = false;
				}
			} else
			{

				_consoleLD41.mainConsole.tipsCooking.gameObject.SetActive(false);
				isOkayToCook = false;
			}



			if (objectHit.CompareTag("craftingTable"))
			{
				if (dist < 4)
				{

					_consoleLD41.mainConsole.tipsCrafting.gameObject.SetActive (true);
					isOkayToCraft = true;
				}
				else
				{
					_consoleLD41.mainConsole.tipsCrafting.gameObject.SetActive(false);
					isOkayToCraft = false;
				}
			} else
			{

				_consoleLD41.mainConsole.tipsCrafting.gameObject.SetActive(false);
				isOkayToCraft = false;
			}

			if (objectHit.CompareTag("wash"))
			{
				if (dist < 4)
				{

					_consoleLD41.mainConsole.tipsWashing.gameObject.SetActive (true);
					isOkayToGetWaterFromSinkWash = true;
				}
				else
				{
					_consoleLD41.mainConsole.tipsWashing.gameObject.SetActive(false);
					isOkayToGetWaterFromSinkWash = false;
				}
			} else
			{

				_consoleLD41.mainConsole.tipsWashing.gameObject.SetActive(false);
				isOkayToGetWaterFromSinkWash = false;
			}


		}



		if (isOkayToOpenDoor) {
			if (Input.GetKeyUp (KeyCode.E)) {
				if (at.isOpen) {
					at.CloseDoor ();
				} else {
					at.OpenDoor ();
				}
			}
		}

		if (isOkayToTakeItem) {
			if (Input.GetButtonUp ("Fire1")) {
				if (_consoleLD41.mainConsole.FindingAllPossibleEmptySlot() != null) {
					BackpackSlot BS = _consoleLD41.mainConsole.FindingAllPossibleEmptySlot();
					_consoleLD41.mainConsole.RegisterItem (BS, fo.itemID, fo.gameObject);
					fo.gameObject.transform.position = placeholderPhysicalObject.transform.position;
					fo.gameObject.transform.parent = placeholderPhysicalObject.transform;
					fo.gameObject.SetActive (false);
					fo = null;
				}
			}
		}

		if (isOkayToCook) {
			if (Input.GetKeyUp (KeyCode.E)) {
				_consoleLD41.mainConsole.CookingMode ();
			}
		}

		if (isOkayToCraft) {
			if (Input.GetKeyUp (KeyCode.E)) {
				_consoleLD41.mainConsole.CraftingResult ();
			}
		}

		if (isOkayToGetWaterFromSinkWash) {
			if (Input.GetKeyUp (KeyCode.E)) {
				_consoleLD41.mainConsole.FillWaterInsideContainer ();
			}
		}

		if (currentWeaponID != 0) {
			if (Input.GetButtonUp ("Fire1")) {
				if (currentWeaponID != 1) {
					Instantiate (whatProjectiles [currentWeaponID], projectileOut.position, projectileOut.rotation);
					_consoleLD41.mainConsole.EverySinglePeoplePanic ();
				} else if (currentWeaponID == 1 && !isOkayToTakeItem) {
					//BANANAS SECTION

					int ifThereisBananas = 0;

					for (int x = 0; x < _consoleLD41.mainConsole.backpackSlots.Count; x++) {
						if (_consoleLD41.mainConsole.backpackSlots [x].itemID == 2) {
							ifThereisBananas++;

						}
					}

					if (ifThereisBananas > 0) {
						Instantiate (whatProjectiles [currentWeaponID], projectileOut.position, projectileOut.rotation);
						_consoleLD41.mainConsole.EverySinglePeoplePanic ();

						RequireCertainAmmunition (2);
					}

					//END SECTION
				}
				if (currentWeaponID == 2) {

					RemoveWeapon ();
				}
				if (currentWeaponID == 3) {

					RemoveWeapon ();
				}
				if (currentWeaponID == 4) {

					RemoveWeapon ();
				}
			}
		}
	}

	IEnumerator RedScreen()
	{
		PPB.profile = damagedScreen;
		yield return new WaitForSeconds(0.25f);
		PPB.profile = normalScreen;
		StopCoroutine (RedScreen ());
	}

	void Update() {
		if (Input.GetKeyUp (KeyCode.Alpha1)) {
			currentWeaponSelection = 0;
			CheckIsWeapon (currentWeaponSelection);
		}
		if (Input.GetKeyUp (KeyCode.Alpha2)) {
			currentWeaponSelection = 1;
			CheckIsWeapon (currentWeaponSelection);
		}
		if (Input.GetKeyUp (KeyCode.Alpha3)) {
			currentWeaponSelection = 2;
			CheckIsWeapon (currentWeaponSelection);
		}
		if (Input.GetKeyUp (KeyCode.Alpha4)) {
			currentWeaponSelection = 3;
			CheckIsWeapon (currentWeaponSelection);
		}
			



	}

	private void RequireCertainAmmunition(int whatIDtaken) {
		for (int x = 0; x < _consoleLD41.mainConsole.backpackSlots.Count; x++) {
			if (_consoleLD41.mainConsole.backpackSlots [x].itemID == whatIDtaken) {
				_consoleLD41.mainConsole.DeletingItemFromExistance (_consoleLD41.mainConsole.backpackSlots [x]);
				CheckIsWeapon (currentWeaponSelection);
				return;
			}
		}

	}

	private void RemoveWeapon() {
		_consoleLD41.mainConsole.DeletingItemFromExistance (_consoleLD41.mainConsole.backpackSlots [currentWeaponSelection]);
		CheckIsWeapon (currentWeaponSelection);
	}

	private void CheckIsWeapon(int whichSlot) {
		int ID = _consoleLD41.mainConsole.backpackSlots [whichSlot].itemID;
		print (ID);
		if (_consoleLD41.mainConsole.allItems [ID].isWeapon) {
			weaponRenderer.gameObject.SetActive (true);
			weaponRenderer.material = _consoleLD41.mainConsole.allItems [ID].weaponTexture;
			if (ID == 5) {
				_consoleLD41.mainConsole.EverySinglePeoplePanic ();
				_consoleLD41.mainConsole.tipsBananaWeapon.SetActive (true);
				currentWeaponID = 1;
			}
			if (ID == 7) {
	
				currentWeaponID = 2;
			}
			if (ID == 8) {

				currentWeaponID = 3;
			}
			if (ID == 6) {

				currentWeaponID = 4;
			}
		} else {
			currentWeaponID = 0;
			weaponRenderer.gameObject.SetActive (false);
		}
	}
}
