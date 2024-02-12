# Magic Square solution with Forward Checking Algorithm

The Magic Square is a classic problem in recreational mathematics, defined as a square/matrix containing distinct numbers arranged so that the final sum of each row, column, and diagonal is the same. Each user can obtain a magic square by entering its dimension and the desired sum.

## Description of Algorithm written in C#

Variables and variable domains are initialized according to the given problem. Iteration over the domain of the selected variable is performed, and for each value in the domain, it is checked whether it is consistent with the values already assigned to other variables and if it meets the proposed target.

Backtracking is used to recursively move to each variable, and the process continues until a solution is found. Forward Checking helps avoid unnecessary exploration of the search space by focusing on the domains of variables that are influenced by previous assignments. This leads to a significant reduction in the search space and speeds up finding the solution or identifying that no solutions exist.
