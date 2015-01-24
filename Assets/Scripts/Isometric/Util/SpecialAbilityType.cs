
// A special class to contain all of the relevant information to tell a controller
// about a unit's ability
public class SpecialAbilityType
{
	// 0 for no ability
	// 1 for targeted ability
	// 2 for self ability
	public int type;
	public float radius;

	public SpecialAbilityType (int typeInput, float radiusInput)
	{
		type = typeInput;
		radius = radiusInput;
	}
}


