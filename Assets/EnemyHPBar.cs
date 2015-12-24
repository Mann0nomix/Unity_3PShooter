using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class EnemyHPBar : MonoBehaviour {
	Slider enemyHpBar;
	// Use this for initialization
	void Start () {
		enemyHpBar = GetComponent<Slider> ();
		enemyHpBar.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
