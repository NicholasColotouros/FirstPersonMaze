using UnityEngine;
using System.Collections;
using Globals;

/*
 * This script is in charge of defining bullets.
 * That includes their color/type and what happens
 * when they collide with objects.
 * 
 * They will be destroyed upon colliding with anything.
 * They will destroy anything that matches their type,
 * like a blue door if the projectile is blue.
 */
public class BulletScript : MonoBehaviour 
{
	private ProjectileType bulletType;
	private Inventory inventoryScript;

	public ProjectileType getBulletType(){ return bulletType; }

	public void initializeBullet(ProjectileType p, Inventory i)
	{
		bulletType = p;
		Color c = ProjectileHelperFunctions.convertToColor (p);
		this.renderer.material.color = c;

		inventoryScript = i;
	}

	// Destroy the bullet on collision
	// Destroy the door if it hits the corresponding coloured door
	// If the bullet doesn't destroy a door, we've entered a fail state
	void OnCollisionEnter(Collision c1)
	{
		var c = c1.collider;
		if(c.gameObject.tag == bulletType + "Door")
		{
			GameObject.DestroyObject(c.gameObject);
		}

		// If the shot does not destroy a door, a fail state is entered
		else
		{
			inventoryScript.setGameState(GState.Lose);
		}


		// destroy the bullet
		GameObject.DestroyObject(this.gameObject);
	}
}
