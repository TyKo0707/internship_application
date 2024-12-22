# Blind Snake Game

## Problem Description
#### Initial Problem Formulation
Initially, our main question is (simplified version): 
A and B are sides of the grid and are unknown. 
Starting at a random point on the grid (toroidal-like), what is the appropriate method of snake movement 
(right, left, down or up moves are available) for finding an apple located randomly on the same grid 
using less than 35S (S = A * B) moves?

#### Reformulated Problem
Let us now consider a few additional observations:
- To calculate the complexity of the algorithm, we need to use the worst-case scenarios. Such a scenario for this problem is as follows: to find an apple, we need to traverse all the cells on the grid.
- We can represent any grid with a snake at random position as a grid where the snake is at coordinates (0, 0) - a graphical example. 

Now we can reformulate our problem: 
A and B are sides of the grid and are unknown. 
Starting at (0, 0)-coordinates on the grid (toroidal-like), what is the appropriate method of snake movement (right, left, down or up moves are available) for visiting each cell of a grid using less than 35S (S = A * B) moves?

#### Environment Type
Having a question in mind, we can now theoretically define the environment type for the problem.
</br>
The environment of the problem is [[1]](#1): 
- **Partially observable**: no access to env's states: snake coord, A, B, ...
- **Deterministic**: the game's rules (movement and wrapping around borders) are fixed and predictable
- **Sequential**: the game depends on the sequence of actions
- **Semidynamic**: the environment does not change, but the performance score does
- **Discrete**: the snake moves in discrete steps (up, down, left, right)
- **Single-agent**: the game involves only the player’s snake

Now we can move to implementing this environment and the solution.

## Baseline Solution
1. **Snake Game Implementation**
   - **Python** (Small Scripts/Testing): Use [main.py](https://github.com/TyKo0707/internship_application/blob/main/main.py) for experiments.
   - **C#** (Game Simulation): Use [C# engine](https://github.com/TyKo0707/internship_application/tree/main/Snake), which:
     - Simulates 35M moves and optionally saves them (`generate_moves` argument).
     - Uses moves to traverse the grid, displaying solution details with optional visualization (`verbose`).
     - Configurable via [main_config.json](https://github.com/TyKo0707/internship_application/blob/main/configs/main_config.json) or terminal arguments.
   - **Run Instructions**: Execute from the root with `dotnet run --project ./Snake/Snake/`, passing ordered (`10 20 0`) or named (`--width 10 --height 20 --verbose 0`) arguments. 
   - **Notes**: Grids with `S > 10000` don’t display by default. Update `visualizationThreshold` in [Player.cs](https://github.com/TyKo0707/internship_application/blob/main/Snake/Snake/src/Player.cs) to change this.

2. **Test Dataset Generation**
   - Use [generate_tests.py](https://github.com/TyKo0707/internship_application/blob/main/generate_tests.py) to create test pairs `(A, B)` in intervals with edge cases:
     - `[1, interval_end]`, `[sqrt(interval_end), sqrt(interval_end)]`.
     - Special cases: `[k, k**2]` and divisor pairs of the interval’s end value.
   - Output: CSV files in `data/tests/` with columns: A, B, and S_interval.

3. **Baseline Solutions and Issues**
   - **Methods**:
     - **Spiral Traversal**: Complexity of O(S^2).
     - **Zig-Zag Traversal**: Loops when GCD(A, B) != 1.
   - **Loop Explanation**: Coprime grid sides loop back to (0,0) after LCM(A, B) * 2 steps, making zig-zag ideal for column-like grids with LCM equal to the longest size, but unusable when GCD(A, B) != 1.

Here is the comparison of the results of the two methods:
![spiral_zigzag_results_combined](https://github.com/user-attachments/assets/fe56eb9c-7eef-4b6b-bf29-2956fbb92c15)

It becomes clear that we need to choose one of the methods and improve it, and the obvious choice for this is zig-zag traversal, as it offers a simple and efficient way to cover the grid systematically. The next task is to break the method's looping during the traversal.

## Improved Solution
1. **How to avoid looping in ZigZag method, intuition**
- Text
- 
2. **Test results and edge cases**
- Text
- 
3. **Comparison of different parameters**
- Text
- 
4. **Theoretical analysis of the solution**
- Text
- 

## Executing program
#### Initial setup
#### Run experiments
#### User-friendly interface

## References
<a id="1">[1]</a> 
Russell, S. and Norvig, P. (2010) 
Artificial Intelligence: A Modern Approach. 3rd Edition, 
Prentice-Hall, Upper Saddle River.