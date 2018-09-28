using G4AW2.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryOpen : MonoBehaviour {

    public Vector2 MaxBounds;
    public Vector2 MinBounds;
    public Vector2 InventoryMaxBounds;
    public Vector2 InventoryMinBounds;

    public float moveDistance;

    public GameObject inventoryScreen;
    public GameObject dragScreen;

    public GameObject equipScreen;

    public GameObject clouds1, clouds2, trees;

    private RectTransform inventoryTransform, screenTransform;
    private Vector3 deltaPosition, deltaInventoryPosition, position, inventoryPosition;

    private bool isMoving,open;


    // Use this for initialization
    void Start () {
        isMoving = false;
        open = false;
        inventoryTransform = inventoryScreen.GetComponent<RectTransform>();
        screenTransform = dragScreen.GetComponent<RectTransform>();
        deltaPosition = new Vector2(0, moveDistance);
        deltaInventoryPosition = Vector2.Scale(deltaPosition, new Vector2(0, (InventoryMaxBounds.y-InventoryMinBounds.y)/MinBounds.y));
        equipScreen.SetActive(false);
	}


    void Update()
    {
        if (isMoving)
        {
            if (!open)
            {
                inventoryPosition = inventoryTransform.localPosition + deltaInventoryPosition;
                inventoryPosition = inventoryPosition.BoundVector3(InventoryMinBounds, InventoryMaxBounds);
                inventoryTransform.localPosition = inventoryPosition;

                position = screenTransform.localPosition - deltaPosition;
                position = position.BoundVector3(MinBounds, MaxBounds);
                screenTransform.localPosition = position;

                if (inventoryTransform.localPosition.y == InventoryMinBounds.y)
                {
                    open = true;
                    isMoving = false;
                    GetComponentInChildren<Text>().text= "^"; // *********REMOVE THIS ONCE YOU CHANGE THE BUTTON
                }

            } else
            {
                inventoryPosition = inventoryTransform.localPosition - deltaInventoryPosition;
                inventoryPosition = inventoryPosition.BoundVector3(InventoryMinBounds, InventoryMaxBounds);
                inventoryTransform.localPosition = inventoryPosition;

                position = screenTransform.localPosition + deltaPosition;
                position = position.BoundVector3(MinBounds, MaxBounds);
                screenTransform.localPosition = position;

                if (inventoryTransform.localPosition.y == InventoryMaxBounds.y)
                {
                    open = false;
                    equipScreen.SetActive(false);
                    isMoving = false;
                    GetComponentInChildren<Text>().text= "V"; // *********REMOVE THIS ONCE YOU CHANGE THE BUTTON
                    clouds1.GetComponent<ScrollingImages>().Play();
                    clouds2.GetComponent<ScrollingImages>().Play();
                    trees.GetComponent<ScrollingImages>().Play();
                }
            }
        }
    }



    public void moveScreen()
    {
        if (!isMoving)
        {
            isMoving = true;
            equipScreen.SetActive(true);
            clouds1.GetComponent<ScrollingImages>().Pause();
            clouds2.GetComponent<ScrollingImages>().Pause();
            trees.GetComponent<ScrollingImages>().Pause();

        }
    }



}
