using UnityEngine;
using System.Collections;
using Globals;

// Data structure used to represent the dungeon layout
// Places the main rooms in the grid and secondary room leading into the exit room

// Start room location:
// 	(0, 15)	(1, 15)	(2,15)
// 	(0, 16)	(1, 16)	(2,16)
// 	(0, 17)	(1, 17)	(2,17)
// With openings at (1, 15) (2, 16) (1, 17)

// end room location (a secondary room to room C):
// 	(27, 15)	(28, 15)	(29,15)
// 	(27, 16)	(28, 16)	(29,16)
// 	(27, 17)	(28, 17)	(29,17)
// With opening at (27,16) -- yellow door

// Primary room A has center (10, 20)

// Primary room B has center (10, 10)

// Primary room C has center (21, 12)

// A Secondary room C has center (25,16) which leads to the exit room. This room is guaranteed to spawn all the time

public class DigoutGrid 
{
	public Room[,] grid;
	public ProjectileType[] projectileOrder;

	// Constructor: initializes the grid with the start and end rooms
	public DigoutGrid(ProjectileType[] pOrder)
	{
		projectileOrder = pOrder;

		grid = new Room[30, 30];
		for(int i = 0; i < 30; i++)
		{
			for(int j = 0; j < 30; j++)
			{
				grid[i,j] = new Room();
			}
		}

		makePrimaryRoom (1, 16);
		grid [1, 16].projectilePickup = projectileOrder [0]; // Place the first projectile at the entrance

		// Now make the exit
		makeRoom (28, 16);
		grid [29, 16].east = ProjectileType.EMPTY; // remove the wall for the exit
		grid [27, 16].west = ProjectileType.Yellow;

		// Primary room A
		makePrimaryRoom (10, 20);

		// Primary room B
		makePrimaryRoom (10, 10);

		// Primary room C
		makePrimaryRoom (21, 12);

		// Secondary room for C that leads to exit
		makeRoom (25, 16);
		grid [25, 16].projectilePickup = ProjectileType.Yellow;
		grid [26, 16].east = ProjectileType.EMPTY; // remove the wall leading to the exit

		RoomPlacer r = new RoomPlacer (this, projectileOrder);
		r.spawnSecondaryRooms ();

	}

	// Given the center coordinates for a room, dig one out
	// A room being a 3x3 grid.
	// Input is assumed to be valid
	// It also marks all rooms as visited so DFS doesn't touch them
	public void makeRoom(int i, int j)
	{
		// Remove all walls from the center square
		grid [i, j].state = RoomState.Room;
		grid [i, j].north = ProjectileType.EMPTY;
		grid [i, j].south = ProjectileType.EMPTY;
		grid [i, j].east = ProjectileType.EMPTY;
		grid [i, j].west = ProjectileType.EMPTY;
		grid [i, j].visited = true;


		// Upper left square is a corner
		grid [i-1, j+1].state = RoomState.Room;
		grid [i-1, j+1].south = ProjectileType.EMPTY;
		grid [i-1, j+1].east = ProjectileType.EMPTY;
		grid [i-1, j+1].north = ProjectileType.NONE;
		grid [i-1, j+1].west = ProjectileType.NONE;
		grid [i-1, j+1].visited = true;

		// Upper square only has the north wall
		grid [i, j+1].state = RoomState.Room;
		grid [i, j+1].south = ProjectileType.EMPTY;
		grid [i, j+1].east = ProjectileType.EMPTY;
		grid [i, j+1].west = ProjectileType.EMPTY;
		grid [i, j + 1].north = ProjectileType.NONE;

		grid [i, j+1].visited = true;

		//upper right square is a corner
		grid [i+1, j+1].state = RoomState.Room;
		grid [i+1, j+1].south = ProjectileType.EMPTY;
		grid [i+1, j+1].west = ProjectileType.EMPTY;
		grid [i+1, j+1].north = ProjectileType.NONE;
		grid [i+1, j+1].east = ProjectileType.NONE;
		grid [i+1, j+1].visited = true;

		// Middle right square only has the right wall
		grid [i+1, j].state = RoomState.Room;
		grid [i+1, j].north = ProjectileType.EMPTY;
		grid [i+1, j].south = ProjectileType.EMPTY;
		grid [i+1, j].west = ProjectileType.EMPTY;
		grid [i+1, j].east = ProjectileType.NONE;
		grid [i+1, j].visited = true;

		// Middle left square only has the left wall
		grid [i-1, j].state = RoomState.Room;
		grid [i-1, j].north = ProjectileType.EMPTY;
		grid [i-1, j].south = ProjectileType.EMPTY;
		grid [i-1, j].east = ProjectileType.EMPTY;
		grid [i-1, j].west = ProjectileType.NONE;
		grid [i-1, j].visited = true;

		// Bottom left square is a corner
		grid [i-1, j-1].state = RoomState.Room;
		grid [i-1, j-1].north = ProjectileType.EMPTY;
		grid [i-1, j-1].east = ProjectileType.EMPTY;
		grid [i-1, j-1].south = ProjectileType.NONE;
		grid [i-1, j-1].west = ProjectileType.NONE;
		grid [i-1, j-1].visited = true;

		// Bottom square only has the south wall
		grid [i, j-1].state = RoomState.Room;
		grid [i, j-1].north = ProjectileType.EMPTY;
		grid [i, j-1].east = ProjectileType.EMPTY;
		grid [i, j-1].west = ProjectileType.EMPTY;
		grid [i, j-1].south = ProjectileType.NONE;
		grid [i, j-1].visited = true;

		// Bottom right is a corner
		grid [i+1, j-1].state = RoomState.Room;
		grid [i+1, j-1].north = ProjectileType.EMPTY;
		grid [i+1, j-1].west = ProjectileType.EMPTY;
		grid [i+1, j-1].south = ProjectileType.NONE;
		grid [i+1, j-1].east = ProjectileType.NONE;
		grid [i+1, j-1].visited = true;
	}

	// Same as making a room except it designates the 4 doorways
	// It also marks the doorways an unvisited because DFS might connect to them
	public void makePrimaryRoom(int i, int j)
	{
		makeRoom (i, j);
		grid [i, j+1].state = RoomState.Door;
		grid [i, j + 1].visited = false;

		grid [i+1, j].state = RoomState.Door;
		grid [i+1, j].visited = false;

		grid [i-1, j].state = RoomState.Door;
		grid [i-1, j].visited = false;

		grid [i, j-1].state = RoomState.Door;
		grid [i, j-1].visited = false;
	}

	// cleans up the placed rooms
	public void cleanup()
	{
		cleanupHelper (1, 16);
		cleanupHelper (28, 16);
		cleanupHelper (10, 20);
		cleanupHelper (10, 10);		
		cleanupHelper (21, 12);		
		cleanupHelper (25, 16);
		RoomPlacer r = new RoomPlacer (this, projectileOrder);
		r.spawnSecondaryRooms();
	}

	private void cleanupHelper(int i, int j)
	{
		grid [i - 1, j + 1].west = ProjectileType.NONE;
		grid [i - 1, j + 1].north = ProjectileType.NONE;

		grid [i - 1, j - 1].south = ProjectileType.NONE;
		grid [i - 1, j - 1].west = ProjectileType.NONE;

		grid[i+1,j+1].north = ProjectileType.NONE;
		grid[i+1,j+1].east = ProjectileType.NONE;

		grid[i+1,j-1].south = ProjectileType.NONE;
		grid[i+1,j-1].east = ProjectileType.NONE;
	}

}
