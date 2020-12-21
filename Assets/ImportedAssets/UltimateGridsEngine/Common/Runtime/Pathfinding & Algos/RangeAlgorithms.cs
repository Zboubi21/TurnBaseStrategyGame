using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;

public static class RangeAlgorithms
{
    public static List<GridTile> SearchByParameters(GridTile start, RangeParameters rangeParameters, bool canFly = false)
    {
        switch (rangeParameters.RangeSearchType)
        {
            case RangeSearchType.RectangleByGridPosition:
            default:
                return RangeAlgorithms.SearchByGridPosition(start, rangeParameters.MaxReach, rangeParameters.WalkableTiles, rangeParameters.FlyableTiles, rangeParameters.UnOccupiedTilesOnly, rangeParameters.SquareRange, rangeParameters.IgnoreTilesHeight, rangeParameters.IncludeStartingTile, rangeParameters.MinReach, rangeParameters.InStraightLine);
            case RangeSearchType.RectangleByMovement:
                return RangeAlgorithms.SearchByMovement(start, rangeParameters.MaxReach, rangeParameters.IgnoreTilesHeight, rangeParameters.IncludeStartingTile, rangeParameters.MinReach, canFly);
        }
    }

    public static List<GridTile> SearchByGridPosition(GridTile start, int maxReach, bool walkableTiles = true, bool flyableTiles = false, bool unoccupiedTilesOnly = true, bool square = true, bool ignoreHeight = false, bool includeStartingTile = false, int minReach = 1, bool inStraightLine = false)
    {
        List<GridTile> range = new List<GridTile>();

        // Start is goal
        if (maxReach <= 0)
        {
            range.Add(start);
            return range;
        }
        if (maxReach < minReach)
        {
            return range;
        }

        Dictionary<GridTile, float> cost_so_far = new Dictionary<GridTile, float>();
        SimplePriorityQueue<GridTile> frontier = new SimplePriorityQueue<GridTile>();
        frontier.Enqueue(start, 0);
        cost_so_far.Add(start, 0);

        GridTile current = start;
        while (frontier.Count > 0)
        {
            current = frontier.Dequeue();
            if (cost_so_far[current] <= maxReach)
            {
                var neighbors = GridManager.Instance.Neighbors(current, GridManager.defaultRectangle8Directions);
                foreach (GridTile next in neighbors)
                {
                    float new_cost = cost_so_far[current] + (square == true ? 1 : Utilities.Heuristic(current, next));
                    if (!cost_so_far.ContainsKey(next))
                    {
                        cost_so_far[next] = new_cost;
                        float priority = new_cost;
                        frontier.Enqueue(next, priority);

                        if (!range.Contains(next) && new_cost >= minReach && new_cost <= maxReach)
                            range.Add(next);
                    }
                }
            }
        }

        // Check if spell can be launched on tiles
        if (range.Count > 0)
        {
            for (int i = 0; i < range.Count; ++i)
            {
                GridTile tile = range[i];
                bool removeTile = false;

                // Check walkables tiles
                if ((tile.m_IsTileWalkable && !walkableTiles) || (tile.m_IsTileFlyable && !flyableTiles))
                {
                    removeTile = true;
                }
                else if (unoccupiedTilesOnly && tile.IsTileOccupied())
                {
                    removeTile = true;
                }
                else if (!tile.m_IsTileWalkable && !tile.m_IsTileFlyable)
                {
                    removeTile = true;
                }

                // Spell is in straight line?
                if (inStraightLine && !IsInStraightLine(start, tile))
                {
                    removeTile = true;
                }

                if (removeTile)
                {
                    range.RemoveAt(i);
                    i--;
                }
            }
        }

        // remove the starting tile if required
        if (!includeStartingTile)
        {
            if (range.Contains(start))
            {
                range.Remove(start);
            }
        }

        return range;
    }

    private static bool IsInStraightLine(GridTile startTile, GridTile targetTile)
    {
        bool isInStraightLine = false;
        Vector2 startPos = startTile.m_GridPosition;
        Vector2 targetPos = targetTile.m_GridPosition;

        if (((startPos.x == targetPos.x) && (startPos.y != targetPos.y)) || ((startPos.x != targetPos.x) && (startPos.y == targetPos.y)))
            isInStraightLine = true;

        return isInStraightLine;
    }

    public static List<GridTile> SearchByMovement(GridTile start, int maxReach, bool ignoreHeight = false, bool includeStartingTile = false, int MinReach = 1, bool canFly = false)
    {
        List<GridTile> range = new List<GridTile>();

        // Start is goal
        if (maxReach == 0)
        {
            range.Add(start);
            return range;
        }
        if (maxReach < MinReach)
        {
            return range;
        }

        Dictionary<GridTile, float> cost_so_far = new Dictionary<GridTile, float>();
        SimplePriorityQueue<GridTile> frontier = new SimplePriorityQueue<GridTile>();
        frontier.Enqueue(start, 0);
        cost_so_far.Add(start, 0);

        GridTile current = start;
        while (frontier.Count > 0)
        {
            current = frontier.Dequeue();
            if (cost_so_far[current] <= maxReach)
            {
                foreach (GridTile next in GridManager.Instance.WalkableNeighbors(current, ignoreHeight, true, null, canFly))
                {
                    float new_cost = cost_so_far[current] + next.Cost();
                    if (!cost_so_far.ContainsKey(next))
                    {
                        cost_so_far[next] = new_cost;
                        float priority = new_cost;
                        frontier.Enqueue(next, priority);

                        if (!range.Contains(next) && new_cost >= MinReach && new_cost <= maxReach)
                        {
                            range.Add(next);
                        }
                    }
                }
            }
        }

        // remove the starting tile if required
        if (!includeStartingTile)
        {
            if (range.Contains(start))
            {
                range.Remove(start);
            }
        }

        return range;
    }
}