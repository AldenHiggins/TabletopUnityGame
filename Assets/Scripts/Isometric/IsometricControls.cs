using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IsometricControls : MonoBehaviour {


	public GameObject player;
	public GameObject linePrefab;
	public IsometricGameLogic gameLogic;
	
	private GameObject line;

	// Use this for initialization
	void Start () 
	{
		line = (GameObject) Instantiate (linePrefab);
		line.renderer.enabled = false;
	}


	void Update () 
	{
		yPressedActions ();
		xPressedActions ();
		aPressedActions ();
		bPressedActions ();
		rBPressedActions ();
	}

	private bool previousRBPressed = false;
	// Perform a "move" command
	void rBPressedActions()
	{
		RaycastHit hit;
		
		bool rBPressed = OVRGamepadController.GPC_GetButton(OVRGamepadController.Button.RightShoulder);
		
		if (!rBPressed && previousRBPressed)
		{
			gameLogic.printCurrentSoldiers();
		}
		previousRBPressed = rBPressed;
	}


	private bool previousBPressed = false;
	// Perform a "move" command
	void bPressedActions()
	{
		RaycastHit hit;
		
		bool bPressed = OVRGamepadController.GPC_GetButton(OVRGamepadController.Button.B);
		
		if (bPressed) 
		{
			displaySelectionLine (Color.yellow);
		}
		else if (!bPressed && previousBPressed)
		{
			if(Physics.Raycast(player.transform.position, player.transform.forward, out hit))
			{
				gameLogic.addPlayer(new Team("Blue", Color.blue), hit.point);
			}
			line.renderer.enabled = false;
		}
		previousBPressed = bPressed;
	}

	private bool previousAPressed = false;
	// Perform a "move" command
	void aPressedActions()
	{
		RaycastHit hit;
		
		bool aPressed = OVRGamepadController.GPC_GetButton(OVRGamepadController.Button.A);
		
		if (aPressed) 
		{
			displaySelectionLine (Color.green);
		}
		else if (!aPressed && previousAPressed)
		{
			if(Physics.Raycast(player.transform.position, player.transform.forward, out hit))
			{
				gameLogic.addPlayer(new Team("Red", Color.red), hit.point);
			}
			line.renderer.enabled = false;
		}
		
		previousAPressed = aPressed;
	}

	private bool previousXPressed = false;
	// Perform a "move" command
	void xPressedActions()
	{
		RaycastHit hit;
		
		bool xPressed = OVRGamepadController.GPC_GetButton(OVRGamepadController.Button.X);
		
		if (xPressed) 
		{
			displaySelectionLine (Color.blue);
		}
		else if (!xPressed && previousXPressed)
		{
			if(Physics.Raycast(player.transform.position, player.transform.forward, out hit))
			{
				LinkedList<IUnit> currentSoldiers = gameLogic.getSoldiers();
				
				foreach(IUnit soldier in currentSoldiers)
				{
					if (soldier == null)
						continue;
					if (soldier.getTeam().getName().Equals("Red"))
					{
						soldier.moveToPosition(hit.point);
					}
				}
			}
			line.renderer.enabled = false;
		}
		
		previousXPressed = xPressed;
	}

	private bool previousYPressed = false;
	// Perform an "attack move" command
	void yPressedActions()
	{
		RaycastHit hit;
		
		bool yPressed = OVRGamepadController.GPC_GetButton(OVRGamepadController.Button.Y);
		
		if (yPressed) 
		{
			displaySelectionLine (Color.red);
		}
		else if (!yPressed && previousYPressed)
		{
			if(Physics.Raycast(player.transform.position, player.transform.forward, out hit))
			{
				LinkedList<IUnit> currentSoldiers = gameLogic.getSoldiers();
				
				foreach(IUnit soldier in currentSoldiers)
				{
					if (soldier.isDead())
						continue;
					if (soldier.getTeam().getName().Equals("Red"))
					{
						soldier.attackMoveToPosition(hit.point);
					}
				}
			}
			line.renderer.enabled = false;
		}

		previousYPressed = yPressed;
	}




	void displaySelectionLine(Color lineColor)
	{
		LineRenderer lineRender;
		RaycastHit hit;
		if(Physics.Raycast(player.transform.position, player.transform.forward, out hit))
		{
			// Draw a line to show the player where they are aiming
			lineRender = (LineRenderer) line.renderer;
			lineRender.enabled = true;

			lineRender.SetColors (lineColor, lineColor);
			lineRender.SetPosition (0, player.transform.position - new Vector3(0.0f, 0.3f, 0.0f));
			lineRender.SetPosition (1, hit.point);
		}
	}
	
	
}
