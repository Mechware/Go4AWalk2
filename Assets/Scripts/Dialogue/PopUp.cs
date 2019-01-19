using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace G4AW2.Dialogue {
	public class PopUp : MonoBehaviour {

		public TextMeshProUGUI Text;
		public Button SingleResponseButton;
		public Button[] TwoResponseButtons;
		public Button[] ThreeResponseButtons;

		public GameObject container;

		private static PopUp singleton;
		private bool inUse = false;

		void Start() {
			singleton = this;
		}

		/// Set pop up text, returns a bool values based off of whether or not it is in use
		public static bool SetPopUp(string text, string[] options, Action[] responses) {
			return singleton.SetPopUpPriv(text, options, responses);
		}

		private bool SetPopUpPriv(string text, string[] options, Action[] responses) {

			container.SetActive(true);

			if (inUse)
				return false;
			if (options.Length > 3)
				throw new Exception("Options for pop up is larger than 3 elements");
			if (options.Length == 0)
				throw new Exception("Empty Options in pop up.");

			Text.text = text;

			Button[] single = {SingleResponseButton};

			single.ForEach(b => b.gameObject.SetActive(false));
			TwoResponseButtons.ForEach(b => b.gameObject.SetActive(false));
			ThreeResponseButtons.ForEach(b => b.gameObject.SetActive(false));

			Button[] response = null;
			if (options.Length == 1) response = single;
			else if (options.Length == 2) response = TwoResponseButtons;
			else if (options.Length == 3) response = ThreeResponseButtons;

			for (int i = 0; i < options.Length; i++) {
				response[i].gameObject.SetActive(true);
				response[i].GetComponentInChildren<TextMeshProUGUI>().text = options[i];
				response[i].onClick.RemoveAllListeners();
				AddListener(response[i], i, responses);
			}

			return true;
		}

		private void AddListener(Button b, int i, Action[] responses) {
			b.onClick.AddListener(() => responses[i]());
			b.onClick.AddListener(() => container.SetActive(false));
		}

	}
}

