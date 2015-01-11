using UnityEngine;


public class Team
{
	private string name;
	private Color color;

	public Team (string nameInput, Color colorInput)
	{
		name = nameInput;
		color = colorInput;
	}

	public Color getColor()
	{
		return color;
	}

	public string getName()
	{
		return name;
	}
}


