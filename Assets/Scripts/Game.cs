using G4AW2;
using G4AW2.Controller;
using G4AW2.Data;
using G4AW2.Data.DropSystem;
using G4AW2.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    // Data
    [SerializeField] private SaveGame _saveGame;

    // Managers - talk with the data and post events
    [SerializeField] private QuestManager _quests;
    [SerializeField] private FollowerManager _followers;
    [SerializeField] private PlayerManager _player;

    private bool _allowedToSave = true;
    private void Awake()
    {
        SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
    }

    private void SceneManager_sceneUnloaded(Scene arg0)
    {
        Save();
    }

    public void Save()
    {
        if (!_allowedToSave) return;
        _saveGame.Save();
    }

    // Start is called before the first frame update
    void Start()
    {
        //bool newGame = _saveGame.Load();
        bool newGame = true;

        _player.Initialize(newGame);
        _quests.Initialize(newGame);
        _followers.Initialize(newGame); // Must be initialized after quests
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