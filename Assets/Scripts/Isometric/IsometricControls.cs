using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IsometricControls : MonoBehaviour {


	public GameObject player;
	public GameObject linePrefab;
	public IsometricGameLogic gameLogic;
	public CircleDrawing drawing;

	private LinkedList<IUnit> selectedUnits;

	private GameObject line;
	private GameObject selectionBox;

	// Use this for initialization
	void Start () 
	{
		line = (GameObject) Instantiate (linePrefab);
		line.renderer.enabled = false;
		selectionBox = drawing.createSelectionBox (Color.green);
		selectionBox.renderer.enabled = false;
		selectedUnits = new LinkedList<IUnit> ();
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
	private Vector3 firstSelectionCorner;
	private Vector3 secondSelectionCorner;
	// Perform a "move" command
	void rBPressedActions()
	{
		RaycastHit hit;
		
		bool rBPressed = OVRGamepadController.GPC_GetButton(OVRGamepadController.Button.RightShoulder);

		if (rBPressed && !previousRBPressed)
		{
			if(Physics.Raycast(player.transform.position, player.transform.forward, out hit))
			{
				if (!hit.collider.gameObject.name.Equals("TableTop"))
				{
					firstSelectionCorner = new Vector3(0.0f,0.0f,0.0f);
				}
				else
				{
					firstSelectionCorner = hit.point;
				}
			}
			else
			{
				firstSelectionCorner = new Vector3(0.0f,0.0f,0.0f);
			}
		}
		else if (rBPressed)
		{
			displaySelectionLine (Color.green);
			// TODO: get rid of extra raycast
			if(Physics.Raycast(player.transform.position, player.transform.forward, out hit))
			{
				secondSelectionCorner = hit.point;
				if (!hit.collider.gameObject.name.Equals("TableTop"))
				{
//					secondSelectionCorner = new Vector3(0.0f,0.0f,0.0f);
				}
				else if (firstSelectionCorner != new Vector3(0.0f, 0.0f, 0.0f))
				{
					LineRenderer lineRender = (LineRenderer) selectionBox.renderer;
					lineRender.enabled = true;
					lineRender.SetPosition (0, firstSelectionCorner);
					lineRender.SetPosition (1, new Vector3(firstSelectionCorner.x, firstSelectionCorner.y, secondSelectionCorner.z));
					lineRender.SetPosition (2, secondSelectionCorner);
					lineRender.SetPosition (3, new Vector3(secondSelectionCorner.x, firstSelectionCorner.y, firstSelectionCorner.z));
					lineRender.SetPosition (4, firstSelectionCorner);
				}
			}
		}
		else if (!rBPressed && previousRBPressed)
		{
			foreach (IUnit selected in selectedUnits)
			{
				selected.setSelected(false);
			}
			selectedUnits.Clear ();

			if((firstSelectionCorner != new Vector3(0.0f, 0.0f, 0.0f)) && (secondSelectionCorner != new Vector3(0.0f, 0.0f, 0.0f)))
			{
				selectedUnits = gameLogic.returnUnitsInArea(firstSelectionCorner, secondSelectionCorner);
			}
			// Turn off the selection line
			line.renderer.enabled = false;
			LineRenderer lineRender = (LineRenderer) selectionBox.renderer;
			lineRender.enabled = false;
		}
		previousRBPressed = rBPressed;
	}

	private bool previousLBPressed = false;
	// Perform a "move" command
	void lBPressedActions()
	{
		RaycastHit hit;
		
		bool lBPressed = OVRGamepadController.GPC_GetButton(OVRGamepadController.Button.LeftShoulder);

		if (lBPressed && !previousLBPressed)
		{

		}
		else if (lBPressed)
		{
			displaySelectionLine (Color.green);
		}
		else if (!lBPressed && previousLBPressed)
		{

		}
		previousLBPressed = lBPressed;
	}


	private bool previousBPressed = false;
	// Perform a "move" command
	void bPressedActions()
	{
		RaycastHit hit;
		
		bool bPressed = OVRGamepadController.GPC_GetButton(OVRGamepadController.Button.B);
		
		if (bPressed) 
		{
			displaySelectionLine (Color.blue);
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
			displaySelectionLine (Color.red);
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
				foreach(IUnit soldier in selectedUnits)
				{
					if (soldier.isDead())
						continue;

					soldier.moveToPosition(hit.point);
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
				foreach(IUnit soldier in selectedUnits)
				{
					if (soldier.isDead())
						continue;

					soldier.attackMoveToPosition(hit.point);
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
