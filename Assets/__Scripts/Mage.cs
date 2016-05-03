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
	static public bool DEBUG = true;

	public float mTapTime = 0.1f; //minimum time to be considered a tap
	public float mDragDist = 5; //minimum number of pixes to be considered a drag

	public float activeScreenWidth = 1; //percentage of screen to use 1 = 100%

	public float speed = 2; //movement speed of mage

	public bool __________________;

	public MPhase mPhase = MPhase.idle;
	public List<MouseInfo> mouseInfos = new List<MouseInfo>();

	public bool walking = false;
	public Vector3 walkTarget;
	public Transform charcterTrans;

	void Awake(){
		S = this;
		mPhase = MPhase.idle;
		charcterTrans = transform.Find ("CharacterTrans");
	}//end of a Awake()

	void Update(){
		bool b0Down = Input.GetMouseButtonDown (0);
		bool b0Up = Input.GetMouseButtonUp (0);

		bool inActiveArea = (float)Input.mousePosition.x / Screen.width < activeScreenWidth;

		if (mPhase == MPhase.idle) {
			if (b0Down && inActiveArea) {
				mouseInfos.Clear ();
				AddMouseInfo ();
				if (mouseInfos [0].hit) {
					MouseDown ();
					mPhase = MPhase.down;
				}//end of double nested if
			}//end of nested if
		}//end of if

		if (mPhase == MPhase.down) {
			AddMouseInfo ();
			if (b0Up) {
				MouseTap ();
				mPhase = MPhase.idle;
			}//end of nested if
			else if (Time.time - mouseInfos [0].time > mTapTime) {
				float dragDist = (lastMouseInfo.screenLoc - mouseInfos[0].screenLoc).magnitude;
				if (dragDist >= mDragDist) mPhase = MPhase.drag;
			}//end of nested else if
		}//end of if

		if (mPhase == MPhase.drag) {
			AddMouseInfo ();
			if(b0Up){
				MouseDragUp ();
				mPhase = MPhase.idle;
			}//end of nested if
			else MouseDrag();
		}//end of if
	}//end of Update()

	MouseInfo AddMouseInfo(){
		MouseInfo mInfo = new MouseInfo ();
		mInfo.screenLoc = Input.mousePosition;
		mInfo.loc = Utils.mouseLoc;
		mInfo.ray = Utils.mouseRay;

		mInfo.time = Time.time;
		mInfo.Raycact ();

		if (mouseInfos.Count == 0) mouseInfos.Add (mInfo);
		else {
			float lastTime = mouseInfos [mouseInfos.Count - 1].time;
			if (mInfo.time != lastTime) mouseInfos.Add(mInfo);
		}//end of else
		return(mInfo);
	}//end of AddMouseInfo

	public MouseInfo lastMouseInfo{
		get{
			if(mouseInfos.Count == 0) return null;
			return(mouseInfos[mouseInfos.Count -1 ]);
		}//end of get
	}//end of lastMouseInfo()

	void MouseDown(){
		if (DEBUG) print ("Mage.MouseDown()");
	}//end of MouseDown()

	void MouseTap(){
		if (DEBUG) print ("Mage.MouseTap()");
		WalkTo (lastMouseInfo.loc);
	}//end of MouseTap()

	void MouseDrag(){
		if (DEBUG) print ("Mage.MouseDrag()");
	}//end of MouseDrag()

	void MouseDragUp(){
		if (DEBUG) print ("Mage.MouseUp()");
	}//end of MouseDragUP

	public void WalkTo(Vector3 xTarget){
		walkTarget = xTarget;
		walkTarget.z = 0;
		walking = true;
		Face (walkTarget);
	}//end of WalkTo(Vector3 xTarget)

	public void Face(Vector3 poi){
		Vector3 delta = poi - pos;
		float rZ = Mathf.Rad2Deg * Mathf.Atan2 (delta.y, delta.x);
		charcterTrans.rotation = Quaternion.Euler (0, 0, rZ);
	}//end of Face(Vector 3 poi)

	public void StopWalking(){
		walking = false;
		GetComponent<Rigidbody> ().velocity = Vector3.zero;
	}//end of StopWalking()

	void FixedUpdate(){
		if (walking) {
			if ((walkTarget - pos).magnitude < speed * Time.fixedDeltaTime){
				pos = walkTarget;
				StopWalking ();
			}//end of nested if
			else GetComponent<Rigidbody>().velocity = (walkTarget - pos).normalized * speed;
		}//end of if
		else GetComponent<Rigidbody>().velocity = Vector3.zero;
	}//end of FixedUpdate()

	void OnCollisionEnter(Collision coll){
		GameObject otherGO = coll.gameObject;
		Tile ti = otherGO.GetComponent<Tile> ();
		if (ti != null) {
			if (ti.height > 0) StopWalking();
		}//end of if
	}//end of OnCollisionEnter(Collision coll)
}//end of class