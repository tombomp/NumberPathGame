using Godot;
using System;
using static Global;

public class GridSprite : TextureRect
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    public Ops type = Ops.None;
    public int number = 0;
    Label l;
    ImageTexture squareImage;
    int width = 128;
    int height = 128;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
        Theme = Instance.gridTheme;
        GD.Print("adding gridsprite");
        ImageTexture squareImage = new ImageTexture();
        squareImage.CreateFromImage(Instance.square);
        this.Texture = squareImage;
        l = new Label();
        l.Text = (char)type + number.ToString();
        l.Align = Label.AlignEnum.Center;
        l.Valign = Label.VAlign.Center;
        l.Modulate = Color.ColorN("black");

        l.RectMinSize = new Vector2(width, height);
        AddChild(l);
        GD.Print(l.RectGlobalPosition);
       
    }

    public int Apply(int value)
	{
        switch (type) {
            case Ops.Divide:
                return value / number;
            case Ops.Minus:
                return value - number;
            case Ops.Plus:
                return value + number;
            case Ops.Times:
                return value * number;
            default:
                return number;
        }
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
