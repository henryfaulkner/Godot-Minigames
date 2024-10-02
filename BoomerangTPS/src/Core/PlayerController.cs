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
using System.Linq;
using System.Collections.Generic;

public partial class PlayerController : CharacterBody3D
{
	#region Annotations

	[ExportGroup("Player Settings")]
	[Export]
	public float PlayerSpeed { get; set; } = 10.0f;

	[Export]
	public float JumpVelocity { get; set; } = 6.0f;

	[Export]
	public int MaxJumpNum { get; set; } = 1;

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

	[Export]
	public Node3D Hand { get; set; }

	[ExportGroup("Player Camera")]
	[Export]
	public Node3D CameraNeck { get; set; }

	[Export]
	public Node3D CameraHead { get; set; }

	[Export]
	public Camera3D PlayerCamera { get; set; }
	
	[ExportGroup("RayCasts")]
	[Export]
	public RayCast3D AimCast { get; set; }

	[Export]
	public RayCast3D GrappleCast { get; set; }
	
	[Export]
	public RayCast3D HeadBonkCast { get; set; }

	#endregion

	#region Variables
	
	private PackedScene BoomerangScene { get; set; }

	private bool IsGrounded { get; set; } // If player is grounded this frame
	private bool WasGrounded { get; set; } // If player was grounded last frame

	private bool IsChargingThrow { get; set; }
	
	private bool IsGrappling { get; set; }
	private Vector3 GrappleHookPoint { get; set; }

	private Vector3 WishDir { get; set; } // Player input direction (wasd)
	private Vector3 ViewDir { get; set; } // Mouse input direction

	private static readonly Vector3 Vertical = new Vector3(0, 1, 0); // Shortcut for converting vectors to vertical
	private static readonly Vector3 Horizontal = new Vector3(1, 0, 1); // Shortcut for converting vectors to horizontal

	private static readonly float Gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");

	private int CurrJumpNum { get; set; } = 0;

	#endregion

	#region Implementation

	#region Runtime Functions

	// Scene loaded
	public override void _Ready()
	{
		try
		{
			// Capture mouse on start
			Input.SetMouseMode(Input.MouseModeEnum.Captured);
			BoomerangScene = GD.Load<PackedScene>("res://src/ObjectLibrary/Boomerang.tscn");
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
		if (@event is InputEventMouseMotion mouseMotionEvent && Input.GetMouseMode() == Input.MouseModeEnum.Captured)
		{
			ProcessCameraInput(mouseMotionEvent);
		}

		// Handle Mouse Button inputs
		if (@event is InputEventMouseButton mouseButtonEvent && Input.GetMouseMode() == Input.MouseModeEnum.Captured)
		{
			if (mouseButtonEvent.Pressed)
			{
				switch (mouseButtonEvent.ButtonIndex)
				{
					case MouseButton.Left:
						if (!IsChargingThrow)
						{
							GD.Print($"Charging boomerang strength");
							IsChargingThrow = true;
						}
						break;
					case MouseButton.Right:
						if (!IsGrappling && GrappleCast.IsColliding())
						{
							GD.Print("Fire grappling hook");
							IsGrappling = true;
							GrappleHookPoint = GrappleCast.GetCollisionPoint() + new Vector3(0, 0.25f, 0);
						}
						break;
				}
			}

			if (!mouseButtonEvent.Pressed)
			{
				if (IsChargingThrow)
				{
					IsChargingThrow = false;
					if (AimCast.IsColliding())
					{
						GD.Print("Throw boomerang");
						Vector3 throwDir = (AimCast.GetCollisionPoint() + new Vector3(0, 0.25f, 0)).Normalized();
						ThrowBoomerang(throwDir);
					}
				}
			}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		try
		{
			// Lock player collider rotations
			PlayerCollider.GlobalRotation = Vector3.Zero;

			// Update player state
			WasGrounded = IsGrounded;
			IsGrounded = IsOnFloor();
			
			// if in Grapple-state, ignore all other character motion
			if (IsGrappling)
			{
				IsGrappling = !ProcessGrapplingHook(GrappleHookPoint, 0.05f);
			}
			else 
			{
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
				else 
				{
					CurrJumpNum = 0;
				}

				// Handle jump 
				if (CurrJumpNum < MaxJumpNum && Input.IsActionJustPressed("move_jump"))
				{
					Velocity = new Vector3(
									Velocity.X,
									JumpVelocity,
									Velocity.Z
								);
					CurrJumpNum += 1;
				}

				// Handle WASD movement
				Velocity = new Vector3(
								WishDir.X * PlayerSpeed,
								Velocity.Y,
								WishDir.Z * PlayerSpeed
							);

				// Stair Step Up
				HandleStairStepUp();

				// Stair Step Down
				HandleStairStepDown();
			}
			
			// Move 
			MoveAndSlide();

			// Smooth camera
			SmoothCameraJitter(delta);
		} 
		catch (Exception ex)
		{
			GD.PrintErr($"Error in _PhysicsProcess {ex.Message}");
		}
	}
	
	#endregion

	#region Gameplay

	public void ThrowBoomerang(Vector3 throwDirection)
	{
		var boomerang = BoomerangScene.Instantiate<Boomerang>();
		Hand.AddChild(boomerang);
		//boomerang.LookAt(throwDirection, Vector3.Up);
		boomerang.Throw(throwDirection);
	}

	// return true if Grappling is complete
	public bool ProcessGrapplingHook(Vector3 target, float weight)
	{
		// Access the player's current position (Transform.Origin)
		var currentPosition = Transform.Origin;

		// Check the distance to the target
		if (target.DistanceTo(currentPosition) > 1)
		{
			// Interpolate between current position and the target
			var newPosition = currentPosition.Lerp(target, weight);

			// Update the entire transform, setting the new origin
			var newTransform = Transform;
			newTransform.Origin = newPosition;
			Transform = newTransform;

			return false;
		}
		return true;
	}

	// return true if head is bonking
	public bool ProcessHeadBonk()
	{
		if (HeadBonkCast.IsColliding())
		{
			IsGrappling = false;
			GrappleHookPoint = Vector3.Zero;
			GlobalTranslate(new Vector3(0, -1, 0));
			return true;
		}
		return false;
	}


	#endregion

	#region Base Controller

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

	#endregion
}
