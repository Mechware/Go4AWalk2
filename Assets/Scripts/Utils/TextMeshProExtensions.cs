using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;

public static class TextMeshProExtensions {
    public static Dictionary<TextMeshProUGUI, string> OriginalText = new Dictionary<TextMeshProUGUI, string>();

    public static void Replace(this TextMeshProUGUI label, params (string key, string value)[] replacements) {
        if (!OriginalText.ContainsKey(label))
            OriginalText[label] = label.text;

        var original = OriginalText[label];

        foreach (var (key, value) in replacements)
            original = original.Replace(key, value);

        label.text = original;
    }

    public static void FillText(this TextMeshProUGUI label, string text, Action onDone = null) {
        label.maxVisibleCharacters = 0;
        label.text = text;
        label.DOKill();
        DOTween.To(() => label.maxVisibleCharacters, val => label.maxVisibleCharacters = val, text.Length,
            0.04f * text.Length).SetTarget(label).SetEase(Ease.Linear).OnComplete(onDone.SafeFire);
    }
}