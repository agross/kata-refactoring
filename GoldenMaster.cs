using System;
using System.Collections.Generic;
using System.IO;
using Algorithm;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;

namespace Refactoring
{
  [TestFixture]
  public class GoldenMaster
  {
    [Test]
    public void Erzeugt_Ausgabe_mit_Liste_von_Things()
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

    [Test]
    public void Findet_keine_Paare_bei_leerer_Liste()
    {
      var finder = new Finder(new List<Thing>());

      var f1 = finder.Find(FT.One);
      var f2 = finder.Find(FT.Two);

      Assert.IsNull(f1.P1);
      Assert.IsNull(f1.P2);

      Assert.IsNull(f2.P1);
      Assert.IsNull(f2.P2);
    }
  }
}
