using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingCamera : MonoBehaviour {

	public Transform positionPlaceholder;

	void Update() {
		if (Input.GetKeyUp (KeyCode.Q)) {
			_consoleLD41.mainConsole.ExitCookingMode ();
		}


			
	}
}
