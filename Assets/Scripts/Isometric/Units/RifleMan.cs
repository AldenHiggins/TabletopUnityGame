using UnityEngine;
using System.Collections;

public class RifleMan : MonoBehaviour, IUnit 
{
	Animator anim;
	NavMeshAgent agent;

	public GameObject ragDoll;
	public GameObject muzzleFlashParticle;

	private UnitMethods unitMethods;

	private Team team;
	private int health;
	private IUnit currentTarget;
	private bool forceMove;
	private bool isSoldierDead;
	private bool isFiring;
	private GameObject highlightCircle;

	// Use this for initialization
	void Awake () 
	{
		// Initialize common unit functionality
		unitMethods = transform.GetChild (0).gameObject.GetComponent<UnitMethods> ();
		unitMethods.createHealthBars (transform.position, .07f, new Vector3 (0.0f, 0.2083f, 0.0f));

		isFiring = false;
		isSoldierDead = false;
		forceMove = false;
		health = 100;
		anim = GetComponent<Animator> ();
		agent = GetComponent<NavMeshAgent> ();
		// Default to red team
		if (team == null)
		{
			this.setTeam (new Team ("Red", Color.red));
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Display healthbars
		unitMethods.displayHealth (transform.position, health, 100);

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
		if (currentTarget.isDead())
		{
			print ("Shooting at a dead guy!");
			noTarget();
			yield break;
		}
		if (currentTarget.getTeam ().getName ().Equals(team.getName ()))
		{
			print ("Same team!");
			noTarget ();
			yield break;
		}
		Instantiate (muzzleFlashParticle, transform.position + new Vector3(0.0f, 0.13f, 0.0f), transform.rotation);
		currentTarget.dealDamage (10, this);
		isFiring = false;
	}
	
	public void dealDamage(int damageDealt, IUnit attackingUnit)
	{
		health -= damageDealt;
		if (health < 0 && !isSoldierDead)
		{
			isSoldierDead = true;
			// Generate a Ragdoll
			Vector3 ragDollPosition = transform.position;
			GameObject ragDollInstance = (GameObject) Instantiate ((Object)ragDoll, ragDollPosition, transform.rotation);
			// Give the ragdoll the correct color
			ragDollInstance.transform.GetChild (0).gameObject.GetComponent<SkinnedMeshRenderer> ().materials [0].color = team.getColor();

			// Clean up this game object
			transform.GetChild (1).gameObject.GetComponent<SkinnedMeshRenderer> ().enabled = false;
			Destroy (this.gameObject);
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

		if (currentTarget != null)
		{
			agent.Stop ();
		}
	}

	public void setTeam(Team newTeam)
	{
		team = newTeam;
		transform.GetChild (1).gameObject.GetComponent<SkinnedMeshRenderer> ().materials [0].color = newTeam.getColor ();
	}

	public Team getTeam()
	{
		return team;
	}

	public void shootAt(IUnit target)
	{
		// Don't pick up a target if the move command was issued
		if (forceMove)
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

		transform.rotation = unitMethods.faceTarget (transform.position, target.getPosition ());
	}

	public void noTarget()
	{
		if (agent == null)
			return;
		anim.SetBool ("Aiming", false);
		agent.Resume ();
		isFiring = false;
		currentTarget = null;
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

	public Quaternion getRotation()
	{
		return transform.rotation;
	}

	public bool hasTarget()
	{
		if (currentTarget != null)
			return true;
		return false;
	}
}
