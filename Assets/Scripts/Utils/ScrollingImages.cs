using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class ScrollingImages : MonoBehaviour {

	public int ScrollSpeed;
	public bool Playing { private set; get; } = true;
	
	[SerializeField] private bool _forceInt;
	
	[SerializeField] private List<Image> Images = new List<Image>();

    private Queue<Image> imQueue = new Queue<Image>();
	private int imageWidth;
	private Vector3 startPosition;
	private Vector3 position;
	private Vector3 roundedPosition;

	private RectTransform rt;

	public Action<float> OnScrolled;

	private void Awake()
    {
		rt = GetComponent<RectTransform>();
    }

    // Use this for initialization
    void Start() {
		imageWidth = (int)_imagePrefab.rectTransform.rect.width;
		startPosition = position = rt.localPosition;
        imQueue = new Queue<Image>();
        Images.ForEach(imQueue.Enqueue);
	}

	public void Pause() {
		Playing = false;
	}
	public void Play() {
		Playing = true;
	}

	public void SetImages(Sprite s)
    {
		Images.ForEach(im => im.sprite = s);
	}

	// Update is called once per frame
	void Update() {
		if (!Playing) return;

		float distance = Time.deltaTime * ScrollSpeed;
		position.x -= distance;
		if (position.x < startPosition.x - imageWidth) {
			Image i = imQueue.Dequeue();
		    i.transform.SetAsLastSibling();
            imQueue.Enqueue(i);
			position.x += imageWidth;
		}

		roundedPosition = position;
		if(_forceInt) roundedPosition.x = Mathf.RoundToInt(position.x);

		rt.localPosition = roundedPosition;
		OnScrolled?.Invoke(distance);
	}

	[Header("Editor Tools")]
	[SerializeField] private int _numberRepeats;
	[SerializeField] private Image _imagePrefab;

#if UNITY_EDITOR
	[ContextMenu("Clear and Add Images")]
	private void AddImages() {
		ClearImages();
        for (int i = 0; i < _numberRepeats; i++) {
			Images.Add(Instantiate(_imagePrefab, transform));
		}
	}

	[ContextMenu("Clear Images")]
	private void ClearImages() {
		foreach (Image image in Images) {
			if (image == null) continue;
			DestroyImmediate(image.gameObject);
		}
		Images.Clear();
	}

    [ContextMenu("Center Images")]
    private void CenterImages() {
        imageWidth = (int)_imagePrefab.rectTransform.rect.width;
        Vector3 v = transform.localPosition;
        v.x = -1 * (imageWidth * Images.Count) / 2f;
        v.x = Mathf.Round(v.x);
        transform.localPosition = v;
    }
#endif

}
