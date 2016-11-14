using UnityEngine;
using System.Collections;

public class Zoom : MonoBehaviour {

	public float zoom = 2f;
	public const float PPU = 32f; //- pixels per unit

	[ContextMenu("Zoom")]
	public void Awake() {
		// 100f is the PixelPerUnit that you have set on your sprite. Default is 100.
		Camera.main.orthographicSize = (Screen.height / PPU / 2.0f / zoom);
	}

}
