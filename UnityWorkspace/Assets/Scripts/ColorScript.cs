using UnityEngine;
using System.Collections;

// Used to give powerups and walls colors on spawn
public class ColorScript : MonoBehaviour 
{
	public Color color;

	// Use this for initialization
	void Start () 
	{
		this.renderer.material.color = color;
	}	
}
