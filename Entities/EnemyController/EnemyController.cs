using Godot;
using System;

public partial class EnemyController : CharacterBody3D
{
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
    public enum TYPE {GROUNDED, FLYING}
    [Export] public TYPE type = TYPE.GROUNDED;
    public CollisionShape3D collider;
    public MeshInstance3D mesh;
	public IEnemyState currentState;

    [Export] public float health = 100;

    public void TakeDamage (float amount, Node source, float knockback = 0f)
    {
        currentState?.TakeDamage(this, amount, source, knockback = 0f);
    }

	public void ChangeState(IEnemyState state)
    {
        currentState?.Exit(this);
        currentState = state;
        currentState?.Enter(this);
    }

    public override void _Ready()
    {
        ChangeState(new IEGroundedState());
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        MoveAndSlide();
    }
}
