using Godot;
using System;
using System.Diagnostics.Tracing;

public class Global : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    public int maxX = 5;
	public int maxY = 5;

    public Image square;
    public DynamicFont gridFont;

    public Theme gridTheme;

	static Global instance;

    public static Global Instance { get { return instance; } }

    public enum State { DrawStart, DrawEnd, DelStart, DelEnd, }

    Global()
    {
        instance = this;
    }


    public enum Ops { 
        None = ' ' , Plus = '+', Minus = '-', Times = '*', Divide = '/', Goal = ' '
    };

    public override void _Ready()
    {
        square = GD.Load<Image>("res://white_square_128.png");
        gridFont = GD.Load<DynamicFont>("res://grid_label_font.tres");
        gridTheme = GD.Load<Theme>("res://grid_square_theme.tres");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
