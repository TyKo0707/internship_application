## Proof

### Question:
Assuming we have an algorithm that uses a zig-zag pattern to move and 
makes additional right moves at some time during its way. 
If we have a grid with sizes A and B and $GCD(A, B) != 1$, 
does the algorithm cover all cells of the grid in a countable number of steps?

### Step 1: Define Diagonals
Let's start the proof by introducing a convenient way of representing the grid. Let's start with a definition of the diagonal. With i being the index of a top row where the diagonal starts, let d_i be a diagonal that starts at point (0, i) and returns to the same point in 2*LCM(A, B ) moves (i.e LCM(A, B ) right and LCM(A, B ) down). We can represent our grid as a set of diagonals in a way shown in Fig. 1. 

<p align="center">
  <img src="https://github.com/user-attachments/assets/492d047d-5f77-4001-93fc-6bbfebcec45a" width="500" title="Figure 1">
</p>
<p align="center">Figure 1: We can represent a grid (a) as a set of diagonals in this way (b) (resulting grid is also toroidal-like)</p>

It is clear that to cover the whole grid, we must visit each cell of each diagonal, for simplicity lets call it visiting all diagonals.

### Step 2: Algorithm Description

### Step 3: Ensuring Coverage
