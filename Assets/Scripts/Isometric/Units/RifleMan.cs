﻿using UnityEngine;
using System.Collections;

public class RifleMan : MonoBehaviour, IUnit 
{
	Animator anim;
	NavMeshAgent agent;

	public GameObject ragDoll;
	public GameObject muzzleFlashParticle;
	public CircleDrawing drawing;

	private UnitMethods unitMethods;

	private Team team;
	private int health;
	private bool isSoldierDead;
	private GameObject highlightCircle;
	private bool selectedBool;

	// Use this for initialization
	void Awake () 
	{
		// Initialize common unit functionality
		unitMethods = transform.GetChild (0).gameObject.GetComponent<UnitMethods> ();
		unitMethods.createHealthBars (transform.position, .07f, new Vector3 (0.0f, 0.2083f, 0.0f));
		unitMethods.setAttackCallback (attackFunction);
		unitMethods.setParent (this);

		highlightCircle = drawing.createCircle (transform.position, Color.green, 0.03f);
		highlightCircle.renderer.enabled = false;
		isSoldierDead = false;
		selectedBool = false;
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
		// Display circle if the unit is highlighted
		if (selectedBool)
		{
			print ("Moving the highlight circle....or are we?");
			print ("New position: " + transform.position);
			highlightCircle.transform.position = transform.position;
		}
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
					unitMethods.setForceMove(false);
				}
			}
		}
	}

	void attackFunction(IUnit target)
	{
		Instantiate (muzzleFlashParticle, transform.position + new Vector3(0.0f, 0.13f, 0.0f), transform.rotation);
		transform.rotation = unitMethods.faceTarget (transform.position, target.getPosition ());
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
			Destroy (highlightCircle);
		}
	}

	public void moveToPosition(Vector3 newDestination)
	{
		unitMethods.setForceMove (true);
		anim.SetBool ("Walking", true);
		agent.SetDestination (newDestination);
	}

	public void attackMoveToPosition(Vector3 newDestination) 
	{
		unitMethods.setForceMove (false);
		anim.SetBool ("Walking", true);
		agent.SetDestination (newDestination);

		if (unitMethods.getCurrentTarget() != null)
		{
			agent.Stop ();
		}
	}

	public void setTeam(Team newTeam)
	{
		team = newTeam;
		transform.GetChild (1).gameObject.GetComponent<SkinnedMeshRenderer> ().materials [0].color = newTeam.getColor ();
		unitMethods.setTeam (newTeam);
	}

	public Team getTeam()
	{
		return team;
	}

	public void shootAt(IUnit target)
	{
		if (unitMethods.sharedShoot (target))
		{
			anim.SetBool ("Aiming", true);
			agent.Stop ();
			transform.rotation = unitMethods.faceTarget (transform.position, target.getPosition ());
		}
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

	public void setSelected(bool selectedOrNot)
	{
		if (selectedOrNot == true)
		{
			highlightCircle.renderer.enabled = true;
		}
		else
		{
			highlightCircle.renderer.enabled = false;
		}
		selectedBool = selectedOrNot;
	}

	public bool isSelected()
	{
		return selectedBool;
	}
}
