using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HpBarScript : MonoBehaviour {
	private Slider playerHealthBar;
	private Text playerRatio;
	// Use this for initialization
	void Start () {
		playerHealthBar = this.GetComponent<Slider>();
		playerRatio = GetComponentInChildren<Text> ();

		playerHealthBar.enabled = false;
		playerHealthBar.onValueChanged.AddListener (updatePlayerRatio);
	}

	// Update is called once per frame
	void Update () {
		playerHealthBar.value -= 100 * Time.deltaTime;
		System.Console.WriteLine ("The time is :" + Time.deltaTime);
	}

	void updatePlayerRatio(float value){
		playerRatio.text = playerHealthBar.value + "/" + playerHealthBar.maxValue;
	}
}
