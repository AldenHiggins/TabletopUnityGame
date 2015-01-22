using UnityEngine;
using System.Collections;

public class UnitMethods : MonoBehaviour
{
	public GameObject greenHealthBar;
	public GameObject redHealthBar;

	public static int test;

	private GameObject thisGreenHealthBar;
	private GameObject thisRedHealthBar;

	private Vector3 healthOffset;

	private Team currentTeam;

	private float startingHealthBarZScale;

	private Team team;

	private bool forceMove;

	private IUnit parentUnit;

	// Shooting stuff
	private IUnit currentTarget;

	private bool isFiring;

	void Awake ()
	{
		isFiring = false;
		if (team == null)
		{
			team = new Team ("Red", Color.red);
		}
		forceMove = false;
	}

	public void setParent(IUnit parentUnitInput)
	{
		parentUnit = parentUnitInput;
	}

	void Update()
	{
		// Check if soldier is attacking
		if (currentTarget != null)
		{
			if (!isFiring)
				StartCoroutine("attackLoop");
		}
		else
		{
			isFiring = false;
			StopCoroutine("attackLoop");
		}
	}

	public delegate void AttackCallback(IUnit target);
	AttackCallback attackCallback;

	public void setAttackCallback(AttackCallback callbackFunc)
	{
		attackCallback = callbackFunc;
	}

	IEnumerator attackLoop()
	{
		isFiring = true;
		yield return new WaitForSeconds(1.0f);
		if (forceMove)
		{
			noTarget();
			yield break;
		}
		if (currentTarget.isDead())
		{
			noTarget();
			yield break;
		}
		if (currentTarget.getTeam ().getName ().Equals(team.getName ()))
		{
			noTarget();
			yield break;
		}
		currentTarget.dealDamage (10, parentUnit);
		isFiring = false;
		if (attackCallback != null)
		{
			attackCallback(currentTarget);
		}
		else
		{
			print ("No attack callback " + parentUnit.getTeam()); 
		}
	}

	void noTarget()
	{
		isFiring = false;
		currentTarget = null;
	}
	

	void OnDestroy()
	{
		Destroy (thisGreenHealthBar);
		Destroy (thisRedHealthBar);
	}

	public void createHealthBars(Vector3 position, float startingScale, Vector3 offset)
	{
		healthOffset = offset;
		// Initialize green health bar
		thisGreenHealthBar = (GameObject)Instantiate (greenHealthBar, position, Quaternion.identity);
		Vector3 currentScale = thisGreenHealthBar.transform.localScale;
		thisGreenHealthBar.transform.localScale.Set (currentScale.x, currentScale.y, startingScale);
		startingHealthBarZScale = startingScale;
		// Initialize red health bar
		thisRedHealthBar = (GameObject)Instantiate (redHealthBar, position, Quaternion.identity);
		currentScale = thisRedHealthBar.transform.localScale;
		thisRedHealthBar.transform.localScale.Set (currentScale.x, currentScale.y, startingScale);
	}

	public void displayHealth(Vector3 position, float currentHealth, float maxHealth)
	{
		if (thisGreenHealthBar == null)
		{
			Debug.LogError("Have to initialize health bars first with createHealthBars!");
			return;
		}
		// Display healthbars in the correct place with the right health value
		Vector3 currentScale = thisGreenHealthBar.transform.localScale;
		thisGreenHealthBar.transform.localScale = new Vector3(currentScale.x, currentScale.y, (startingHealthBarZScale * currentHealth / maxHealth));
		thisGreenHealthBar.transform.position = position + healthOffset;
		thisRedHealthBar.transform.position = position + healthOffset;
	}

	public bool sharedShoot(IUnit target)
	{
		// Don't pick up a target if the move command was issued
		if (forceMove)
			return false;
		if (currentTarget != null)
		{
			print ("Have a target in shared shoot");
			if (currentTarget.isDead ())
			{
				print ("Shared shoot target is dead");
				currentTarget = null;
			}
			else
			{
				print ("Shared shoot target is alive");
				return false;
			}
		}
		// Clear current destination
		currentTarget = target;
		return true;

	}

	public Quaternion faceTarget(Vector3 position, Vector3 faceThisPosition)
	{
		Vector3 vectorToTarget = faceThisPosition - position;
		float radianAngle = Mathf.Atan2 (vectorToTarget.x, vectorToTarget.z);
		float degreesAngle = (radianAngle / Mathf.PI) * 180;
		
		return Quaternion.Euler (0.0f, degreesAngle, 0.0f);
	}

	public IUnit getCurrentTarget()
	{
		return currentTarget;
	}

	public void setTeam(Team newTeam)
	{
		team = newTeam;
	}

	public bool isSoldierFiring()
	{
		return isFiring;
	}

	public void setForceMove(bool onOrOff)
	{
		forceMove = onOrOff;
	}
}



