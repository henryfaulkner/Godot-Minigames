// C//-implementation of the following gdscript controller
// https://github.com/JheKWall/Godot-Stair-Step-Demo

/// Credits:
// Special thanks to Majikayo Games for original solution to stair_step_down!
// (https://youtu.be/-WjM1uksPIk)
//
// Special thanks to Myria666 for their paper on Quake movement mechanics (used for stair_step_up)!
// (https://github.com/myria666/qMovementDoc)
//
// Special thanks to Andicraft for their help with implementation stair_step_up!
// (https://github.com/Andicraft)

/// Notes:
// 0. All shape colliders are supported. Although, I would recommend Capsule colliders for enemies
//		as it works better with the Navigation Meshes. Its up to you what shape you want to use
//		for players.
//
// 1. To adjust the step-up/down height, just change the MAX_STEP_UP/MAX_STEP_DOWN values below.
//
// 2. This uses Jolt Physics as the default Godot Physics has a few bugs:
//	1: Small gaps that you should be able to fit through both ways will block you in Godot Physics.
//		You can see this demonstrated with the floating boxes in front of the big stairs.
//	2: Walking into some objects may push the player downward by a small amount which causes
//		jittering and causes the floor to be detected as a step.
//	TLDR: This still works with default Godot Physics, although it feels a lot better in Jolt Physics.

using Godot;
using System;

public partial class PlayerController : CharacterBody3D
{
	#region Annotations

	[ExportGroup("Player Settings")]
	[Export]
	public float PlayerSpeed { get; set; } = 10.0f;

	[Export]
	public float JumpVelocity { get; set; } = 6.0f;

	[Export]
	public float MaxStepUp { get; set; } = 0.5f;

	[Export] 
	public float MaxStepDown { get; set; } = -0.5f;

	[Export] 
	public float MouseSensitivity { get; set; } = 0.4f;

	[Export] 
	public float CameraSmoothing { get; set; } = 18.0f;

	#endregion

	#region Nodes

	[ExportGroup("Player Object")]
	[Export]
	public CollisionShape3D PlayerCollider { get; set; }

	[ExportGroup("Player Camera")]
	[Export]
	public Node3D CameraNeck { get; set; }

	[Export]
	public Node3D CameraHead { get; set; }

	[Export]
	public Camera3D PlayerCamera { get; set; }

	#endregion

	#region Variables

	private bool IsGrounded { get; set; } // If player is grounded this frame
	private bool WasGrounded { get; set; } // If player was grounded last frame

	private Vector3 WishDir { get; set; } // Player input direction (wasd)

	private static readonly Vector3 Vertical = new Vector3(0, 1, 0); // Shortcut for converting vectors to vertical
	private static readonly Vector3 Horizontal = new Vector3(1, 0, 1); // Shortcut for converting vectors to horizontal

	private static readonly float Gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");

	#endregion

	#region Implementation

	// Scene loaded
	public override void _Ready()
	{
		GD.Print("_Ready");
		try
		{
			// Capture mouse on start
			Input.SetMouseMode(Input.MouseModeEnum.Captured);
		} 
		catch (Exception ex)
		{
			GD.PrintErr($"Error in _Ready {ex.Message}");
		}
	}

	// Input event detected
	public override void _Input(InputEvent @event)
	{
		// Handle ESC input
		if (@event.IsActionPressed("mouse_toggle")) 
		{
			ToggleMouseMode();
		}

		// Handle Mouse input
		if (@event is InputEventMouseMotion mouseEvent && Input.GetMouseMode() == Input.MouseModeEnum.Captured)
		{
			ProcessCameraInput((InputEventMouseMotion)@event);
		}
	}

	public void ProcessCameraInput(InputEventMouseMotion @event)
	{
		var yRotation = Mathf.DegToRad(-@event.Relative.X * MouseSensitivity);
		RotateY(yRotation);
		CameraHead.RotateY(yRotation);
		PlayerCamera.RotateX(Mathf.DegToRad(-@event.Relative.Y * MouseSensitivity));
		PlayerCamera.Rotation = new Vector3(
									Mathf.Clamp(PlayerCamera.Rotation.X, Mathf.DegToRad(-90), Mathf.DegToRad(90)),
									PlayerCamera.Rotation.Y,
									PlayerCamera.Rotation.Z
								);
	}

