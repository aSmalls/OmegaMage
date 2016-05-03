using UnityEngine;
using System.Collections.Generic;

public class EnemyBug : PT_MonoBehaviour, Enemy{

	[SerializeField]
	private float _touchDamage = 1;
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

	public string roomXMLString;

	public float speed = 0.5f;
	public float health = 10;
	public float damageScale = 0.8f; 
	public float damageScaleDuration = 0.25f;

	public bool ________________;

	private float damageScaleStartTime;
	private float _maxHealth;
	public Vector3 walkTarget;
	public bool walking;
	public Transform characterTrans;

	public Dictionary<ElementType, float> damageDict;

	void Awake(){
		characterTrans = transform.Find("CharacterTrans");
		_maxHealth = health;
		ResetDamageDict();
	}//end of Awake()

	void ResetDamageDict(){
		if (damageDict == null) damageDict = new Dictionary<ElementType, float>();
		
		damageDict.Clear();
		damageDict.Add(ElementType.earth, 0);
		damageDict.Add(ElementType.water, 0);
		damageDict.Add(ElementType.air, 0);
		damageDict.Add(ElementType.fire, 0);
		damageDict.Add(ElementType.aether, 0);
		damageDict.Add(ElementType.none, 0);
	}//end of ResetDamageDict()

	void Update(){
		WalkTo(Mage.S.pos);
	}//end of Update()

	// ---------------- Walking Code ----------------
	public void WalkTo(Vector3 xTarget){
		walkTarget = xTarget; 
		walkTarget.z = 0; 
		walking = true; 
		Face(walkTarget); 
	}//end of WalkTo(Vector3 xTarget)

	public void Face(Vector3 poi){ 
		Vector3 delta = poi - pos; 
		float rZ = Mathf.Rad2Deg * Mathf.Atan2(delta.y, delta.x);
		characterTrans.rotation = Quaternion.Euler(0, 0, rZ);
	}//end of Face(Vector3 poi

	public void StopWalking(){ 
		walking = false;
		GetComponent<Rigidbody>().velocity = Vector3.zero;
	}//end of StopWalking()

	void FixedUpdate(){ 
		if (walking){
			if ((walkTarget - pos).magnitude < speed * Time.fixedDeltaTime){
				pos = walkTarget;
				StopWalking();
			}//end of nested if
			else GetComponent<Rigidbody>().velocity = (walkTarget - pos).normalized * speed;
		}//end of if

		else GetComponent<Rigidbody>().velocity = Vector3.zero;
	}//end of FixedUpdate()
		
	public void Damage(float amt, ElementType eT, bool damageOverTime = false){
		if (damageOverTime) amt *= Time.deltaTime;
		
		switch (eT){
			case ElementType.fire:
				damageDict[eT] = Mathf.Max(amt, damageDict[eT]);
				break;
			case ElementType.air:
				break;
			default:
				damageDict[eT] += amt;
				break;
		}//end of switch
	}//end of Damage(float amt, ElementType eT, bool damageOverTime = false)
		
	void LateUpdate(){
		float dmg = 0;
		foreach (KeyValuePair<ElementType, float> entry in damageDict) dmg += entry.Value;

		if (dmg > 0){ 
			if (characterTrans.localScale == Vector3.one) damageScaleStartTime = Time.time; //**
		}//end of if

		float damU = (Time.time - damageScaleStartTime) / damageScaleDuration;
		damU = Mathf.Min(1, damU);
		float scl = (1 - damU) * damageScale + damU * 1;
		characterTrans.localScale = scl * Vector3.one;

		health -= dmg;
		health = Mathf.Min(_maxHealth, health); 
		ResetDamageDict();

		if (health <= 0) Die();	
	}//end of LateUpdate()

	public void Die(){
		Destroy(gameObject);
	}//end of Die()
}//end of class