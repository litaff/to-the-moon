namespace ToTheMoon.scripts;

using Core.Movement;
using Godot;

public partial class PlayerPosition : Node3D, IReferencePosition
{
    [Export]
    private float speed;
    [Export]
    private ProjectionCamera camera;
    
    private IMovement movement;
    
    public override void _Ready()
    {
        base._Ready();
        movement = new MouseMovement(this, camera);
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        movement.UpdateInput(@event);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        Move(movement.CurrentVector, (float)delta);
    }

    private void Move(Vector3 vector, float delta)
    {
        var offset = vector * speed * delta;
        Translate(offset);
    }
}