	public void ToggleMouseMode()
	{
		if (Input.MouseMode == Input.MouseModeEnum.Captured)
		{
			Input.SetMouseMode(Input.MouseModeEnum.Visible);
		}
		else 
		{
			Input.SetMouseMode(Input.MouseModeEnum.Captured);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		try
		{
			// Lock player collider rotation
			PlayerCollider.GlobalRotation = Vector3.Zero;

			// Update player state
			WasGrounded = IsGrounded;
			IsGrounded = IsOnFloor();
			
			// Get player input direction
			var inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
			WishDir = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

			// Handle gravity
			if (!IsOnFloor()) 
			{
				Velocity = new Vector3(
								Velocity.X,
								Velocity.Y - (Gravity * (float)delta),
								Velocity.Z
							);
			}

			// Handle jump 
			if (Input.IsActionPressed("move_jump"))
			{
				Velocity = new Vector3(
								Velocity.X,
								JumpVelocity,
								Velocity.Z
							);
			}

			// Handle WASD movement
			Velocity = new Vector3(
							WishDir.X * PlayerSpeed,
							Velocity.Y,
							WishDir.Z * PlayerSpeed
						);

			// Stair Step Up
			HandleStairStepUp();

			// Move 
			MoveAndSlide();

			// Stair Step Down
			HandleStairStepDown();

			// Smooth camera
			SmoothCameraJitter(delta);
		} 
		catch (Exception ex)
		{
			GD.PrintErr($"Error in _PhysicsProcess {ex.Message}");
		}
	}

	private void HandleStairStepDown()
	{
		if (IsGrounded) return;

		if (Velocity.Y <= 0 && WasGrounded)
		{
			var bodyTestResult = new PhysicsTestMotionResult3D();
			var bodyTestParams = new PhysicsTestMotionParameters3D();

			bodyTestParams.From = GlobalTransform; // Get the player's current global_transform
			bodyTestParams.Motion = new Vector3(0, MaxStepDown, 0); // Project the player downward

			// Enters if a collision is detected
			if (PhysicsServer3D.BodyTestMotion(GetRid(), bodyTestParams, bodyTestResult))
			{
				// Get distance to step and move player downward by that much
				Position = new Vector3(
								Position.X,
								Position.Y + bodyTestResult.GetTravel().Y,
								Position.Z
							);
				ApplyFloorSnap();
				IsGrounded = true;
			}
		}
	}

	private void HandleStairStepUp()
	{
		if (WishDir == Vector3.Zero) return;

		// Step 0. Initialize Testing Variables
		var bodyTestResult = new PhysicsTestMotionResult3D();
		var bodyTestParams = new PhysicsTestMotionParameters3D();
		var testTransform = GlobalTransform; // Storing curr global transform for testing
		var distance = WishDir * 0.1f; // Distance forward we want to check
		bodyTestParams.From = GlobalTransform; // Set self as origin point
		bodyTestParams.Motion = distance; // Project player forward

		// Pre-check: are we colliding?
		// If we don't collide, exit
		if (!PhysicsServer3D.BodyTestMotion(GetRid(), bodyTestParams, bodyTestResult))
		{
			return;
		}

		// Step 1. Move test transform to collision location
		var remainder = bodyTestResult.GetRemainder(); // Get remainder from collision
		testTransform = testTransform.Translated(bodyTestResult.GetTravel()); // Move test transform by distance before collision

		// Step 2. Move test transform up to ceiling (if any)
		var stepUp = MaxStepUp * Vertical;
		bodyTestParams.From = testTransform;
		bodyTestParams.Motion = stepUp;
		PhysicsServer3D.BodyTestMotion(GetRid(), bodyTestParams, bodyTestResult);
		testTransform = testTransform.Translated(bodyTestResult.GetTravel());

		// Step 3. Move test transform forward by remaining distance 
		bodyTestParams.From = testTransform;
		bodyTestParams.Motion = remainder;
		PhysicsServer3D.BodyTestMotion(GetRid(), bodyTestParams, bodyTestResult);
		testTransform = testTransform.Translated(bodyTestResult.GetTravel());

		// Step 3.5. Project remainder along wall normal (if any)
		// So you can walk into a wall and up a step 
		if (bodyTestResult.GetCollisionCount() != 0)
		{
			float remainderLen = bodyTestResult.GetRemainder().Length();

			// Vector projection into wall
			var wallNormal = bodyTestResult.GetCollisionNormal();
			var projectedVector = WishDir.Project(wallNormal).Normalized();

			bodyTestParams.From = testTransform;
			bodyTestParams.Motion = remainder * projectedVector;
			PhysicsServer3D.BodyTestMotion(GetRid(), bodyTestParams, bodyTestResult);
			testTransform = testTransform.Translated(bodyTestResult.GetTravel());
		}

		// Step 4. Move test transform down onto step
		bodyTestParams.From = testTransform;
		bodyTestParams.Motion = MaxStepUp * -Vertical;

		// If we don't collide, exit
		if (!PhysicsServer3D.BodyTestMotion(GetRid(), bodyTestParams, bodyTestResult))
		{
			return;
		}

		testTransform = testTransform.Translated(bodyTestResult.GetTravel());

		// Step 5. Check floor normal for un-walkable slope
		var surfaceNormal = bodyTestResult.GetCollisionNormal();
		if (Mathf.Snapped(surfaceNormal.AngleTo(Vertical), 0.001f) > FloorMaxAngle)
		{
			return;
		}

		// Step 6. Move player up
		var stepUpDist = testTransform.Origin.Y - GlobalPosition.Y;

		Velocity = new Vector3(
						Velocity.X,
						0,
						Velocity.Z	
					);
		GlobalPosition = new Vector3(
							GlobalPosition.X,
							testTransform.Origin.Y,
							GlobalPosition.Z
						);
	}

	private void SmoothCameraJitter(double delta)
	{
		CameraHead.GlobalPosition = new Vector3(
										CameraNeck.GlobalPosition.X,
										(float)Mathf.Lerp(CameraHead.GlobalPosition.Y, CameraNeck.GlobalPosition.Y, CameraSmoothing * delta),
										CameraNeck.GlobalPosition.Z
									);

		// Limit how far camera can lag behind its desired position
		CameraHead.GlobalPosition = new Vector3(
										CameraHead.GlobalPosition.X,
										Mathf.Clamp(
											CameraHead.GlobalPosition.Y, 
											-CameraNeck.GlobalPosition.Y - 1, 
											CameraNeck.GlobalPosition.Y + 1
										),
										CameraHead.GlobalPosition.Z
									);
	}

	#endregion
}
