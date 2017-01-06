using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float					life = 1500;
	public float					speed = 5f;
	public Vector2					maxSpeed = Vector2.one * 8;
	public Vector2					spawnOffset =  Vector2.one;
	public PolygonSpawnPattern		projectileSpawnPattern;
	public const string				playerBulletTag = "PlayerBullets";
	public const string				playerTag = "Player";
	public AudioClip				playerShotClip;
	public AudioClip[]				soundsList;

	float							maxLife;
	AudioSource						audioSource;
	bool							godMode = false;
	
	[HideInInspector]
	public float		lifePercent {
		get {
			return life / maxLife;
		}
	}

	// Use this for initialization
	void Start () {
		PlayerCollider.player = this;
		maxLife = life;
		tag = playerTag;
		projectileSpawnPattern.attachedGameObject = gameObject;
		projectileSpawnPattern.maxObjects = -1;
		audioSource = GetComponent< AudioSource >();
		audioSource.clip = soundsList[Random.Range(0, soundsList.Length)];
		audioSource.volume = .20f;
		audioSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
		if (Globals.gameWin || Globals.gameOver)
			return ;
		if (projectileSpawnPattern.InstanciateFramePolygons(playerBulletTag))
			audioSource.PlayOneShot(playerShotClip, .05f);

		if (Input.GetKeyDown("g"))
			godMode = !godMode;

		if (!godMode && life <= 0)
			GameOver();
	}

	void	GameOver()
	{
		Globals.gameOver = true;
	}

	void FixedUpdate() {
		if (Globals.gameOver || Globals.gameWin)
			return ;
		Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		input.x = Mathf.Clamp(input.x, -maxSpeed.x, maxSpeed.x);
		input.y = Mathf.Clamp(input.y, -maxSpeed.y, maxSpeed.y);

		transform.position += Vector3.ClampMagnitude(input, 1) * Time.fixedDeltaTime * speed;
		// rbody.AddForce((Vector3)input.normalized * Time.fixedDeltaTime * speed);

		Vector3 mouseDiff = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
		float z = Mathf.Atan2(mouseDiff.x, mouseDiff.y) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0, 0, -z);
	}
}
