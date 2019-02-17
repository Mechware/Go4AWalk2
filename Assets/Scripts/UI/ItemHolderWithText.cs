using CustomEvents;
using G4AW2.Data.DropSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemHolderWithText : MonoBehaviour {

    public ItemViewer ItemSelector;

    public Button ButtonToSelectNewItem;
    public Image ItemImage;
    public TextMeshProUGUI ItemDescription;

    public ItemReference CurrentItem;

    public void Start() {
        ButtonToSelectNewItem.onClick.AddListener(() => { ItemSelector.ShowAllMaterialFromInventory(true, SetItem); });
    }

    public void SetItem(Item it) {
        CurrentItem.Value = it;
        ItemImage.sprite = it.Image;
        ItemDescription.text = it.name + "\n<size=80%" + it.Description;
    }
}
