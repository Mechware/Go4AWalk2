using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.Dialogue {
	public class DialogueData : ScriptableObject {

		public string Text;
		public Sprite Speaker;
		public UnityEvent OnReturn;

		public DialogueData( string text, Sprite speaker, UnityEvent onReturn ) {
			Text = text;
			Speaker = speaker;
			OnReturn = onReturn;
		}
	}

}


