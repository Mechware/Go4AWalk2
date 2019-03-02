using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FadeText : MonoBehaviour {
    private TextMeshProUGUI text;

    public Color StartColor;
    public Color EndColor;
    public float Duration;
    public float Delay;

    // Use this for initialization
    IEnumerator Start () {
	    text = GetComponent<TextMeshProUGUI>();
	    text.color = StartColor;

        yield return new WaitForSeconds(Delay);
        text.CrossFadeColor(EndColor, Duration, true, true);
        yield return new WaitForSeconds(Duration);
        Destroy(gameObject);
	}



    // Update is called once per frame
    void Update () {
	}
}
