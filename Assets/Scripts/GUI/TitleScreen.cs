using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {

	public Text		titleText;
	public Text		startText;

	IEnumerator		TextBlink(Text t)
	{
		while (true)
		{
			t.enabled = true;
			yield return new WaitForSeconds(1f);
			t.enabled = false;
			yield return new WaitForSeconds(.5f);
		}
	}

	// Use this for initialization
	void Start () {
		StartCoroutine(TextBlink(startText));
	}
	
	// Update is called once per frame
	void Update () {
		titleText.transform.localScale *= 1f + Mathf.Sin(Time.timeSinceLevelLoad * 3) / 300f;

		if (Input.GetKeyDown(KeyCode.Return))
			SceneManager.LoadScene("main");
	}
}
