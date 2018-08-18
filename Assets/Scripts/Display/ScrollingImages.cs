using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class ScrollingImages : MonoBehaviour {

	public int ScrollSpeed;
	public Image ImagePrefab;
	public int NumberRepeats;
	public bool Playing = true;


	public List<Image> Images = new List<Image>();

	private int imageWidth;
	private Vector3 startPosition;
	private Vector3 position;
	private Vector3 roundedPosition;

	private RectTransform _rt;
	private RectTransform rt {
		get {
			if (_rt == null)
				_rt = GetComponent<RectTransform>();
			return _rt;
		}
	}

	// Use this for initialization
	void Start() {
		imageWidth = (int)ImagePrefab.rectTransform.rect.width;
		startPosition = position = rt.localPosition;
	}

	public void Pause() {
		Playing = false;
	}
	public void Play() {
		Playing = true;
	}

	// Update is called once per frame
	void Update() {
		if (!Playing) return;
		position.x -= Time.deltaTime * ScrollSpeed;
		if (position.x < startPosition.x - imageWidth) {
			Images[0].transform.SetAsLastSibling();
			Image i = Images[0];
			Images[0] = Images[1];
			Images[Images.Count - 1] = i;
			position.x += imageWidth;
		}

		roundedPosition = position;
		roundedPosition.x = Mathf.RoundToInt(position.x);

		rt.localPosition = roundedPosition;
	}

	public void ChangeSpriteOfChildren(Sprite s) {
		Images.ForEach(im => im.sprite = s);
	}

#if UNITY_EDITOR
	[ContextMenu("Clear and Add Images")]
	private void AddImages() {
		ClearImages();
		for (int i = 0; i < NumberRepeats; i++) {
			Images.Add(Instantiate(ImagePrefab, transform));
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

#endif

}
