using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Grid : MonoBehaviour
{
    [SerializeField]
    private int xSize, ySize;
    [SerializeField]
    private float generateDelaySeconds;
    [SerializeField]
    private bool ShowVertices;

    private Mesh mesh;
    private Vector3[] gridVertices;

    void Start()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";
        StartCoroutine("Generate");
    }

    private IEnumerator Generate()
    {

        gridVertices = new Vector3[(xSize + 1) * (ySize + 1)];
        Vector2[] uv = new Vector2[gridVertices.Length];
        for (int i = 0, y = 0; y <= ySize; y++)
            for (int x = 0; x <= xSize; x++, i++)
            {
                gridVertices[i] = new Vector3(x, y);
                uv[i] = new Vector2((float) x/ xSize, (float) y / ySize);
                yield return new WaitForSeconds(generateDelaySeconds);
            }

        mesh.vertices = gridVertices;
        mesh.uv = uv;

        int[] triangles = new int[6 * xSize * ySize];
        for (int ti = 0, y = 0; y < ySize; y++)
            for (int x = 0; x < xSize; ti += 6, x++) 
       {
            triangles[ti] = y * (xSize + 1) + x;
            triangles[ti + 1] = triangles[ti + 4] = y * (xSize + 1) + (xSize + 1) + x; 
            triangles[ti + 2] = triangles[ti + 3] = y * (xSize + 1) + x + 1;
            triangles[ti + 5] = y * (xSize + 1) + xSize + (x + 2);
            mesh.triangles = triangles;
            yield return new WaitForSeconds(generateDelaySeconds);
        }
        Debug.Log(mesh.triangles.Length);
        mesh.RecalculateNormals();
        yield return 0;
    }

    void OnDrawGizmos()
    {
        if(ShowVertices == true)
        {
            if (gridVertices != null)
                for (int i = 0; i < gridVertices.Length; i++)
                    Gizmos.DrawSphere(gridVertices[i], 0.1f);
        }
    }


}
