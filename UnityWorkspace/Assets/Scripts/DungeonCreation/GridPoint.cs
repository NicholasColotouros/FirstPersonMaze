using UnityEngine;
using System.Collections.Generic;

// represents a point on the grid
public class GridPoint : IEqualityComparer<GridPoint>
{
	public int i;
	public int j;

	public GridPoint(int pI, int pJ)
	{
		i = pI;
		j = pJ;
	}

	public override bool Equals(object obj)
	{
		if (obj == null) return false;
		GridPoint objAsGP = obj as GridPoint;
		if (objAsGP == null) return false;
		return (this.i == objAsGP.i && this.j == objAsGP.j);
	}

	public bool equals(GridPoint p)
	{
		return (this.i == p.i && this.j == p.j);
	}

	public override int GetHashCode()
	{
		return this.i * 100 + this.j;
	}

	public override string ToString ()
	{
		return "(" + i + ", " + j + ")";
	}

	public bool Equals(GridPoint p1, GridPoint p2)
	{
		return p1.equals(p2);
	}

	public int GetHashCode(GridPoint p)
	{
		return p.GetHashCode ();
	}
}
