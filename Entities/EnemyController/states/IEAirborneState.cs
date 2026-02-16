using Godot;
using System;

public partial class IEAirborneState : IEnemyState
{
    public void Enter(EnemyController enemy)
    {
        GD.Print("enemy airborne");
    }
    public void TakeDamage(EnemyController enemy, float amount, Node source, float knockback = 0f)
    {
        
    }
    public void Update(EnemyController enemy, double delta)
    {
        enemy.Velocity = new Vector3(0, -enemy.gravity, 0);
        if (enemy.IsOnFloor())
        {
            enemy.ChangeState(new IEGroundedState());
        }
    }
    public void Exit(EnemyController player)
    {
        
    }
}