using Godot;
using System;

public class Weapon : Node2D
{
    [Export]
    private NodePath projectileSpawnPath;

    ProjectileSpawn projectileSpawn;

    public override void _Ready()
    {
        if (projectileSpawnPath != null)
        {
            projectileSpawn = GetNode(projectileSpawnPath) as ProjectileSpawn;
        }
        else
        {
            projectileSpawn = FindNode("ProjectileSpawn") as ProjectileSpawn;
            
            if (projectileSpawn == null)
            {
                GD.printerr("Weapon has no specified projectileSpawn and could not find a projectile spawn.");
            }
        }

        SetProcessInput(true);
    }

    public override void _Input(InputEvent ev)
    {
        if (ev is InputEventMouse)
        {
            var mousePos = GetGlobalMousePosition();
            var diff = mousePos - GetGlobalPosition();
            var theta = Mathf.atan2(diff.y, diff.x);

            SetRotation(theta);
        }

        if (ev is InputEventMouseButton)
        {
            if (ev.IsPressed())
            {
                projectileSpawn._Spawn();
            }
        }
    }
}
