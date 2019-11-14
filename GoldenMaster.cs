using System;
using System.IO;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;

namespace Refactoring
{
  [TestFixture]
  public class GoldenMaster
  {
    [Test]
    public void Vorlage()
    {
      var fakeConsole = new StringWriter();
      Console.SetOut(fakeConsole);

      Refactoring_1.Program.Run();

      var stringOutput = fakeConsole.ToString();
      var expectedOutput = @"286.00:00:00
M
A
19221.00:00:00
W
B
";

      Assert.AreEqual(stringOutput, expectedOutput);
    }

  }
}
