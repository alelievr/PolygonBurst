using UnityEngine;
using System.Collections;

public class BackgroundColor : MonoBehaviour {
	
	Camera	cam;
	Color[]	colorTable = new Color[7];
	int		index;

	IEnumerator		incrementIndex() {
		while (true)
		{
			yield return new WaitForSeconds(5F);
			index++;
		}
	}

	// Use this for initialization
	void Start () {
		index = 0;
		colorTable[0] = new Color(0.5F, 0.7F, 0);
		colorTable[1] = new Color(0.6F, 1, 0.3F);
		colorTable[2] = new Color(1, 1, 0);
		colorTable[3] = new Color(0.4F, 0.8F, 1);
		colorTable[4] = new Color(0.8F, 0.5F, 1);
		colorTable[5] = new Color(1, 0.8F, 0);
		colorTable[6] = new Color(0.2F, 0.5F, 0);
		cam = GetComponent< Camera >();
		StartCoroutine(incrementIndex());
		cam.backgroundColor = colorTable[0];
	}
	
	// Update is called once per frame
	void Update () {
		cam.backgroundColor = Color.Lerp(cam.backgroundColor, colorTable[index + 1], 0.05F);
		if (index == colorTable.Length - 2)
			index = 0;
	}
}
