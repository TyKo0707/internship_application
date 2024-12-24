## Proof

### Question:
Assuming we have an algorithm that uses a zig-zag pattern to move and 
makes additional right moves at some time during its way. 
If we have a grid with sizes $A$ and $B$ and $GCD(A, B) \neq 1$, 
does the algorithm cover all cells of the grid in a countable number of steps?

### Step 1: Define Diagonals
Let's start the proof by introducing a convenient way of representing the grid. Let's start with a definition of the diagonal. With $i$ being the index of a top row where the diagonal starts, let $d_i$ be a diagonal that starts at point $(0, i)$ and returns to the same point in $2 \cdot LCM(A, B)$ (again, because $GCD(A, B) \neq 1$ and we are guaranteed to return to the starting point) moves (i.e $LCM(A, B)$ right and $LCM(A, B)$ down). We can represent our grid as a set of diagonals in a way shown in Fig. 1. 

<p align="center">
  <img src="https://github.com/user-attachments/assets/492d047d-5f77-4001-93fc-6bbfebcec45a" width="500" title="Figure 1">
</p>
<p align="center">Figure 1: We can represent a grid (a) as a set of diagonals in this way (b) (resulting grid is also toroidal-like)</p>

It is clear that to cover the whole grid, we must visit each cell of each diagonal, for simplicity lets call it visiting all diagonals.

### Step 2: Pattern of Movement
Knowing that the algorithm utilizes a zig-zag pattern (right + down pairs of moves) and shifts (adds an additional right step). We assume that we know (or can compute) the pattern of shifts (after which move snake should make additional right move). </br>
Then we can compute the differences of the $j_{th}$ and $j+1_{th}$ elements of this set. Let's name these sets $I$ and $L$ respectively. For example:
- The set of indices for additional steps is $I = \{0, 2, 5, 9, 14, ...\}$. I.e. algorithm makes additional step to the right after $0_{th}$, $2_{nd}$, $5_{th}$, ... move.
- The set of differences is then $L = \{i_{(j+1)} - i_{(j)} \mid i \in I, j \in \mathbb{N^+}, \, j < |I|\}$, which evaluates to $L = \{2, 3, 4, 5, ...\}$.

It is obvious that $I, L \in \mathbb{N^+}$.
We also need to note that in our algorithm $L$ is always increasing and we will use it later to answer the main question. 
An increasing set is defined as a set $S = \{s_1, s_2, s_3, \dots\} \subset \mathbb{R}$ where for all $i \in \mathbb{N}$, $s_{i+1} > s_i$.

### Step 3: Ensuring Coverage
Intuition: If $L$ is increasing, then after some number of steps, the number of moves that the snake performs before the next additional step will be greater than the length of the diagonal, which is equal to $2 \cdot LCM(A, B)$.