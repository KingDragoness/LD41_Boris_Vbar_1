using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour {

	public float eulerAngleYOpen = 10f;
	public bool isOpen = false;
	private float normalAngleYClosed = 0;

	private void Start() {
		normalAngleYClosed = this.transform.localEulerAngles.y;
	}

	public void OpenDoor() {
		this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, eulerAngleYOpen, this.transform.localEulerAngles.z);
		isOpen = true;
	}

	public void CloseDoor() {
		this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, normalAngleYClosed, this.transform.localEulerAngles.z);
		isOpen = false;
	}
}
