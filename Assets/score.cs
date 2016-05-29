using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class score : MonoBehaviour {
		
	public Text scoreDisplay;

	void OnTriggerEnter(Collider collider) {
		scoreDisplay.text = (int.Parse(scoreDisplay.text) + 1) + "";
	}	
}
