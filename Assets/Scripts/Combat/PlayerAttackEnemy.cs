using System.Collections;
using System.Collections.Generic;
using G4AW2.Combat;
using UnityEngine;

public class PlayerAttackEnemy : MonoBehaviour {
    public Player Player;
    public EnemyDisplay Enemy;

    public void OnScreenTap() {
        print("Attacking enemy");
        Enemy.ApplyDamage(Player.GetLightDamage());
    }

    public void OnScreenSwipe(Vector3[] points) {
        print("Swiping enemy");
        Enemy.ApplyDamage(Player.GetHeavyDamage(points));
    }

    public void AttackPlayer(int damage) {
        print("Player Attacked with: " + damage + " damage");
        Player.Hit(damage);
    }
}
