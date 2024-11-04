using System.Diagnostics;
using System.Reflection;
using Checkers;
using Microsoft.Maui.Graphics.Platform;

public class CheckersBoardDrawable : IDrawable
{
  public float tileSize;
  public float boardSize;
  private Board board;
  private int[,] _board;
  public int[]? highlightedTile;

  public CheckersBoardDrawable(Board board)
  {
    this.board = board;
    this._board = board.getBoard();
  }

  public void setHighlightedTile(int? fromX, int? fromY)
  {
    if (fromX == null || fromY == null)
    {
      highlightedTile = null;
    }
    else
    {
      if (highlightedTile == null)
      {
        highlightedTile = new int[2];
      }

      highlightedTile[0] = fromX.Value;
      highlightedTile[1] = fromY.Value;
    }
  }

  public void Draw(ICanvas canvas, RectF dirtyRect)
  {
    tileSize = (int)Math.Min((dirtyRect.Width / 8), dirtyRect.Height / 8);
    boardSize = tileSize * 8;

    DrawBoardGrids(canvas);
    DrawBoardOutline(canvas);
    DrawHighlightedTile(canvas);
    DrawPieces(canvas);
  }
  public void DrawBoardGrids(ICanvas canvas)
  {
    for (int y = 0; y < 8; y++)
    {
      for (int x = 0; x < 8; x++)
      {
        var color = (x + y) % 2 == 0 ? Colors.White : Colors.DarkGray;
        canvas.FillColor = color;
        canvas.FillRectangle(x * tileSize, y * tileSize, tileSize, tileSize);
      }
    }
  }
  public void DrawBoardOutline(ICanvas canvas)
  {
    canvas.StrokeColor = Colors.Black;
    canvas.StrokeSize = 3;
    canvas.DrawRectangle(0, 0, boardSize, boardSize);
  }
  public void DrawHighlightedTile(ICanvas canvas)
  {
    if (highlightedTile != null)
    {
      canvas.FillColor = Colors.Green;
      canvas.FillRectangle(highlightedTile[0] * tileSize, highlightedTile[1] * tileSize, tileSize, tileSize);
    }
  }
  public void DrawBlackPiece(ICanvas canvas, float x, float y)
  {
    canvas.FillColor = Colors.Black;

    float radius = tileSize * 0.4f;
    float centerX = tileSize * x + tileSize / 2;
    float centerY = tileSize * y + tileSize / 2;
    canvas.FillCircle(centerX, centerY, radius);
  }
  public void DrawBlackKing(ICanvas canvas, float x, float y)
  {
    canvas.FillColor = Colors.DarkTurquoise;
    Debug.WriteLine("Drawing black king");
    float radius = tileSize * 0.4f;
    float centerX = tileSize * x + tileSize / 2;
    float centerY = tileSize * y + tileSize / 2;
    canvas.FillCircle(centerX, centerY, radius);
  }
  public void DrawRedPiece(ICanvas canvas, float x, float y)
  {
    canvas.FillColor = Colors.Red;

    float radius = tileSize * 0.4f;
    float centerX = tileSize * x + tileSize / 2;
    float centerY = tileSize * y + tileSize / 2;
    canvas.FillCircle(centerX, centerY, radius);
  }
  public void DrawRedKing(ICanvas canvas, float x, float y)
  {
    canvas.FillColor = Colors.DeepPink;
    Debug.WriteLine("Drawing red king");
    float radius = tileSize * 0.4f;
    float centerX = tileSize * x + tileSize / 2;
    float centerY = tileSize * y + tileSize / 2;
    canvas.FillCircle(centerX, centerY, radius);
  }
  public void DrawPieces(ICanvas canvas)
  {
    for (int i = 0; i < _board.GetLength(0); i++)
    {
      for (int j = 0; j < _board.GetLength(1); j++)
      {
        switch (_board[i, j])
        {
          case 0:
            break;
          case 1:
            DrawBlackPiece(canvas, j, i);
            break;
          case 2:
            DrawBlackKing(canvas, j, i);
            break;
          case -1:
            DrawRedPiece(canvas, j, i);
            break;
          case -2:
            DrawRedKing(canvas, j, i);
            break;
        }
      }
    }
  }
}