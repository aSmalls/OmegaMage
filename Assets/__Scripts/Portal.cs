using UnityEngine;
using System.Collections;
public class Portal : PT_MonoBehaviour{
	public string toRoom;

	public bool justArrived = false;
	void OnTriggerEnter(Collider other){
		if (justArrived) return;

		GameObject go = other.gameObject;
		GameObject goP = Utils.FindTaggedParent(go);

		if (goP != null) go = goP;
		if (go.tag != "Mage") return;

		LayoutTiles.S.BuildRoom(toRoom);
	}//end of OnTriggerEnter(Collider other)

	void OnTriggerExit(Collider other){
		if (other.gameObject.tag == "Mage") justArrived = false;
	}//end of OnTriggerExit(Collider other)
}//end of class