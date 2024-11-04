namespace Checkers
{
  public partial class App : Application
  {
    public App()
    {
      Console.WriteLine("App is starting...");
      InitializeComponent();

      MainPage = new AppShell();
    }
  }
}
