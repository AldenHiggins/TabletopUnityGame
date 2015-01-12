using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IsometricGameLogic : MonoBehaviour {

	public GameObject soldier;
	public GameObject table;
	public GameObject bases;

	private LinkedList<IUnit> gameUnits;
	private LinkedList<IUnit> soldiersToRemove;

	private LinkedList<IUnit> baseList;

	// Use this for initialization
	void Start () 
	{
		baseList = new LinkedList<IUnit> ();
		gameUnits = new LinkedList<IUnit> ();
		soldiersToRemove = new LinkedList<IUnit> ();

		for (int i = 0; i < bases.transform.childCount; i++)
		{
			IUnit baseUnit = (IUnit) bases.transform.GetChild (i).gameObject.GetComponent(typeof(IUnit));
			baseList.AddLast(baseUnit);
			gameUnits.AddLast (baseUnit);
			spawnSoldierOutsideBase(baseUnit);
		}
	}

	// Update is called once per frame
	void Update () 
	{
		// Check if soldiers should attack each other
		foreach (IUnit soldier in gameUnits)
		{
			if (soldier.isDead())
			{
				soldiersToRemove.AddLast(soldier);
				continue;
			}
			if (soldier.hasTarget())
			{
				continue;
			}

			float closestDistance = 2;
			IUnit closestEnemy = null;
			foreach (IUnit secondSoldier in gameUnits)
			{
				// TODO: Check if this works
				if (soldier == secondSoldier)
					continue;
				if (secondSoldier.isDead ())
					continue;
				// Do nothing if they are on the same team
				if (soldier.getTeam().getName ().Equals(secondSoldier.getTeam().getName ()))
					continue;

				float distance = Vector3.Distance (soldier.getPosition(), secondSoldier.getPosition ());

				if (distance < closestDistance)
				{
					closestEnemy = secondSoldier;
					closestDistance = distance;
				}
			}

			if (closestDistance < 1)
			{
				soldier.shootAt (closestEnemy);
			}
			else
			{
				soldier.noTarget ();
			}
		}

		// Iterate through "dead soldiers" and clean them out of the list
		foreach(IUnit deadSoldier in soldiersToRemove)
		{
			gameUnits.Remove(deadSoldier);
		}
		soldiersToRemove.Clear ();
	}

	public void addPlayer(Team playerTeam, Vector3 spawnPosition)
	{
		GameObject newSoldier = (GameObject) Instantiate (soldier, spawnPosition, Quaternion.identity);
		IUnit newIUnit = ((IUnit)(newSoldier.GetComponent (typeof(IUnit))));
		newIUnit.setTeam (playerTeam);
		gameUnits.AddLast (newIUnit);
	}

	void spawnSoldierOutsideBase(IUnit spawnBase)
	{
		NavMeshHit closestHit;
		float degreeRotation = spawnBase.getRotation ().eulerAngles.y;
		float rotation = (degreeRotation / 180) * Mathf.PI;
		Vector3 spawnPosition = spawnBase.getPosition () + new Vector3(Mathf.Cos (rotation) * -.4f, 0.0f, Mathf.Sin (rotation) * .4f);
		if( NavMesh.SamplePosition(  spawnPosition, out closestHit, 500, 1 ) )
		{
			addPlayer(spawnBase.getTeam(), closestHit.position);
		}
	}

	public LinkedList<IUnit> getSoldiers()
	{
		return gameUnits;
	}

	public void printCurrentSoldiers()
	{
		print ("Printing info about current soldiers!");
		int i = 0;
		foreach (IUnit soldier in gameUnits)
		{
			i++;
			print ("---SOLDIER---");
			print ("Team: " + soldier.getTeam ().getName ());
			print ("Position: " + soldier.getPosition());
			print ("Is this soldier firing: " + soldier.isSoldierFiring());
			IUnit thisTarget = soldier.getCurrentTarget();
			if (thisTarget == null)
				continue;
			print ("Target team: " + thisTarget.getTeam ().getName ());
			print ("Target position: " + thisTarget.getPosition());

		}
		print ("Total soldiers: " + i);
	}


}
