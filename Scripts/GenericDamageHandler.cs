using Godot;
using System;

public class GenericDamageHandler : StaticBody2D
{
    public override void _Ready()
    {
    }

    public void OnHit(float damage)
    {
        GetNode("Health").Call("TakeDamage", damage);
    }
}
