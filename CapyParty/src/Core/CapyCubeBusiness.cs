using Godot;
using System;
using System.Collections.Generic;

public class CapyCubeBusiness
{
	public List<Vector3> Coords { get; private set; }

	public CapyCubeBusiness()
	{
		// Set the increment for values between 0.0 and 1.0
		double increment = 0.1;

		// Calculate M, N, P based on increment (for a cube)
		int M = (int)(1.0 / increment) + 1;  // Number of rows
		int N = (int)(1.0 / increment) + 1;  // Number of columns
		int P = (int)(1.0 / increment) + 1;  // Depth

		// Generate all permutations for the given 3D cube
		Coords = GenerateCoordPermutations(M, N, P, increment);
	}

	public CapyCubeBusiness(double increment)
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

					// Print the coordinate (x, y, z)
					Console.WriteLine($"({x}, {y}, {z})");
					result.Add(new Vector3((float)x, (float)y, (float)z));
				}
			}
		}

		return result;
	}
}
