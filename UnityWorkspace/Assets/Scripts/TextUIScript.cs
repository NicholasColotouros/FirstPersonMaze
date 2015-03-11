using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Globals;

// Used to display the projectile type on the HUD
// It also shows if the game is in a win or lose state
public class TextUIScript : MonoBehaviour, IObserver
{
	// The text the script is attached to
	public Text text;
	private Inventory inventoryScript;
	
	// Initializes by adding itself as an observer
	void Start () 
	{
		inventoryScript = this.GetComponentInParent<Inventory>();
		inventoryScript.addObserver (this);
	}

	// Observer update method which changes the text.
	public void updateObserver (Inventory pInv)
	{
		ProjectileType pType = pInv.getProjectileType ();
		// No projectile being carried
		if(pType == ProjectileType.NONE)
		{
			text.color = Color.black;
			text.text = "Projectile: NONE";
		}

		// Projectile being carried
		else
		{
			Color color = ProjectileHelperFunctions.convertToColor(pType);
			text.color = color;
			text.text = "Projectile: " + pType;
		}

		// Check for the game state, if the game state is win or lose, only display that
		GState state = pInv.getGameState ();
		if(state != GState.Play)
		{
			if(state == GState.Lose)
			{
				text.text = "YOU LOSE";
				text.color = Color.red;
			}

			// Else win condition satified
			else
			{
				text.text = "YOU WIN";
				text.color = Color.yellow;
			}
		}
	}
}
