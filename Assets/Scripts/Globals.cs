using UnityEngine;
using System.Collections;

public class Globals : MonoBehaviour {

	public GameObject			_payer;
	public Material				_spriteListMaterial;

	public static GameObject	player;
	public static Material		spriteLitMaterial;
	public static Enemy			currentBoss;

	// Use this for initialization
	void Awake () {
		player = _payer;
		spriteLitMaterial = _spriteListMaterial;
	}
}
