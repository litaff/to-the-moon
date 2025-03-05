namespace ToTheMoon.Core.Movement;

using Godot;

public class MouseMovement(IReferencePosition referencePosition, IMousePositionProjector mousePositionProjector) : IMovement
{
    private Vector2 screenPositionCache;
    private Vector3 currentVector;

    public Vector3 CurrentVector
    {
        get
        {
            UpdateVector(screenPositionCache);
            return currentVector;
        }
    }

    public event Action<Vector3>? OnVectorChanged;

    public void UpdateInput(InputEvent? @event)
    {
        if (@event is not InputEventMouseMotion motion) return;
        
        screenPositionCache = motion.Position;
        UpdateVector(screenPositionCache);
    }

    public void UpdateVector(Vector2 screenPosition)
    {
        var position = mousePositionProjector.Project(screenPosition);
        currentVector = position - referencePosition.Position;
        OnVectorChanged?.Invoke(currentVector);
    }
}