using UnityEngine;
using System.Collections;

public class Globals : MonoBehaviour {

	public GameObject			_payer;
	public Material				_spriteListMaterial;

	public static GameObject		player;
	public static Material			spriteLitMaterial;
	public static Enemy				currentBoss;
	public static PlayerController	playerScript;
	public static bool				gameOver;
	public static bool				gameWin;

	// Use this for initialization
	void Awake () {
		gameOver = false;
		player = _payer;
		spriteLitMaterial = _spriteListMaterial;
		playerScript = player.GetComponent< PlayerController >();
	}
}
