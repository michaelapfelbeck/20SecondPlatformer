using Godot;

public enum Direction
{
    Left,
    Right
}

public class PlayerController : KinematicBody2D, PlayerBlackboard
{
    public bool debug = false;
    public Vector2 velocity = new Vector2(0, 0);
    // values are a;; pixels/second unless otherwise noted
    [Export]
    public float jumpHeight = 128;
    [Export]
    public float jumpTimeToPeak = 1;
    // when jumping, jumpVelocityCut = true cuts the players' upward velocity in half when they let go of the button,
    // this lets you have a variable jump height without making fall gravity higher (jumpTimeToPeak vs jumpTimeToFall)
    // maybe re-use jumpTimeToFall for how much to cut the velocity?
    [Export]
    public bool jumpVelocityCut = false;
    [Export]
    public float jumpTimeToFall = 0.5f;
    [Export]    // seconds, time to go from stopped to max running speed, 0 = instant
    public float accTime = 0.5f;
    [Export]    // seconds, time to go from max running speed to stopped if the let go of the controls, 0 = instant
    public float deccTime = 0.5f;
    [Export]
    public float playerMaxSpeed = 200;
    [Export]
    public float dashMultiplier = 2;
    [Export]
    public float dashLength = 1;
    [Export]
    public float dashCooldown = 1;

    [Export]
    public float jumpBufferLifespan = 0.2f;
    [Export]
    public bool doubleJump = true;
    [Export]
    public float terminalVelocity = 1000;
    [Export]
    public float wallSlideVelocity = 100;

    [Export]
    public float fallDeathHeight = 600;
    [Export]
    public float coyoteTime = 0.1f;

    // derive variables in DeriveVariables()
    private float jumpForce = 800;
    private float gravity = 600;
    private float fallGravity = 1200;
    private float acceleration = 1200;
    private float decceleration = 1200;

    // Blackboard variables
    public float Gravity { get { return gravity; } }
    public float FallGravity { get { return fallGravity; } }
    public float TerminalVelocity { get { return sliding ? wallSlideVelocity : terminalVelocity; } }
    public float WallSlideVelocity { get { return wallSlideVelocity; } }
    public float PlayerMaxSpeed { get { return dashing ? playerMaxSpeed * dashMultiplier : playerMaxSpeed; } }
    public bool InstantAcceleration { get; private set; }
    public float Acceleration { get { return dashing ? acceleration * dashMultiplier : acceleration; } }
    public float Decceleration { get => decceleration; }
    public float JumpForce { get { return jumpForce; } }
    public bool VelocityCut { get { return jumpVelocityCut; } }
    public bool DoubleJump { get { return doubleJump; } }
    public bool DoubleJumped { get; set; }
    public BufferButton JumpBuffer { get { return jumpBuffer; } }
    public BoolBuffer CoyoteBuffer { get => coyoteBuffer; }
    public Vector2 Velocity { get { return velocity; } set { velocity = value; } }
    public bool IsWallSlide => sliding;
    public Direction WallSlideDirection => slideSide;

    private AnimatedSprite sprite;
    private PlayerStateMachine stateMachine;
    private BufferButton jumpBuffer;
    private BoolBuffer coyoteBuffer;
    private BufferButton dashBuffer;
    private bool dashing = false;
    private bool sliding = false;
    private Direction slideSide;
    private float dashTime = 0f;
    private float dashReset = 0f;

    private RayCast2D[] rightRays;
    private RayCast2D[] leftRays;
    private RayCast2D[] bottomRays;

    public override void _Ready()
    {
        Node result = GetNode("AnimatedSprite");

        rightRays = new RayCast2D[] { GetNode<RayCast2D>("RightTopRay"), GetNode<RayCast2D>("RightBottomRay") };
        leftRays = new RayCast2D[] { GetNode<RayCast2D>("LeftTopRay"), GetNode<RayCast2D>("LeftBottomRay") };
        bottomRays = new RayCast2D[] { GetNode<RayCast2D>("BottomLeftRay"), GetNode<RayCast2D>("BottomRightRay") };

        sprite = (AnimatedSprite)result;

        jumpBuffer = new BufferButton("jump", jumpBufferLifespan);

        coyoteBuffer = new BoolBuffer(() => IsOnGround(), coyoteTime);

        dashBuffer = new BufferButton("dash", jumpBufferLifespan);

        DeriveVariables();

        //GD.Print("Jump force: " + jumpForce);
        //GD.Print("gravity force: " + gravity);
        //GD.Print("fall force: " + fallGravity);

        SetupStateMachine();
    }


