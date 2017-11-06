using Godot;
using System;

public class Projectile : Node2D
{
    [Export]
    private float speed = 700;

    public override void _Ready()
    {
        SetProcess(true);
    }

    public override void _Process(float delta)
    {
        var theta = GetGlobalRotation();

        var dir = new Vector2(Mathf.cos(theta), Mathf.sin(theta));

        var offset = dir * delta * speed;

        Translate(offset);
    }
}
