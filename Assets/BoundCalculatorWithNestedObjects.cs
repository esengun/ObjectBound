using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundCalculatorWithNestedObjects : MonoBehaviour
{
    private MeshRenderer[] _meshRenderer;

    void Start()
    {
        _meshRenderer = GetComponentsInChildren<MeshRenderer>();
    }

    Vector3 min;
    Vector3 max;

    // Update is called once per frame
    void OnDrawGizmos()
    {
        MeshRenderer renderer = _meshRenderer[0];       

        var minMax = GetMinMax(renderer);
        min = minMax.Min; 
        max = minMax.Max;

        for (int i = 1; i < _meshRenderer.Length; i++)
        {
            var minMaxSub = GetMinMax(_meshRenderer[i]);
            max = Vector3.Max(minMaxSub.Max, max);
            min = Vector3.Min(minMaxSub.Min, min);
        }
        var bounds = new Bounds();
        bounds.SetMinMax(min, max);

        Debug.Log($"Size: {bounds.size}");
        Debug.Log($"Extents: {bounds.extents}");
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
        Gizmos.DrawWireSphere(bounds.center, 0.3f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(bounds.center, new Vector3(bounds.size.x, bounds.size.y, 0.2f));
        Gizmos.DrawWireSphere(bounds.center, 0.3f);
    }

    private (Vector3 Min,Vector3 Max) GetMinMax(MeshRenderer meshRenderer)
    {
        var meshFilter = meshRenderer.GetComponent<MeshFilter>();
        var mesh = meshFilter.mesh;
        var vertices = mesh.vertices;

        // TransformPoint converts the local mesh vertice dependent on the transform
        // position, scale and orientation into a global position
        var min = meshFilter.transform.TransformPoint(vertices[0]);
        var max = min;

        // Iterate through all vertices
        // except first one
        for (var i = 1; i < vertices.Length; i++)
        {
            var V = meshFilter.transform.TransformPoint(vertices[i]);

            // Go through X,Y and Z of the Vector3
            for (var n = 0; n < 3; n++)
            {
                max = Vector3.Max(V, max);
                min = Vector3.Min(V, min);
            }
        }

        return (min, max);
    }
}
