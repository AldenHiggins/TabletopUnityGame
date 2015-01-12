using UnityEngine;

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

	private IUnit currentTarget;

	void Update()
	{
		if (currentTarget != null)
		{

		}
		else
		{

		}
	}

	void Awake ()
	{

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

	public void sharedShoot(IUnit target)
	{
//		// Don't pick up a target if the move command was issued
//		if (forceMove)
//			return;
//		if (currentTarget != null)
//		{
//			if (currentTarget.isDead ())
//			{
//				currentTarget = null;
//			}
//			else
//			{
//				agent.Stop ();
//				return;
//			}
//		}
//		// Clear current destination
//		agent.Stop ();
//		
//		currentTarget = target;
//		anim.SetBool ("Aiming", true);
//		
//		
//		Vector3 vectorToTarget = target.getPosition () - transform.position;
//		float radianAngle = Mathf.Atan2 (vectorToTarget.x, vectorToTarget.z);
//		float degreesAngle = (radianAngle / Mathf.PI) * 180;
//		
//		transform.rotation = Quaternion.Euler (0.0f, degreesAngle, 0.0f);
	}

	public Quaternion faceTarget(Vector3 position, Vector3 faceThisPosition)
	{
		Vector3 vectorToTarget = faceThisPosition - position;
		float radianAngle = Mathf.Atan2 (vectorToTarget.x, vectorToTarget.z);
		float degreesAngle = (radianAngle / Mathf.PI) * 180;
		
		return Quaternion.Euler (0.0f, degreesAngle, 0.0f);
	}



//	public UnitMethods()
//	{
//		print ("Starting public variable: " + test);
//	}

//	void attackMoveToPosition(Vector3 newDestination);
//	
//	void moveToPosition(Vector3 newDestination);
//	
//	void setTeam(Team newTeam);
//	
//	Team getTeam();
//	
//	void shootAt(IUnit target);
//	
//	Vector3 getPosition();
//	
//	void noTarget();
//	
//	void dealDamage(int damageDealt, IUnit attackingUnit);
//	
//	bool isDead();
//	
//	IUnit getCurrentTarget();
//	
//	bool isSoldierFiring();
//	
//	Quaternion getRotation();
}



