using Godot;
using System;

public class Projectile : KinematicBody2D
{
    [Export]
    private float speed = 700;

    [Export]
    private float baseDamage = 50;

    [Export]
    private float timeTillFree = 5; // Free projectile after this many seconds

    public override void _Ready()
    {
        SetProcess(true);

        Timer timer = new Timer();
        timer.WaitTime = timeTillFree;
        timer.Connect("timeout", this, "OnTimeout");
        timer.Start();

        AddChild(timer);
    }

    public override void _Process(float delta)
    {
        var theta = GetGlobalRotation();

        var dir = new Vector2(Mathf.cos(theta), Mathf.sin(theta));

        var offset = dir * delta * speed;

        var collision = MoveAndCollide(offset);

        if (collision != null)
        {
            if (collision.Collider.HasMethod("OnHit"))
            {
                collision.Collider.Call("OnHit", baseDamage);
            }

            QueueFree();
        }
    }

    public void OnTimeout()
    {
        QueueFree();
    }
}
