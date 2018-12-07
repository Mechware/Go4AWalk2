using UnityEngine;

namespace CustomEvents {
    [UnityEngine.CreateAssetMenu(menuName = "Variable/General/Int")]
	public class IntVariable : Variable<int, UnityEventInt> {
	    public override string GetSaveData() {
		    return Value.ToString();
	    }

	    public override void LoadString(string data) {
		    int newValue;
		    if (int.TryParse(data, out newValue)) {
			    Value = newValue;
		    }
		    else {
			    Debug.LogWarning("Cannot load data: " + data);
		    }

	    }
    }
}
