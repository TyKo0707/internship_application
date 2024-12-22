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
1. **"Exterior" and "Interior" of Snake game**
- For small scripts and testing, I use Python, and for game simulation, I use C#. The C# engine works as follows: the selected algorithm simulates 35 million moves, saves them to a file (if `generate_moves` argument), then a task visits all grid fields using these moves, and finally displays the solution details (adds visualization if specified in `verbose`). All arguments are in the [config file](https://github.com/TyKo0707/internship_application/blob/main/cfg.json).
- Config parameters can be changed manually or via terminal arguments. Run C# code from the root directory using `dotnet run --project ./Snake/Snake/`, which reads arguments from `cfg.json`. Modify arguments with ordered (`10 20 0`) or named (`--width 10 --height 20 --verbose 0`) arguments.
- Note: The code doesn’t print grids for `S > 10000` by default. To change this, modify the `visualizationThreshold` in [Player.cs](https://github.com/TyKo0707/internship_application/blob/main/Snake/Snake/src/Player.cs).
2. **Test dataset**
- The Python script generates pairs (A, B) in specific intervals, including edge (`[1, interval_end]`, `[sqrt(interval_end), sqrt(interval_end)]`) and special cases (`[k, k**2]` - will be described later), and divisor pairs for the interval’s end value. 
- These examples are saved to a CSV file at `data/tests/` with columns: A, B, and S_interval.
3. **Baseline solutions** (and their problems)
- Text
- 

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