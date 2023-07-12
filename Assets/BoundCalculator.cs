using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundCalculator : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private Mesh _mesh;

    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter = _meshRenderer.GetComponent<MeshFilter>();
        _mesh = _meshFilter.mesh;
    }

    // Update is called once per frame
    void OnDrawGizmos()
    {
        var vertices = _mesh.vertices;
        if (vertices.Length <= 0) return;

        // TransformPoint converts the local mesh vertice dependent on the transform
        // position, scale and orientation into a global position
        var min = transform.TransformPoint(vertices[0]);
        var max = min;

        // Iterate through all vertices
        // except first one
        for (var i = 1; i < vertices.Length; i++)
        {
            var V = transform.TransformPoint(vertices[i]);

            // Go through X,Y and Z of the Vector3
            for (var n = 0; n < 3; n++)
            {
                max = Vector3.Max(V, max);
                min = Vector3.Min(V, min);
            }
        }

        var bounds = new Bounds();
        bounds.SetMinMax(min, max);


        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
        Gizmos.DrawWireSphere(bounds.center, 0.3f);
    }
}
