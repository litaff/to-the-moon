namespace ToTheMoon.scripts;

using System;
using Core.Movement;
using Godot;

public partial class ProjectionCamera : Camera3D, IMousePositionProjector
{
    public Vector3 Project(Vector2 screenPosition)
    {
        var targetPlane = new Plane(Vector3.Up, Vector3.Zero);
        var from = ProjectRayOrigin(screenPosition);
        var to = ProjectRayNormal(screenPosition);
        var currentCursorPosition = targetPlane.IntersectsRay(from, to);
        if (currentCursorPosition is null)
        {
            throw new NullReferenceException("Projection failed, rays might be setup in a wrong way.");
        }
        return currentCursorPosition.Value;
    }
}