namespace ToTheMoon.Core.Movement;

using Godot;

public interface IMovement
{
    public Vector3 CurrentVector { get; }
    public event Action<Vector3>? OnVectorChanged;
    public void UpdateInput(InputEvent @event);
}