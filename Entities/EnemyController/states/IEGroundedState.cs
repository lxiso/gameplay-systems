using Godot;
using System;

public partial class IEGroundedState : IEnemyState
{
    Vector3 velocity = Vector3.Zero;
    public void Enter(EnemyController enemy)
    {
        GD.Print("enemy grounded");
    }
    public void TakeDamage(EnemyController enemy, float amount, Node source, float knockback = 0f, bool canStun = false)
    {
        enemy.health -= amount;
        if (source is Node3D)
        {
            Node3D origin = source as Node3D;
            enemy.ApplyImpulse((enemy.GlobalPosition - origin.GlobalPosition).Normalized() * knockback);
            if (canStun == true) enemy.ChangeState(new IEStunnedState());
        }
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