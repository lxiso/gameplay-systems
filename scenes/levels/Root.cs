using Godot;
using System;

public partial class Root : Node3D
{
    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);

		if (@event.IsActionPressed("ui_cancel", false))
		{
			if (Input.MouseMode == Input.MouseModeEnum.Captured)
			{
				Input.MouseMode = Input.MouseModeEnum.Visible;
			}
			else
			{
				Input.MouseMode = Input.MouseModeEnum.Captured;
			}
		}
    }

}
