using System;
using System.IO;
using Xunit;

namespace Refactoring_1
{
  // Observe output from current implementation
  public class GoldenMaster
  {
    readonly StringWriter _output = new StringWriter();

    public GoldenMaster()
    {
      Console.SetOut(_output);
    }
    
    [Fact]
    public void Approve_implemenation()
    {
      Program.Run();
      
      Assert.Equal("286.00:00:00\nM\nA\n19221.00:00:00\nW\nB\n", _output.ToString());
    }
  }
}
