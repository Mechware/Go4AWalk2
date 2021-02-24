using G4AW2;
using G4AW2.Controller;
using G4AW2.Data;
using G4AW2.Data.DropSystem;
using G4AW2.Managers;
using Items;
using UnityEngine;

public class Game : MonoBehaviour
{
    // Data
    [SerializeField] private SaveGame _saveGame;

    [SerializeField] private GameEvents _events;

    // Managers - talk with the data and post events
    [SerializeField] private AchievementManager _achievements;
    [SerializeField] private TutorialManager _tutorial;
    [SerializeField] private QuestManager _quests;
    [SerializeField] private FollowerManager _followers;
    [SerializeField] private BaitBuffManager _bait;
    [SerializeField] private ConsumableManager _consumables;
    [SerializeField] private PlayerManager _player;
    [SerializeField] private ItemManager _inventory;
    [SerializeField] private RecipeManager _crafting;

    // Coordinators controls the flow of complex interactions
    [SerializeField] private InteractionCoordinator _interactions;

    // Controllers control the game and are kind of heirarchical by nature
    [SerializeField] private WorldController _gameWorld;
    [SerializeField] private UIController _gameUI;

    public WeaponConfig StartWeapon;
    public ArmorConfig StartArmor;
    public QuestConfig StartQuest;

    private bool _allowedToSave = true;
    private void Awake()
    {
        // Does this need to be an event?
        _events.OnSceneExitEvent += Save;
        _events.OnPause += Save;
        _events.OnQuit += Save;

        // Game specific events:
        _events.OnQuestSet += q =>
        {
            // TODO: Pass the event manager into these objects and let these items deal with this 
            // themselves
            _tutorial.SetQuest(q.Config);
            _followers.SetQuest(q.Config);
        };

        _interactions.OnEnemyDeathFinished += _events.OnEnemyKilled;
        _inventory.OnItemObtained += _events.OnLootObtained;

        _achievements.OnAchievementObtained += _events.OnAchievementObtained;
        _crafting.RecipeUnlocked += _events.OnRecipeUnlocked;
    }

    public void OnNewGame()
    {
        _player.Weapon = new WeaponInstance(StartWeapon, 1);
        _player.Weapon.SaveData.Level = 1;
        _player.Weapon.SaveData.Random = 30;

        _player.Armor = new ArmorInstance(StartArmor, 1);
        _player.Armor.SaveData.Level = 1;
        _player.Armor.SaveData.Random = 50;

        SaveGame.SaveData.CurrentQuests.Add((new QuestInstance(StartQuest, true)).SaveData);

        _player.NewGame();
    }

    public void Save()
    {
        if (!_allowedToSave) return;

        _saveGame.Save();
    }

    public void Load()
    {
        bool newGame = _saveGame.Load();
        if(newGame)
        {
            OnNewGame();
        }
    }

    bool _successfulInitialization = false;

    // Start is called before the first frame update
    void Start()
    {
        Load();

        _player.Initialize();
        _inventory.Initialize();
        _quests.Initialize(_events, _inventory);
        _followers.Initialize(_events, _quests);
        _consumables.Initialize();

        _interactions.Initialize();

        _gameWorld.Initialize(_inventory, _player, _followers, _events);
        _gameUI.Initialize(_player, _inventory, _events, _quests);
        _interactions.Initialize();

        _successfulInitialization = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_successfulInitialization) return;
        _bait.MyUpdate();
        _gameUI.GameUpdate(Time.deltaTime);
        _gameWorld.GameUpdate(Time.deltaTime);
    }
}