using Godot;
using System;
using static Global;

public class GridScreen : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    GridContainer grid;
    GridSprite[][] sprites;
    public override void _Ready()
    {
        sprites = new GridSprite[Instance.maxX][];
        grid = GetNode<GridContainer>("NumberGrid");
        Grid g = new Grid(5, 5);
        Random rand = new Random(2);
        var path = g.RandomPath(rand);
        g.FillPath(30, 90, path, rand);
        grid.Columns = Instance.maxX;
        for (int x = 0; x < Instance.maxX; x++)
        {
            sprites[x] = new GridSprite[Instance.maxY];
            for (int y = 0; y < Instance.maxY; y++)
            {
                sprites[x][y] = new GridSprite
                {
                    type = g.At(x,y).op,
                    number = g.At(x, y).value
                };
                grid.AddChild(sprites[x][y]);
            }
        }

        foreach (var xy in path)
        {
            sprites[xy.x][xy.y].Modulate = Color.ColorN("red");
        }

    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
