using System.Collections.Generic;
using UnityEngine;

    [System.Serializable]
    public struct CircleStruct
    {
        public float radius;
        public Vector3 center;
    }
    [System.Serializable]
    public class CircleMaker
    {
    public List<Vector3> CreateHalfCircle(Vector3 p1, Vector3 p2, int segments)
    {
        if (segments < 1) return null;
        List<Vector3> result = new List<Vector3>();

        Vector3 center = (p2 + p1) / 2;
        Vector3 radiusVector = (p1 - center).normalized * Vector3.Distance(p2, p1) / 2;
        Vector3 orthogonalVector = Vector3.Cross(radiusVector, p2 - p1).normalized * radiusVector.magnitude;

        for (int i = 0; i <= segments; i++)
        {
            float rad = Mathf.PI * i / segments;
            Vector3 pointOnCircle = center + radiusVector * Mathf.Cos(rad) + orthogonalVector * Mathf.Sin(rad);
            result.Add(pointOnCircle);
        }

        return result;
    }

    public List<Vector3> CreateCircle(float radius, Vector3 center, int segments)
        {
            if (segments < 1) return null;
            List<Vector3> result = new List<Vector3>();
            for (int x = 0; x < segments; x++)
            {
                var rad = Mathf.Deg2Rad * (x * 360f / (segments));
                result.Add(center + new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad)).normalized * radius);
            }
            return result;
        }
    }
