using UnityEngine;
using System.Collections.Generic;
using PriorityQueueDemo;

public class Util
{

	public static Vector3 MouseToWorldPos(float zPos)
	{
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = zPos - Camera.main.transform.position.z;
		return Camera.main.ScreenToWorldPoint(mousePos);
	}

	public static Vector3 ScreenPointToWorld(Vector3 screenPoint, float zPos)
	{
		screenPoint.z = zPos - Camera.main.transform.position.z;
		return Camera.main.ScreenToWorldPoint(screenPoint);
	}

	public static Vector3 RandomVector3(Vector3 min, Vector3 max)
	{
		return new Vector3(Random.Range(min.x,max.x),Random.Range (min.y,max.y),Random.Range (min.z,max.z));
	}

	public static Vector3 RandomVector3(float minLen, float maxLen)
	{
		return Random.insideUnitSphere.normalized*Random.Range(minLen,maxLen);
	}

	public static Vector2 RandomVector2(float minLen, float maxLen)
	{
		return Random.insideUnitSphere.normalized*Random.Range(minLen,maxLen);
	}
		
	public static T GetRandomElement<T>(List<T> list)
	{
		return list[Random.Range(0,list.Count)];
	}

	public static bool PointInside(Vector2 topLeft, Vector2 bottomRight, Vector2 point)
	{
		return point.x <= bottomRight.x && point.x >= topLeft.x &&
			point.y <= topLeft.y && point.y >= bottomRight.y;

	}

	public static bool PointInside(Vector2 center, float radius, Vector2 point)
	{
		return Vector2.Distance(point,center) < radius;
	}

	public static Vector2 RandomPointInside(Vector2 topLeft, Vector2 bottomRight)
	{
		return new Vector2(Random.Range (topLeft.x,bottomRight.x),Random.Range (bottomRight.y,topLeft.y));
	}

	public static Vector2 RandomPointInside(Vector2 center, float radius)
	{
		return Random.insideUnitCircle*radius + center;
	}

	public static Vector2 RandomPointInside(Collider2D col)
	{

		Vector2 point;
		do {
			point = RandomPointInside(col.bounds);
		} while(!col.OverlapPoint(point));
		return point;
	}

	public static Vector3 RandomPointInside(Bounds bounds)
	{
		return bounds.center + RandomVector3(-bounds.extents,bounds.extents);
	}

	public static Vector2 RandomVectorBetween(Vector2 origin, Vector2 p1, Vector2 p2)
	{
		Vector2 v1 = p1-origin;
		Vector2 v2 = p2-origin;
		float angle = Vector2.Angle(v1,v2);
		//Debug.Log ("Angle - " + angle);
		return Quaternion.AngleAxis(Random.Range(0,angle),Vector3.forward)*v1;
	}

	public static void DestroyChildren(Transform transform)
	{
		List<Transform> toDestroy = new List<Transform>();
		CollectChildrenToBeDestroyed(transform, ref toDestroy);
		foreach(Transform t in toDestroy)
			t.SendMessage("Destroy", SendMessageOptions.DontRequireReceiver);
	}
	
	static void CollectChildrenToBeDestroyed(Transform t, ref List<Transform> toDestroy)
	{
		//Debug.Log ("Clearing children of " + t.name + " " + t.GetInstanceID() + "\n has " + t.childCount + " children");
		foreach(Transform child in t) {
			CollectChildrenToBeDestroyed(child, ref toDestroy);
			toDestroy.Add(child);
			//child.GetComponent<AttachedBall>().Destroy();
		}
		
	}

	//just does minutes, seconds
	public static string FormatTime(float seconds)
	{
		System.TimeSpan ts = new System.TimeSpan(0,0,(int)seconds);
		string secs = ts.Seconds.ToString();
		if(ts.Seconds < 10)
			secs = "0" + secs;
		return ts.Minutes.ToString() + ":" + secs;
	}

	public static List<Vector3> GetNClosest(int n, Vector3 anchor, List<Vector3> points)
	{
		List<Vector3> returnPoints = new List<Vector3>(points);
		if(returnPoints.Count < n)
			return returnPoints;

		returnPoints.Sort(new Vector3DistanceToPointComparer(anchor));
		returnPoints.RemoveRange(n,returnPoints.Count - n);
		return returnPoints;
	}

	public class Vector3DistanceToPointComparer : IComparer<Vector3>
	{

		Vector3 point;

		public Vector3DistanceToPointComparer(Vector3 point)
		{
			this.point = point;
		}

		public int Compare(Vector3 v1, Vector3 v2)
		{
			return (int)(Vector3.Distance(point,v1) - Vector3.Distance(point,v2));
		}

	}

	public static void DebugDrawConnected(List<Vector3> points, Color color, float duration)
	{
		if(points.Count > 1) {
			for(int i = 0; i < points.Count-1; i++) {
				Debug.DrawLine(points[i],points[i+1],color,duration);
			}
			Debug.DrawLine(points[points.Count-1],points[0],color,duration);
		}
	}

	public static float AngleBetween(Vector3 origin, Vector3 v1, Vector3 v2)
	{
		return Vector3.Angle(v1-origin,v2-origin);
	}

	public static void SetMaterialAllChildren(Transform t, Material mat)
	{
		t.renderer.sharedMaterial = mat;
		foreach(Transform child in t)
			SetMaterialAllChildren(child,mat);
	}
	


}

