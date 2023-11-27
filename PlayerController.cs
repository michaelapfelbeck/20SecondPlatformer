using Godot;
using System;

public class PlayerController : KinematicBody2D
{
    Vector2 velocity = new Vector2(0, 0);
    [Export]
    public float gravity = 800;
    [Export]
    public float speed = 200;
    [Export]
    public float jumpForce = 800;
    [Export]
    public float fallDeathHeight = 600;

    private AnimatedSprite sprite;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("PlayerController Ready!");
        Node result = GetNode("AnimatedSprite");
        sprite = (AnimatedSprite)result;
    }
    /*
     func _process(delta):
    velocity.x = 0
    if Input.is_action_pressed("move_left"):
        velocity.x -= speed
    if Input.is_action_pressed("move_right"):
        velocity.x += speed
        
    velocity.y += gravity * delta
    
    if Input.is_action_pressed("jump") && is_on_floor():
        velocity.y -= jumpForce
    
    if velocity.x < 0:
        sprite.flip_h = true
    elif velocity.x > 0:
        sprite.flip_h = false
    
    velocity = move_and_slide(velocity, Vector2.UP)
    
    if position.y > fallDeathHeight:
        die()
     */
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        base._Process(delta);
        velocity.x = 0;
        if (Input.IsActionPressed("move_left"))
        {
            velocity.x -= speed;
        }
        if (Input.IsActionPressed("move_right"))
        {
            velocity.x += speed;
        }

        velocity.y += gravity * delta;

        if (Input.IsActionPressed("jump") && IsOnFloor())
        {
            velocity.y -= jumpForce;
        }

        if(velocity.x == 0)
        {
            sprite.Play("front");
        }
        else if(velocity.x < 0)
        {
            sprite.FlipH = true;
            sprite.Play("walk");
        } else if(velocity.x > 0)
        {
            sprite.FlipH = false;
            sprite.Play("walk");
        }

        velocity = MoveAndSlide(velocity, Vector2.Up);

        if(Position.y > fallDeathHeight)
        {
            die();
        }
    }

    private void die()
    {
        GetTree().ReloadCurrentScene();
    }
}
