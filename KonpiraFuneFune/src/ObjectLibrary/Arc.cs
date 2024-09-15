using Godot;
using System;

public partial class Arc : Path3D
{
	private const int NUM_POINTS = 100;

	public override void _Ready()
	{
		// assume 3 points for quadratic bezier; else, explode
		if (Curve.PointCount < 3)
		{
			GD.PrintErr("Bezier curve requires exactly 3 points.");
			return;
		}

		Vector3 p0 = Curve.GetPointPosition(0);
		Vector3 p1 = Curve.GetPointPosition(1);
		Vector3 p2 = Curve.GetPointPosition(2);
		
		GD.Print($"p0: x = {p0.X}, y = {p0.Y}, z = {p0.Z}");
		GD.Print($"p1: x = {p1.X}, y = {p1.Y}, z = {p1.Z}");
		GD.Print($"p2: x = {p2.X}, y = {p2.Y}, z = {p2.Z}");

		Curve.ClearPoints();

		for (int i = 0; i < NUM_POINTS; i++)
		{
			// Normalize t to be in the range [0, 1]
			float t = (float)i / (NUM_POINTS - 1);
			var point = QuadraticBezier(p0, p1, p2, t);
			//GD.Print($"Curve Point {point}");
			Curve.AddPoint(point);
		}
	}

	private Vector3 QuadraticBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
	{
		Vector3 q0 = p0.Lerp(p1, t);
		Vector3 q1 = p1.Lerp(p2, t);
		return q0.Lerp(q1, t);
	}
}
