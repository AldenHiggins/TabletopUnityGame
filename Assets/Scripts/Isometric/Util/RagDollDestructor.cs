using UnityEngine;
using System.Collections;

public class RagDollDestructor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine("selfDestructLoop");
	}

	IEnumerator selfDestructLoop()
	{
		yield return new WaitForSeconds(3.0f);
		Destroy (this.gameObject);
	}
}
