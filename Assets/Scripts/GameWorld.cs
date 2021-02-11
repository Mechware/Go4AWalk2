using G4AW2.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for everything that appears in the game world
/// </summary>
public class GameWorld : MonoBehaviour
{
    [SerializeField] private DragObject _dragger;
    [SerializeField] private Player _player;
    [SerializeField] private PlayerAnimations _playerAnims;
    [SerializeField] private EnemyArrowIndicator _arrow;
    [SerializeField] private ScrollingImages _background;

    [SerializeField] private DeadEnemyController _deadEnemies;
    [SerializeField] private MiningPoints _miningPoints;

    private Inventory _inventory;

    public void Initialize(Inventory i)
    {
        _inventory = i;

        _background.OnScrolled += _miningPoints.Scroll;
        _background.OnScrolled += _deadEnemies.Scroll;

        _dragger.OnDragEvent += _background.Pause;
        _dragger.OnDragEvent += _playerAnims.StopWalking;
        _dragger.OnDragEvent += () => _arrow.SetOnMainScreen(false);

        _dragger.OnReset += _background.Play;
        _dragger.OnReset += _playerAnims.StartWalking;
        _dragger.OnReset += () => _arrow.SetOnMainScreen(true);

        _miningPoints.OnItemReceived += _inventory.Add;
    }
}
