using UnityEngine;

namespace Globals
{
	public enum ProjectileType {Red, Blue, Green, Yellow, NONE, EMPTY};
	public enum GState{Play, Win, Lose};
	public enum RoomState{Room, Door, Corridor, SecondaryCorridor, Unused};

	public class ProjectileHelperFunctions
	{
		// Converts the projectile to their respective Color
		public static Color convertToColor(ProjectileType p)
		{
			if(p == ProjectileType.Red){ return Color.red; }
			else if(p == ProjectileType.Blue){ return Color.blue; }
			else if(p == ProjectileType.Green){ return Color.green; }
			else if(p == ProjectileType.Yellow){ return Color.yellow; }
			else if(p == ProjectileType.NONE) { return Color.grey; }
			return Color.black; // empty
		}

		// Takes an integer and returns the ordinal value of the enum
		public static ProjectileType projectileOrdinal(int ord)
		{
			if(ord == 0){ return ProjectileType.Red; }
			if(ord == 1){ return ProjectileType.Green; }
			if(ord == 2){ return ProjectileType.Blue; }
			if(ord == 3){ return ProjectileType.Yellow; }
			if(ord == 4){ return ProjectileType.NONE; }
			return ProjectileType.NONE;
		}

	}
}