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

	void dealDamage(int damageDealt);

	bool isDead();

	IUnit getCurrentTarget();

	bool isSoldierFiring();

	Quaternion getRotation();
}


