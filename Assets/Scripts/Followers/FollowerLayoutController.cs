using G4AW2.Followers;
using UnityEngine;

public class FollowerLayoutController : MonoBehaviour {

    // Use this for initialization
    void Awake() {
        transform.hasChanged = true;
    }

    public float StartX = 16;
    public float StartY = 32;
    public float XGap = 32;
    public float YGap = 0;

    // Update is called once per frame
    void Update() {
        if(transform.hasChanged) {
            transform.hasChanged = false;
            ChangeLayout();
        }
    }


    [ContextMenu("Apply Layout")]
    public void ChangeLayout() {

        if (transform.childCount == 0) return;

        Transform child = transform.GetChild(transform.childCount - 1);
        float distBetween = child.GetComponent<FollowerDisplay>().Data.SpaceBetweenEnemies;

        float x = StartX;
        float y = StartY;

        for(int i = transform.childCount-1; i >= 0; i--) {
            RectTransform rectChild = (RectTransform) transform.GetChild(i);
            distBetween = rectChild.GetComponent<FollowerDisplay>().Data.SpaceBetweenEnemies;

            Vector3 pos = rectChild.anchoredPosition;
            pos.x = x - distBetween / 2;
            pos.y = y;
            rectChild.anchoredPosition = pos;

            x -= distBetween;
            y += YGap;
        }
    }
}
