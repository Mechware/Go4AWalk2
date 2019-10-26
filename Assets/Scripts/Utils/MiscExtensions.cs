using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MiscExtensions {

	public static bool Near(this float val, float otherVal, float tolerance)
    {
        return Mathf.Abs(val - otherVal) < tolerance;
    }

    public static Vector3 SetX(this Vector3 vec, float x) {
        vec.x = x;
        return vec;
    }
    
    public static Vector3 SetY(this Vector3 vec, float y) {
        vec.y = y;
        return vec;
    }
    
    public static Vector3 AddX(this Vector3 vec, float x) {
        vec.x += x;
        return vec;
    }
    
    public static Vector3 AddY(this Vector3 vec, float y) {
        vec.y += y;
        return vec;
    }
    
    public static Vector2 SetX(this Vector2 vec, float x) {
        vec.x = x;
        return vec;
    }
    
    public static Vector2 SetY(this Vector2 vec, float y) {
        vec.y = y;
        return vec;
    }
    
    public static Vector2 AddX(this Vector2 vec, float x) {
        vec.x += x;
        return vec;
    }
    
    public static Vector2 AddY(this Vector2 vec, float y) {
        vec.y += y;
        return vec;
    }
    
    public static Vector2 Add(this Vector2 vec, float x, float y) {
        vec.x += x;
        vec.y += y;
        return vec;
    }

    public static void ForEach<T>(this IEnumerable<T> arr, Action<T> func) {
        foreach(var i in arr) {
            func(i);
        }
    }

    public static Color SetA(this Color c, float alpha) {
        c.a = alpha;
        return c;
    }
}
