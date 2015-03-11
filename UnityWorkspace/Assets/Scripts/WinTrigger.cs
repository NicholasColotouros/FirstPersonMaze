using UnityEngine;
using System.Collections;

// When the player enters, a win is triggered
public class WinTrigger : MonoBehaviour 
{
	void OnTriggerEnter (Collider c)
	{
		// If the player enters, it's a win
		if(c.collider.tag == "Player")
		{
			GameObject player = GameObject.Find("Player");
			player.GetComponent<Inventory>().setGameState(Globals.GState.Win);
		}
	}
}