    private float velocityBuffer = 0;
    public override void _Process(float delta)
    {
        base._Process(delta);
        // update every frame when tweaking variables
        if (debug)
        {
            DeriveVariables();
        }

        DashHandler(delta);

        jumpBuffer.TIck(delta);
        dashBuffer.TIck(delta);
        coyoteBuffer.TIck(delta);

        WallSlideHandler();

        stateMachine.Tick(delta);

        PlatformHandler(delta);

        velocityBuffer = velocity.y;
        velocity = MoveAndSlide(velocity, Vector2.Up);

        if (Position.y > fallDeathHeight)
        {
            die();
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
    }

    private void WallSlideHandler()
    {
        sliding = false;
        if (this.IsOnFloor() || velocity.y < 0)
        {
            return;
        }

        float movementInput = Input.GetAxis("move_left", "move_right");
        if (movementInput == 0)
        {
            return;
        }

        bool rightCollide = RightCollide();
        bool leftCollide = LeftCollide();

        if (rightCollide || leftCollide)
        {
            sliding = true;
            if (rightCollide)
            {
                slideSide = Direction.Right;
            }
            else
            {
                slideSide = Direction.Left;
            }
        }
    }

    private void PlatformHandler(float delta)
    {
        if (bottomRays[0].IsColliding())
        {
            IPlatform collided = (IPlatform)bottomRays[0].GetCollider();
            ApplyPlatformEffects(delta, collided);
        }
        else if (bottomRays[1].IsColliding())
        {
            IPlatform collided = (IPlatform)bottomRays[1].GetCollider();
            ApplyPlatformEffects(delta, collided);
        }
    }

    private void ApplyPlatformEffects(float delta, IPlatform collided)
    {
        Position += collided.PlatformVelocity * delta;
        if(collided.Bounciness > 0 && velocity.y >= 0)
        {
            float frameVelocity = Mathf.Max(Mathf.Abs(velocityBuffer), Mathf.Abs(velocity.y));
            float newVelocity = frameVelocity + Mathf.Abs(collided.Bounciness);
            newVelocity = Mathf.Min(newVelocity, terminalVelocity);
            if (JumpBuffer.JustPressed)
            {
                newVelocity = Mathf.Max(newVelocity, jumpForce * 1.1f);
            }
            velocity.y = -1 * newVelocity;
        }
    }

    private bool RightCollide()
    {
        return rightRays[0].IsColliding() && rightRays[1].IsColliding();
    }
    private bool LeftCollide()
    {
        return leftRays[0].IsColliding() && leftRays[1].IsColliding();
    }


    // https://www.youtube.com/watch?v=hG9SzQxaCm8
    private void DeriveVariables()
    {
        if (accTime <= 0)
        {
            InstantAcceleration = true;
            acceleration = 0;
            decceleration = 0;
        }
        else
        {
            acceleration = playerMaxSpeed / accTime;
            decceleration = playerMaxSpeed / deccTime;
        }

        jumpForce = 2.0f * jumpHeight / jumpTimeToPeak;
        gravity = 2.0f * jumpHeight / (jumpTimeToPeak * jumpTimeToPeak);
        fallGravity = 2.0f * jumpHeight / (jumpTimeToFall * jumpTimeToFall);
    }

    private void SetupStateMachine()
    {
        stateMachine = new PlayerStateMachine();
        stateMachine.Debug = debug;

        Idle idle = new Idle(this, this, sprite);
        Running run = new Running(this, this, sprite);
        Falling fall = new Falling(this, this, sprite);
        Jumping jump = new Jumping(this, this, sprite);
        WallSlide slide = new WallSlide(this, this, sprite);

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
        stateMachine.At(fall, jump, IsJumping, "Double Jump");
        stateMachine.At(fall, slide, () => IsWallSlide, "fall -> wall slide");
        stateMachine.At(slide, idle, () => !IsWallSlide && IsOnGround(), "wall slide -> idle");
        stateMachine.At(slide, fall, () => !IsWallSlide && IsFalling(), "wall slide -> fall");

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

    private void DashHandler(float delta)
    {
        if (dashing && IsOnFloor() && dashTime >= dashLength)
        {
            dashing = false;
        }
        else if (dashing && dashBuffer.Pressed)
        {
            dashTime += delta;
        }
        else if (dashBuffer.JustPressed && IsOnFloor() && dashReset >= dashCooldown)
        {
            dashing = true;
            dashReset = 0;
            // originally this set the player velocity to at least your max run speed when you start dashing
            // turns out that feels super odd from a standstill
            //if(velocity.x < 0)
            //{
            //    velocity.x = Mathf.Min(-1 * PlayerMaxSpeed * (1 + dashMultiplier / 2), velocity.x);
            //} else
            //{
            //    velocity.x = Mathf.Max(PlayerMaxSpeed * (1 + dashMultiplier / 2), velocity.x);
            //}
        }
        else
        {
            dashing = false;
        }

        if (Input.IsActionJustReleased("dash"))
        {
            dashTime = 0;
        }

        if (!dashing)
        {
            dashReset += delta;
        }
    }

    private void die()
    {
        GetTree().ReloadCurrentScene();
    }
}
