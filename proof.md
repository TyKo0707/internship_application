## Proof

### Question:
Given an algorithm that uses a zig-zag pattern to move and makes additional right moves at specific steps, does the algorithm cover all cells of a grid with dimensions $A \times B$, where $\text{GCD}(A, B) \neq 1$, in a finite number of steps?

---

### Step 1: Define Diagonals
To simplify the analysis, we represent the grid as a set of diagonals. Let us define a **diagonal** as follows:
- Let's assume that A < B (B < A) and A (B) is the number of rows (columns) in a grid. 
- Then, a diagonal $d_i$ starts at point $(0, i)$ (at point $(i, 0)$) on the first row (column) of the grid and returns to the same point after $2 \cdot \text{LCM}(A, B)$ moves. This is because, with $\text{GCD}(A, B) \neq 1$, the grid's periodicity guarantees that the snake's path is toroidal-like, and it will return to the starting point after $\text{LCM}(A, B)$ right moves and $\text{LCM}(A, B)$ down moves, summing to $2 \cdot \text{LCM}(A, B)$ moves. 
- Moreover, number of diagonal for any grid is bottom-limited at $min(A, B)$ (even if less number of diagonals can cover the whole grid), for ease of understanding. 

We can represent our grid as a set of diagonals in a way shown in Fig. 1. 

<p align="center">
  <img src="https://github.com/user-attachments/assets/69547855-f3bd-4ea0-be4a-7ab816b622f" width="500" title="Figure 1">
</p>
<p align="center">Figure 1: We can represent a grid (a) as a set of diagonals in this way (b) (resulting grid is also toroidal-like).</p>

To cover the entire grid, the algorithm must visit every cell of each diagonal. For simplicity, we refer to this as **visiting all diagonals**.

---

### Step 2: Pattern of Movement
Tthe algorithm follows a zig-zag pattern (alternating right and down moves) and periodically performs additional right moves. We assume that we know (or can compute) the pattern of shifts (after which move snake should make additional right move). </br>
Then we can compute the differences of the $j_{th}$ and $j+1_{th}$ elements of this set. Let's name these sets $I$ and $L$ respectively. For example:
- The set of indices for additional steps is $I =$ { $0, 2, 5, 9, 14, ...$ }. I.e. algorithm makes additional step to the right after $0_{th}$, $2_{nd}$, $5_{th}$, ... move.
- The set of differences is then $L =$ { $i_{(j+1)} - i_{(j)} \mid i \in I, j \in \mathbb{N^+}, j < |I|$ }, which evaluates to $L =$ { $2, 3, 4, 5, ...$ }.

It is obvious that $I, L \in \mathbb{N^+}$.
We also need to note that in our algorithm $L$ is always increasing and we will use it later to answer the main question. 
An increasing set is defined as a set $S =$ { $s_1, s_2, s_3, \dots$ } $\subset \mathbb{R}$ where for all $i \in \mathbb{N}$, $s_{i+1} > s_i$.

---

### Step 3: Ensuring Coverage
To prove that the algorithm covers all cells of the grid, we analyze its movement relative to the diagonals.
#### **Intuition** 
If $L$ is increasing and large enough (but finite), then after some number of steps (let's call this number $n$), the number of moves that the snake performs before the next additional right step will be greater than the length of the diagonal, which is equal to $2 \cdot LCM(A, B)$. 

#### **Example**:
Consider a grid with dimensions $A = 4$, $B = 6$:  
- $\text{LCM}(4, 6) = 12$, so the length of a diagonal is $2 \cdot 12 = 24$.  
- Assume $I = \{0, 20, 41, 63, 86, 110, 135, \dots\}$.  
- Then $L = \{20, 21, 22, 23, 24, 25, \dots\}$.  

Here, the first element of $L$ that satisfies $L_j \geq 2 \cdot \text{LCM}(A, B) = 24$ is $L_5 = 24$. The corresponding element in $I$ is $I_5 = 86$, meaning that after 86 moves, the algorithm begins covering each diagonal fully.

#### **General Case**
From the moment $n$ is reached, the algorithm will:  
1. Cover one full diagonal per additional right step, as the movement between steps exceeds the diagonal length.  
2. Progress to the next diagonal by eventually moving right.

Since the number of diagonals is finite and to cover the whole grid we need to make all diagonals visited, we can say that the grid will be covered in finite number of steps. Moreover, the algorithm will visit all diagonals in $m$ additional right steps, and $m \leq$ # $\text{of diagonals}$.

---

### General Statement

*Claim*: Given an algorithm that moves in a zig-zag pattern with periodic additional right moves at specific steps from $I$ and the set of its differences $L$, where $L$ is increasing, it can be proven that if the grid dimensions $A \times B$ satisfy $\text{GCD}(A, B) \neq 1$, the algorithm will eventually cover all cells of the grid in a finite number of steps. </br>

*Intuition*: This is due to the periodic nature of the grid, where the movement follows a set of diagonals, and the algorithm's increasing step pattern ensures that after a finite number of moves, all diagonals will be fully visited, covering the entire grid.

---

### Notes
1. We can ommit a proof for the case where $A \times B$ satisfy $\text{GCD}(A, B) = 1$, because the single diagonal can cover the whole grid and proof is very similar unless we are waiting for difference that is greater than $2S = 2(A \cdot B)$.
2. There is no analysis in this file, only proof of coverage. To see the analysis, you can go to the main README.md. 
