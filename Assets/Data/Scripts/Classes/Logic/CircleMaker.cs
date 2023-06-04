using System.Collections.Generic;
using UnityEngine;

namespace MyDice.Helpers
{
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
            float radius = (Vector3.Distance(p2, p1)) / 2;
            Vector3 center = (p2 + p1) / 2;

            for (int i = 0; i <= segments; i++)
            {
                float rad = Mathf.Deg2Rad * ((i * 180f / segments)-90f);
                float x = Mathf.Sin(rad) * radius;
                float y = Mathf.Cos(rad) * radius;
                float z = 0;

                Vector3 p = new Vector3(x, y, z) + center;

                float angle = Vector3.Angle(p1-center, p-center);
                

                result.Add(p);

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
}