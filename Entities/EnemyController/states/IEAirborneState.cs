using Godot;
using System;

public partial class IEAirborneState : IEnemyState
{
    Vector3 velocity = Vector3.Zero;
    public void Enter(EnemyController enemy)
    {
        GD.Print("enemy airborne");
    }
    public void TakeDamage(EnemyController enemy, float amount, Node source, float knockback = 0f, bool canStun = false)
    {
        
    }
    public void Update(EnemyController enemy, double delta)
    {
        if (enemy.IsOnFloor())
        {
            enemy.ChangeState(new IEGroundedState());
        }
    }
    public void Exit(EnemyController player)
    {
        
    }
}