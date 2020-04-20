using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Formulas {

    public const float MinSpeed = 0.5f;
    public const float MaxSpeed = 4f;

    public static float NormalizeSpeed(float speed) {
        return (speed - MinSpeed) / (MaxSpeed - MinSpeed);
    }
    
    public static int GetValue(int start, int level, float mod = 1) {
        return Mathf.RoundToInt(start * (1 + level / 10f) * mod);
    }

    public static float GetMasteryFromTaps(double taps) {
        return (float) Math.Min(99, Math.Pow(taps, 1d / 3d));
    }

    public static float MultiplierFromMaster(int mastery) {
        return (Mathf.Log10((mastery+1)/100f)+2)/2f;
    }

    public static bool Dodge(float weight, float incomingAttackSpeed, int perkCount) {

        float attackSpeedNormalized = (incomingAttackSpeed - MinSpeed) / (MaxSpeed - MinSpeed);
        float weightNormalized = weight / 100;
        
        float chance = Mathf.Pow(1 - weightNormalized / 100f, 2f) * Mathf.Pow(1 - attackSpeedNormalized, 0.5f) * (0.2f + 0.018f * perkCount);
        Debug.Log($"Dodge Chance: {chance}");
        return Random.value < chance; 
    }
}
