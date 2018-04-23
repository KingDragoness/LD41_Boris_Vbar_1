using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour {

	public List<Rigidbody> allRbs = new List<Rigidbody>();
	public bool isOkayToDestroy = true;
	public float time = 10f;

	private void Start() {
		for (int x = 0; x < allRbs.Count; x++) {
			allRbs [x].AddForce (transform.forward * 1800);
		}
	}

	void Update() {
		if (isOkayToDestroy) {
			time -= Time.deltaTime;
			if (time < 0) {
				for (int x = 0; x < allRbs.Count; x++) {
					Destroy (allRbs [x]);
				}
			}
		}
	}
}
