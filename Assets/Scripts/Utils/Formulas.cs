using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Formulas {

    public static int GetValue(int start, int level, float mod = 1) {
        return Mathf.RoundToInt(start * (1 + level / 10f) * mod);
    }

    public static float GetMasteryFromTaps(double taps) {
        return (float) Math.Min(99, Math.Pow(taps, 1d / 3d));
    }

    public static float MultiplierFromMaster(int mastery) {
        return (Mathf.Log10((mastery+1)/100f)+2)/2f;
    }
}
