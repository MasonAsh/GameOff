using Godot;
using System;

public class ProjectileSpawn : Node2D
{
    [Export(GD.PROPERTY_HINT_FILE, "*tscn")]
    String projectileScenePath;

    PackedScene projectileScene;

    public override void _Ready()
    {
        projectileScene = ResourceLoader.Load(projectileScenePath) as PackedScene;
    }

    public void _Spawn()
    {
        var projectile = projectileScene.Instance() as Node2D;

        projectile.GlobalPosition = GetGlobalPosition();
        projectile.GlobalRotation = GetGlobalRotation();

        GetTree().GetRoot().AddChild(projectile);
    }
}
