using UnityEngine;
using System.Collections;

using Globals;
using System.Collections.Generic;

// Helper class which serves to take a gris with all rooms
// already placed and connect the main rooms such that the
// maze is solvable using Depth First Search
public class MainRoomConnector
{
	private DigoutGrid dGrid;

	// These are used to ensure that the rooms are linked
	// and they will be used to randomly determine how many
	// connections go to each room
	private bool entryRoomConnection;
	private bool roomAConnection;
	private bool roomBConnection;
	private bool roomCConnection;

	// Only the center is present because to
	// get the entries we just get the adjacent
	// points
	private GridPoint EntryRoomCenter = new GridPoint(1,16);
	private GridPoint RoomACenter = new GridPoint(10,20);
	private GridPoint RoomBCenter = new GridPoint(10,10);
	private GridPoint RoomCCenter = new GridPoint(21,12);


	public MainRoomConnector(DigoutGrid g)
	{
		dGrid = g;

		entryRoomConnection = false;
		roomAConnection = false;
		roomBConnection = false;
		roomCConnection = false;
	}

	public void connectPrimaryRooms()
	{
		GridPoint start;
		GridPoint entryPoint;

		// Determine if there are paths from primary tooms 
		// A to B and/or rooms B to C
		potentiallyLinkPrimaryRooms ();

		// Randomly select the first room from the entry point to start the DFS from
		int rand = Random.Range (1, 4);
		if(rand == 1)
		{
			start = new GridPoint (3, 16);
			entryPoint = new GridPoint(2,16);
			dGrid.grid[start.i, start.j].state = RoomState.Corridor;
		}
		else if(rand == 2)
		{
			start = new GridPoint (1, 18);
			entryPoint = new GridPoint(1,17);
			dGrid.grid[start.i, start.j].state = RoomState.Corridor;
		}
		else
		{
			start = new GridPoint (1, 14);
			entryPoint = new GridPoint(1,15);
			dGrid.grid[start.i, start.j].state = RoomState.Corridor;
		}

		// Now do a DFS and carve out a path from that starting point
		dfs (start, entryPoint);

		// Now randomly add connections to make it a braided maze and ensure that
		// the structure of the main rooms has been maintained
		braidTheMaze();
		cleanup();
	}

	// randomly links 30 adjacent cells to make the maze braided
	private void braidTheMaze()
	{
		int numBraidings = 30;
		for(int x = 0; x < numBraidings; x++)
		{
			int i = Random.Range(1,29);
			int j = Random.Range(1,29);
			GridPoint randomGP = new GridPoint(i,j);
			List<GridPoint> adj = getAdjacentCells(randomGP);

			linkAdjacentRooms(randomGP, adj[0]);
		}
	}

	// randomly determines if there will be a corridor from A to B and/or B to C
	private void potentiallyLinkPrimaryRooms()
	{
		int rng = Random.Range (0, 2);
		if(rng == 1)
		{
			CorridorPlacer.AtoB(dGrid);
			roomBConnection = true;
		}

		rng = Random.Range (0, 2);
		if(rng == 1) 
		{
			CorridorPlacer.BtoC(dGrid);
			roomCConnection = true;
		}
	}

	private void cleanup()
	{
		dGrid.cleanup ();
		linkAdjacentRooms (new GridPoint (10, 11), new GridPoint (10, 12));
	}

	// Takes two adjacent gridpoints and removes the walls between them
	// assumes valid points that are not identical
	public void linkAdjacentRooms(GridPoint p1, GridPoint p2)
	{
		// First we determine how they are placed relative to each other based
		// on the difference of coordinates, then remove the walls

		/////////// Horizontally placed //////////
		if( p1.i < p2.i) // p1 is on the left of p2
		{
			dGrid.grid[p1.i,p1.j].east = ProjectileType.EMPTY;
			dGrid.grid[p2.i,p2.j].west = ProjectileType.EMPTY;
		}
		else if( p1.i > p2.i) // p1 is on the right of p2
		{
			dGrid.grid[p1.i,p1.j].west = ProjectileType.EMPTY;
			dGrid.grid[p2.i,p2.j].east = ProjectileType.EMPTY;
		}

		///////// Vertically place /////////////
		else if(p1.j < p2.j) // p1 is below p2
		{
			dGrid.grid[p1.i,p1.j].north = ProjectileType.EMPTY;
			dGrid.grid[p2.i,p2.j].south = ProjectileType.EMPTY;
		}
		else if(p1.j > p2.j) // p1 is above p2
		{
			dGrid.grid[p1.i,p1.j].south = ProjectileType.EMPTY;
			dGrid.grid[p2.i,p2.j].north = ProjectileType.EMPTY;
		}
	}

