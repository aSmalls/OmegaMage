using UnityEngine;
using System.Collections;

public class Tile : PT_MonoBehaviour {
	public string type;

	private string _tex;
	private int _height = 0;
	private Vector3 _pos;

	public int height{
		get{
			return _height;
		}//end of get
		set{
			_height = value;
			AdjustHeight ();
		}//end of set
	}//end of height

	public string tex{
		get{
			return _tex;
		}//end of get
		set{
			_tex = value;
			name = "TilePrefab_" + _tex;
			Texture2D t2D = LayoutTiles.S.GetTileTex (_tex);
			if (t2D == null) {
				Utils.tr ("Error", "Tile.type(set)=", value, "No matching Texture2D in LayoutTiles.S.tileTextures:");
			}//end of if
			else GetComponent<Renderer>().material.mainTexture = t2D;
		}//end of set
	}//end of tex

	new public Vector3 pos{
		get{
			return _pos;
		}//end of get
		set{
			_pos = value;
			AdjustHeight ();
		}//end of set
	}//end of pos

	public void AdjustHeight(){
		Vector3 vertOffset = Vector3.back * (_height - 0.5f);
		transform.position = _pos + vertOffset;
	}//end of AdjustHeight()
}//end of class