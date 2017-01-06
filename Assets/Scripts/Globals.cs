using UnityEngine;
using System.Collections;

public class Globals : MonoBehaviour {

	public GameObject			_payer;
	public Material				_spriteListMaterial;
	public AudioClip			_explosionClip;

	public static GameObject		player;
	public static Material			spriteLitMaterial;
	public static Enemy				currentBoss;
	public static PlayerController	playerScript;
	public static bool				gameOver;
	public static bool				gameWin;
	public static AudioClip			explosionClip;

	private static AudioSource		playerAudioSource;

	// Use this for initialization
	void Awake () {
		gameOver = false;
		player = _payer;
		spriteLitMaterial = _spriteListMaterial;
		explosionClip = _explosionClip;
		playerScript = player.GetComponent< PlayerController >();
		playerAudioSource = player.GetComponents< AudioSource >()[1];
	}

	public static void PlayExplosion()
	{
		playerAudioSource.Play();
	}
}
