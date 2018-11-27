using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameEventsManager : MonoBehaviour {

	public UnityEvent OnSceneExitEvent;
	public UnityEvent OnAwake;

	void Awake() {
		OnAwake.Invoke();
	}

	// Use this for initialization
	void Start () {
		SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
	}

	private void SceneManager_sceneUnloaded( Scene arg0 ) {
		OnSceneExitEvent.Invoke();
	}
}
