using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreController : MonoBehaviour {

	private Text text;
	private NestSetup nestSetup;

	// Use this for initialization
	void Start () {
	
		text = GetComponent<Text> ();
		nestSetup = GameObject.FindGameObjectWithTag ("Nest").GetComponent<NestSetup> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		text.text = "Stock: " + nestSetup.score * 4;
	}
}
