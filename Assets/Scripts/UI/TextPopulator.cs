using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.UI {
	public class TextPopulator<TBase, TVar, TEvent> : MonoBehaviour where TEvent : UnityEvent<TBase>,new() where TVar : Variable<TBase, TEvent>  {

		public string DisplayText;
		public TVar Reference;

		private TextMeshProUGUI Text;

		// Use this for initialization
		void Start() {
			Text = GetComponentInChildren<TextMeshProUGUI>();
			Reference.OnChange.RemoveListener(UpdateUI); // just in case
			Reference.OnChange.AddListener(UpdateUI);
			UpdateUI(Reference.Value);
		}

		void UpdateUI( TBase num ) {
			Text.text = string.Format(DisplayText, num);
		}
	}
}

