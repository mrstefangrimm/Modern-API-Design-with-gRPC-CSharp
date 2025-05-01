namespace GreetClient.Tests
{
  public class GreeterTest
  {
    [Fact]
    public void Constructor_FirstAndLastName_SetAsFirstAndLastName()
    {
      var greeter = new Greeter("first", "last");
      Assert.Equal("first", greeter.first_name);
      Assert.Equal("last", greeter.last_name);
    }
  }
}