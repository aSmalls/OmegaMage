using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpiker : PT_MonoBehaviour, Enemy{
	[SerializeField]
	private float _touchDamage = 0.5f;

	public float touchDamage{
		get {
			return (_touchDamage); 
		}//end of get
		set { 
			_touchDamage = value; 
		}//end of set
	}//end of touchDamage

	public string typeString{
		get { 
			return (roomXMLString); 
		}//end of get
		set { 
			roomXMLString = value; 
		}//end of set
	}//end of typeString

	public float speed = 5f;
	public string roomXMLString = "{";
	public bool ________________;
	public Vector3 moveDir;
	public Transform characterTrans;

	void Awake(){
		characterTrans = transform.Find("CharacterTrans");
	}//end of Awake()

	void Start(){
		switch (roomXMLString){
			case "^":
				moveDir = Vector3.up;
				break;
			case "v":
				moveDir = Vector3.down;
				break;
			case "{":
				moveDir = Vector3.left;
				break;
			case "}":
				moveDir = Vector3.right;
				break;
		}//end of switch
	}//end of Start()

	void FixedUpdate(){ 
		GetComponent<Rigidbody>().velocity = moveDir * speed;
	}//end of FixedUpdate()

	public void Damage(float amt, ElementType eT, bool damageOverTime = false){
	}//end of Damage(float amt, ElementType eT, bool damageOverTime = false)

	void OnTriggerEnter(Collider other){
		GameObject go = Utils.FindTaggedParent(other.gameObject);
		if (go == null) return; 
		if (go.tag == "Ground"){
			float dot = Vector3.Dot(moveDir, go.transform.position - pos);
			if (dot > 0){
				moveDir *= -1;
			}//end of nested if
		}//end of if
	}//end of OnTriggerEnter(Collider other)
}//end of class