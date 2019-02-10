using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MiscExtensions {

	public static bool Near(this float val, float otherVal, float tolerance)
    {
        return Mathf.Abs(val - otherVal) < tolerance;
    }
}
