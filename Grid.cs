using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using static Global;

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

		public int Apply(int start)
		{
			switch (op)
			{
				case Ops.Divide:
					return start / value;
				case Ops.Minus:
					return start - value;
				case Ops.Plus:
					return start + value;
				case Ops.Times:
					return start * value;
				default:
					return value;
			}
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

		public override string ToString()
		{
			return "x: " + x + " y: " + y;
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
		var list = result.ToList<XY>();
		list.Reverse();
		return list;
	}

	public void FillPath(int start, int end, List<XY> path, Random rand)
	{
		Set(0, 0, new GridEle(Global.Ops.Goal, start));
		Set(maxX-1, maxY-1, new GridEle(Global.Ops.Goal, end));
		int i = 0;
		int value = start;
		foreach(var xy in path)
		{
			GD.Print("xy on path ", xy, " index ", i);
			i++;
			GD.Print("ele is ", At(xy).op, " ", At(xy).value);
			GD.Print("current value is ", value);
			if(At(xy).op == Global.Ops.Goal)
			{
				GD.Print("at goal");
				continue;
			}
			else if(i == path.Count - 1)
			{
				GD.Print("at end but one");
				int diff = end-value;
				if(diff >= 0)
				{
					Set(xy, new GridEle(Ops.Plus, diff));
				}
				else
				{
					Set(xy, new GridEle(Ops.Minus, Math.Abs(diff)));
				}
			}
			else
			{
				Ops op = Ops.None;
				int randomOp = rand.Next(4);
				switch(randomOp)
				{
					case 0:
						op = Ops.Plus;
						break;
					case 1:
						op = Ops.Minus;
						break;
					case 2:
						op = Ops.Divide;
						break;
					case 3:
						op = Ops.Times;
						break;
				}
				if(op == Ops.Divide && value % 2 == 1)
				{
					op = Ops.Minus;
				}
				if(op == Ops.Plus || op == Ops.Minus)
				{
					int randValue = rand.Next(Math.Min(start / 2, end / 2), Math.Max(start * 2, start / 2));
					Set(xy, new GridEle(op, Math.Abs(value - randValue)));
					if(op == Ops.Plus)
					{
						value += Math.Abs(value - randValue);
					}
					else
					{
						value -= Math.Abs(value - randValue);
					}
				}
				else if(op == Ops.Divide)
				{ 
					if(value > end*2)
					{
						Set(xy, new GridEle(op, 3));
						value /= 4;
					}
					else
					{
						Set(xy, new GridEle(op, 2));
						value /= 2;
					}
				}
				else if(op == Ops.Times)
				{
					if(value > end*2)
					{
						Set(xy, new GridEle(op, 2));
						value *= 2;
					}
					else
					{
						Set(xy, new GridEle(op, 3));
						value *= 3;
					}
				}
			}
		}
		GD.Print("value at end of path: ", value);
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

	public GridEle At(XY xy)
	{
		return grid[xy.x][xy.y];
	}

	public void Set(int x, int y, GridEle ele)
	{
		grid[x][y] = ele;
	}

	public void Set(XY xy, GridEle ele)
	{
		grid[xy.x][xy.y] = ele;
	}
}
