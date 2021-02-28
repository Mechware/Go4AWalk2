using DG.Tweening;
using G4AW2.Controller;
using G4AW2.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WalkingUIController : MonoBehaviour {

    public RectTransform PlayerHealthFill;
    public TextMeshProUGUI PlayerHealthText;
    public RectTransform MasteryFill;
    public TextMeshProUGUI MasteryText;

    public ItemViewer ItemViewer;
    public WeaponUI WeaponViewer;

    public InventoryItemDisplay Weapon;
    public InventoryItemDisplay Armor;
    public InventoryItemDisplay Headgear;
    [SerializeField] private Animator animator;
    private static readonly int ShowHash = Animator.StringToHash("Showing");
    [SerializeField] private InteractionCoordinator _interactionController;
    public ClickReceiver ArrowReceiver;
    public Image Arrow;
    public TextMeshProUGUI NumberOfFollowersText;

    [SerializeField] private WorldController _gameController;

    [SerializeField] private PlayerManager _player;
    [SerializeField] private FollowerManager _followers;
    [SerializeField] private PlayerClickController _playerArmorView;

    void Awake() {
        ItemViewer.Init();
        WeaponViewer.Init();
        Arrow.rectTransform.anchoredPosition = Arrow.rectTransform.anchoredPosition.SetX(9);
        Arrow.rectTransform.DOAnchorPosX(13, 1).SetLoops(-1, LoopType.Yoyo);

        ArrowReceiver.MouseClick2D.AddListener(a => {
            _gameController.ScrollToEnemies();
        });
    }
    
    public void Show()
    {
        animator.SetBool(ShowHash, true);
    }

    public void Hide()
    {
        animator.SetBool(ShowHash, false);
    }

    public void Initialize()
    {
        Show();
        _interactionController.OnFightStart += () =>
        {
            Hide();
        };

        _interactionController.OnEnemyDeathFinished += e =>
        {
            Show();
        };

        _interactionController.OnPlayerDeathReset += () =>
        {
            Show();
        };

        ItemViewer.Init();
        WeaponViewer.Init();
    }

    // Update is called once per frame
    void Update() {
        var weapon = _player.Weapon;
        
        float currentDamage = weapon.RawDamage;
        
        MasteryText.text = $"Weapon Mastery {weapon.Mastery}";
        
        if(weapon.Mastery == 99) {
            MasteryFill.anchorMax = MasteryFill.anchorMax.SetX(1);
        }
        else {
            float masteryProgress = weapon.RawMastery - Mathf.Floor(weapon.RawMastery);
            float nextLevelDamage = weapon.GetDamage(mastery: weapon.Mastery+1);
            MasteryFill.anchorMax = MasteryFill.anchorMax.SetX(masteryProgress);
        }

        float playerHealth = Mathf.Clamp01(_player.Health / (float) _player.MaxHealth);
        PlayerHealthFill.anchorMax = PlayerHealthFill.anchorMax.SetX(playerHealth);
        PlayerHealthText.text = $"{_player.Health} / {_player.MaxHealth}";        
        
        Weapon.SetDataInstance(_player.Weapon, 0, ChangeWeapon, null, true);
        Armor.SetDataInstance(_player.Armor, 0, ChangeArmor, null, true);
        if(_player.Headgear != null) Headgear.SetDataInstance(_player.Headgear, 0, ChangeHeadgear, null, true);
        
        
        bool HasFollowers = _followers.Followers.Count > 0;

        NumberOfFollowersText.text = $"x{FollowerManager.Instance.Followers.Count}";
        Arrow.gameObject.SetActive(_gameController.CanScroll && HasFollowers);
    }

    public void ChangeWeapon(InventoryItemDisplay it) {
        _playerArmorView.ChangePlayerWeapon();
    }
    
    public void ChangeArmor(InventoryItemDisplay it) {
        _playerArmorView.ChangePlayerArmor();
    }
    
    public void ChangeHeadgear(InventoryItemDisplay it) {
        _playerArmorView.ChangePlayerHeadgear();
    }

    public RobustLerper MasteryPopUpLerp;
    public TextMeshProUGUI MasteryPopUpText;
    public void MasteryPopUp(string popUp) {
        MasteryPopUpText.text = popUp;
        MasteryPopUpLerp.StartLerping();
    }
}
