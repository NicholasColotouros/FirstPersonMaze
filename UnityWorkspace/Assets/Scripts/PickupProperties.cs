using UnityEngine;
using System.Collections;
using Globals;

/*
 * This class is used to distinguish between
 * the various projectile types without having
 * to make different prefabs.
 * 
 * It contains properties for the projectile.
 */
public class PickupProperties : MonoBehaviour 
{
	public ProjectileType projectileType;

	// Use this for initialization
	// Defaults to blue
	void Start () 
	{
		setProjectileType (projectileType);
	}

	public void setProjectileType(ProjectileType t)
	{
		projectileType = t;
		renderer.material.color = ProjectileHelperFunctions.convertToColor (projectileType);
		gameObject.name = t + " Projectile";
	}

	public ProjectileType getProjectileType(){ return projectileType; }
}
