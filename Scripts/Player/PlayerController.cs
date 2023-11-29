using Godot;
using System;

public class PlayerController : KinematicBody2D
{
    public Vector2 velocity = new Vector2(0, 0);
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

    public override void _Ready()
    {
        Node result = GetNode("AnimatedSprite");
        sprite = (AnimatedSprite)result;

        SetupStateMachine();
    }

    private void SetupStateMachine()
    {
        stateMachine = new PlayerStateMachine();

        Idle idle = new Idle(this, sprite);
        Running run = new Running(this, sprite);
        Falling fall = new Falling(this, sprite);
        Jumping jump = new Jumping(this, sprite);

        stateMachine.At(idle, fall, () => !IsOnGround());
        stateMachine.At(fall, idle, () => IsOnGround() && !IsRunning());
        stateMachine.At(jump, idle, () => IsOnGround() && !IsRunning());
        stateMachine.At(fall, run, IsRunning);
        stateMachine.At(jump, run, IsRunning);
        stateMachine.At(jump, fall, IsFalling);
        stateMachine.At(idle, fall, IsFalling);
        stateMachine.At(idle, jump, IsJumping);
        stateMachine.At(idle, run, IsRunning);
        stateMachine.At(run, idle, () => !IsRunning());

        stateMachine.SetState(idle);
    }

    private bool IsOnGround()
    {
        return this.IsOnFloor();
    }
    private bool IsFalling()
    {
        return !IsOnGround() && this.velocity.y > 0;
    }
    private bool IsJumping()
    {
        return !IsOnGround() && this.velocity.y < 0;
    }
    private bool IsRunning()
    {
        return IsOnGround() && this.velocity.x != 0;
    }

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

        velocity = MoveAndSlide(velocity, Vector2.Up);

        stateMachine.Tick();

        if (Position.y > fallDeathHeight)
        {
            die();
        }
    }

    private void die()
    {
        GetTree().ReloadCurrentScene();
    }
}
