namespace ToTheMoon.Core.Tests.Movement;

using Core.Movement;
using Godot;
using Moq;

[TestFixture]
public class MouseMovementTests
{
    private MouseMovement movement;
    private Mock<IMousePositionProjector> projector;
    private Mock<IReferencePosition> reference;
    
    [SetUp]
    public void SetUp()
    {
        projector = new Mock<IMousePositionProjector>();
        reference = new Mock<IReferencePosition>();
        movement = new MouseMovement(reference.Object, projector.Object);
    }

    [Test]
    public void UpdateVector_ProjectsMousePosition()
    {
        projector.Setup(mock => mock.Project(Vector2.Zero)).Returns(Vector3.Zero);
        
        movement.UpdateVector(Vector2.Zero);
        
        projector.Verify(mock => mock.Project(Vector2.Zero), Times.Once);
    }
    
    [TestCase(0, 0, 0, 0)]
    [TestCase(1, 1, 0, 0)]
    [TestCase(0, 0, 1, 1)]
    [TestCase(1, 1, 1, 1)]
    public void UpdateVector_SetsCurrentVector_BySubtractingProjectedAndReferencePositions(float projX, float projY, float posX, float posY)
    {
        var projected = new Vector3(projX, 0f, projY);
        var pos = new Vector3(posX, 0f, posY);
        projector.Setup(mock => mock.Project(Vector2.Zero)).Returns(projected);
        reference.Setup(mock => mock.Position).Returns(pos);
        
        movement.UpdateVector(Vector2.Zero);
        
        Assert.That(movement.CurrentVector, Is.EqualTo(projected - pos));
    }
    
    [Test]
    public void UpdateVector_InvokesOnVectorChanged_WithCurrentVector()
    {
        var projected = new Vector3(1, 0, 1);
        var pos = new Vector3(0, 0, 0);
        var calledVector = Vector3.Zero;
        var called = false;
        projector.Setup(mock => mock.Project(Vector2.Zero)).Returns(projected);
        reference.Setup(mock => mock.Position).Returns(pos);
        movement.OnVectorChanged += vector2 =>
        {
            calledVector = vector2;
            called = true;
        };
        
        movement.UpdateVector(Vector2.Zero);
        
        Assert.Multiple(() =>
        {
            Assert.That(calledVector, Is.EqualTo(movement.CurrentVector));
            Assert.That(called, Is.True);
        });
    }
    
    [Test]
    public void CurrentVectorGetter_InvokesUpdateInput()
    {
        var projected = new Vector3(1, 0, 1);
        var pos = new Vector3(0, 0, 0);
        var called = false;
        projector.Setup(mock => mock.Project(Vector2.Zero)).Returns(projected);
        reference.Setup(mock => mock.Position).Returns(pos);
        // This is supposed to be called so this should be a valid case.
        movement.OnVectorChanged += _ =>
        {
            called = true;
        };

        _ = movement.CurrentVector;
        
        Assert.That(called, Is.True);
    }
}