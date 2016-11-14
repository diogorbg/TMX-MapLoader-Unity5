using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public SpriteAnim stand;
	public SpriteAnim walk;

	private Rigidbody2D rigb;
	private SpriteRenderer sRender;

	void Start () {
		rigb = GetComponent<Rigidbody2D>();
		sRender = GetComponentInChildren<SpriteRenderer>();
		//walk = GetComponentInChildren<SpriteAnim>();
	}
	
	void Update () {
		Vector2 dir = Vector2.zero;
		dir.x = Input.GetAxis("Horizontal");
		dir.y = Input.GetAxis("Vertical");
		dir.Normalize();
		rigb.velocity = dir * 5f;
		if (dir.magnitude <= 0.01f) {
			stand.enabled = true;
			stand.dir = walk.dir;
			walk.enabled = false;
		} else {
			stand.enabled = false;
			walk.enabled = true;
			walk.dir = dir;
		}

		sRender.sortingOrder = (int)(-transform.position.y*32f);

		if (Input.GetKeyDown(KeyCode.F1))
			Application.LoadLevel(0);
		if (Input.GetKeyDown(KeyCode.F2))
			Application.LoadLevel(1);
	}
}
