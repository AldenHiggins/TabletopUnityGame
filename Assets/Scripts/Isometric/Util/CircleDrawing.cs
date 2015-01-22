using UnityEngine;
using System.Collections;

public class CircleDrawing : MonoBehaviour {

	public int numberOfSides;
	public GameObject linePrefab;


	public GameObject createCircle(Vector3 lineCenter, Color lineColor, float lineRadius)
	{
		GameObject line = (GameObject)Instantiate (linePrefab, new Vector3 (0.0f, 0.0f, 0.0f), Quaternion.identity);

		line.transform.position = lineCenter;
		LineRenderer lineRender;
		
		// Draw a line to show the player where they are aiming
		lineRender = (LineRenderer) line.renderer;
		lineRender.enabled = true;
		
		lineRender.SetColors (lineColor, lineColor);
		lineRender.SetVertexCount (numberOfSides);
		
		float radsPerLine = 2 * Mathf.PI / (numberOfSides - 1);
		for (int i = 0; i < numberOfSides; i++)
		{
			float zChange = lineRadius * Mathf.Cos (i * radsPerLine);
			float xChange = lineRadius * Mathf.Sin (i * radsPerLine);
//			lineRender.SetPosition (i, lineCenter + new Vector3(xChange, 0.0f, zChange));
			lineRender.SetPosition (i, new Vector3(xChange, 0.0f, zChange));
		}
		return line;
	}

	public GameObject createSelectionBox(Color lineColor)
	{
		GameObject line = (GameObject)Instantiate (linePrefab, new Vector3 (0.0f, 0.0f, 0.0f), Quaternion.identity);
		
		LineRenderer lineRender;
		
		// Draw a line to show the player where they are aiming
		lineRender = (LineRenderer) line.renderer;
		lineRender.enabled = true;
		
		lineRender.SetColors (lineColor, lineColor);
		lineRender.SetVertexCount (5);
	
		return line;
	}


	public GameObject getPathLine()
	{
		GameObject line = (GameObject)Instantiate (linePrefab, new Vector3 (0.0f, 0.0f, 0.0f), Quaternion.identity);

		LineRenderer lineRender;
		
		// Draw a line to show the player where they are aiming
		lineRender = (LineRenderer) line.renderer;
		lineRender.enabled = false;

		return line;
	}


}
