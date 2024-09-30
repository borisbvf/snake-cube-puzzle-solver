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
DateTime start = DateTime.Now;

// Run a search
bool result = FindSolution([0, 0, 0], 0, 1, 0);

TimeSpan diff = DateTime.Now - start;
Console.WriteLine($"Finding a solution took {diff.TotalSeconds} seconds.");

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
bool FindSolution(int[] start, int axis, int sign, int lengthIndex)
{
	int length = (lengthValues[lengthIndex] - 1);

	int[] current = GetResultPoint(start, axis, sign, length);

	// Check if the new point is out of the cube dimensions
	if (current[axis] < 0 || current[axis] >= cubeSize)
		return false;

	// Check if the new point crosses earlier added
	if (!CheckState(start, axis, sign, length))
		return false;

	// Save the path
	path.Add(new Tuple<int, int, int>(axis, sign, length + 1));

	// Check if this is the last move
	if (lengthIndex == lengthValues.Length - 1)
		return true;

	// Set state
	SetState(start, axis, sign, length, true);

	// Try recursive next ways from here
	for (int nextAxis = 0; nextAxis <= 2; nextAxis++)
		if (nextAxis != axis)
			for (int nextSign = -1; nextSign <= 1; nextSign += 2)
				if (FindSolution(current, nextAxis, nextSign, lengthIndex + 1))
					return true;

	// If there is no way to the result, roll back the path
	path.RemoveAt(path.Count - 1);

	// Get state back
	SetState(start, axis, sign, length, false);

	return false;
}

int[] GetResultPoint(int[] start, int axis, int sign, int length)
{
	int x = axis == 0 ? length * sign : 0;
	int y = axis == 1 ? length * sign : 0;
	int z = axis == 2 ? length * sign : 0;
	return [start[0] + x, start[1] + y, start[2] + z];
}

void SetState(int[] start, int axis, int sign, int length, bool value)
{
	for (int i = 1; i <= length; i++)
	{
		int[] point = GetResultPoint(start, axis, sign, i);
		state[point[0]][point[1]][point[2]] = value;
	}
}

bool CheckState(int[] start, int axis, int sign, int length)
{
	for (int i = 1; i <= length; i++)
	{
		int[] point = GetResultPoint(start, axis, sign, i);
		if (state[point[0]][point[1]][point[2]])
			return false;
	}
	return true;
}
