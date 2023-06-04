using System.Collections.Generic;
using UnityEngine;
namespace MyDice.Helpers
{
    public struct TriangleStruct
    {
        public float a;
        public float angle;
        public Vector3 center;
    }
    public class TriangleMaker
    {
        public List<Vector3> CreateTriangle(TriangleStruct triangleStruct, int segment)
        {
             return CreateTriangle(triangleStruct.a, triangleStruct.center, triangleStruct.angle, segment);
        }
        public List<Vector3> CreateTriangle(float a, Vector3 center, float angle, int segment)
        {
            List<Vector3> result = new List<Vector3>();
            if (segment < 3) return result;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.up);
            float val = (3 * a) / segment;
            Vector3 a1 = center + q * (Vector3.forward * val * a);
            Vector3 a2 = center + Quaternion.AngleAxis(angle + 120, Vector3.up) * (Vector3.forward * val * a);
            Vector3 a3 = center + Quaternion.AngleAxis(angle + 240, Vector3.up) * (Vector3.forward * val * a);

            createLine(ref result, a1, a2, (int)(segment / 3));
            createLine(ref result, a2, a3, (int)(segment / 3));
            createLine(ref result, a3, a1, segment - ( 2*segment / 3));

            return result;
        }
        private void createLine(ref List<Vector3> result, Vector3 a, Vector3 b, int n)
        {
            int cntr = 1;
            result.Add(a);
            Vector3 dir = (b - a).normalized;
            float val = Vector3.Distance(a, b) / n;
            while (cntr < n)
            {
                result.Add(a + cntr * val * dir);
                cntr++;
            }
        }
    }
}