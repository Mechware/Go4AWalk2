using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class RandomUtils {

        public static int GetValueInRange(int min, int max)
        {
            float minf = min - 0.5f;
            float maxf = max + 0.5f;
            return Mathf.RoundToInt(Random.value * (maxf - minf) + minf);
        }

        public static void RunGetValueInRangeTests()
        {
            int min = -7;
            int max = 30;
            int total_runs = 1000000;

            Dictionary<int, int> counts = new Dictionary<int, int>();
            
            for (int i = 0; i < total_runs; i++)
            {
                int count = 0;
                int val = GetValueInRange(min, max);
                if (counts.TryGetValue(val, out count))
                {
                    counts.Remove(val);
                }
                counts.Add(val, count + 1);
            }

            for (int i = min; i <= max; i++)
            {
                Debug.Log(string.Format("Key: {0}, Value: {1}", i, counts[i] / (float)total_runs));
            }
        }
    }

    public static class DebugUtils
    {
        public static void PrintDictionary<T1, T2>(Dictionary<T1, T2> d)
        {
            foreach (var kvp in d) {
                Debug.Log(string.Format("Key: {0}, Value: {1}", kvp.Key, kvp.Value));
            }
        }
    }

}

