using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestAI : MonoBehaviour {

	public GameObject mehMode;
	public GameObject panicMode;
	public GameObject diedMode;
	public ParticleSystem damageParticleSystem;
	public Transform target;
	private bool isPanic = false;
	private bool isAnnoyed = false;
	private bool isDead = false;
	private Rigidbody rb;
	private float rangeOperator = 1;
	private float timeOperator = 1f;
	private float HP = 100f;

	public Transform center;
	public Vector3 axis = Vector3.up;
	public float radius = 2.0f;
	public float radiusSpeed = 0.5f;
	public float rotationSpeed = 80.0f; 

	private float randomValueOfSomething = 0;


	void Start() {
		rb = GetComponent<Rigidbody> ();
		randomValueOfSomething = Random.value;
	}

	void Update() {
		if (!isDead) {
			if (isAnnoyed) {
				transform.LookAt (target);
			}
			if (isPanic) {
				if (randomValueOfSomething < 0.25f){
					transform.RotateAround (center.position, axis, rotationSpeed * Time.deltaTime);
					Vector3 desiredPosition = (transform.position - center.position).normalized * radius + center.position;
					transform.position = Vector3.MoveTowards (transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
				}

			}
		}
		if (HP < 0) {
			Died ();
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag ("damage")) {
			GameObject go = other.gameObject;
			Destroy (go);
			print ("if this appear i give up");
			TakeDamage (25);
		}
		if (other.CompareTag ("explosion")) {
			TakeDamage (150);
		}
	}

	public void Died() {
		if (!isDead) {
			_consoleLD41.mainConsole.OnePersonIsDead ();
			isDead = true;
			diedMode.SetActive (true);
			mehMode.SetActive (false);
			panicMode.SetActive (false);
		}
	}

	public void TakeDamage(float damage) {
		if (!isDead) {
			HP -= damage;
			damageParticleSystem.Play ();
		}

	}

	public void MakeGuestAnnoyed() {
		isAnnoyed = true;
	}

	public void MakeGuestPanic() {
		if (!isDead) {
			isPanic = true;
			PanicmodeActivated ();
		}
	}

	void PanicmodeActivated(){
		if (!isDead) {
			mehMode.SetActive (false);
			panicMode.SetActive (true);
			isPanic = true;
		}
	}
}
