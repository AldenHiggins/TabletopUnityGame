using UnityEngine;
using System.Collections;

public class RifleMan : MonoBehaviour, IUnit {

	Animator anim;
	NavMeshAgent agent;

	public GameObject ragDoll;
	public GameObject muzzleFlashParticle;

	public GameObject greenHealthBar;
	public GameObject redHealthBar;
	
	private GameObject thisGreenHealthBar;
	private GameObject thisRedHealthBar;

	private float startingHealthBarZScale;

	private Team team;
	private int health;
	private IUnit currentTarget;
	private bool forceMove;
	private bool isSoldierDead;
	private bool isFiring;
	private GameObject highlightCircle;

	// Use this for initialization
	void Start () 
	{
		thisGreenHealthBar = (GameObject)Instantiate (greenHealthBar, transform.position, Quaternion.identity);
		thisRedHealthBar = (GameObject)Instantiate (redHealthBar, transform.position, Quaternion.identity);
		startingHealthBarZScale = thisGreenHealthBar.transform.localScale.z;
		isFiring = false;
		isSoldierDead = false;
		forceMove = false;
		health = 100;
		anim = GetComponent<Animator> ();
		agent = GetComponent<NavMeshAgent> ();
		if (team == null)
		{
			this.setTeam (new Team ("Red", Color.red));
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Display healthbars in the correct place
		thisGreenHealthBar.transform.position = transform.position + new Vector3 (0.0f, 0.2083f, 0.0f);
		Vector3 currentScale = thisGreenHealthBar.transform.localScale;
		thisGreenHealthBar.transform.localScale = new Vector3(currentScale.x, currentScale.y, (startingHealthBarZScale * health / 100));
		thisRedHealthBar.transform.position = transform.position + new Vector3 (0.0f, 0.2083f, 0.0f);

		// Check if the soldier is done moving
		if (!agent.pathPending)
		{
			if (agent.remainingDistance <= agent.stoppingDistance)
			{
				if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
				{
					anim.SetBool ("Walking", false);
					forceMove = false;
				}
			}
		}


		// Check if soldier is attacking
		if (anim.GetBool ("Aiming") == true)
		{
			if (!isFiring)
				StartCoroutine("attackLoop");
		}
		else
		{
			isFiring = false;
			StopCoroutine("attackLoop");
		}
		// Gets info about the current animation state
		// AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0)
	}



	IEnumerator attackLoop()
	{
		isFiring = true;
		yield return new WaitForSeconds(1.0f);
		if (currentTarget == null)
		{
			print ("Shooting at a dead guy!");
			yield break;
		}
		Instantiate (muzzleFlashParticle, transform.position + new Vector3(0.0f, 0.13f, 0.0f), transform.rotation);
		currentTarget.dealDamage (10);
		isFiring = false;
	}
	
	public void dealDamage(int damageDealt)
	{
		health -= damageDealt;
		if (health < 0 && !isSoldierDead)
		{
			isSoldierDead = true;
			// Generate a Ragdoll
			Vector3 ragDollPosition = transform.position + new Vector3(0.0f, 0.0f, 0.0f);
			GameObject ragDollInstance = (GameObject) Instantiate ((Object)ragDoll, ragDollPosition, transform.rotation);
			// Give the ragdoll the correct color
			ragDollInstance.transform.GetChild (0).gameObject.GetComponent<SkinnedMeshRenderer> ().materials [0].color = team.getColor();

			// Clean up this game object
			transform.GetChild (0).gameObject.GetComponent<SkinnedMeshRenderer> ().enabled = false;
			Destroy (this.gameObject);
			Destroy (thisGreenHealthBar);
			Destroy (thisRedHealthBar);
		}
	}

	public void moveToPosition(Vector3 newDestination)
	{
		forceMove = true;
		anim.SetBool ("Walking", true);
		agent.SetDestination (newDestination);
	}

	public void attackMoveToPosition(Vector3 newDestination) 
	{
		forceMove = false;
		anim.SetBool ("Walking", true);
		agent.SetDestination (newDestination);
	}

	public void setTeam(Team newTeam)
	{
		team = newTeam;
		transform.GetChild (0).gameObject.GetComponent<SkinnedMeshRenderer> ().materials [0].color = newTeam.getColor ();
	}

	public Team getTeam()
	{
		return team;
	}

	public void shootAt(IUnit target)
	{
		if (agent == null)
			return;
		// Don't pick up a target if the move command was issued
		if (forceMove)
			return;
		// Kind of hacky check to make sure they're not attacking while dead
		if (isSoldierDead)
			return;
		if (currentTarget != null)
		{
			if (currentTarget.isDead ())
			{
				currentTarget = null;
			}
			else
			{
				agent.Stop ();
				return;
			}
		}
		// Clear current destination
		agent.Stop ();

		currentTarget = target;
		anim.SetBool ("Aiming", true);


		Vector3 vectorToTarget = target.getPosition () - transform.position;
		float radianAngle = Mathf.Atan2 (vectorToTarget.x, vectorToTarget.z);
		float degreesAngle = (radianAngle / Mathf.PI) * 180;

		transform.rotation = Quaternion.Euler (0.0f, degreesAngle, 0.0f);

	}

	public void noTarget()
	{
		if (agent == null)
			return;
		anim.SetBool ("Aiming", false);
		agent.Resume ();
	}

	public Vector3 getPosition()
	{
		return transform.position;
	}

	public bool isDead()
	{
		return isSoldierDead;
	}

	public IUnit getCurrentTarget()
	{
		return currentTarget;
	}

	public bool isSoldierFiring()
	{
		return isFiring;
	}

	public 	Quaternion getRotation()
	{
		return transform.rotation;
	}
}
