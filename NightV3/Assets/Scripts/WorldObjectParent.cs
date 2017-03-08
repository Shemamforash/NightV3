using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectParent : MonoBehaviour {
	protected MenuNavigator menu_navigator;

	public virtual void Init () {
		menu_navigator = Camera.main.GetComponent<MenuNavigator>();
	}
}
