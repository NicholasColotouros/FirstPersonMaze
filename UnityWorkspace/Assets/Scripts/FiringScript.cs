using UnityEngine;
using System.Collections;
using Globals;

public class FiringScript : MonoBehaviour 
{
	GameObject bulletPrefab;
	int strength = 2500;

	void Start()
	{
		bulletPrefab = Resources.Load ("Bullet") as GameObject;
	}

	// Update is called once per frame
	void Update () 
	{
		// If the right mouse button is pressed, fire a projectile if we have one
		if(Input.GetKeyDown(KeyCode.Mouse0))
		{
			Inventory inventoryScript = this.GetComponentInParent<Inventory>();
			ProjectileType bulletType = inventoryScript.useProjectile();

			if(bulletType != ProjectileType.NONE)
			{
				// spawn the bullet, give it the appropriate type and send it forward with a force
				GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
				bullet.GetComponent<BulletScript>().initializeBullet(bulletType, inventoryScript);

				// FIRE
				bullet.rigidbody.AddForce(this.transform.forward * strength * -1); // -1 because my gun prefab is backward
			}
		}
	}
}