using System;
using System.Linq;
using G4AW2.Followers;
using UnityEngine;

public class FollowerLayoutController : MonoBehaviour {


    [Obsolete("Singleton")] public static FollowerLayoutController Instance;
    public FollowerDisplayController FollowerController;
    public float StartX = 16;
    public float StartY = 32;
    public float XGap = 32;
    public float YGap = 0;

    private void Awake() {
        Instance = this;
    }

    [ContextMenu("Apply Layout")]
    public void ChangeLayout() {

        if (transform.childCount == 0) return;

        float x = StartX;
        float y = StartY;

        foreach(FollowerDisplay followerDisplay in FollowerController.FollowerPool.InUse.Select(f => f.GetComponent<FollowerDisplay>())) {
            RectTransform rectChild = (RectTransform) followerDisplay.transform;
            int distBetween = followerDisplay.Data.SpaceBetweenEnemies;

            Vector3 pos = rectChild.anchoredPosition;
            pos.x = x - distBetween / 2;
            pos.y = y;
            rectChild.anchoredPosition = pos;

            x -= distBetween;
            y += YGap;
        }
    }
}
