using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObject : MonoBehaviour {

	public int itemID;
	public bool isWeapon = false;
	public GameObject notyetDamagedFood;
	public GameObject damagedFood;
	private float timeLeftToCook = 3f;
	private bool isCooked = false;
	private Rigidbody rb;
	private ParticleSystem PS;

	private void Start() {
		rb = GetComponent<Rigidbody> ();
		PS = GetComponentInChildren<ParticleSystem> ();
		PS.gameObject.SetActive (false);
	}

	public void FoodDestroyed() {
		damagedFood.SetActive (true);
		notyetDamagedFood.SetActive (false);

	}

	private void OnTriggerStay(Collider other) {
		if (other.CompareTag ("cookingUnit")) {
			timeLeftToCook -= Time.deltaTime;
		}
	}

	private void Update() {
		if (timeLeftToCook < 0 && !isCooked) {
			rb.AddForce (Vector3.up * 300);
			rb.AddForce (Vector3.right * -150);
			PS.gameObject.SetActive (true);
			PS.Play ();
			if (itemID == 2) {
				itemID = 4;

			}
			if (itemID == 3) {
				itemID = 15;

			}


			if (itemID == 7) {
				_consoleLD41.mainConsole.specialTrigger ();
			}

			isCooked = true;
			print ("cooked. ready to serve.");
		}
	}

	private void OnDisabled(){
		PS.gameObject.SetActive (false);
	}



}
