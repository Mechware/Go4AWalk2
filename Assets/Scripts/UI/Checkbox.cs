using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Sprite))]
public class Checkbox : MonoBehaviour, IPointerDownHandler {

    public Sprite NormalImage;
    public Sprite SelectedImage;

    public bool Selected;

    private Image im;

    void Awake() {
        im = GetComponent<Image>();
        im.sprite = Selected ? SelectedImage : NormalImage;
    }

	// Use this for initialization
	public void OnAfterLoad () {
	    im.sprite = Selected ? SelectedImage : NormalImage;
    }

    public void OnPointerDown(PointerEventData eventData) {
        Selected = !Selected;
        im.sprite = Selected ? SelectedImage : NormalImage;
    }
}
