using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyguardAI : MonoBehaviour {

	public GameObject mehMode;
	public GameObject diedMode;
	public ParticleSystem damageParticleSystem;
	public Transform target;
	public bool isDead = false;
	public bool alertMode = false;
	private Rigidbody rb;

	private float HP = 100f;




	void Start() {
		rb = GetComponent<Rigidbody> ();
		target = FindObjectOfType<PlayerFPSCookingGame> ().transform;
	}

	void Update() {
		if (alertMode && !isDead) {
			transform.LookAt (target);
			rb.AddForce (transform.forward * 90);
			rb.AddForce (transform.up * 20);

			if (rb.velocity.magnitude > 7) {
				rb.velocity = rb.velocity.normalized * 7;
			}
		}
		if (HP < 0) {
			Died ();
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag ("damage")) {
			TakeDamage (25);
			GameObject go = other.transform.parent.gameObject;
			Destroy (go);
	
		}
		if (other.CompareTag ("explosion")) {
			TakeDamage (150);
		}
	}

	public void Alerted() {
		alertMode = true;
	}

	public void Died() {
		if (!isDead) {
			_consoleLD41.mainConsole.OnePersonIsDead ();
			isDead = true;
			diedMode.SetActive (true);
			mehMode.SetActive (false);
		}
	}

	public void TakeDamage(float damage) {
		if (!isDead) {
			HP -= damage;
			damageParticleSystem.Play ();
		}

	}


}
