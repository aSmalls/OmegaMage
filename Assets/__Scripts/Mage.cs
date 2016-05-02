using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum MPhase{
	idle,
	down,
	drag
}//end of enum MPhase

[System.Serializable]
public class MouseInfo{
	public Vector3 loc;
	public Vector3 screenLoc;
	public Ray ray;
	public float time;
	public RaycastHit hitInfo;
	public bool hit;

	public RaycastHit Raycact(){
		hit = Physics.Raycast (ray, out hitInfo);
		return hitInfo;
	}//end of Raycast

	public RaycastHit Raycast(int mask){
		hit = Physics.Raycast (ray, out hitInfo, mask);
		return hitInfo;
	}//end of Raycast(int mask)
}//end of MouseInfo

public class Mage : PT_MonoBehaviour {
	static public Mage S;
	//******** CONTINUE CODE HERE ********//

	void Awake(){
		S = this;
	}//end of a Awake()
}//end of class