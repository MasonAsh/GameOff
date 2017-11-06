using Godot;
using System;

public class Health : Node
{
    [Export]
    private float baseHealth = 100;

    private float health;

    public override void _Ready()
    {
        health = baseHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            if (GetParent().HasMethod("Die"))
            {
                GetParent().Call("Die");
            }
            else
            {
                GetParent().QueueFree();
            }
        }
    }
}