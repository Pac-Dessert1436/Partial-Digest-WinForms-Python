#!/usr/bin/env python3
"""
Partial Digest Algorithm (PDP) Implementation in Python

This module implements the Partial Digest Algorithm for reconstructing a set of points
from their pairwise distances, a common problem in bioinformatics for determining
restriction site positions along DNA molecules.
"""

from typing import Optional
import sys


def calculate_distances(pt: int, points: list[int]) -> list[int]:
    """
    Calculate distances from a given point to all points in a list.
    
    Args:
        pt: The reference point.
        points: The list of points to calculate distances to.
        
    Returns:
        A list of absolute distances from the reference point to each point in the input list.
    """
    return [abs(p - pt) for p in points]


def can_remove_distances(dist_to_remove: list[int], dist_counts: dict[int, int]) -> bool:
    """
    Check if a collection of distances can be removed from the distance multiset.
    
    Args:
        dist_to_remove: The list of distances to remove.
        dist_counts: The dictionary containing counts of each distance in the multiset.
        
    Returns:
        True if all distances can be removed (i.e., they exist in sufficient quantities), False otherwise.
    """
    temp_counts = dist_counts.copy()
    for dist in dist_to_remove:
        if dist not in temp_counts or temp_counts[dist] <= 0:
            return False
        temp_counts[dist] -= 1
    return True


def create_distance_counts(distances: list[int]) -> dict[int, int]:
    """
    Create a dictionary that counts occurrences of each distance in a list.
    
    Args:
        distances: The list of distances to count.
        
    Returns:
        A dictionary where keys are distance values and values are their counts in the input list.
    """
    counts = {}
    for dist in distances:
        if dist in counts:
            counts[dist] += 1
        else:
            counts[dist] = 1
    return counts


def find_solution(width: int, distances: list[int], points: list[int]) -> Optional[list[int]]:
    """
    Implement the improved Partial Digest algorithm to find a set of points that produce the given distances.
    
    Args:
        width: The maximum distance in the input (represents the total length).
        distances: The list of pairwise distances (partial digest).
        points: The initial set of points (typically containing 0 and width).
        
    Returns:
        An ordered list of points that produce the input distances, or None if no solution exists.
    """
    from collections import deque
    
    stack = deque()
    
    # Create initial distance counts
    initial_counts = create_distance_counts(distances)
    stack.append((initial_counts, points.copy()))
    
    while stack:
        curr_counts, curr_points = stack.pop()
        
        # Check if we've found a solution (all distances accounted for)
        if sum(curr_counts.values()) == 0:
            return sorted(curr_points)
        
        # Find the maximum remaining distance
        max_dist = max(curr_counts.keys())
        candidate1 = max_dist
        candidate2 = width - max_dist
        
        # Process first candidate point
        distances1 = calculate_distances(candidate1, curr_points)
        if can_remove_distances(distances1, curr_counts):
            # Create new counts by removing distances
            new_counts1 = curr_counts.copy()
            for dist in distances1:
                new_counts1[dist] -= 1
            # Filter out counts that are zero
            new_counts1 = {k: v for k, v in new_counts1.items() if v > 0}
            
            # Create new points list with the candidate
            new_points1 = curr_points.copy()
            if candidate1 not in new_points1:
                new_points1.append(candidate1)
                stack.append((new_counts1, new_points1))
        
        # Process second candidate point
        distances2 = calculate_distances(candidate2, curr_points)
        if can_remove_distances(distances2, curr_counts):
            # Create new counts by removing distances
            new_counts2 = curr_counts.copy()
            for dist in distances2:
                new_counts2[dist] -= 1
            # Filter out counts that are zero
            new_counts2 = {k: v for k, v in new_counts2.items() if v > 0}
            
            # Create new points list with the candidate
            new_points2 = curr_points.copy()
            if candidate2 not in new_points2:
                new_points2.append(candidate2)
                stack.append((new_counts2, new_points2))
    
    return None


def partial_digest(fragment_lengths: list[int]) -> Optional[list[int]]:
    """
    Run the Partial Digest algorithm on a list of fragment lengths.
    
    Args:
        fragment_lengths: A list containing all pairwise distances between points.
        
    Returns:
        An ordered list of points that produce the input fragment lengths, or None if no solution exists.
    """
    if not fragment_lengths:
        return None
        
    width = max(fragment_lengths)
    fragment_lengths = [d for d in fragment_lengths if d != width]
    starter = [0, width]
    
    return find_solution(width, fragment_lengths, starter)


def extract_array_from_input(input_str: str) -> Optional[list[int]]:
    """
    Extract a list of integers from a string formatted as [n1, n2, n3, ...].
    
    Args:
        input_str: The input string in the format [n1, n2, n3, ...].
        
    Returns:
        A list of integers parsed from the input string, or None if parsing fails.
    """
    try:
        # Remove brackets and split by commas
        content = input_str.strip()[1:-1]
        if not content:
            return []
        return [int(x.strip()) for x in content.split(',')]
    except (ValueError, IndexError) as e:
        print(f"Error parsing input: {e}", file=sys.stderr)
        return None
    

def all_pairwise_distances(points: list[int]) -> list[int]:
    """
    Calculate all pairwise distances between points for verification.
    
    Args:
        points: The list of points.
        
    Returns:
        A sorted list of all pairwise distances between points.
    """
    distances = []
    sorted_points = sorted(points)
    for i in range(len(sorted_points)):
        for j in range(i + 1, len(sorted_points)):
            distances.append(abs(sorted_points[j] - sorted_points[i]))
    return sorted(distances)


def main():
    """
    Command-line interface for the Partial Digest Algorithm.
    """
    import argparse
    
    parser = argparse.ArgumentParser(description="Partial Digest Algorithm for DNA restriction site mapping")
    parser.add_argument('--input', type=str, help='Input distances as a string in format [d1, d2, d3, ...]')
    parser.add_argument('--distances', type=int, nargs='+', help='Input distances as space-separated integers')
    
    args = parser.parse_args()
    
    if args.input:
        fragment_lengths = extract_array_from_input(args.input)
    elif args.distances:
        fragment_lengths = args.distances
    else:
        # Interactive input
        input_str = input("Enter distances in format [d1, d2, d3, ...]: ")
        fragment_lengths = extract_array_from_input(input_str)
    
    if fragment_lengths is None:
        print("Invalid input format. Please use format [d1, d2, d3, ...] or provide space-separated integers.")
        sys.exit(1)
    
    result = partial_digest(fragment_lengths)
    
    if result:
        print(f"Reconstructed points: {result}")
        print(f"Distances check: {all_pairwise_distances(result)}")
    else:
        print("No solution found.")


if __name__ == "__main__":
    main()