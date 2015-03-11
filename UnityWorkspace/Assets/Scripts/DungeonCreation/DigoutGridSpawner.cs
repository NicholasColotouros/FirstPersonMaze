using UnityEngine;
using System.Collections;
using Globals;

// Helper class to spawn the grid once the layout has been generated
// In this class, room means 1x1 square with 4 walls, floor and roof
public class DigoutGridSpawner:MonoBehaviour
{
	public static void spawnGrid(DigoutGrid dGrid, Vector3 origin, Transform roomPrefab, Transform pickupPrefab)
	{
		for(int i = 0; i < 30; i++)
		{
			for(int j = 0; j < 30; j++)
			{
				// If the square is being used, spawn a room there
				if(dGrid.grid[i,j].state != RoomState.Unused)
				{
					Transform spawnedRoom = Instantiate(roomPrefab,
					            new Vector3(origin.x+i*5,origin.y, 1*(origin.z+j*5)), 
					            Quaternion.identity) as Transform;
					spawnedRoom.name = "(" + i + ", "+j+")";

					// Now that the wall is spawned, change the walls according to the grid
					fixSpawnedWalls(dGrid, i, j, spawnedRoom);

					// If there is a projectile pickup to be placed, place it
					ProjectileType pickup = dGrid.grid[i,j].projectilePickup;
					if(pickup != ProjectileType.NONE && pickup != ProjectileType.EMPTY)
					{
						Vector3 roomPosition = spawnedRoom.position;
						Transform spawnedPickup = Instantiate( pickupPrefab,
						                                      new Vector3(roomPosition.x,
						            										roomPosition.y + 1.5f,
						            										roomPosition.z),
						                                      Quaternion.identity
						                                      ) as Transform;
						spawnedPickup.parent = spawnedRoom;
						spawnedPickup.GetComponent<PickupProperties>().setProjectileType(pickup);
					}

					//If it's the entracne, remove the roof/ceiling
					if(i == 1 && j == 16) Destroy(spawnedRoom.FindChild("Roof").gameObject);
				}
			}
		}
	}

	// Removes all unused walls or turns them into walls that can be destroyed by projectiles
	// All as specified by the grid
	public static void fixSpawnedWalls(DigoutGrid grid, int i, int j, Transform spawnedRoom)
	{
		wallHelper (grid.grid [i, j].north, spawnedRoom.FindChild ("N").gameObject);
		wallHelper (grid.grid [i, j].east, spawnedRoom.FindChild ("E").gameObject);
		wallHelper (grid.grid [i, j].south, spawnedRoom.FindChild ("S").gameObject);
		wallHelper (grid.grid [i, j].west, spawnedRoom.FindChild ("W").gameObject);
	}

	// Checks the wall type and performs the appropriate action
	private static void wallHelper(ProjectileType wallType, GameObject wall)
	{
		// If the wall is empty, destroy it
		if(wallType.Equals(ProjectileType.EMPTY)) Destroy(wall);
		else if(wallType.Equals(ProjectileType.NONE)){} // do nothing, it's an ordinary wall
		else // it's a destroyable wall
		{
			Color wallColor = ProjectileHelperFunctions.convertToColor(wallType);
			wall.tag = wallType + "Door";
			wall.renderer.material.color = wallColor;
		}

	}
}
