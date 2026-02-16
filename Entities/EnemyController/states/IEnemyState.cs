using Godot;
using System;

public interface IEnemyState
{
    public void Enter(EnemyController enemy);
    public void TakeDamage(EnemyController enemy, float amount, Node source, float knockback = 0f);
    public void Update(EnemyController enemy, double delta);
    public void Exit(EnemyController player);
}