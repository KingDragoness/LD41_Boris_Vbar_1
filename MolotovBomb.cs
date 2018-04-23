using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolotovBomb : MonoBehaviour {

	private bool hasExploded = false;
	public bool isBomb = true;

	private void OnTriggerEnter(Collider col) {
		if (isBomb) {
			if (!hasExploded) {
				Instantiate (_consoleLD41.mainConsole.prefabExplosion, transform.position, _consoleLD41.mainConsole.prefabExplosion.transform.rotation);
				hasExploded = true;
			}
		} else {
			Destroy (this.gameObject);
		}
	}
}
