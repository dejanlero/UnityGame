using System.Collections.Generic;
using UnityEngine;
namespace MyDice.Helpers
{
    public struct DiamondStruct
    {
        public float a;
        public float b;
        public float angle;
        public Vector3 center;
    }
    public class DiamondMaker
    {
        public List<Vector3> CreateDiamond(DiamondStruct diamondStruct, int segment)
        {
            return CreateDiamond(diamondStruct.a, diamondStruct.b, diamondStruct.angle, diamondStruct.center, segment);
        }
        public List<Vector3> CreateDiamond(float a, float b, float angle, Vector3 center, int segment)
        {
            List<Vector3> result = new List<Vector3>();
            if (segment < 4) return result;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.up);
            float val = (4 * a) / segment;
            Vector3 a1 = center + q * (Vector3.back * val * a / 2);
            Vector3 a2 = center + q * (Vector3.right * val * b / 2);
            Vector3 a3 = center + q * (Vector3.forward * val * a / 2);
            Vector3 a4 = center + q * (Vector3.left * val * b / 2);

            createLine(ref result, a1, a2, (int)(segment / 4));
            createLine(ref result, a2, a3, (int)(segment / 4));
            createLine(ref result, a3, a4, (int)(segment / 4));
            createLine(ref result, a4, a1, segment - (3 * segment / 4));

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