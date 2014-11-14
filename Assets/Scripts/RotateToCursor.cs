using UnityEngine;
using System.Collections;

public class RotateToCursor : MonoBehaviour
{

	Vector3 cursorPos;

	void Update()
	{
		cursorPos = MouseToWorldPos(0);
		Vector3 diff = cursorPos - transform.position;
		diff.Normalize();
		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
	}

	public Vector3 MouseToWorldPos(float zPos)
	{
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = zPos - Camera.main.transform.position.z;
		return Camera.main.ScreenToWorldPoint(mousePos);
	}
	
}

