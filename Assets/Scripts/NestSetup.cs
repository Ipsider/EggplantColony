using UnityEngine;
using System.Collections;

public class NestSetup : MonoBehaviour {

	// Component of the agent for rendering purposes.
	private Renderer renderer;

	// Use this for initialization
	void Start () {
	
		// Get the renderer component and set the color of the agent.
		renderer = GetComponent<Renderer> ();
		renderer.material.color = Color.gray;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
