using UnityEngine;
using System.Collections;
using Globals;

// Used as a helper class to place the corridors from
// A-B and B-C if they're used. Whether or not these
// methods are ever called is determined randomly
public class CorridorPlacer 
{
	// insert corridor from A to B
	public static void AtoB(DigoutGrid dGrid)
	{
		for(int i = 19; i >= 11; i--)
		{
			dGrid.grid [10, i].north = ProjectileType.EMPTY;
			dGrid.grid [10, i].south = ProjectileType.EMPTY;
			dGrid.grid [10, i].state = RoomState.Corridor;
			dGrid.grid [10, i].visited = true;
		}
		dGrid.grid [10, 19].state = RoomState.Room;
		dGrid.grid [10, 11].state = RoomState.Room;
	}

	// insert corridor from B to C
	public static void BtoC(DigoutGrid dGrid)
	{
		for(int i = 11; i <= 18; i++)
		{
			dGrid.grid [i, 10].east = ProjectileType.EMPTY;
			dGrid.grid [i, 10].west = ProjectileType.EMPTY;
			dGrid.grid [i, 10].state = RoomState.Corridor;
			dGrid.grid [i, 10].visited = true;
		}
		dGrid.grid [11, 10].state = RoomState.Room;

		// the turn
		dGrid.grid [19, 10].west = ProjectileType.EMPTY;
		dGrid.grid [19, 10].north = ProjectileType.EMPTY;
		dGrid.grid [19, 10].state = RoomState.Corridor;
		dGrid.grid [19, 10].visited = true;

		// that one straight away
		dGrid.grid [19, 11].north = ProjectileType.EMPTY;
		dGrid.grid [19, 11].south = ProjectileType.EMPTY;
		dGrid.grid [19, 11].state = RoomState.Corridor;
		dGrid.grid [19, 11].visited = true;

		// the final turn
		dGrid.grid [19, 12].east = ProjectileType.EMPTY;
		dGrid.grid [19, 12].south = ProjectileType.EMPTY;
		dGrid.grid [19, 12].state = RoomState.Corridor;
		dGrid.grid [19, 12].visited = true;

		dGrid.grid [20, 12].west = ProjectileType.EMPTY;
		dGrid.grid [20, 12].state = RoomState.Room;
		dGrid.grid [20, 12].visited = true;
	}
}
