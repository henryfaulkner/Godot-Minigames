using Godot;
using System;

public partial class Arc : Path3D
{
	private const int NUM_POINTS = 100;

	public override void _Ready()
	{
		// assume 3 points for quadratic bezier; else, explode
		var manualPoints = Curve.GetBakedPoints();
		Curve.ClearPoints();

		var forRatio = NUM_POINTS / 100;
		for (int i = 0; i < 1.0; i += forRatio)
		{
			Curve.AddPoint(QuadraticBezier(manualPoints[0], manualPoints[1], manualPoints[2], i));
		}
	}

	private Vector3 QuadraticBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
	{
		Vector3 q0 = p0.Lerp(p1, t);
		Vector3 q1 = p1.Lerp(p2, t);
		Vector3 r = q0.Lerp(q1, t);
		return r;
	}
}
