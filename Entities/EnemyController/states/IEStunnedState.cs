using Godot;
using System;

public partial class IEStunnedState : IEnemyState
{
    public void Enter(EnemyController enemy)
    {
        
    }
    public void TakeDamage(EnemyController enemy, float amount, Node source, float knockback = 0f)
    {
        enemy.health -= amount;
        if (source is Node3D body)
        {
            Vector3 direction = (enemy.GlobalPosition - body.GlobalPosition).Normalized();
            enemy.Velocity += direction * knockback;
        }
    }
    public void Update(EnemyController enemy, double delta)
    {
        if (!enemy.IsOnFloor())
        {
            enemy.Velocity -= new Vector3(0, -enemy.gravity, 0);
        }
    }
    public void Exit(EnemyController player)
    {
        
    }
}