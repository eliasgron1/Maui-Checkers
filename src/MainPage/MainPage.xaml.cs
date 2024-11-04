using System.Diagnostics;

namespace Checkers
{
  public partial class MainPage : ContentPage
  {
    private CheckersController checkersController;
    private int[] toXY;
    private int[] fromXY;
    public MainPage()
    {
      InitializeComponent();

      fromXY = new int[2];
      toXY = new int[2];

      checkersController = new CheckersController(turnLabel, graphicsView);
      checkersController.InitializeGame();
    }

    private void GraphicsView_StartInteraction(object sender, TouchEventArgs e)
    {
      var touchPoints = e.Touches;


      if (touchPoints.Count() > 0)
      {
        var touch = touchPoints[0];

        if (fromXY[0] == 0 || fromXY[1] == 0)
        {
          fromXY[0] = (int)touch.X;
          fromXY[1] = (int)touch.Y;
          checkersController.HighlightChosenTile(fromXY);
        }
        else
        {
          toXY[0] = (int)touch.X;
          toXY[1] = (int)touch.Y;
          checkersController.RequestMoveTo(fromXY, toXY);
          fromXY = new int[2];
          toXY = new int[2];
        }
      }
    }
  }
}
