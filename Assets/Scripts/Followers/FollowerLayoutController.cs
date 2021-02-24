using System;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Followers;
using UnityEngine;

namespace G4AW2.Component.World {
    public class FollowerLayoutController : MonoBehaviour
    {
        public float StartX = 16;
        public float StartY = 32;
        public float XGap = 32;
        public float YGap = 0;

        [ContextMenu("Apply Layout")]
        public void ChangeLayout(IEnumerable<FollowerDisplay> followers)
        {

            if (transform.childCount == 0) return;

            float x = StartX;
            float y = StartY;

            foreach (FollowerDisplay followerDisplay in followers)
            {
                RectTransform rectChild = (RectTransform)followerDisplay.transform;
                int distBetween = followerDisplay.Instance.Config.SpaceBetweenEnemies;

                Vector3 pos = rectChild.anchoredPosition;
                pos.x = x - distBetween / 2;
                pos.y = y;
                rectChild.anchoredPosition = pos;

                x -= distBetween;
                y += YGap;
            }
        }
    }
}

