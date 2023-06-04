using UnityEngine;

namespace MyDice.Editors { 
public class SelectionInfo 
{
        public int pointIndex = -1;
        public bool mouseIsOverPoint;
        public bool pointIsSelected;
        public Vector3 positionAtStartOfDrag;
        /////////
        public int lineIndex = -1;
        public int lineIndexConnectionValue = -1;
        public bool mouseIsOverLine;
        /////////
        public int followerLineFromPointIndex = -1;
        //////////////////
        ///
    }
}