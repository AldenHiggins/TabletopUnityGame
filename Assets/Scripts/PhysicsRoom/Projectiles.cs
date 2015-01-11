using UnityEngine;
using System.Collections;

public class Projectiles : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	public GameObject cubeProjectile;
	public GameObject player;

	private GameObject cubeProjectileObject;
	private bool previousBDown = false;

	// Update is called once per frame
	void Update () {
		bool bPressed = OVRGamepadController.GPC_GetButton(OVRGamepadController.Button.B);

		// Create projectile
		if (bPressed && !previousBDown)
		{
			cubeProjectileObject = (GameObject) Instantiate (cubeProjectile);
		}
		// Animate projectile in front of player
		else if (bPressed)
		{
			print ("B being pressed");
			cubeProjectileObject.transform.position = player.transform.position + 2 * player.transform.forward;
			cubeProjectileObject.transform.Rotate (new Vector3(15, 30, 35) * Time.deltaTime);
		}
		// Fire projectile
		else if (!bPressed && previousBDown)
		{
			print ("B released");
			cubeProjectileObject.rigidbody.velocity += 8 * player.transform.forward;
		}

		previousBDown = bPressed;
	}

}
