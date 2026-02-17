using Godot;
using System;

public partial class Kick : Node3D
{
	public Area3D area;
	public PlayerController player;

	[Export] float damage = 30f;

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);

		if (@event.IsActionPressed("action_left"))
		{
			Attack();
		}
    }

	private void Attack()
	{
		foreach (Node3D body in area.GetOverlappingBodies())
		{
			GD.Print(body.Name);
			if (body.HasMethod("TakeDamage"))
			{
				EnemyController enemy = body as EnemyController;
				enemy.TakeDamage(damage, player, 100f, true);
			}
		}
	}

    public override void _Ready()
    {
        base._Ready();

		area = FindChild("Area3D") as Area3D;
		player = GetParent<PlayerController>();
    }

}
