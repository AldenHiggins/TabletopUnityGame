using UnityEngine;

public interface IUnit
{
	void attackMoveToPosition(Vector3 newDestination);

	void moveToPosition(Vector3 newDestination);

	void setTeam(Team newTeam);

	Team getTeam();

	void shootAt(IUnit target);

	Vector3 getPosition();

	void noTarget();

	void dealDamage(int damageDealt, IUnit attackingUnit);

	bool isDead();

	IUnit getCurrentTarget();

	bool isSoldierFiring();

	Quaternion getRotation();

	bool hasTarget();

	bool isSelected();

	void setSelected(bool selectedOrNot);

	void setFocused(bool focusedOrNot);

	string getName();

	void useSpecialAbility();

	SpecialAbilityType getSpecialAbility();
}


