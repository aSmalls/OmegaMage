using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TileTex{
	public string str;
	public Texture2D tex;
}//end of TileTex class

public class LayoutTiles : MonoBehaviour {
	static public LayoutTiles S;

	public TextAsset roomsText;
	public string roomNumber = "0";
	public GameObject tilePrefab;
	public TileTex[] tileTextures;

	public bool ____________________;

	public PT_XMLReader roomsXMLR;
	public PT_XMLHashList roomsXML;
	public Tile[,] tiles;
	public Transform tileAnchor;

	void Awake(){
		S = this;

		GameObject tAnc = new GameObject ("TileAnchor");
		tileAnchor = tAnc.transform;

		roomsXMLR = new PT_XMLReader ();
		roomsXMLR.Parse (roomsText.text);
		roomsXML = roomsXMLR.xml ["xml"] [0] ["room"];

		BuildRoom (roomNumber);
	}//end of Awake()

	public Texture2D GetTileTex(string tStr){
		foreach (TileTex tTex in tileTextures) {
			if (tTex.str == tStr) return tTex.tex;
		}//end of foreach
		return null;
	}//end of GetTileTex(string tStr)

	public void BuildRoom(PT_XMLHashtable room){
		string floorTexStr = room.att ("floor");
		string wallTexStr = room.att ("wall");

		string[] roomRows = room.text.Split ('\n');

		for (int i = 0; i < roomRows.Length; i++) roomRows[i] = roomRows[i].Trim('\t');

		tiles = new Tile[100, 100];

		Tile ti;
		string type, rawType, tileTexStr;
		GameObject go;
		int height;
		float maxY = roomRows.Length - 1;

		for (int y = 0; y < roomRows.Length; y++) {
			for (int x = 0; x < roomRows [y].Length; x++) {
				height = 0;
				tileTexStr = floorTexStr;

				type = rawType = roomRows [y] [x].ToString ();

				switch (rawType) {
				case " ":
				case "_":
					continue;
				case ".":
					break;
				case "|":
					height = 1;
					break;
				default:
					type = ".";
					break;
				}//end of switch

				if (type == ".") tileTexStr = floorTexStr;
				else if (type == "|") tileTexStr = wallTexStr;

				go = Instantiate(tilePrefab) as GameObject;
				ti = go.GetComponent<Tile>();
				ti.transform.parent = tileAnchor;
				ti.pos = new Vector3(x, maxY-y, 0);
				tiles[x,y] = ti;

				ti.type = type;
				ti.height = height;
				ti.tex = tileTexStr;

				if (rawType == type) continue;

				switch (rawType) {
				case "X":
					Mage.S.pos = ti.pos;
					break;
				}//end of switch
			}//end of for x
		}//end of for y
	}//end of BuildRoom(PT_XMLHashtable room)


	public void BuildRoom(string rNumStr) {
		PT_XMLHashtable roomHT = null;
		for (int i=0; i<roomsXML.Count; i++) {
			PT_XMLHashtable ht = roomsXML[i];
			if (ht.att("num") == rNumStr) {
				roomHT = ht;
				break;
			}//end of if
		}//end of for loop
		if (roomHT == null) {
			Utils.tr("ERROR","LayoutTiles.BuildRoom()","Room not found: "+rNumStr);
			return;
		}//end of if
		BuildRoom(roomHT);
	}//end of BuildRoom(string rNumStr)
}//end of LayoutTiles class