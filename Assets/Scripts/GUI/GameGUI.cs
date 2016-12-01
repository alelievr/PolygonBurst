using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameGUI : MonoBehaviour {

	public Text			bossNameText;
	public Text			pressEnterText;
	public Scrollbar	bossHealthBar;
	public GameObject	gameOver;
	public GameObject	gameWin;
	public Scrollbar	playerHealthBar;
	public GameObject	healthBarWrapper;

	IEnumerator		BlinkRestartText()
	{
		while (true)
		{
			pressEnterText.enabled = false;
			yield return new WaitForSeconds(.5f);
			pressEnterText.enabled = true;
			yield return new WaitForSeconds(.5f);
		}
	}

	// Use this for initialization
	void Start () {
		gameOver.SetActive(false);
		StartCoroutine(BlinkRestartText());
	}
	
	// Update is called once per frame
	void Update () {
		if (Globals.currentBoss != null)
		{
			//update text with boss name
			bossNameText.enabled = true;
			healthBarWrapper.SetActive(true);
			bossNameText.text = Globals.currentBoss.name.ToUpper();
			bossHealthBar.size = Globals.currentBoss.lifePercent;
		}
		else
		{
			bossNameText.enabled = false;
			healthBarWrapper.SetActive(false);
		}
		playerHealthBar.size = Globals.playerScript.lifePercent;
		if (Globals.gameOver)
			gameOver.SetActive(true);
		if (Globals.gameOver && Input.GetKeyDown(KeyCode.Return))
			Application.LoadLevel(Application.loadedLevel);
		if (Globals.gameWin)
			gameWin.SetActive(true);
		if (Globals.gameWin && Input.GetKeyDown(KeyCode.Return))
		{
			
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