	// Returns all adjacent cells assuming the input is valid
	// Adjacent does not include diagonals
	public List<GridPoint> getAdjacentCells(GridPoint p)
	{
		List<GridPoint> ret = new List<GridPoint> ();

		// If the point to the left is valid, add it
		if(p.i - 1 >= 0) ret.Add(new GridPoint(p.i-1, p.j));

		// Check if the point to the right is valid
		if(p.i + 1 <= 29) ret.Add (new GridPoint(p.i+1, p.j));

		// Check for the points above and below
		if(p.j + 1 <= 29) ret.Add(new GridPoint(p.i, p.j+1));
		if(p.j - 1 >= 0) ret.Add(new GridPoint(p.i, p.j-1));

		// Now randomize the order of the list and return it
		int count = ret.Count;
		while( count > 1)
		{
			int n = Random.Range(0, count);
			count --;

			GridPoint temp = ret[n];
			ret[n] = ret[count];
			ret[count] = temp;
		}


		return ret;
	}

	// returns true if the two points are adjacent
	private bool areCellsAdjacent(GridPoint p1, GridPoint p2)
	{
		// One coordinate has to be the same and the other has to
		// have a difference of 1 in order for the cells to be 
		// adjacent
		if(p1.i == p2.i)
		{
			if( Mathf.Abs(p1.j - p2.j) == 1) 
			{
				return true;
			}
		}
		else if(p1.j == p2.j)
		{
			if(Mathf.Abs(p1.i - p2.i) == 1)
			{
				return true;
			}
		}
		return false;
	}

	// returns true if the point is not in a room
	// if it is a room, it will ensure that the DFS
	// connects to each room once.
		// Each additional connection is determined randomly
	private bool connectToRoom(GridPoint p1)
	{
		if(getAdjacentCells(EntryRoomCenter).Contains(p1))
		{
			if(entryRoomConnection == false)
			{
				entryRoomConnection = true;
				return true;
			}
			
			// If there's already a connection, decide randomly if there should be one
			else
			{
				return Random.Range(0, 2) == 1;
			}
		}

		else if(getAdjacentCells(RoomACenter).Contains(p1))
		{
			if(roomAConnection == false)
			{
				roomAConnection = true;
				return true;
			}

			// If there's already a connection, decide randomly if there should be one
			else
			{
				return Random.Range(0, 2) == 1;
			}
		}

		else if(getAdjacentCells(RoomBCenter).Contains(p1))
		{
			if(roomBConnection == false)
			{
				roomBConnection = true;
				return Random.Range(0, 2) == 1;
			}
			
			// If there's already a connection, decide randomly if there should be one
			else
			{
				return false; //return Random.Range(0, 2) == 1;
			}
		}

		if(getAdjacentCells(RoomCCenter).Contains(p1))
		{
			if(roomCConnection == false)
			{
				roomCConnection = true;
				return true;
			}
			
			// If there's already a connection, decide randomly if there should be one
			else
			{
				return Random.Range(0, 2) == 1;
			}
		}


		return true; // it's not a room
	}

	// Does a DFS, branching into all cells with state labelled "room"
	// A second stack is used to track the previously visited so that way
	// when the algorithm backtracks it can backtrack through where it visited
	// cells and find the adjacent and carve out the walls as it goes

	// Entry cell is the original cell from the room entry.
	private void dfs(GridPoint start, GridPoint entryCell)
	{
		Stack previouslyVisited = new Stack ();
		previouslyVisited.Push (entryCell);

		Stack stack = new Stack ();
		stack.Push (start);

		while(stack.Count != 0)
		{
			GridPoint point = (GridPoint) stack.Pop();
			Room room = dGrid.grid[point.i, point.j];

			GridPoint prevPoint = (GridPoint) previouslyVisited.Peek();

			// The the current point is not adjacent to the previous,
			// it means we've backtracked, which means we need to backtracj
			// through our secondary stack to find an adjacent point
			while(!areCellsAdjacent(point, prevPoint))
			{
				prevPoint = (GridPoint) previouslyVisited.Pop();
			}
			previouslyVisited.Push(prevPoint); // push it back for future use

			// Now we check to see if we can link the two rooms
			if((room.state == RoomState.Unused || room.state == RoomState.Door) && connectToRoom(point))
			{
				room.state = RoomState.Corridor;
				linkAdjacentRooms(point, prevPoint);
			}

			// Now that we're done with the current cell, push it to the stack of
			// previously visited and continue with DFS as usual
			previouslyVisited.Push(point);

			if(room.visited == false)
			{
				room.visited = true;

				List<GridPoint> adjacents = getAdjacentCells(point);
				foreach(GridPoint adj in adjacents)
				{
					stack.Push(adj);
				}
			}
		}
	}
}
