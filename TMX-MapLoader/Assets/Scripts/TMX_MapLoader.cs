using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TMX_MapLoader : MonoBehaviour {
	#if UNITY_EDITOR
	public Object file;
	#endif

	public string filename;
	public SpriteRenderer tile;
	public GameObject col1, col2;
	public Player player;
	public TMX.Map map;

	private TilesetSprites[] tileSprites;

	void Start () {
		tileSprites = GetComponents<TilesetSprites>();

		loadMap();

		mountTiles();
		setPlayer();
	}

	void loadMap () {
		#if UNITY_EDITOR
		filename = AssetDatabase.GetAssetPath (file);
		#endif

		// Cria uma intancia do XmlSerializer especificando o tipo e namespace.
		XmlSerializer serializer = new XmlSerializer(typeof(TMX.Map));

		// Um FileStream é necessário para ler um documento XML.
		FileStream fs = new FileStream(filename, FileMode.Open);
		XmlReader reader = XmlReader.Create(fs);

		// Declaração do objeto raiz que irá receber a deserialização.
		map = (TMX.Map)serializer.Deserialize(reader);
		fs.Close();

		if(map!=null) {
			foreach (TMX.Layer layer in map.layers) {
				string[] split = layer.data.text.Split(',');
				layer.data.ids = new int[split.Length];
				for (int i=0; i<split.Length; i++) {
					layer.data.ids[i] = int.Parse(split[i]);
				}
				layer.data.text = null;
			}
		}
	}

	void mountTiles () {

		int idColisao = -1;
		foreach (TMX.Tileset tset in map.tilesets) {

			//- Identifica tile de colisao
			if (tset.getSource().EndsWith("colisao.png"))
				idColisao = tset.firstgid;

			//- Identifica o tileset
			foreach (TilesetSprites tSprite in tileSprites) {
				if (tset.getSource().EndsWith(tSprite.filename)) {
					tset.obj = tSprite;
					break;
				}
			}
		}

		//- loop para montar o mapa
		int idTile;
		bool layerColisao = false;
		Vector3 pos = Vector3.zero;
		string sortLayer = "Default";
		foreach (var layer in map.layers) {
			layerColisao = layer.name.StartsWith("Colisao") || layer.name.StartsWith("Collision");

			if (layer.name.StartsWith("Objeto") || layer.name.StartsWith("Fringe")) {
				sortLayer = "Objeto";
				//pos.z = 0f;
			} else if (sortLayer.Equals("Objeto")) {
				sortLayer = "Over";
			}

			GameObject objLayer = new GameObject(layer.name);
			objLayer.transform.parent = transform;

			for (int j=0; j<map.height; j++) {
				for (int i=0; i<map.width; i++) {
					idTile = layer.getId(i, j);
					if (idTile==0)
						continue;
					pos.x = i + (float)layer.offsetx/32f;
					pos.y = -j-1;
					if (!layerColisao) {
						SpriteRenderer newTile = Instantiate(tile, pos, Quaternion.identity) as SpriteRenderer;
						newTile.sprite = getSprite(idTile);
						newTile.sortingOrder = -(int)pos.y*32;
						newTile.sortingLayerName = sortLayer;
						newTile.transform.parent = objLayer.transform;
					} else {
						GameObject obj = null;
						if (idTile==idColisao)
							obj = Instantiate(col1, pos, Quaternion.identity) as GameObject;
						else if (idTile==idColisao+1)
							obj = Instantiate(col2, pos, Quaternion.identity) as GameObject;
						else
							continue;
						obj.transform.parent = objLayer.transform;
					}
				}
			}
			pos.z -= 0.1f;
		}

		tile.gameObject.SetActive(false);
		col1.SetActive(false);
		col2.SetActive(false);
	}

	Sprite getSprite (int id) {
		TMX.Tileset[] tsets = map.tilesets;
		if (id<=0)
			return null;
		for (int i=0; i<tsets.Length; i++) {
			if (i<tsets.Length && tsets[i+1].firstgid<=id) //FIXME bug i+1
				continue;
			return (tsets[i].obj as TilesetSprites).sprites[id - tsets[i].firstgid];
		}
		return null;
	}

	void setPlayer () {
		Vector3 pos = player.transform.position;
		foreach (TMX.ObjectGroup objGroup in map.objectGroups) {
			foreach (TMX.Object obj in objGroup.objects) {
				if (obj.type.Equals("player")) {
					pos.x = obj.x/32;
					pos.y = -obj.y/32;
					player.transform.position = pos;
					return;
				}
			}
		}
	}

}
