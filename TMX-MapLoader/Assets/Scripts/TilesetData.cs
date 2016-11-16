using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TilesetSprite : IComparable<TilesetSprite> {
	public string source;
	public Sprite[] sprites;

	#region IComparable implementation
	public int CompareTo (TilesetSprite other) {
		return source.CompareTo( other.source );
	}
	#endregion
}

[CreateAssetMenu]
public class TilesetData : ScriptableObject {

	public TilesetSprite[] tilesets;

	[ContextMenu("Sort by source name")]
	void sort () {
		List<TilesetSprite> list = new List<TilesetSprite>( tilesets );
		list.Sort();
		tilesets = list.ToArray();
	}

	//public GameObject obj;
	/*[ContextMenu("funcao")]
	void funcao () {
		TilesetSprites[] tSprites = obj.GetComponents<TilesetSprites>();
		//Debug.Log( tSprites.Length );

		tilesets = new TilesetSprite[ tSprites.Length ];
		for (int i=0; i<tSprites.Length; i++) {
			tilesets[i].source = tSprites[i].filename;
			tilesets[i].sprites = tSprites[i].sprites;
		}
	}*/
}
