using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export]
    public float motionSpeed = 140;

    public override void _Ready()
    {
        SetPhysicsProcess(true);
    }

    public override void _PhysicsProcess(float delta)
    {
        var offset = new Vector2();

        if (Input.IsActionPressed("move_up")) {
            offset.y -= 1;
        }

        if (Input.IsActionPressed("move_down")) {
            offset.y += 1;
        }

        if (Input.IsActionPressed("move_left")) {
            offset.x -= 1;
        }

        if (Input.IsActionPressed("move_right")) {
            offset.x += 1;
        }

        offset = offset.normalized();
        offset *= motionSpeed * delta;

        this.MoveAndCollide(offset);
    }
}
