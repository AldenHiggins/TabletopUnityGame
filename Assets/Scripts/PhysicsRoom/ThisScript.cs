using UnityEngine;
using System.Collections;

public class ThisScript : MonoBehaviour
{

//	// Use this for initialization
//	void Start () {
//	
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}

	public float speed;

	void FixedUpdate () 
	{
		float horizontalMovement = Input.GetAxis ("Horizontal");
		float verticalMovement = Input.GetAxis ("Vertical");


		Vector3 movement = new Vector3 (horizontalMovement, 0.0f, verticalMovement);

		rigidbody.AddForce (movement * speed * Time.deltaTime);

	}


}
