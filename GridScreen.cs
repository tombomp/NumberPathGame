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
        grid.Columns = Instance.maxX;
        for (int x = 0; x < Instance.maxX; x++)
        {
            sprites[x] = new GridSprite[Instance.maxY];
            for (int y = 0; y < Instance.maxY; y++)
            {
                GD.Print(x, " ", y);
                sprites[x][y] = new GridSprite
                {
                    type = Ops.Divide,
                    number = 5
                };
                grid.AddChild(sprites[x][y]);
            }
        }
        Grid g = new Grid(5, 5);
        Random rand = new Random(1);
        var path = g.RandomPath(rand);
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
