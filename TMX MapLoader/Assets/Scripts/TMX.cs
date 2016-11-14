using UnityEngine;
using System;
using System.Xml.Serialization;

namespace TMX {

	[Serializable, XmlRoot("map")]
	public class Map {
		[XmlAttribute] public string version;
		[XmlAttribute] public string orientation;
		[XmlAttribute] public string renderorder;
		[XmlAttribute] public int width;
		[XmlAttribute] public int height;
		[XmlAttribute] public int tilewidth;
		[XmlAttribute] public int tileheight;
		[XmlAttribute] public int nextobjectid;

		[XmlElement("tileset")]
		public Tileset[] tilesets;

		[XmlElement("layer")]
		public Layer[] layers;

		[XmlElement("objectgroup")]
		public ObjectGroup[] objectGroups;
	}

	[Serializable]
	public class Tileset {
		[XmlAttribute] public string name;
		[XmlAttribute] public int firstgid;
		[XmlAttribute] public int tilewidth;
		[XmlAttribute] public int tileheight;
		[XmlAttribute] public int tilecount;
		[XmlAttribute] public int columns;

		//- Tileset para arquivos .tsx
		[XmlAttribute] public string source;

		[XmlElement("image")]
		public Image image;

		public object obj = null;

		public string getSource() {
			if (image!=null && image.source!=null)
				return image.source;
			else if (source!=null)
				return source;
			return "";
		}
	}
	
	[Serializable]
	public class Image {
		[XmlAttribute] public string source;
		[XmlAttribute] public int width;
		[XmlAttribute] public int height;
	}

	[Serializable]
	public class Layer {
		[XmlAttribute] public string name;
		[XmlAttribute] public int width;
		[XmlAttribute] public int height;
		[XmlAttribute] public int offsetx;
		[XmlAttribute] public int offsety;

		[XmlElement("data")]
		public Data data;

		public int getId (int x, int y) {
			if (x<0 || y<0 || x>=width || y>=height)
				return 0;
			return data.ids[x + y*width];
		}
	}

	[Serializable]
	public class Data {
		[XmlAttribute] public string encoding;
		[XmlText, HideInInspector] public string text;
		public int[] ids;
	}

	[Serializable]
	public class ObjectGroup {
		[XmlAttribute] public string name;

		[XmlElement("object")]
		public Object[] objects;
	}

	[Serializable]
	public class Object {
		[XmlAttribute] public string name;
		[XmlAttribute] public string type;
		[XmlAttribute] public int id;
		[XmlAttribute] public int gid;
		[XmlAttribute] public int x;
		[XmlAttribute] public int y;
		[XmlAttribute] public int width;
		[XmlAttribute] public int height;
	}

}
