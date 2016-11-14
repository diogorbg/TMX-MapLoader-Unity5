using UnityEngine;
using System.Collections;
using UnityEngine.Events;

// Alternativa para animaçao de sprites.

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnim : MonoBehaviour {
	public string name;
	public bool loop = true;
	public float fps = 12f;

	public Sprite[] spritesUp;
	public Sprite[] spritesLeft;
	public Sprite[] spritesDown;
	public Sprite[] spritesRight;

	public UnityEvent onPlay;

	private SpriteRenderer render;
	private Sprite[] sprites;
	private float tStart = 0f;
	private float tPause = 0f;

	private Vector2 _dir;
	public Vector2 dir {
		get { return _dir; }
		set { setDir(value); }
	}

	void Awake () {
		sprites = spritesDown;
		render = GetComponent<SpriteRenderer>();
		play();
	}

	// toca a animacao a partir do primeiro quadro
	public void play () {
		tStart = Time.time;
		tPause = 0f;
		onPlay.Invoke();
	}

	void Update () {
		if (tStart==0f)
			tStart = Time.time;
		if (tStart!=-2f) // -2f ?
			render.sprite = getSprite();
	}

	/** Seta a direção do sprite. */
	private void setDir (Vector2 dir) {
		if (dir.magnitude <= 0.01f)
			return;
		this._dir = dir;
		if (dir.y>0f) {
			if (dir.x>0.7f)
				sprites = spritesRight;
			else if (dir.x<-0.7f)
				sprites = spritesLeft;
			else
				sprites = spritesUp;
		} else {
			if (dir.x>0.7f)
				sprites = spritesRight;
			else if (dir.x<-0.7f)
				sprites = spritesLeft;
			else
				sprites = spritesDown;
		}
	}

	/** Retorna o sprite selecionado para o time atual. */
	public Sprite getSprite () {
		return sprites[ calcPos() ];
	}

	/** Retorna a posição selecionada para o time atual. */
	public int calcPos () {
		int pos = calcPos2();
		if (loop) {
			return pos % sprites.Length;
		} else {
			if (pos >= sprites.Length)
				return sprites.Length-1;
			return pos;
		}
	}

	private int calcPos2 () {
		float tempo = Time.time - tStart;
		if (tPause>0f)
			tempo = tPause - tStart;
		return (int) (tempo*fps);
	}

	/** calcula o numero de voltas dadas pelo sprite. */
	public float calcLoops () {
		return calcPos2() / sprites.Length;
	}

	/** Retoma o tempo do sprite se o script for reativado. */
	void OnEnable () {
		if (tPause!=0f) {
			tStart += Time.time - tPause;
			tPause = 0f;
		}
	}

	/** pausa o sprite se o script for desativado. */
	void OnDisable () {
		if (tPause==0f)
			tPause = Time.time;
		//render.sprite = getSprite();
	}

}
