using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameGUI : MonoBehaviour {

	public Text			bossNameText;
	public Scrollbar	healthBar;
	public GameObject	healthBarWrapper;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Globals.currentBoss != null)
		{
			//update text with boss name
			bossNameText.enabled = true;
			healthBarWrapper.SetActive(true);
			bossNameText.text = Globals.currentBoss.name.ToUpper();
			healthBar.size = Globals.currentBoss.lifePercent;
		}
		else
		{
			bossNameText.enabled = false;
			healthBarWrapper.SetActive(false);
		}
	}
}
