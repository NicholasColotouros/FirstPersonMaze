using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Globals; 


// This class takes care of what powerup/projectile the player
// is carrying and is responsible for picking them up/dropping.

// It also takes care of the game state
public class Inventory : MonoBehaviour 
{
	private ProjectileType projectileAcquired;
	private GameObject powerUpPrefab;
	private GState gameState;

	private List<IObserver> observers = new List<IObserver>();

	void Start()
	{
		projectileAcquired = ProjectileType.NONE;
		powerUpPrefab = Resources.Load ("ProjectilePickup") as GameObject;
		gameState = GState.Play;
	}

	// When the player passes through a projectile pickup
	void OnTriggerStay(Collider c)
	{
		// The user needs to be pressing . to pick it up and not have any projectile being carried
		if(Input.GetKeyDown(KeyCode.Period) && projectileAcquired == ProjectileType.NONE)
		{
			// Check that it is indeed a projectile pickup
			if(c.gameObject.tag == "ProjectilePickup")
			{
				// Get the type of projectile and make that the currently pickup the projectile
				ProjectileType pickupType = c.gameObject.GetComponent<PickupProperties>().getProjectileType();
				projectileAcquired = pickupType;

				Destroy(c.gameObject);
				notifyObservers(); // notify observers that there is a new projectile being carried
			}
		}
	}

	// Drop the projectile if comma is pressed and a projectile is contained
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Comma) && projectileAcquired != ProjectileType.NONE)
		{
			ProjectileType powerupTypeToDrop = useProjectile();
			Vector3 dropPosition = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
			GameObject drop = Instantiate(powerUpPrefab, dropPosition, Quaternion.identity) as GameObject;
			drop.GetComponent<PickupProperties>().setProjectileType(powerupTypeToDrop);
			notifyObservers();
		}
	}

	// Used when firing the projectile. It returns the projectile type and consumes it
	// (ie sets the projectile in the inventory to null).
	public ProjectileType useProjectile()
	{
		ProjectileType ret = projectileAcquired;
		projectileAcquired = ProjectileType.NONE;
		notifyObservers ();
		return ret;
	}

	// getters
	public ProjectileType getProjectileType(){ return projectileAcquired; }
	public GState getGameState(){ return gameState; }

	// setter
	public void setGameState(GState newState)
	{
		gameState = newState;
		notifyObservers();
	}

	// Add observer also updates it upon addition so that the element knows
	// the current information.
	public void addObserver(IObserver o)
	{
		observers.Add (o);
		o.updateObserver (this);
	}

	private void notifyObservers()
	{
		foreach(IObserver o in observers)
		{
			o.updateObserver(this);
		}
	}
}

// Observer interface for the UI
public interface IObserver
{
	void updateObserver (Inventory pInv);
}