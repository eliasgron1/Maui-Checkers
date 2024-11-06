using Checkers;
using System.Diagnostics;

public class CheckersController
{
  private CheckersBoardDrawable checkersBoardDrawable;
  private TurnLabelController turnLabelController;
  private GraphicsView graphicsView;
  private Label turnLabel;
  private Board board;
  private string currentTurn;

  public int ConvertToBoardInt(int num) => (int)(num / checkersBoardDrawable.tileSize);

  public CheckersController(Label label, GraphicsView graphicsView)
  {
    this.turnLabel = label;
    this.graphicsView = graphicsView;
    this.turnLabelController = new TurnLabelController(label);
    this.board = new Board();
    this.checkersBoardDrawable = new CheckersBoardDrawable(board);
    this.currentTurn = "";
  }

  public void InitializeGame()
  {
    graphicsView.Drawable = checkersBoardDrawable;
    this.currentTurn = "black";
    turnLabelController.SwitchTurn(currentTurn);
  }

  public void HighlightChosenTile(int[] fromXY)
  {
    checkersBoardDrawable.setHighlightedTile(ConvertToBoardInt(fromXY[0]), ConvertToBoardInt(fromXY[1]));
    graphicsView.Invalidate();
  }

  public void RequestMoveTo(int[] fromXY, int[] toXY)
  {
    toXY[0] = ConvertToBoardInt(toXY[0]);
    toXY[1] = ConvertToBoardInt(toXY[1]);
    fromXY[0] = ConvertToBoardInt(fromXY[0]);
    fromXY[1] = ConvertToBoardInt(fromXY[1]);
    checkersBoardDrawable.setHighlightedTile(null, null);
    graphicsView.Invalidate();
    Debug.WriteLine($"Moving from: ({fromXY[0]}, {fromXY[1]}), to ({toXY[0]}, {toXY[1]})");
    Debug.WriteLine($"current turn {currentTurn}");

    if (currentTurn == "black")
    {
      BlackToMove(fromXY, toXY);
      if (board.captureAgain) currentTurn = "black";
    }
    else if (currentTurn == "red")
    {
      RedToMove(fromXY, toXY);
      if (board.captureAgain) currentTurn = "red";
    }
    turnLabelController.SwitchTurn(currentTurn);
    ShouldGameEnd();
    board.PrintBoardToConsole();
  }

  public void BlackToMove(int[] fromXY, int[] toXY)
  {
    int[,] _board = board.getBoard();
    int piece = _board[fromXY[1], fromXY[0]];

    Debug.WriteLine($"piece is {piece}");

    if (piece >= 1)
    {
      if (board.MoveIsValid(fromXY, toXY, piece))
      {
        board.SetPieceTo(toXY, piece);
        board.CheckForKingAndPromote(toXY, piece);
        board.EmptyBoardElement(fromXY[0], fromXY[1]);
        currentTurn = "red";
        graphicsView.Invalidate();
      }
    }
    else Debug.WriteLine("bad move try again");
  }

  public void RedToMove(int[] fromXY, int[] toXY)
  {
    int[,] _board = board.getBoard();
    int piece = _board[fromXY[1], fromXY[0]];

    Debug.WriteLine($"piece is {piece}");
    if (piece <= -1)
    {
      if (board.MoveIsValid(fromXY, toXY, piece))
      {
        board.SetPieceTo(toXY, piece);
        board.CheckForKingAndPromote(toXY, piece);
        board.EmptyBoardElement(fromXY[0], fromXY[1]);
        currentTurn = "black";
        graphicsView.Invalidate();
      }
    }
    else Debug.WriteLine("bad move try again");
  }

  public void ShouldGameEnd()
  {
    if (!board.PlayerHasPieces("red"))
    {
      turnLabelController.GameOverNotifier("black");
    }
    else if (!board.PlayerHasPieces("black"))
    {
      turnLabelController.GameOverNotifier("red");
    }
  }
}
