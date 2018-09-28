using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace G4AW2.Utils
{
    public static class TransformToCanvas
    {

        private static Vector2 WorldBottomLeft(RectTransform rt)
        {
            Vector2 center = new Vector2(0, 0);
            Vector3[] corners = new Vector3[4];
            rt.GetWorldCorners(corners);
            return corners[0];
        }

        private static Vector2 LocalBottomLeft(RectTransform rt)
        {
            Vector2 center = new Vector2(0, 0);
            Vector3[] corners = new Vector3[4];
            rt.GetLocalCorners(corners);
            return corners[0];
        }

        private static Vector2 WorldCenter(RectTransform rt)
        {
            Vector2 center = new Vector2(0, 0);
            Vector3[] corners = new Vector3[4];
            rt.GetWorldCorners(corners);
            center.y = (corners[1].y+corners[0].y)/2;
            center.x = (corners[2].x+corners[1].x)/2;
            return center;
        }

        private static Vector2 GetTransformScale(Vector2 worldCenter, Vector2 worldBottomLeft, Vector2 localBottomLeft)
        {
            Vector2 scale = new Vector2(0, 0);

            scale.x = localBottomLeft.x/(worldCenter.x-worldBottomLeft.x);
            scale.y = localBottomLeft.y/(worldCenter.y-worldBottomLeft.y);

            return scale;
        }

        public static Vector3 Transform(Vector3 position, RectTransform rt)
        {
            Vector3 result = new Vector3(0, 0, 0);
            Vector2 center = WorldCenter(rt);
            Vector2 scale = GetTransformScale(center, WorldBottomLeft(rt), LocalBottomLeft(rt));

            result.x = (center.x-position.x)*scale.x;
            result.y = (center.y-position.y)*scale.y;

            return result;
        }


    }
}
