using UnityEditor;
using UnityEngine;

namespace MyDice.Editors
{
    public class MousePositionHandler
    {
        public double LastClickTime { get { return lastClicktime; } }
        private Vector3[] positions;
        private double lastClicktime;
        public float Y { get; protected set; }
        public MousePositionHandler(int size)
        {
            positions = new Vector3[size];
        }
        public Vector3 getMousePosition()
        {
            Ray mouseRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            float drawPlaneHeight = 0;
            float distanceToDrawPlane = (drawPlaneHeight - mouseRay.origin.y) / mouseRay.direction.y;
            var result = mouseRay.GetPoint(distanceToDrawPlane);
            Y = result.y;
            return result;
        }
        public Vector3 getPointInRegion(Vector3 input)
        {
            return new Vector3(input.x, Y, input.z);
        }
        public void AddPosition(Vector3 position)
        {
            int i = positions.Length;
            while (--i > 0)
            {
                positions[i] = positions[i - 1];
            }
            lastClicktime = EditorApplication.timeSinceStartup;
            positions[0] = position;
        }
        public Vector3 getPosition(int index)
        {
            if (index >= 0 && index < positions.Length)
            {
                return positions[0];
            }
            return Vector3.zero;
        }
        public Vector3 getLastPosition()
        {
            return positions[0];
        }
    }
}