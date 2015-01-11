using UnityEngine;
using System.Collections;

public class Explosions : MonoBehaviour {

	public float force;
	public float radius;
	public GameObject player;
	public GameObject linePrefab;
	public GameObject explosion;

	private GameObject line;

	void Start() {
		line = (GameObject) Instantiate (linePrefab);
		line.renderer.enabled = false;
	}

	private bool previousYPressed = false;
	// Update is called once per frame
	void Update () {
		LineRenderer lineRender;
		RaycastHit hit;

		bool yPressed = OVRGamepadController.GPC_GetButton(OVRGamepadController.Button.Y);

		if (yPressed) 
		{
			if(Physics.Raycast(player.transform.position, player.transform.forward, out hit))
			{
				// Draw a line to show the player where they are aiming
				lineRender = (LineRenderer) line.renderer;
				lineRender.enabled = true;

				lineRender.SetPosition (0, player.transform.position - new Vector3(0.0f, 0.3f, 0.0f));
				lineRender.SetPosition (1, hit.point);
			}
		}
		else if (!yPressed && previousYPressed)
		{
			if(Physics.Raycast(player.transform.position, player.transform.forward, out hit))
			{
				// Create the explosion's particle effect
				Instantiate (explosion, hit.point, Quaternion.identity);

				// Create an explosion around where the player aims
				Collider[] colliders = Physics.OverlapSphere(hit.point, radius);
				
				foreach(Collider collider in colliders)
				{
					if (collider.rigidbody == null)
						continue;
					collider.rigidbody.AddExplosionForce(force, hit.point, radius, 0.0f, ForceMode.Impulse);
				}
			}
			line.renderer.enabled = false;
		}

		previousYPressed = yPressed;
	}
}
