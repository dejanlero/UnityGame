using System.Collections.Generic;
using UnityEngine;

namespace MyDice.Helpers
{
    public struct PolygonStruct
    {
        public float a;
        public int edges;
        public float angle;
        public Vector3 center;
    }
    public class PolygonMaker 
    {
        public List<Vector3> CreatePolygon(PolygonStruct polygonStruct, int segment)
        {
            return CreatePolygon(polygonStruct.a, polygonStruct.edges, polygonStruct.center, polygonStruct.angle,segment);
        }
        public List<Vector3> CreatePolygon(float a,int edges, Vector3 center, float angle, int segment)
        {
            List<Vector3> result = new List<Vector3>();
            if (segment < 2 || edges<3) return result;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.up);
            float val = (edges * a) / segment;
            Vector3[] vectors = new Vector3[edges];
            

            for(int i = 0; i < edges; i++)
            {
                float theta =i*( 360f / edges);
                vectors[i]= center + Quaternion.AngleAxis(angle + theta, Vector3.up) * (Vector3.forward * val * a);
            }
            for(int i = 0; i < edges-1; i++)
            {
                createLine(ref result, vectors[i], vectors[i+1], (int)(segment / edges));
            }
            createLine(ref result, vectors[edges-1], vectors[0], segment - ((edges-1) * segment / edges));

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