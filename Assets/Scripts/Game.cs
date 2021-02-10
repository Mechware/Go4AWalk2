using CustomEvents;
using G4AW2.Data.DropSystem;
using G4AW2.Followers;
using G4AW2.Saving;
using G4AW2.UI.Areas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    // TODO: Move these into player?
    [SerializeField] private WeaponVariable _playerWeapon;
    [SerializeField] private ArmorVariable _playerArmor;

    [SerializeField] private GameEvents _events;
    [SerializeField] private SaveManager[] _saveManagers;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private ConfigObject _configs;
    [SerializeField] private MasteryLevels _masteryLevels;
    [SerializeField] private AreaManager _areaManager;
    [SerializeField] private QuestingStatWatcher _questWatcher;
    [SerializeField] private MiningPoints _mining;
    [SerializeField] private TutorialManager _tutorial;

    [SerializeField] private AchievementManager _achievements;
    [SerializeField] private QuestManager _quests;
    [SerializeField] private PassiveQuestManager _passiveQuests;

    // Move to follower display class
    [SerializeField] private FollowerDisplayController _followerDisplay;
    [SerializeField] private FollowerSpawner _followerSpawner;

    // Move into player display class
    [SerializeField] private PlayerHealthIncreaser _playerHealthIncreaser;

    // Move into combat controller type thing
    [SerializeField] private InteractionController _interactions;



    private bool _allowedToSave = true;

    private void Awake()
    {
        
        // TODO: Look into why this is required
        _configs.RegisterChanges();
        _masteryLevels.Register();

        _events.OnNewGame += OnNewGame;
        _events.OnNewGame += _masteryLevels.OnNewGame;

        // Does this need to be an event?
        _events.OnAfterLoad += _followerDisplay.AfterLoadEvent;
        _events.OnAfterLoad += _followerSpawner.LoadFinished;
        _events.OnAfterLoad += _achievements.InitAchievements;
        _events.OnAfterLoad += _quests.Initialize;
        _events.OnAfterLoad += _passiveQuests.Initialize;
        _events.OnAfterLoad += _playerHealthIncreaser.OnGameStateLoaded;

        _events.OnSceneExitEvent += Save;
        _events.OnPause += Save;
        _events.OnQuit += Save;

        // Game specific events:

        _events.OnQuestSet += q =>
        {
            // TODO: Pass the event manager into these objects and let these items deal with this 
            // themselves
            _areaManager.SetArea(q.Area);
            _questWatcher.SetQuest(q);
            _mining.QuestChanged(q); // Could probably go into a background owner type thing
            _tutorial.QuestUpdated(q);
        };

        _interactions.OnEnemyKilled += _events.OnEnemyKilled;
        _inventory.OnLootObtained += _events.OnLootObtained;
    }

    public void OnNewGame()
    {
        Weapon original = _playerWeapon.Value;
        _playerWeapon.Value = Instantiate(original);
        _playerWeapon.Value.CreatedFromOriginal = true;
        _playerWeapon.Value.Level = 1;
        _playerWeapon.Value.TapsWithWeapon.Value = 0;
        _playerWeapon.Value.Random = 30;
        _playerWeapon.Value.SetValuesBasedOnRandom();

        Armor originalArmor = _playerArmor;
        _playerArmor.Value = Instantiate(originalArmor);
        _playerArmor.Value.CreatedFromOriginal = true;
        _playerArmor.Value.Level = 1;
        _playerArmor.Value.Random = 50;
        _playerArmor.Value.SetValuesBasedOnRandom();
    }

    public void Save()
    {
        if (!_allowedToSave) return;

        foreach(var manager in _saveManagers)
        {
            manager.Save();
        }
    }

    public void Load()
    {
        bool newGame = true;
        bool failed = false;
        foreach (var manager in _saveManagers)
        {
            var res = manager.Load();
            newGame &= res.noFile;
            failed |= res.failed; 
        }
        if(failed)
        {
            _allowedToSave = false;
            return;
        }
        if(newGame)
        {
            _events.OnNewGame?.Invoke();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Load();
        _events.OnAfterLoad.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
