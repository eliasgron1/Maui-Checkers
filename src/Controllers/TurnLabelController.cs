using Checkers;
using System.Diagnostics;
public class TurnLabelController
{
  private Label turnLabelInstance;

  public TurnLabelController(Label label)
  {
    turnLabelInstance = label;
  }

  public void SwitchTurn(string currentTurn)
  {
    Debug.WriteLine($"Changing label text to {currentTurn}'s turn");
    turnLabelInstance.Text = $"{currentTurn}'s turn";
  }

  public void GameOverNotifier(string winner)
  {
    Debug.WriteLine($"GAME OVER {winner} won");
    turnLabelInstance.Text = $"Game Over, {winner} won";
  }

}