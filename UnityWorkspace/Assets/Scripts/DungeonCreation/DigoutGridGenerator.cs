using UnityEngine;
using System.Collections;
using Globals;

// Master class for generating the dungeon
public class DigoutGridGenerator : MonoBehaviour 
{
	public Transform RoomPrefab;
	public Transform pickupPrefab;

	DigoutGrid dungeonGrid;

	// Use this for initialization
	void Start () 
	{
		Debug.Log ("Grid generation initialized, determining projectile order.");
		ProjectileType[] order = initializeProjectileOrder ();

		Debug.Log ("Projectile Order determined, placing main and secondary rooms.");
		dungeonGrid = new DigoutGrid (order);

		Debug.Log ("Primary and secondary room placement complete, connecting rooms.");
		MainRoomConnector mrc = new MainRoomConnector (dungeonGrid);
		mrc.connectPrimaryRooms ();

		Debug.Log ("Maze complete. Spawning grid.");
		DigoutGridSpawner.spawnGrid (dungeonGrid, gameObject.transform.position, RoomPrefab, pickupPrefab); 
		Debug.Log("Spawning Complete.");
	}

	// Will determine the order that projectiles will appear in
	private ProjectileType[] initializeProjectileOrder()
	{
		ProjectileType[] projectileOrder = new ProjectileType[4];
		int numProjectiles = 3;
		
		// start by filling the array with empty values
		for(int i = 0; i < projectileOrder.Length; i++)
		{
			projectileOrder[i] = ProjectileType.NONE;
		}
		
		// Will take the first 3 ordinal values of ProjectileType (Red, Green, Blue) and insert them into random indexes
		for(int i = 0; i < numProjectiles; i++)
		{
			// Get the next projectile to be placed
			ProjectileType nextProjectile = ProjectileHelperFunctions.projectileOrdinal(i);
			
			// insert it into an unavailable slot. If the randomly chosen slot is taken,
			// Put it into the first available spot
			int insertAtIndex = Random.Range(0, numProjectiles);
			
			if(projectileOrder[insertAtIndex] == ProjectileType.NONE)
			{
				projectileOrder[insertAtIndex] = nextProjectile;
			}
			
			else
			{
				for(int j = 0; j < numProjectiles; j++)
				{
					if(projectileOrder[j] == ProjectileType.NONE)
					{
						projectileOrder[j] = nextProjectile;
						break;
					}
				}
			}
			
		}
		// finally, the yellow projectile (last one)
		if(projectileOrder[numProjectiles] != ProjectileType.NONE) Debug.LogError(projectileOrder[numProjectiles] + " placed at last index for projectile order.");
		projectileOrder [numProjectiles] = ProjectileType.Yellow;
		Debug.Log ("Projectile order: " + projectileOrder[0] + " " + projectileOrder[1] + " " + 
		           projectileOrder[2] + " " + projectileOrder[3] + " ");
		return projectileOrder;
	}
}
