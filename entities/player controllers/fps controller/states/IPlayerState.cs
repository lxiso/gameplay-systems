using Godot;
using System;

public interface IPlayerState
{
    void Enter(PlayerController player);
    void HandleInput(PlayerController player, InputEvent @event);
    void Update(PlayerController player, double delta);
    void Exit(PlayerController player);
}