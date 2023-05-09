using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyDice.Helpers;

public class EllipsesObject : MonoBehaviour
{
    public GameObject object1, object2;

    private void OnDrawGizmos()
    {
        if (object1 == null || object2 == null) return;
        Vector3 p1 = object1.transform.position;
        Vector3 p2 = object2.transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(p1, p2);

        Gizmos.DrawLine(p1+Vector3.right, p1);

        EllipseMaker e = new EllipseMaker();
        
        var points = e.CreateHalfEllipse(p1, p2, .5f, 10);
        for (int i = 0; i < points.Count - 1; i++)
            Gizmos.DrawLine(points[i], points[i + 1]);
            
    }

}
