using Godot;
using System;

public partial class BouncePath : Path3D
{
	[Signal]
	public delegate void CycleFinishedEventHandler();

	private ILoggerService _logger { get; set; }

	private PathFollow3D _pathFollow;
	private RemoteTransform3D _remoteTransform;

	private float Speed { get; set; } = 0.1f;
	private bool IsAnimating { get; set; } = false;

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!IsAnimating) return;
		var isCycleFinished = ProcessPathFollow(delta);
		if (isCycleFinished) EmitSignal(SignalName.CycleFinished);
	}

	public void ReadyInstance(CharacterBody3D character, MeshInstance3D mesh)
	{
		_pathFollow = GetNode<PathFollow3D>("./PathFollow3D");
		_remoteTransform = GetNode<RemoteTransform3D>("./PathFollow3D/RemoteTransform3D");
		
		PopulatePoints(character, mesh);
		AttachRemoteToCharacter(character);
	}

	public void StartAnimation()
	{
		IsAnimating = true;
	}

	private void PopulatePoints(CharacterBody3D character, MeshInstance3D mesh)
	{
		var pos = character.Position;
		var size = GetSize(mesh);

		Curve.AddPoint(pos);
		Curve.AddPoint(new Vector3(
			pos.X,
			pos.Y + size.Y,
			pos.Z
		));
		Curve.AddPoint(pos);
		Curve.AddPoint(new Vector3(
			pos.X,
			pos.Y * (size.Y * 2),
			pos.Z
		));
		Curve.AddPoint(pos);
	}

	private void AttachRemoteToCharacter(CharacterBody3D character)
	{
		if (_remoteTransform == null)
		{
			_logger.LogError("RemoteTransform3D is not assigned.");
			return;
		}

		NodePath relativePath = _remoteTransform.GetPathTo(character);
		_remoteTransform.RemotePath = relativePath;
	}

	private bool ProcessPathFollow(double delta)
	{
		var result = false;
		if (_pathFollow == null)
		{
			_logger.LogError("PathFollow3D is not assigned.");
			return result;
		}

		if (_pathFollow.ProgressRatio + Speed * (float)delta < 0)
		{
			result = true;
		}

		return result;
	}

	private Vector3 GetSize(MeshInstance3D mesh)
	{
		return mesh.GetAabb().Size;
	}

}
