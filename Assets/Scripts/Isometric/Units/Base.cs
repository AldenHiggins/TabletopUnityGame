using UnityEngine;
using System.Collections;

public class Base : MonoBehaviour, IUnit
{
	public GameObject greenHealthBar;
	public GameObject redHealthBar;

	private float startingHealthBarZScale;

	private Team currentTeam;
	private int health;


	
	private GameObject thisGreenHealthBar;
	private GameObject thisRedHealthBar;

	void Start()
	{
		thisGreenHealthBar = (GameObject)Instantiate (greenHealthBar, transform.position, Quaternion.identity);
		thisRedHealthBar = (GameObject)Instantiate (redHealthBar, transform.position, Quaternion.identity);
		startingHealthBarZScale = thisGreenHealthBar.transform.localScale.z;
		health = 100;
		// Hacky hard-coded check, change later
		if (gameObject.name.Equals("RedTent"))
		{
			currentTeam = new Team("Red", Color.red);
		}
		else
		{
			currentTeam = new Team("Blue", Color.blue);
		}
	}

	void Update()
	{
		// Display healthbars in the correct place
		thisGreenHealthBar.transform.position = transform.position + new Vector3 (0.0f, 0.2083f, 0.0f);
		Vector3 currentScale = thisGreenHealthBar.transform.localScale;
		thisGreenHealthBar.transform.localScale = new Vector3(currentScale.x, currentScale.y, (startingHealthBarZScale * health / 100));
		thisRedHealthBar.transform.position = transform.position + new Vector3 (0.0f, 0.2083f, 0.0f);
	}


	// Do nothing, bases don't move
	public void attackMoveToPosition(Vector3 newDestination){}
	// Do nothing, bases don't move 
	public void moveToPosition(Vector3 newDestination){}
	
	public void setTeam(Team newTeam)
	{
		currentTeam = newTeam;
	}
	
	public Team getTeam()
	{
		return currentTeam;
	}
	
	public void shootAt(IUnit target)
	{

	}
	
	public Vector3 getPosition()
	{
		return transform.position;
	}
	
	public void noTarget()
	{

	}
	
	public void dealDamage(int damageDealt)
	{
		health -= damageDealt;
		if (health < 0)
		{
			// TODO: implement bases switching teams
			health = 100;
		}
	}
	
	public bool isDead()
	{
		return false;
	}
	
	public IUnit getCurrentTarget()
	{
		return null;
	}
	
	public bool isSoldierFiring()
	{
		return false;
	}

	public Quaternion getRotation()
	{
		return transform.rotation;
	}
}


