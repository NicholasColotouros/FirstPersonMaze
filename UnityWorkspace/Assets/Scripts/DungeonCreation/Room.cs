using UnityEngine;
using System.Collections;
using Globals;

// Data representation of a 1x1 room with 4 walls which may or may not exist
// and may or may not be destroyed by a projectile. There may also be a 
// projectile pickup located in the center.
public class Room 
{
	// Represents what kind of wall they should have -- is it destructible by projectiles?
	// ProjectileType.EMPTY means no wall
	public ProjectileType north;
	public ProjectileType south;
	public ProjectileType east;
	public ProjectileType west;

	public RoomState state;
	public ProjectileType projectilePickup;

	public bool visited; // used for when DFS happens in the grid generation

	public Room()
	{
		north = ProjectileType.NONE;
		south = ProjectileType.NONE;
		east = ProjectileType.NONE;
		west = ProjectileType.NONE;

		projectilePickup = ProjectileType.EMPTY;
		state = RoomState.Unused;
		visited = false;
	}

	override
	public string ToString()
	{
		return "State: " + state + ", Projectile: " + projectilePickup +
						"\nNorth: " + north + 
						"\nEast: " + east +
						"\nSouth: " + south +
						"\nWest: " + west;

	}
}
