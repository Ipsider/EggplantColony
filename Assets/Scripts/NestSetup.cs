using UnityEngine;
using System.Collections;

public class NestSetup : MonoBehaviour {

	// Component of the agent for rendering purposes.
	private Renderer myRenderer;

	// Amount of food in the nest.
	public float score;

	// Use this for initialization
	void Start () {
	
		// Get the renderer component and set the color of the agent.
		myRenderer = GetComponent<Renderer> ();
		myRenderer.material.color = new Color (0.81f, 0.11f, 0.37f);
	}
	
	// Update is called once per frame
	void Update () {
	
		Debug.Log (score * 4);
	}
}
