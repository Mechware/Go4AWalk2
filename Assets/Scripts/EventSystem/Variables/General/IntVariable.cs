using UnityEngine;

namespace CustomEvents {
    [UnityEngine.CreateAssetMenu(menuName = "Variable/General/Int")]
	public class IntVariable : Variable<int, UnityEventInt> {
        public void Add(int other) {
            Value += other;
        }
    }
}
