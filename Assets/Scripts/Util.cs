using UnityEngine;
using System.Collections.Generic;
using PriorityQueueDemo;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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

	public static T GetRandomElement<T>(T[] array)
	{
		return array[Random.Range (0,array.Length)];
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

	//Gets a centroid defined bounding box that includes all child collider2Ds
	public static Bounds Get2DObjectBounds(GameObject obj)
	{
		Collider2D[] colliders = obj.GetComponentsInChildren<Collider2D>();
		Vector2 max = new Vector2(float.MinValue,float.MinValue);
		Vector2 min = new Vector2(float.MaxValue,float.MaxValue);
		Vector2 centerSum = Vector2.zero;
		foreach(Collider2D col in colliders) {
			Vector3 max3 = col.bounds.center + col.bounds.extents;
			Vector3 min3 = col.bounds.center - col.bounds.extents;
			max.x = Mathf.Max(max.x,max3.x);
			max.y = Mathf.Max(max.y,max3.y);
			min.x = Mathf.Min(min.x,min3.x);
			min.y = Mathf.Min(min.y,min3.y);
			centerSum += (Vector2)col.bounds.center;
		}
		Vector2 centroid = centerSum/colliders.Length;
		Vector2 extents = new Vector2();
		extents.x = Mathf.Max(Mathf.Abs(centroid.x-max.x),Mathf.Abs(centroid.x - min.x));
		extents.y = Mathf.Max(Mathf.Abs(centroid.y-max.y),Mathf.Abs(centroid.y - min.y));
		Bounds bounds = new Bounds(centroid,2*extents);
		return bounds;
	}

	//gets the smallest bounding box that fits all child colliders
	public static Bounds GetSmallest2DObjectBounds(GameObject obj)
	{
		Collider2D[] colliders = obj.GetComponentsInChildren<Collider2D>();
		Vector2 max = new Vector2(float.MinValue,float.MinValue);
		Vector2 min = new Vector2(float.MaxValue,float.MaxValue);
		foreach(Collider2D col in colliders) {
			Vector3 max3 = col.bounds.center + col.bounds.extents;
			Vector3 min3 = col.bounds.center - col.bounds.extents;
			max.x = Mathf.Max(max.x,max3.x);
			max.y = Mathf.Max(max.y,max3.y);
			min.x = Mathf.Min(min.x,min3.x);
			min.y = Mathf.Min(min.y,min3.y);
		}
		Vector2 size = max - min;
		Vector2 center = max - size/2;
		Bounds bounds = new Bounds(center,size);
		return bounds;
	}

	public static bool IntersectsAny(Bounds bounds, List<Bounds> boundsList)
	{
		foreach(Bounds b in boundsList)
			if(bounds.Intersects(b))
				return true;
		return false;
	}

	public static Vector2 RandomVectorBetween(Vector2 origin, Vector2 p1, Vector2 p2)
	{
		Vector2 v1 = p1-origin;
		Vector2 v2 = p2-origin;
		float angle = Vector2.Angle(v1,v2);
		//Debug.Log ("Angle - " + angle);
		return Quaternion.AngleAxis(Random.Range(0,angle),Vector3.forward)*v1;
	}

	public static Quaternion RandomRotation()
	{
		return Quaternion.Euler(RandomVector3(0,360));
	}

	public static Quaternion RandomRotation(Vector3 axis, float minAngle, float maxAngle)
	{
		return Quaternion.AngleAxis(Random.Range(minAngle,maxAngle),axis);
	}

	static float[] signs = new float[]{-1,1};

	public static float RandomSign()
	{
		return GetRandomElement<float>(signs);
	}

	public static void DestroyChildrenWithComponent<T>(Transform transform)
	{
		List<Transform> toDestroy = new List<Transform>();
		CollectChildrenToBeDestroyed(transform, ref toDestroy);
		foreach(Transform t in toDestroy)
			if(t.GetComponent(typeof(T)))
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



	public static bool TryLoadFromPlayerPrefs<T>(string key, out T obj)
	{
		string data = PlayerPrefs.GetString(key);
		obj = default(T);
		if(!string.IsNullOrEmpty(data)) {
			//Binary formatter for loading back
			BinaryFormatter b = new BinaryFormatter();
			//Create a memory stream with the data
			MemoryStream m = new MemoryStream(System.Convert.FromBase64String(data));
			//Load back the scores
			obj = (T)b.Deserialize(m);
			return true;
		}
		return false;
	}

	public static void SaveToPlayerPrefs<T>(string key, T obj)
	{
		//Get a binary formatter
		var b = new BinaryFormatter();
		//Create an in memory stream
		var m = new MemoryStream();
		//Save the scores
		b.Serialize(m, obj);
		//Add it to player prefs
		PlayerPrefs.SetString(key,System.Convert.ToBase64String(m.GetBuffer()));
	}
	
	public static bool ExistsWithinSphere(Vector3 point, float radius, string searchTag)
	{
		Collider[] cols = Physics.OverlapSphere(point,radius);
		foreach(Collider col in cols)
			if(col.CompareTag(searchTag))
				return true;
		return false;
	}

	public static Color GetRandomPixel(Texture2D texture)
	{
		return texture.GetPixel(Random.Range(0,texture.width),Random.Range(0,texture.height));
	}

	public static Color GetAverageColor(Texture2D texture, int sampleRate)
	{
		Color[] pixels = texture.GetPixels();
		float r = 0;
		float g = 0;
		float b = 0;
		int samples = 0;
		for(int i = 0; i < pixels.Length; i += sampleRate) {
			r += pixels[i].r;
			g += pixels[i].g;
			b += pixels[i].b;
			samples++;
		}
		r = r/samples;
		g = g/samples;
		b = b/samples;
		//Debug.Log ("Sample color " + (new Color(r,g,b)).ToString());
		return new Color(r,g,b);
	}

}

