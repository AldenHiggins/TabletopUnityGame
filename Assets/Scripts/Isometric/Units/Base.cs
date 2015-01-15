using UnityEngine;
using System.Collections;

public class Base : MonoBehaviour, IUnit
{
	public GameObject explosion;

	private Team currentTeam;
	private int health;

	private UnitMethods unitMethods;
	private bool selectedBool;

	void Awake()
	{
		unitMethods = transform.GetChild (0).gameObject.GetComponent<UnitMethods> ();
		unitMethods.createHealthBars (transform.position, .07f, new Vector3 (0.0f, 0.2083f, 0.0f));
		unitMethods.setAttackCallback (attackFunction);
		unitMethods.setParent (this);
		selectedBool = false;
		health = 100;
		// Hacky hard-coded check, change later
		if (gameObject.name.Equals("RedTent"))
		{
			setTeam (new Team("Red", Color.red));
		}
		else
		{
			setTeam (new Team("Blue", Color.blue));
		}
	}

	void Update()
	{
		unitMethods.displayHealth (transform.position, health, 100);
	}

	void attackFunction(IUnit currentTarget)
	{
		Instantiate (explosion, currentTarget.getPosition(), Quaternion.identity);
	}

	// Do nothing, bases don't move
	public void attackMoveToPosition(Vector3 newDestination){}
	// Do nothing, bases don't move 
	public void moveToPosition(Vector3 newDestination){}
	
	public void setTeam(Team newTeam)
	{
		currentTeam = newTeam;
		transform.gameObject.GetComponent<MeshRenderer> ().materials [0].color = newTeam.getColor ();
		unitMethods.setTeam (newTeam);
	}
	
	public Team getTeam()
	{
		return currentTeam;
	}
	
	public void shootAt(IUnit target)
	{
		if (unitMethods.sharedShoot (target))
		{
			// Functionality if the base starts shooting
		}
	}
	
	public Vector3 getPosition()
	{
		return transform.position;
	}
	
	public void noTarget()
	{

	}
	
	public void dealDamage(int damageDealt, IUnit attackingUnit)
	{
		health -= damageDealt;
		if (health < 0)
		{
			health = 100;
			setTeam (attackingUnit.getTeam ());
		}
	}
	
	public bool isDead()
	{
		return false;
	}
	
	public IUnit getCurrentTarget()
	{
		return unitMethods.getCurrentTarget();
	}
	
	public bool isSoldierFiring()
	{
		return unitMethods.isSoldierFiring();
	}

	public Quaternion getRotation()
	{
		return transform.rotation;
	}

	public bool hasTarget()
	{
		if (unitMethods.getCurrentTarget() != null)
			return true;
		return false;
	}

	public bool isSelected()
	{
		return selectedBool;
	}

	public void setSelected(bool selectedOrNot)
	{
		selectedBool = selectedOrNot;
	}

	public string getName()
	{
		return gameObject.name;
	}
}


