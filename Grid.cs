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

	public struct XY
	{
		public readonly int x;
		public readonly int y;
		public XY(int _x, int _y)
		{
			x = _x;
			y = _y;
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



	public List<XY> RandomPath(Random rand)
	{
		Stack<XY> result = new Stack<XY>();
		result.Push(new XY(0, 0));
		bool finished = false;
		HashSet<XY> tried = new HashSet<XY>();
		tried.Add(new XY(0, 0));
		do
		{
			// reset and try again
			// getting a "good" path is common enough that this is fine
			if(result.Count == 0)
			{
				result = new Stack<XY>();
				result.Push(new XY(0, 0));
				tried = new HashSet<XY>();
				tried.Add(new XY(0, 0));
			}
			var adj = AdjacentTo(result.Peek());
			if (adj.Contains(new XY(4,4))) {
				GD.Print("Reached end");
				result.Push(new XY(4, 4));
				finished = true;
				break;
			}
			adj = adj.Except(tried).ToList<XY>();
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

		return result.ToList<XY>();
	}

	public void FillPath(int start, int end, List<XY> path, Random rand)
	{
		Set(0, 0, new GridEle(Global.Ops.Goal, start));
		Set(maxX, maxY, new GridEle(Global.Ops.Goal, end));

	}

	public List<XY> AdjacentTo(XY pos)
	{
		List<XY> result = new List<XY>();
		if(pos.x + 1 < maxX)
		{
			result.Add(new XY(pos.x + 1,pos.y));
		}
		if (pos.y + 1 < maxY)
		{
			result.Add(new XY(pos.x, pos.y + 1));
		}
		if (pos.x - 1 >= 0)
		{
			result.Add(new XY(pos.x - 1, pos.y));
		}
		if (pos.y - 1 >= 0)
		{
			result.Add(new XY(pos.x, pos.y - 1 ));
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
