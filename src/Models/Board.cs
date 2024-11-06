using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

public class Board
{
  public bool debugMode = false;
  public bool captureAgain = false;

  private int[,] _board;
  public Board()
  {
    _board = debugMode
       ? new int[,]
       {
            {0,0,0,0,-1,0,0,0},
            {0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0},
            {0,0,0,-1,0,0,0,0},
            {0,0,0,0,1,0,0,0},
            {0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0}
       }
       : new int[,]
       {
            {-1,0,-1,0,-1,0,-1,0},
            {0,-1,0,-1,0,-1,0,-1},
            {-1,0,-1,0,-1,0,-1,0},
            {0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0},
            {0,1,0,1,0,1,0,1},
            {1,0,1,0,1,0,1,0},
            {0,1,0,1,0,1,0,1}
       };
  }

  public int[,] getBoard()
  {
    return _board;
  }

  public void PrintBoardToConsole()
  {
    for (int row = 0; row < _board.GetLength(0); row++)
    {
      string rowString = "";
      for (int col = 0; col < _board.GetLength(1); col++)
      {
        rowString += _board[row, col] + " ";
      }
      Debug.WriteLine(rowString);
    }
  }

  public bool MoveIsValid(int[] fromXY, int[] toXY, int piece)
  {
    bool moveLengthIsCorrect = IsMoveLengthCorrect(fromXY, toXY, piece);
    bool moveDirectionIsCorrect = (IsMoveDirectionCorrect(fromXY, toXY, piece) || piece == -2 || piece == 2);

    if (moveLengthIsCorrect && moveDirectionIsCorrect)
    {
      return true;
    }
    else if (moveDirectionIsCorrect && IsCaptured(fromXY, toXY, piece))
    {
      return true;
    }
    return false;
  }

  public void CheckForKingAndPromote(int[] toXY, int piece)
  {
    (int toX, int toY) = (toXY[0], toXY[1]);
    if (piece == -1 && toY == 7)
    {
      _board[toY, toX] = -2;
      Debug.WriteLine($"red promoted a piece board element is now {_board[toY, toX]}");
    }
    else if (piece == 1 && toY == 0)
    {
      _board[toY, toX] = 2;
      Debug.WriteLine($"black promoted a piece board eleemnt is now {_board[toY, toX]}");
    }
  }

  public bool IsMoveDirectionCorrect(int[] fromXY, int[] toXY, int player)
  {
    if (player == 1)
    {
      if (toXY[1] < fromXY[1]) return true;
    }
    if (player == -1)
    {
      if (toXY[1] > fromXY[1]) return true;
    }
    return false;
  }

  public bool IsMoveLengthCorrect(int[] fromXY, int[] toXY, int player)
  {
    (int fromX, int fromY) = (fromXY[0], fromXY[1]);
    (int toX, int toY) = (toXY[0], toXY[1]);

    Debug.WriteLine("validating move");
    if (player == 1)
    {
      if (_board[fromY, fromX] == player && _board[toY, toX] == 0 && toY < fromY)
      {
        if (Math.Abs(toX - fromX) == 1 && Math.Abs(toY - fromY) == 1) return true;
      }
    }
    else if (player == -1)
    {
      if (_board[fromY, fromX] == player && _board[toY, toX] == 0 && toY > fromY)
      {
        if (Math.Abs(toX - fromX) == 1 && Math.Abs(toY - fromY) == 1) return true;
      }
    }
    else if (player == 2 || player == -2)
    {
      if (_board[fromY, fromX] == player && _board[toY, toX] == 0)
      {
        if (Math.Abs(toX - fromX) == 1 && Math.Abs(toY - fromY) == 1) return true;
      }
    }
    Debug.WriteLine("diagonal check returning false");
    return false;
  }

  public bool IsCaptured(int[] fromXY, int[] toXY, int player)
  {
    (int fromX, int fromY) = (fromXY[0], fromXY[1]);
    (int toX, int toY) = (toXY[0], toXY[1]);

    if (Math.Abs(toX - fromX) == 2 && Math.Abs(toY - fromY) == 2)
    {
      int midX = (fromX + toX) / 2;
      int midY = (fromY + toY) / 2;

      if (_board[midY, midX] != 0 && _board[midY, midX] != player && _board[toY, toX] == 0)
      {
        EmptyBoardElement(midX, midY);
        Debug.WriteLine("IsCaptured returning true");
        captureAgain = CanPlayerCaptureAgain(toXY, player);
        return true;
      }
    }
    captureAgain = false;
    Debug.WriteLine("IsCaptured returning false");
    return false;
  }

  public bool CanPlayerCaptureAgain(int[] toXY, int player)
  {
    int opponent;

    if (player == 1) opponent = -1;
    else opponent = 1;

    int toX = toXY[0];
    int toY = toXY[1];
    Debug.WriteLine($"tox and toy in repeat capture checker:({toX}, {toY})");
    int[,] directions = { { -1, -1 }, { -1, 1 }, { 1, -1 }, { 1, 1 } };
    for (int i = 0; i < directions.GetLength(0); i++)
    {
      int dx = directions[i, 0];
      int dy = directions[i, 1];
      int midX = toX + dx;
      int midY = toY + dy;
      int newTileX = toX + 2 * dx;
      int newTileY = toY + 2 * dy;

      // Makes sure new tile is on the board
      if (newTileX < 0 || newTileX >= _board.GetLength(1) || newTileY < 0 || newTileY >= _board.GetLength(0)) continue;
      if (newTileX < 0 || newTileX >= _board.GetLength(1) || newTileY < 0 || newTileY >= _board.GetLength(0)) continue;

      if (_board[midY, midX] == opponent)
      {
        if (_board[newTileY, newTileX] == 0)
        {
          if ((player == 1 && newTileY < toY) || (player == -1 && newTileY > toY) || (player == 2 || player == -2))
          {
            Debug.WriteLine("new capture is possible");
            return true;
          }
        }
      }
    }
    Debug.WriteLine("new capture not possible");
    return false;
  }

  public void EmptyBoardElement(int x, int y)
  {
    try
    {
      _board[y, x] = 0;
    }
    catch (Exception exception)
    {
      throw new Exception("Error occurred while setting the board value.", exception);
    }
  }

  public void SetPieceTo(int[] toXY, int piece)
  {
    (int toX, int toY) = (toXY[0], toXY[1]);
    try
    {
      _board[toY, toX] = piece;
    }
    catch (Exception exception)
    {
      throw new Exception("Error occurred while setting the board value. Out of bounds?", exception);
    }
  }

  public bool PlayerHasPieces(string player)
  {
    int redPieces = 0;
    int blackPieces = 0;
    for (int row = 0; row < _board.GetLength(0); row++)
    {
      for (int col = 0; col < _board.GetLength(1); col++)
      {
        int piece = _board[row, col];
        redPieces += piece < 0 ? 1 : 0;
        blackPieces += piece > 0 ? 1 : 0;
      }
    }
    if (player == "red" && redPieces > 0)
    {
      return true;
    }
    else if (player == "black" && blackPieces > 0)
    {
      return true;
    }
    return false;
  }
}