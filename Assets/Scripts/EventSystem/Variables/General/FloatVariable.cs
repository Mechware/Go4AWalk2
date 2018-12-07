namespace CustomEvents {
    [UnityEngine.CreateAssetMenu(menuName = "Variable/General/Float")]
	public class FloatVariable : Variable<float, UnityEventFloat> {

	    public void Add(float other) {
		    Value += other;
	    }

	    public void Subtract(float other) {
		    Value -= other;
	    }

	    public override string GetSaveData() {
		    return Value.ToString();
	    }

	    public override void LoadString( string data ) {
		    float newValue;
		    if (float.TryParse(data, out newValue)) {
			    Value = newValue;
		    } else {
			    UnityEngine.Debug.LogWarning("Cannot load data: " + data);
		    }

	    }
	}
}
