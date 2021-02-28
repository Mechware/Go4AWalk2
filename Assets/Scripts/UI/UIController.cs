using G4AW2;
using G4AW2.Combat;
using G4AW2.Component.UI;
using G4AW2.Managers;
using System;
using UnityEngine;

public class UIController : MonoBehaviour
{
    
    [SerializeField] private PopUp _deathPopUp;
    [SerializeField] private EnemyDisplay _enemy;

    [Obsolete("Create a battle ui class that deals with this")] [SerializeField] private DamageNumberSpawner _playerDamageNumberSpawner;
    [Obsolete("Create a battle ui class that deals with this")] [SerializeField] private DamageNumberSpawner _enemyDamageNumberSpawner;
    [SerializeField] private QuickPopUp[] _quickPopUps;
    [SerializeField] private QuickPopUp _mainQuickPopUp;

    [Obsolete("Create a battle ui class that deals with this")] [SerializeField] private Animator _battleAnim;
    private static readonly int ShowHash = Animator.StringToHash("Showing");

    [SerializeField] private Sprite _questionMark;
    [SerializeField] private SmoothPopUpManager _smoothPopUps;
    [SerializeField] private InteractionCoordinator _interactionController;
    [SerializeField] private WalkingUIController _walkingController;
    [SerializeField] private RecipeManager _recipes;

    [SerializeField] private QuestingStatWatcher _questStats;

    [SerializeField] private PlayerManager _player;
    [SerializeField] private ItemManager _inventory;
    [SerializeField] private QuestManager _quests;

    public void Awake()
    {
        _interactionController.OnPlayerDeathDone += lost =>
        {
            _deathPopUp.SetPopUpNew($"You died! You lost: {lost} gold.", new string[] { "Ok" }, new Action[] { () => { } });
        };

        _player.OnDamageTaken += (dmg, type) =>
        {
            _playerDamageNumberSpawner.SpawnNumber(dmg, type?.DamageColor ?? Color.white);
        };

        _enemy.OnDamageTaken += (dmg, type) =>
        {
            _enemyDamageNumberSpawner.SpawnNumber(dmg, type?.DamageColor ?? Color.white);
        };

        _interactionController.OnFightEnter += () =>
        {
            _smoothPopUps.Disable();
            _quickPopUps.ForEach(q => q.Disable());
        };

        //_interactionController.OnFightEnter += _arrowIndicator.Disable;

        _battleAnim.SetBool(ShowHash, false);
        _interactionController.OnFightStart += () =>
        {
            _battleAnim.SetBool(ShowHash, true);
            
        };

        _interactionController.OnEnemyDeath += (it) =>
        {
            _smoothPopUps.Enable();
            _quickPopUps.ForEach(q => q.Enable());
            _battleAnim.SetBool(ShowHash, false);
        };

        _interactionController.OnPlayerDeathReset += () =>
        {
            _battleAnim.SetBool(ShowHash, false);
        };


        _recipes.RecipeUnlocked += recipe =>
        {
            string postText = "";
            foreach (var component in recipe.Components)
            {
                postText +=
                    $"{component.Amount} {component.Item.Name}{(component.Amount > 1 ? "s" : "")}\n";
            }
            _mainQuickPopUp.Show(_questionMark, $"<size=150%>New Craftable Recipe!</size>\nA new recipe is now craftable!\nRequires:{postText}");
        };

        _walkingController.Initialize();
    }
}
