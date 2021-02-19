using G4AW2.Combat;
using G4AW2.Dialogue;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    
    [SerializeField] private PopUp _deathPopUp;
    [SerializeField] private EnemyDisplay _enemy;

    [SerializeField] private DamageNumberSpawner _playerDamageNumberSpawner;
    [SerializeField] private DamageNumberSpawner _enemyDamageNumberSpawner;
    [SerializeField] private QuickPopUp[] _quickPopUps;
    [SerializeField] private QuickPopUp _mainQuickPopUp;
    [SerializeField] private EnemyArrowIndicator _arrowIndicator;

    [Obsolete("Create a battle ui class that deals with this")] [SerializeField] private AlphaOfAllChildren _battleUi;
    [Obsolete("Create a battle ui class that deals with this")] [SerializeField] private RobustLerperSerialized _battleUiLerper;

    [SerializeField] private Sprite _questionMark;
    [SerializeField] private SmoothPopUpManager _smoothPopUps;
    [SerializeField] private InteractionController _interactionController;

    private Player _player;
    private Inventory _inventory;
    private GameEvents _events;

    public void Initialize(Player player, Inventory inventory, GameEvents events)
    {
        _player = player;
        _inventory = inventory;
        _events = events;

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

        _interactionController.OnFightEnter += _arrowIndicator.Disable;

        _interactionController.OnFightStart += () =>
        {
            _battleUi.gameObject.SetActive(true);
            _battleUi.SetAlphaOfAllChildren(0);
            _battleUiLerper.StartLerping();
        };

        _interactionController.OnEnemyDeath += (it) =>
        {
            _smoothPopUps.Enable();
            _quickPopUps.ForEach(q => q.Enable());
            _battleUiLerper.StartReverseLerp();
        };

        _interactionController.OnPlayerDeathReset += () =>
        {
            _battleUi.gameObject.SetActive(false);
        };

        _inventory.OnNewRecipeCraftable += recipe =>
        {
            string postText = "";
            foreach (var component in recipe.Components)
            {
                postText +=
                    $"{component.Amount} {component.Item.GetName()}{(component.Amount > 1 ? "s" : "")}\n";
            }
            _mainQuickPopUp.Show(_questionMark, $"<size=150%>New Craftable Recipe!</size>\nA new recipe is now craftable!\nRequires:{postText}");
        };

        _events.AchievementObtained += a =>
        {
            _mainQuickPopUp.Show(a.AchievementIcon, "<size=150%>Achievement!</size>\n" + a.AchievementCompletedText);
        };
    }

    private void Update()
    {
        _battleUiLerper.Update(Time.deltaTime);
    }
}
