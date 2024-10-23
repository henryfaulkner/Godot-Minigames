using Godot;
using System;
using System.Collections.Generic;

public class CapyCubeBusiness
{
	public List<Vector3> Coords { get; private set; }

	public CapyCubeBusiness(double increment = 0.1)
	{
		// Calculate M, N, P based on increment (for a cube)
		int M = (int)(1.0 / increment) + 1;  // Number of rows
		int N = (int)(1.0 / increment) + 1;  // Number of columns
		int P = (int)(1.0 / increment) + 1;  // Depth

		// Generate all permutations for the given 3D cube
		Coords = GenerateCoordPermutations(M, N, P, increment);
	}

	// Method to generate all permutations of a 3D matrix (MxNxP) with values between 0.0 and 1.0
	private List<Vector3> GenerateCoordPermutations(int M, int N, int P, double increment)
	{
		var result = new List<Vector3>();

		// Create the list of possible values from 0.0 to 1.0 (inclusive)
		int numValues = (int)(1.0 / increment) + 1;
		double[] possibleValues = new double[numValues];

		for (int i = 0; i < numValues; i++)
		{
			possibleValues[i] = Math.Round(i * increment, 1); // Store the rounded values for precision
		}

		// Calculate the total number of elements in the 3D matrix
		int totalElements = M * N * P;

		// Iterate through each permutation (each coordinate)
		for (int i = 0; i < M; i++)
		{
			for (int j = 0; j < N; j++)
			{
				for (int k = 0; k < P; k++)
				{
					// Get the coordinate based on the indices (i, j, k)
					double x = possibleValues[i];
					double y = possibleValues[j];
					double z = possibleValues[k];

					result.Add(new Vector3((float)x, (float)y, (float)z));
				}
			}
		}

		return result;
	}

	// Method to adjust the coordinates based on a sine wave along the y-axis
	public void ApplySineWaveToYAxis(float frequency = 1.0f, float amplitude = 1.0f)
	{
		for (int i = 0; i < Coords.Count; i++)
		{
			var coord = Coords[i];
			// Apply sine wave to the y-coordinate. You can adjust which axis affects the sine wave (x or z)
			// Here, we use the x-coordinate to control the sine wave
			float sineWaveY = (float)Math.Sin(coord.X * frequency) * amplitude;

			// Create a new Vector3 with the modified y coordinate
			Coords[i] = new Vector3(coord.X, sineWaveY, coord.Z);
		}
	}
}
