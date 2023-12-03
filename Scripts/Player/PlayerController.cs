using Godot;
using System;

public class PlayerController : KinematicBody2D, PlayerBlackboard
{
    public Vector2 velocity = new Vector2(0, 0);
    [Export]
    public float gravity = 600;
    [Export]
    public float fallGravity = 1200;
    [Export]
    public float playerMaxSpeed = 200;
    [Export]
    public float jumpForce = 800;
    [Export]
    public float fallDeathHeight = 600;

    public float Gravity { get { return gravity; } }
    public float FallGravity { get { return fallGravity; } }
    public float PlayerMaxSpeed { get { return playerMaxSpeed; } }
    public float JumpForce { get { return jumpForce; } }
    public Vector2 Velocity { get { return velocity; } set { velocity = value; } }

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
        stateMachine.Debug = false;

        Idle idle = new Idle(this, this, sprite);
        Running run = new Running(this, this, sprite);
        Falling fall = new Falling(this, this, sprite);
        Jumping jump = new Jumping(this, this, sprite);

        stateMachine.At(idle, fall, IsFalling, "1");
        stateMachine.At(run, fall, IsFalling, "2");
        stateMachine.At(run, jump, IsJumping, "8");
        stateMachine.At(fall, idle, () => IsOnGround() && !IsRunning(), "3");
        stateMachine.At(jump, idle, () => IsOnGround() && !IsRunning(), "4");
        stateMachine.At(jump, fall, IsFalling, "5");
        stateMachine.At(jump, run, IsRunning, "6");
        stateMachine.At(fall, run, IsRunning, "7");
        stateMachine.At(idle, jump, IsJumping, "9");
        stateMachine.At(idle, run, IsRunning, "10");
        stateMachine.At(run, idle, () => !IsRunning(), "11");

        stateMachine.SetState(idle);
    }

    private bool IsOnGround()
    {
        return this.IsOnFloor();
    }
    private bool IsFalling()
    {
        //if (!IsOnGround())
        //{
        //    GD.Print(String.Format("fall check, velocity.y: {0}, falling: {1}", velocity.y, !IsOnGround() && this.velocity.y > 0));
        //}
        return !IsOnGround() && this.velocity.y > 0;
    }
    private bool IsJumping()
    {
        //if (!IsOnGround())
        //{
        //    GD.Print(String.Format("jump check, velocity.y: {0}, jumping: {1}", velocity.y, !IsOnGround() && this.velocity.y < 0));
        //}
        return !IsOnGround() && this.velocity.y < 0;
    }
    private bool IsRunning()
    {
        return IsOnGround() && this.velocity.x != 0;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        stateMachine.Tick(delta);

        velocity = MoveAndSlide(velocity, Vector2.Up);

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
