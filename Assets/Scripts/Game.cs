using G4AW2;
using G4AW2.Controller;
using G4AW2.Data;
using G4AW2.Data.DropSystem;
using G4AW2.Managers;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    // Data
    [SerializeField] private SaveManager _saveGame;

    // Managers - talk with the data and post events
    [SerializeField] private QuestManager _quests;
    [SerializeField] private FollowerManager _followers;
    [SerializeField] private PlayerManager _player;

    private string SAVE_FILE_LOCATION;

    private bool _allowedToSave = true;
    private void Awake()
    {
        SAVE_FILE_LOCATION = Application.persistentDataPath + "Save.json";
        SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
    }

    private void SceneManager_sceneUnloaded(Scene arg0)
    {
        Save();
    }

    public void Save()
    {
        if (!_allowedToSave) return;
        _saveGame.Save(SAVE_FILE_LOCATION);
    }

    // Start is called before the first frame update
    void Start()
    {
        bool newGame = _saveGame.Load(SAVE_FILE_LOCATION);

        _player.Initialize(newGame);
        _quests.Initialize(newGame);
        _followers.Initialize(newGame); // Must be initialized after quests
    }

    [ContextMenu("Clear Save Game")]
    private void ClearSaveGame()
    {
        SAVE_FILE_LOCATION = Application.persistentDataPath + "Save.json";
        File.Delete(SAVE_FILE_LOCATION);
    }

    private void Update()
    {
        _followers.CheckSpawns(Time.deltaTime);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Save();
        }
        else
        {
        }
    }
    private void OnApplicationQuit()
    {
        Save();
    }
}