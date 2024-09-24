
// The lengths of the snake cube puzzle
int[] lengthValues = [3, 2, 3, 2, 2, 4, 2, 3, 2, 3, 2, 3, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 2, 2, 2, 2, 2, 3, 4, 2, 2, 2, 4, 2, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 2];

// The dimension of the cube
int cubeSize = 4;

// Initialize state array
bool[][][] state = new bool[cubeSize][][];
for (int x = 0; x < cubeSize; x++)
{
	state[x] = new bool[cubeSize][];
	for (int y = 0; y < cubeSize; y++)
	{
		state[x][y] = new bool[cubeSize];
		for (int z = 0; z < cubeSize; z++)
			state[x][y][z] = false;
	}
}

// A place for storing the solution
List<Tuple<int, int, int>> path = new();

state[0][0][0] = true;

// Run a search
bool result = FindSolution(	[0, 0, 0], 0, 1, 0,	path);

// Show a solution
foreach (Tuple<int, int, int> move in path)
{
	string s = "";
	if (move.Item1 == 0 && move.Item2 == 1)
		s = "To the right";
	else if (move.Item1 == 0 && move.Item2 == -1)
		s = "To the left";
	else if (move.Item1 == 1 && move.Item2 == 1)
		s = "Up";
	else if (move.Item1 == 1 && move.Item2 == -1)
		s = "Down";
	else if (move.Item1 == 2 && move.Item2 == 1)
		s = "Forward";
	else if (move.Item1 == 2 && move.Item2 == -1)
		s = "Backward";
	Console.WriteLine($"{s} {move.Item3}");
}

Console.ReadLine();

// Recursive function with backtracking
bool FindSolution(int[] start, int axis, int sign, int lengthIndex,	List<Tuple<int, int, int>> path)
{
	int length = (lengthValues[lengthIndex] - 1) * sign;

	int delta0 = axis == 0 ? length : 0;
	int delta1 = axis == 1 ? length : 0;
	int delta2 = axis == 2 ? length : 0;
	int[] current = [start[0] + delta0, start[1] + delta1, start[2] + delta2];

	// Check if the new point is out of the cube dimensions
	if (current[axis] < 0 || current[axis] >= cubeSize)
	{
		return false;
	}

	// Check if the new point crosses earlier added
	for (int i = 1; i <= Math.Abs(length); i++)
	{
		int d0 = axis == 0 ? i * sign : 0;
		int d1 = axis == 1 ? i * sign : 0;
		int d2 = axis == 2 ? i * sign : 0;
		if (state[start[0] + d0][start[1] + d1][start[2] + d2])
		{
			return false;
		}
	}

	// Check if this is the last move
	if (lengthIndex == lengthValues.Length - 1)
	{
		return true;
	}

	// Set state
	for (int i = 1; i <= Math.Abs(length); i++)
	{
		int d0 = axis == 0 ? i * sign : 0;
		int d1 = axis == 1 ? i * sign : 0;
		int d2 = axis == 2 ? i * sign : 0;
		state[start[0] + d0][start[1] + d1][start[2] + d2] = true;
	}

	// Save the path
	path.Add(new Tuple<int, int, int>(axis, sign, Math.Abs(length) + 1));

	// Try recursive next ways from here
	for (int i = 0; i <= 2; i++)
	{
		if (i != axis)
		{
			for (int j = -1; j <= 1; j += 2)
			{
				if (FindSolution(current, i, j, lengthIndex + 1, path))
				{
					return true;
				}
			}
		}
	}

	// If there is no way to the result, roll back the path
	path.RemoveAt(path.Count - 1);

	// Get state back
	for (int i = 1; i <= Math.Abs(length); i++)
	{
		int d0 = axis == 0 ? i * sign : 0;
		int d1 = axis == 1 ? i * sign : 0;
		int d2 = axis == 2 ? i * sign : 0;
		state[start[0] + d0][start[1] + d1][start[2] + d2] = false;
	}

	return false;
}
