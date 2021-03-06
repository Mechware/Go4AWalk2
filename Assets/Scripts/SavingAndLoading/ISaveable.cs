using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable {
    string GetSaveString();
    void SetData(string saveString, params object[] otherData);
}
