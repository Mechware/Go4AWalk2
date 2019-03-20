using UnityEngine;

namespace CustomEvents {
    [UnityEngine.CreateAssetMenu(menuName = "SO Architecture/Variable/General/Int")]
	public class IntVariable : Variable<int, UnityEventInt> {
        public void Add(int other) {
            Value += other;
        }
    }
}
