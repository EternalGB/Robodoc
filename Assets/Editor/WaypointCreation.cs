using UnityEngine;
using UnityEditor;
using System.Collections;

public class WaypointCreation : EditorWindow
{

	enum ShapeType
	{
		CIRCLE,NGON,FIGURE8
	}

	[MenuItem("Window/Waypoints")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow<WaypointCreation>();
	}

	ShapeType shape = ShapeType.NGON;
	float radius = 0;
	int segments = 0;

	int ngonSides = 3;
	float size = 1;

	float figure8Size = 1;
	int figure8Segments = 10;

	void OnGUI()
	{
		GameObject parent = null;
		shape = (ShapeType)EditorGUILayout.EnumPopup("Shape",shape);
		if(shape == ShapeType.CIRCLE) {
			radius = Mathf.Clamp(EditorGUILayout.FloatField("Radius",radius),0,float.MaxValue);
			segments = Mathf.Clamp (EditorGUILayout.IntField("Segments",segments),0,int.MaxValue);
			if(GUILayout.Button("Create")) {
				parent = CreateCircle(radius,segments);
			}
		} else if(shape == ShapeType.NGON) {
			ngonSides = Mathf.Clamp(EditorGUILayout.IntField("N",ngonSides),3,int.MaxValue);
			size = Mathf.Clamp(EditorGUILayout.FloatField("Size",size),1,float.MaxValue);
			if(GUILayout.Button("Create")) {
				parent = CreateNgon(ngonSides,size);
			}
		} else if(shape == ShapeType.FIGURE8) {
			figure8Size = Mathf.Clamp(EditorGUILayout.FloatField("Size",figure8Size),0,float.MaxValue);
			figure8Segments = Mathf.Clamp (EditorGUILayout.IntField("Segments",figure8Segments),0,int.MaxValue);
			if(GUILayout.Button("Create")) {
				parent = CreateFigureEight(figure8Size,figure8Segments);
			}
		}
		if(parent != null) {
			Undo.RegisterCreatedObjectUndo(parent, "Created Waypoints");
		}

	}

	GameObject CreateCircle(float radius, int resolution)
	{
		GameObject parent = new GameObject("WP-Circle",typeof(WaypointPathDrawer));
		parent.transform.position = Vector3.zero;

		float angle = Mathf.PI*2/resolution;
		for(int i = 0; i < resolution; i++) {
			GameObject next = new GameObject((i+1).ToString());
			next.transform.position = radius*(new Vector3(Mathf.Cos(angle*i),Mathf.Sin (angle*i)));
			next.transform.parent = parent.transform;
		}
		return parent;
	}

	GameObject CreateNgon(int n, float size)
	{
		GameObject parent = new GameObject("WP-" + n + "-GON",typeof(WaypointPathDrawer));
		parent.transform.position = Vector3.zero;

		float internalAngle = (n-2)*180f/n;
		Vector3 top = Vector3.up;
		for(int i = 0; i < n; i++) {
			GameObject next = new GameObject((i+1).ToString());
			next.transform.position = Quaternion.AngleAxis(i*(180-internalAngle),Vector3.forward)*(size*top);
			next.transform.parent = parent.transform;
		}
		return parent;
	}

	GameObject CreateFigureEight(float a, int resolution)
	{
		GameObject parent = new GameObject("WP-Figure8",typeof(WaypointPathDrawer));
		parent.transform.position = Vector3.zero;


		float angleIncr = Mathf.PI/resolution;
		for(float angle = -Mathf.PI/4; angle <= Mathf.PI/4; angle += angleIncr) {
			GameObject next = new GameObject((angle).ToString());
			next.transform.position = GetFigureEightPoint(a,angle);
			next.transform.parent = parent.transform;
		}
		for(float angle = 5*Mathf.PI/4; angle >= 3*Mathf.PI/4; angle -= angleIncr) {
			GameObject next = new GameObject((angle).ToString());
			next.transform.position = GetFigureEightPoint(a,angle);
			next.transform.parent = parent.transform;
		}
		return parent;
	}

	Vector2 GetFigureEightPoint(float a, float angle)
	{
		float r = Mathf.Pow(a,2)*Mathf.Cos(2*angle)*Mathf.Pow(1/Mathf.Cos(angle),4);
		if(r != 0)
			r = Mathf.Sqrt(r);
		Vector2 pos = new Vector2(r,0);
		pos = Quaternion.AngleAxis(Mathf.Rad2Deg*angle,Vector3.forward)*pos;
		return pos;
	}

}

