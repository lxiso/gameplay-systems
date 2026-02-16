using Godot;
using System;

public partial class IEGroundedState : IEnemyState
{
    public void Enter(EnemyController enemy)
    {
        GD.Print("enemy grounded");
    }
    public void TakeDamage(EnemyController enemy, float amount, Node source, float knockback = 0f)
    {
        
    }
    public void Update(EnemyController enemy, double delta)
    {
        if (!enemy.IsOnFloor())
        {
            enemy.ChangeState(new IEAirborneState());
        }
    }
    public void Exit(EnemyController player)
    {
        
    }
}