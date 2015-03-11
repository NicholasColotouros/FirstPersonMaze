using UnityEngine;
using System.Collections;
using Globals;

// Places secondary rooms
// Secondary rooms are fixed but 
// which ones show up is randomly chosen at runtime
public class RoomPlacer 
{
	DigoutGrid dgrid;
	ProjectileType[] projectileOrder;

	public RoomPlacer(DigoutGrid g, ProjectileType[] order)
	{
		dgrid = g;
		projectileOrder = order;
	}

	public void spawnSecondaryRooms()
	{
		// select the number of secondary rooms from 0 to 2
		int numRooms = Random.Range (0, 3);
		spawnSecondaryRoomsForA (numRooms);

		numRooms = Random.Range (0, 3);
		spawnSecondaryRoomsForB (numRooms);

		numRooms = Random.Range (0, 3);
		spawnSecondaryRoomsForC (numRooms);
	}

	private void spawnSecondaryRoomsForA (int numRooms)
	{
		// spawn the guaranteed secondary room
		dgrid.grid [10, 21].north = ProjectileType.EMPTY;
		dgrid.makeRoom (10, 23);
		dgrid.grid [10, 22].south = projectileOrder [0];
		dgrid.grid [10, 23].projectilePickup = projectileOrder [1];
	}

	private void spawnSecondaryRoomsForB(int numRooms)
	{
		// spawn the guaranteed secondary room
		dgrid.grid [10, 9].south = ProjectileType.EMPTY;
		dgrid.makeRoom (10, 7);
		dgrid.grid [10, 8].north = projectileOrder [1];
		dgrid.grid [10, 7].projectilePickup = projectileOrder [2];

		dgrid.grid [11, 10].state = RoomState.Door;
		dgrid.grid [10, 11].state = RoomState.Door;

		dgrid.grid [11, 10].visited = false;
		dgrid.grid [10, 11].visited = false;


	}

	private void spawnSecondaryRoomsForC(int numRooms)
	{

		// make a path to the guaranteed secondary room
		dgrid.grid [22, 12].east = projectileOrder [2];

		dgrid.grid [23, 12].state = RoomState.SecondaryCorridor;
		dgrid.grid [23, 12].west = ProjectileType.EMPTY;
		dgrid.grid [23, 12].east = ProjectileType.EMPTY;
		dgrid.grid [23, 12].visited = true;
		
		dgrid.grid [24, 12].state = RoomState.SecondaryCorridor;
		dgrid.grid [24, 12].west = ProjectileType.EMPTY;
		dgrid.grid [24, 12].east = ProjectileType.EMPTY;
		dgrid.grid [24, 12].visited = true;

		dgrid.grid [25, 12].state = RoomState.SecondaryCorridor;
		dgrid.grid [25, 12].west = ProjectileType.EMPTY;
		dgrid.grid [25, 12].north = ProjectileType.EMPTY;
		dgrid.grid [25, 12].visited = true;
		
		dgrid.grid [25, 13].state = RoomState.SecondaryCorridor;
		dgrid.grid [25, 13].south = ProjectileType.EMPTY;
		dgrid.grid [25, 13].north = ProjectileType.EMPTY;
		dgrid.grid [25, 13].visited = true;

		dgrid.grid [25, 14].state = RoomState.SecondaryCorridor;
		dgrid.grid [25, 14].south = ProjectileType.EMPTY;
		dgrid.grid [25, 14].north = ProjectileType.EMPTY;
		dgrid.grid [25, 14].visited = true;

		dgrid.grid [25, 15].south = ProjectileType.EMPTY;

	}
}
