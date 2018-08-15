using System.Collections;
using System.Collections.Generic;
using G4AW2.DropSystem;
using UnityEngine;

namespace G4AW2.Testing {
	public class ItemDropperTester : MonoBehaviour {

		public ItemDropper Dropper;

		
		public void DropItem(int times) {

			Dictionary<Item, int> counts = new Dictionary<Item, int>();
			for (int i = 0; i < times; i++) {
				List<Item> items = Dropper.GetItems();
				foreach (var item in items) {
					int count = 0;
					if (counts.TryGetValue(item, out count)) {
						counts.Remove(item);
					}
					counts.Add(item, count + 1);
				}
			}

			foreach (var kvp in counts) {
				Debug.Log(string.Format("Key: {0}, Value: {1}", kvp.Key.description, kvp.Value / (float)times));
			}
		}

		[ContextMenu("Drop 1")]
		public void DropOne() {
			DropItem(1);
		}

		[ContextMenu("Drop 10")]
		public void Drop10() {
			DropItem(10);
		}

		[ContextMenu("Drop 1000")]
		public void Drop1000() {
			DropItem(1000);
		}

		[ContextMenu("Drop 1000000")]
		public void Drop1000000() {
			DropItem(1000000);
		}
	}
}

