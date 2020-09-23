using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Grid
{
    public struct GridEle
	{
		public Global.Ops op;
		public int value;

		public GridEle(Global.Ops o, int v)
		{
			op = o;
			value = v;
		}
	}

	GridEle[][] grid;

	int maxX;
	int maxY;

	public Grid(int x, int y)
	{
		maxX = x;
		maxY = y;
		grid = new GridEle[x][];
		for(int i = 0; i < x; i++)
		{
			grid[i] = new GridEle[y];
			for(int j = 0; j < y; j++)
			{
				grid[i][j] = new GridEle { op = Global.Ops.None, value = 0 };
			}
		}
	}

	public List<Vector2> RandomPath(Random rand)
	{
		Stack<Vector2> result = new Stack<Vector2>();
		result.Push(new Vector2(0, 0));
		bool finished = false;
		HashSet<Vector2> tried = new HashSet<Vector2>();
		tried.Add(new Vector2(0, 0));
		do
		{
			// reset and try again
			// getting a "good" path is common enough that this is fine
			if(result.Count == 0)
			{
				result = new Stack<Vector2>();
				result.Push(new Vector2(0, 0));
				tried = new HashSet<Vector2>();
				tried.Add(new Vector2(0, 0));
			}
			var adj = AdjacentTo(result.Peek());
			if (adj.Contains(new Vector2(4,4))) {
				GD.Print("Reached end");
				result.Push(new Vector2(4, 4));
				finished = true;
				break;
			}
			adj = adj.Except(tried).ToList<Vector2>();
			if(adj.Count == 0)
			{
				GD.Print("No adjs");
				result.Pop();
				continue;
			}
			var toTry = adj[rand.Next(adj.Count)];
			GD.Print("Trying " + toTry);
			tried.Add(toTry);
			result.Push(toTry);
		} while (!finished);

		return result.ToList<Vector2>();
	}

	public List<Vector2> AdjacentTo(Vector2 pos)
	{
		List<Vector2> result = new List<Vector2>();
		if(pos.x + 1 < maxX)
		{
			result.Add(new Vector2 { x = pos.x + 1, y = pos.y });
		}
		if (pos.y + 1 < maxY)
		{
			result.Add(new Vector2 { x = pos.x, y = pos.y + 1 });
		}
		if (pos.x - 1 >= 0)
		{
			result.Add(new Vector2 { x = pos.x - 1, y = pos.y });
		}
		if (pos.y - 1 >= 0)
		{
			result.Add(new Vector2 { x = pos.x, y = pos.y - 1 });
		}
		return result;
	}

	public GridEle At(int x, int y)
	{
		return grid[x][y];
	}

	public void Set(int x, int y, GridEle ele)
	{
		grid[x][y] = ele;
	}
}