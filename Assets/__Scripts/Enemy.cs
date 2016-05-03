using UnityEngine;
using System.Collections;

public interface Enemy{
	Vector3 pos { 
		get; 
		set; 
	}//end of pos

	float touchDamage { 
		get; 
		set; 
	}//end of touchDamage

	string typeString { 
		get; 
		set; 
	}//end of typeString

	GameObject gameObject { 
		get; 
	}//end of gameObject

	Transform transform { 
		get; 
	}//end of transform
}//end of class