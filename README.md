# 🚀 AVL Tree Autocomplete: Efficiency Comparison in C#

**Repository Description:** Optimized Autocomplete implementation using AVL Trees in C#. This project compares three search algorithms (Recursive DFS, Iterative BFS, and Iterative DFS (Stack)) to analyze search efficiency and practical performance.

---

## Overview

This is a **simple application** demonstrating the implementation and performance testing of a **Self-Balancing AVL Tree** designed to store string data. The primary function is to provide an efficient prefix-matching (Autocomplete) feature.

The core AVL structure includes full $O(\log N)$ implementations for `Insert`, `Delete`, and the necessary rotation logic.

## 🧠 Autocomplete Search Implementations

To find all words matching a given prefix, three distinct search algorithms are compared. All methods utilize **AVL/BST Pruning Logic** to avoid traversing unnecessary subtrees, ensuring optimal $O(\log N + K)$ time complexity (where K is the number of results). 

| Method | Traversal Style | Execution Model | Performance Note |
| :--- | :--- | :--- | :--- |
| **`AutoComplete` (Method 0)** | **Recursive DFS** | Eager (Full List) | Simple and clean; relies on the Call Stack. |
| **`AutoComplete1`** | **Iterative BFS (Queue)** | Deferred (`yield return`) | Good for quick responses on a level-by-level basis. |
| **`AutoComplete2`** | **Iterative DFS (Stack)** | Deferred (`yield return`) | **Optimal Practical Performance.** Improves Cache Locality (better memory access). |

## 🛠️ Key Optimization: Cache Locality

The difference in practical speed between the BFS and DFS iterative approaches lies in how the CPU accesses memory. DFS (`AutoComplete2`) is often faster because its vertical traversal pattern leads to better **Cache Locality** (memory utilization) and fewer **Cache Misses** on modern CPUs, despite the identical theoretical time complexity. 

## 📂 Project Structure

The solution contains the `AVLTree` class, which includes:

* Full AVL Logic (`Insert`, `Delete`, `Balance`, `Rotate`).
* The three optimized `AutoComplete` functions.

### Installation and Usage

Clone the repository and run the C# console application to test the insertion and search logic.