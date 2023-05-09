using System.Collections.Generic;
using UnityEngine;
namespace MyDice.Helpers
{
    public struct LineStruct
    {
        public float a;
        public float angle;
        public Vector3 center;
    }
    public class LineMaker
    {
        public List<Vector3> CreateLine(LineStruct lineStruct, int segment)
        {
            return CreateLine(lineStruct.a, lineStruct.center, lineStruct.angle, segment);
        }
        public List<Vector3> CreateLine(float a, Vector3 center, float angle, int segment)
        {
            List<Vector3> result = new List<Vector3>();
            if (segment < 2) return result;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.up);

            float val = (a) / segment;

            Vector3 a1 = center + q * (Vector3.forward * val * a / 2);
            Vector3 a2 = center + q * (Vector3.back * val * a / 2);

            createLine(ref result, a1, a2, segment);

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