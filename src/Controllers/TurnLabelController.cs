using System.Diagnostics;
using Checkers;
public class TurnLabelController
{
  private Label turnLabelInstance;

  public TurnLabelController(Label label)
  {
    turnLabelInstance = label;
  }

  public void SwitchTurn(string currentTurn)
  {
    turnLabelInstance.Text = $"{currentTurn}'s turn";
  }
}