# Blind Snake Game

## Contents

1. [Problem Description](#problem-description)
2. [Baseline Solution](#baseline-solution)
3. [Improved Solution](#improved-solution)
4. [Executing program](#executing-program)
5. [References](#references)

---

## Problem Description

#### Initial Problem Formulation

Initially, our main question is (simplified version):
A and B are sides of the grid and are unknown.
Starting at a random point on the grid (toroidal-like), what is the appropriate method of snake movement
(right, left, down or up moves are available) for finding an apple located randomly on the same grid
using less than 35S (S = A * B) moves?

#### Reformulated Problem

Let us now consider a few additional observations:

- To calculate the complexity of the algorithm, we need to use the worst-case scenarios. Such a scenario for this
  problem is as follows: to find an apple, we need to traverse all the cells on the grid.
- We can represent any grid with a snake at random position as a grid where the snake is at coordinates (0, 0). See Fig.
  1.

<p align="center">
  <img src="https://github.com/user-attachments/assets/29e24df1-f64a-4eb1-9777-4d22c61b1975" width="500" title="Figure 1">
</p>
<p align="center">Figure 1: Shows that any grid with a snake can be represented as grid with a snake at position (0, 0)</p>

Now we can reformulate our problem:
A and B are sides of the grid and are unknown.
Starting at (0, 0)-coordinates on the grid (toroidal-like), what is the appropriate method of snake movement (right,
left, down or up moves are available) for visiting each cell of a grid using less than 35S (S = A * B) moves?

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
    - **Python** (Small Scripts/Testing):
      Use [main.py](https://github.com/TyKo0707/internship_application/blob/main/main.py) for experiments.
    - **C#** (Game Simulation): Use [C# engine](https://github.com/TyKo0707/internship_application/tree/main/Snake),
      which:
        - Simulates 35M moves and optionally saves them (`generate_moves` argument).
        - Uses moves to traverse the grid, displaying solution details with optional visualization (`verbose`).
        - Configurable
          via [main_config.json](https://github.com/TyKo0707/internship_application/blob/main/configs/main_config.json)
          or terminal arguments.
    - **Run Instructions**: Execute from the root with `dotnet run --project ./Snake/Snake/`, passing
      ordered (`10 20 0`) or named (`--width 10 --height 20 --verbose 0`) arguments.
    - **Notes**: Grids with `S > 10000` don’t display by default. Update `visualizationThreshold`
      in [Player.cs](https://github.com/TyKo0707/internship_application/blob/main/Snake/Snake/src/Player.cs) to change
      this.

2. **Test Dataset Generation**
    - Use [generate_tests.py](https://github.com/TyKo0707/internship_application/blob/main/generate_tests.py) to create
      test pairs `(A, B)` in intervals with edge cases:
        - `[1, interval_end]`, `[sqrt(interval_end), sqrt(interval_end)]`.
        - Special cases: `[k, k**2]` and divisor pairs of the interval’s end value.
    - Output: CSV files in `data/tests/` with columns: A, B, and S_interval.

3. **Baseline Solutions and Issues**
    - **Methods**:
        - **Spiral Traversal**: Complexity of O(S^2).
        - **Zig-Zag Traversal**: Loops when GCD(A, B) != 1.
    - **Loop Explanation**: Coprime grid sides loop back to (0,0) after LCM(A, B) * 2 steps, making zig-zag ideal for
      column-like grids with LCM equal to the longest size, but unusable when GCD(A, B) != 1.

Here is the comparison of the results of the two methods:
![zigzag_results](https://github.com/user-attachments/assets/0a492d97-50d4-40aa-8d11-7947c7cb9ad1)
<p align="center">Figure 2: Plot showing maximum and mean # of turns taken by the zig-zag method to cover S from different intervals and their comparison with 35S.</p>

![spiral_results](https://github.com/user-attachments/assets/1acfbe07-9e65-420a-8df4-051a1f607ad9)
<p align="center">Figure 3: Plot showing maximum and mean # of turns taken by the spiral method to cover S from different intervals and their comparison with 35S.</p>

It becomes clear that we need to choose one of the methods and improve it, and the obvious choice for this is zig-zag
traversal, as it offers a simple and efficient way to cover the grid systematically. The next task is to break the
method's looping during the traversal.

## Improved Solution

1. **How to avoid looping in ZigZag method, intuition**

- The original idea for breaking the loop in the zig-zag method was to take an extra step to the right (or down, it
  doesn't make any difference) after some constant value of moves.
- But, this approach has disadvantages - for large S, it takes the extra step too often, which doesn't allow filling the
  grid quickly because of the gaps that occur.
- So, I came to the conclusion that it makes sense to use a dynamic number of steps before the next extra step.
- Pseudocode for the improved method:

``` 
function SimulateKDynamicZigZagMoves(k=1):
    movesUntilNextExtraStep = 1
    prevMovesUntilNextExtraStep = movesUntilNextExtraStep
    moveCount = 0
    zigZagMoves = ["r", "d"]
    zigZagIndex = 0

    while moveCount < 35_000_000:
        // Execute the move and check if game is finished
        executeMove(zigZagMoves[zigZagIndex])
        sendSignal()

        moveCount++
        zigZagIndex = (zigZagIndex + 1) % 2

        if --movesUntilNextExtraStep == 0:
            // Execute additional right step and check if game is finished
            executeMove("r")
            sendSignal()
            
            // Set of all movesUntilNextExtraStep values is always increasing
            moveCount++
            movesUntilNextExtraStep = prevMovesUntilNextExtraStep + k
            prevMovesUntilNextExtraStep = movesUntilNextExtraStep
```

- Intuition behind the algorithm:
    - This algorithm dynamically adjusts the number of moves before taking an additional step. Instead of using a fixed
      number of steps between extra moves, the algorithm increases the interval (controlled by k).
    - This approach prevents the snake from looping and allows it to cover the grid more efficiently by adapting to the
      grid's size. It ensures better area coverage with fewer redundant moves.

2. **Proof of coverage**

- You can find the proof that such algorithm always covers the grid in finite number of steps
  at [proof.md](https://github.com/TyKo0707/internship_application/blob/main/proof.md).
- Insights:
    1. **Dynamic Step Adjustment**:  
       By adjusting the number of moves before taking an extra step, the algorithm avoids repetitive loops and fills the
       grid.
    2. **Finite Coverage Guarantee**:
       The algorithm guarantees that all grid cells will be visited in a finite number of steps, as the number of extra
       moves increases dynamically.
    3. **Limitations of the proof**:
       The proof does not show that the grid will be covered in 35S, instead, it shows that this always happens in a
       finite number of steps.
- We can show that the algorithm covers grids with S < 1M in under 35S in two ways:
    - building a mathematical model to find the "latest" point (the point that the algorithm visits last) of all grids
      with dimension < S
    - testing the algorithm on a sufficient number of cases.
- While the first option is useful for calculating complexity, I chose the second as it helps better understand the
  algorithm’s strengths and weaknesses with different inputs.

3. **Test results and analysis**

- Our goal now is to find which shape of the grid is the worst as k increases and to understand which value of k is the
  tipping point.
    - As k increases, the number of additional steps becomes smaller. The smaller the number of extra steps, the harder
      it is for the algorithm to fill the square grid (justify) (Fig. 4).

<p align="center">
<img src="https://github.com/user-attachments/assets/05c99b30-a54f-427c-8d07-ce7f22d1724d" width="750" title="frequency_by_k">
</p>
<p align="center">Figure 4: Frequency of adding extra step for different values of k.</p>

- Using this logic, we can assume that after some k the largest square grid will be the worst case. Let's create a plot
  and analyse it (Fig. 5).

<p align="center">
<img src="https://github.com/user-attachments/assets/d90a9f99-5add-48de-b52a-0c87deee6f7b" width="750" title="moves_by_k">
</p>
<p align="center">Figure 5: Number of maximum moves for different values of k, and it's comparison with the same plot but taking only A, B = (1000, 1000).</p>

- We see that our assumption was correct: starting from k=18, the worst case is always the grid of size (1000, 1000),
  i.e. the largest square grid. Our hypothesis is confirmed.

4. **Comparison of different parameters**
- Now it makes sense to take two values of k (before 18 and after 18) and compare their maximum and average values on
  different dimensions.
- Let's take k=11 and k=20 and compare their maximum and average values on different dimensions (Fig. 6).

<p align="center">
<img src="https://github.com/user-attachments/assets/e4b9015c-0b3c-4df1-8bad-3146199ee007" width="800" title="combined_k11_k20_results">
</p>
<p align="center">Figure 6: Plot showing maximum and mean # of turns taken by the zig-zag method with k=11 (left) and k=20 (right) to cover S.</p>

- As expected, the maximum values for k=20 at each interval are square grids and the number of steps for them is always
  approximately equal to 11S, while for k=11 it is limited to 11S but rarely reaches this value. At the same time we see
  that the average value for all intervals is about the same. My conclusion is that both values of k are well suited for
  our problem, but from now on, when referring to the algorithm, I will imply that the default value of k in it is 11.

## Executing program

#### Initial setup
- Download the repository from GitHub in a convenient way. 
- Set up the config file in [configs/main_config.json](https://github.com/TyKo0707/internship_application/blob/main/configs/main_config.json): choose what parameters you want to use by default and provide an absolute path to the `data/moves` folder (ex. `/Users/username/snake_project/data/moves/`). Same for all other configs.
- Download the requirements by running `pip install -r requirements.txt`.
- [Download dotnet](https://dotnet.microsoft.com/en-us/download) if needed.

#### Run experiments
- By default, you can run an experiment (consisting of: generating tests, evaluating the algorithm on these tests, building a plot) for a single algorithm in [main.py](https://github.com/TyKo0707/internship_application/blob/main/main.py)
- You can build any experiment you want using the components of this script. You can find one more experiment for a few `dynamic_zigzag` in the same file. 

#### User-friendly interface
- If you want to see the GUI of the algorithm running, you need to run the C# engine from the main directory using `dotnet run --project ./Snake/Snake/ --width 10 --height 10 --verbose 1` or with different parameters. 
- It is important to note that the grid will not be shown if S > 10_000, but you can change this in [Snake/Player.cs](https://github.com/TyKo0707/internship_application/blob/main/Snake/Snake/src/Player.cs) by changing the `visualisationThreshold` variable.

## Conclusion

## References

<a id="1">[1]</a>
Russell, S. and Norvig, P. (2010)
Artificial Intelligence: A Modern Approach. 3rd Edition,
Prentice-Hall, Upper Saddle River.