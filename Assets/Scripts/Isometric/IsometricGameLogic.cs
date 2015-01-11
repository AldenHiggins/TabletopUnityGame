using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IsometricGameLogic : MonoBehaviour {

	public GameObject soldier;
	public GameObject table;
	public int startingSoldierCount;
	public GameObject bases;

	private LinkedList<GameObject> createdSoldiers;
	private LinkedList<GameObject> soldiersToRemove;

	private LinkedList<GameObject> baseList;

	// Use this for initialization
	void Start () 
	{
		baseList = new LinkedList<GameObject> ();
		createdSoldiers = new LinkedList<GameObject> ();
		soldiersToRemove = new LinkedList<GameObject> ();

		for (int i = 0; i < bases.transform.childCount; i++)
		{
			baseList.AddLast(bases.transform.GetChild(i).gameObject);
			IUnit addedBase = (IUnit) bases.transform.GetChild (i).gameObject.GetComponent(typeof(IUnit));
			print ("Base team: " + addedBase.getTeam().getName ());
			spawnSoldierOutsideBase(addedBase);
		}


		// Old method of starting unit generation
//		for (int i = 0; i < startingSoldierCount - 1; i++)
//		{
//			NavMeshHit closestHit;
//			if( NavMesh.SamplePosition(  table.transform.position + new Vector3(0.0f, 0.25f, i * .5f), out closestHit, 500, 1 ) )
//			{
//				addPlayer(new Team("Red", Color.red), closestHit.position);
//			}
//		}
//
//		// Create a blue team soldier as well
//		NavMeshHit closeHit;
//		if( NavMesh.SamplePosition(  table.transform.position + new Vector3(0.0f, 0.25f, -1.5f), out closeHit, 500, 1 ) )
//		{
//			addPlayer(new Team("Blue", Color.blue), closeHit.position);
//		}
	}

	// Update is called once per frame
	void Update () 
	{
		// Check if soldiers should attack each other
		foreach (GameObject soldier in createdSoldiers)
		{
			if (soldier == null)
			{
				soldiersToRemove.AddLast(soldier);
				continue;
			}
			IUnit firstIUnit = (IUnit)soldier.GetComponent (typeof(IUnit));
			bool targetFound = false;
			foreach (GameObject secondSoldier in createdSoldiers)
			{
				// TODO: Check if this works
				if (soldier == secondSoldier)
					continue;
				if (secondSoldier == null)
					continue;
				IUnit secondIUnit = (IUnit)secondSoldier.GetComponent (typeof(IUnit));
				// Do nothing if they are on the same team
				if (firstIUnit.getTeam().getName ().Equals(secondIUnit.getTeam().getName ()))
					continue;

				if (Vector3.Distance (soldier.transform.position, secondSoldier.transform.position) < 1)
				{
					firstIUnit.shootAt(secondIUnit);
					targetFound = true;
				}
			}

			if (!targetFound)
			{
				firstIUnit.noTarget ();
			}
		}

		// Iterate through "dead soldiers" and clean them out of the list
		foreach(GameObject deadSoldier in soldiersToRemove)
		{
			createdSoldiers.Remove(deadSoldier);
		}
		soldiersToRemove.Clear ();
	}

	public void addPlayer(Team playerTeam, Vector3 spawnPosition)
	{
		GameObject newSoldier = (GameObject) Instantiate (soldier, spawnPosition, Quaternion.identity);
		((IUnit)(newSoldier.GetComponent(typeof(IUnit)))).setTeam (playerTeam);
		createdSoldiers.AddLast (newSoldier);
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

	public LinkedList<GameObject> getSoldiers()
	{
		return createdSoldiers;
	}

	public void printCurrentSoldiers()
	{
		print ("Printing info about current soldiers!");
		int i = 0;
		foreach (GameObject soldier in createdSoldiers)
		{
			i++;
			IUnit thisSoldier = ((IUnit)(soldier.GetComponent(typeof(IUnit))));
			print ("---SOLDIER---");
			print ("Team: " + thisSoldier.getTeam ().getName ());
			print ("Position: " + soldier.transform.position);
			print ("Is this soldier firing: " + thisSoldier.isSoldierFiring());
			IUnit thisTarget = thisSoldier.getCurrentTarget();
			if (thisTarget == null)
				continue;
			print ("Target team: " + thisTarget.getTeam ().getName ());
			print ("Target position: " + thisTarget.getPosition());

		}
		print ("Total soldiers: " + i);
	}


}
