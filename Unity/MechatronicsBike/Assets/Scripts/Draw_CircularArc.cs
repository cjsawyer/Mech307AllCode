using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter)), ExecuteInEditMode]
public class Draw_CircularArc : MonoBehaviour
{
	public int SegmentCount;
	public float OuterRadius;
	public float InnerRadius;
	public float DeltaStart;
	public float DeltaSize;

	private Mesh _mesh;

	private void Awake()
	{
		_mesh = new Mesh();
		UpdateMesh ();

		GetComponent<MeshFilter>().sharedMesh = _mesh;
	}

	public void Update() {
		//UpdateMesh();
	}

	// Call this if you change any parameters!
	public void UpdateMesh()
	{
		_mesh.Clear();

		Vector3[] vertices = new Vector3[(SegmentCount + 1) * 2];
		int[] indices = new int[SegmentCount * 6];

		for (int i = 0; i <= SegmentCount; ++i)
		{
			float angle = (DeltaStart * Mathf.Deg2Rad) + ((DeltaSize * Mathf.Deg2Rad) / SegmentCount) * i;
			Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

			vertices[i * 2] = direction * OuterRadius;
			vertices[(i * 2) + 1] = direction * InnerRadius;
		}

		for (int i = 0; i < SegmentCount; ++i)
		{
			int baseIndex = i * 6;

			indices[baseIndex] = (i * 2);
			indices[baseIndex + 1] = ((i * 2) + 1);
			indices[baseIndex + 2] = (((i + 1) * 2) + 1);

			indices[baseIndex + 3] = (((i + 1) * 2) + 1);
			indices[baseIndex + 4] = ((i + 1) * 2);
			indices[baseIndex + 5] = (i * 2);
		}

		_mesh.vertices = vertices;
		_mesh.triangles = indices;
	}
}