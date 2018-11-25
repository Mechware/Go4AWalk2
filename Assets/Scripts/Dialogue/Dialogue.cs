using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace G4AW2.Dialogue {
	public class Dialogue : MonoBehaviour {

		public Image Person;
		public TextMeshProUGUI Text;
		private bool playingConversation = false;
		private DialogueData currentConversation;
		private Queue<DialogueData> dialogueQueue = new Queue<DialogueData>();

		public void AdvanceConversation() {
			if (currentConversation == null) {
				Debug.LogError("Tried to advance a non-existent conversation");
				return;
			}
			currentConversation.OnReturn.Invoke();
			playingConversation = false;
			ProcessQueue();
		}

		public void SetConversation( DialogueData convo ) {
			dialogueQueue.Enqueue(convo);
			ProcessQueue();
		}

		private void ProcessQueue() {
			if (playingConversation || dialogueQueue.Count == 0) {
				return;
			}

			playingConversation = true;
			currentConversation = dialogueQueue.Dequeue();

			Person.sprite = currentConversation.Speaker;
			Text.text = currentConversation.Text;
		}
	}
}