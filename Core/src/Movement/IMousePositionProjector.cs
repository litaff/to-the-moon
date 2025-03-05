namespace ToTheMoon.Core.Movement;

using Godot;

public interface IMousePositionProjector
{
    public Vector3 Project(Vector2 screenPosition);
}