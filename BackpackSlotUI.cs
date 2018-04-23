using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackpackSlotUI : MonoBehaviour {

	public Text nameSlot;
	public Image backpackLogo;
	public int backpackSlot = 0;

	public void ThrowItem() {
		_consoleLD41.mainConsole.RemoveItem (_consoleLD41.mainConsole.backpackSlots [backpackSlot]);
	}
}
