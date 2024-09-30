import time
# Recursive function with backtracking
def find_solution(start, axis, sign, lengthIndex, path) -> bool:
    length = (lengthValues[lengthIndex] - 1) * sign

    x = length if axis == 0 else 0
    y = length if axis == 1 else 0
    z = length if axis == 2 else 0
    current = (start[0] + x, start[1] + y, start[2] + z)

	# Check if the new point is out of the cube dimensions
    if current[axis] < 0 or current[axis] >= cubeSize:
        return False

	# Check if the new point crosses earlier added
    for i in range(1, abs(length) + 1):
        d0 = i * sign if axis == 0 else 0
        d1 = i * sign if axis == 1 else 0
        d2 = i * sign if axis == 2 else 0
        if state[start[0] + d0][start[1] + d1][start[2] + d2]:
            return False

	# Check if this is the last move
    if lengthIndex == len(lengthValues) - 1:
        return True

	# Set state
    for i in range(1, abs(length) + 1):
        d0 = i * sign if axis == 0 else 0
        d1 = i * sign if axis == 1 else 0
        d2 = i * sign if axis == 2 else 0
        state[start[0] + d0][start[1] + d1][start[2] + d2] = True

	# Save the path
    path.append((axis, sign, abs(length) + 1))

	# Try recursive next ways from here
    for i in (i for i in range(3) if i != axis):
        for j in range(-1, 2, 2):
            if find_solution(current, i, j, lengthIndex + 1, path):
                return True

	# If there is no way to the result, roll back the path
    del path[-1]

	# Get state back
    for i in range(1, abs(length) + 1):
        d0 = i * sign if axis == 0 else 0
        d1 = i * sign if axis == 1 else 0
        d2 = i * sign if axis == 2 else 0
        state[start[0] + d0][start[1] + d1][start[2] + d2] = False

    return False

# The lengths of the snake cube puzzle
lengthValues = [3, 2, 3, 2, 2, 4, 2, 3, 2, 3, 2, 3, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 2, 2, 2, 2, 2, 3, 4, 2, 2, 2, 4, 2, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 2]

# The dimension of the cube
cubeSize = 4

# Initialize state array
state = [[[False for x in range(cubeSize)] for y in range(cubeSize)] for z in range(cubeSize)]

# A place for storing the solution
path = []

state[0][0][0] = True

start = time.time()

# Run a search
result = find_solution([0, 0, 0], 0, 1, 0, path)

end = time.time()
print(end - start)

# Show a solution
for move in path:
    s = ""
    if move[0] == 0 and move[1] == 1:
        s = "To the right"
    elif move[0] == 0 and move[1] == -1:
        s = "To the left"
    elif move[0] == 1 and move[1] == 1:
        s = "Up"
    elif move[0] == 1 and move[1] == -1:
        s = "Down"
    elif move[0] == 2 and move[1] == 1:
        s = "Forward"
    elif move[0] == 2 and move[1] == -1:
        s = "Backward"
    print(f"{s} {move[2]}")