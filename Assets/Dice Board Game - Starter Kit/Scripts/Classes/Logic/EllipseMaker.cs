using System.Collections.Generic;
using UnityEngine;

namespace MyDice.Helpers
{
    public struct EllipseStruct
    {
        public float a;
        public float b;
        public float angle;
        public Vector3 center;
    }
    public class EllipseMaker
    {
        #region functions
        #region Half Ellipse
        public Vector3[] CreateHalfEllipse(Vector3 position1, Vector3 position2, float maxHeightPercentage, float theta1, float theta2, int resolution)
        {
            if (resolution < 1) return null;
            Vector3 center = (position1 + position2) / 2;
            float a = Vector3.Distance(position1, position2) / 2;
            float b = a * maxHeightPercentage;

            Vector3 v = (position2 - position1);

            var positions = new Vector3[resolution + 1];

            Quaternion q1 = Quaternion.AngleAxis(45, v.normalized);
            //v = v.normalized;
            float angle;
            Vector3 newPosition;
            for (int i = 0; i <= resolution; i++)
            {
                angle = (i * 2.0f * Mathf.PI) / (float)(resolution * 2);
                newPosition = new Vector3(a * Mathf.Cos(angle), b * Mathf.Sin(angle), 0);
                positions[i] = (Quaternion.LookRotation(v, center) * newPosition + center);
            }

            return positions;
        }
        public List<Vector3> CreateHalfEllipse(Vector3 p1, Vector3 p2, float maxHeightPercentage, int segments)
        {
            List<Vector3> positions = new List<Vector3>();
            if (segments < 1) return positions;

            float a = Vector3.Distance(p1, p2) / 2;
            float b = a * maxHeightPercentage;

            Vector3 center = (p1 + p2) / 2;
            Vector3 v = (p2 - p1);


            float angle = Vector3.Angle((p2 - p1),Vector3.right);
            if (angle > 180)
            {
                angle = Vector3.Angle((p2 - p1), Vector3.left);
            }
            Debug.Log(angle);
            Quaternion q1 = Quaternion.AngleAxis(angle,(p2- p1));

            for (int x = 0; x <= segments; x++)
            {
                var rad = Mathf.Deg2Rad * (x * 360f / (segments));
                positions.Add(center +  q1 * ( new Vector3(Mathf.Sin(rad) * a, Mathf.Cos(rad) * b, 0)));
            }

            return positions;
        }
        #endregion
        #region Ellipse

        public Vector3[] CreateEllipse(float a, float b, float h, float k, float theta, int resolution, Vector3 direction)
        {

            Vector3[] positions = new Vector3[resolution + 1];
            Quaternion q = Quaternion.AngleAxis(theta, direction);
            Vector3 center = new Vector3(h, k, 0.0f);

            for (int i = 0; i <= resolution; i++)
            {
                float angle = (float)i / (float)resolution * 2.0f * Mathf.PI;
                positions[i] = new Vector3(a * Mathf.Cos(angle), b * Mathf.Sin(angle), 0.0f);
                positions[i] = q * positions[i] + center;
            }

            return positions;
        }
        public List<Vector3> CreateEllipse(EllipseStruct ellipseStruct, int segment)
        {
            return CreateEllipse(ellipseStruct.a, ellipseStruct.b, ellipseStruct.angle, ellipseStruct.center, segment);
        }
        public List<Vector3> CreateEllipse(float a, float b, float angle, Vector3 center, int segment)
        {
            List<Vector3> result = new List<Vector3>();
            if (segment < 1) return result;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.up);
            for (int i = 0; i < segment; i++)
            {
                float theta = (float)i / (float)segment * 2.0f * Mathf.PI;
                Vector3 temp = new Vector3(a * Mathf.Cos(theta), 0.0f, b * Mathf.Sin(theta));
                result.Add(q * temp + center);
            }
            return result;
        }
        #endregion
        #endregion
    }
}