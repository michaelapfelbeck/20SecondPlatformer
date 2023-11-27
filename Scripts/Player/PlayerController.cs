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

    private PlayerStateMachine stateMachine;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("PlayerController Ready!");
        
        if(sprite != null)
        {
            GD.Print("already setup");
        }
        Node result = GetNode("AnimatedSprite");
        sprite = (AnimatedSprite)result;

        SetupStateMachine();
    }

    private void SetupStateMachine()
    {
        stateMachine = new PlayerStateMachine();

        Idle idle = new Idle();
        Running run = new Running();
        Falling fall = new Falling();
        Jumping jump = new Jumping();

        stateMachine.At(idle, fall, () => !OnGround());
        stateMachine.At(fall, idle, OnGround);

        stateMachine.SetState(idle);
    }

    private bool OnGround()
    {
        return this.IsOnFloor();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        base._Process(delta);

        stateMachine.Tick();

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
