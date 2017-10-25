using System;
using System.Linq;
using Algorithm;

namespace Refactoring_1
{
  public class Program
  {
    public static void Run()
    {
      var things = new[]
      {
        new Thing {Moniker = "A", Date = new DateTime(1980, 5, 23)},
        new Thing {Moniker = "B", Date = new DateTime(1991, 4, 27)},
        new Thing {Moniker = "C", Date = new DateTime(1954, 4, 19)},
        new Thing {Moniker = "W", Date = new DateTime(1938, 9, 11)},
        new Thing {Moniker = "M", Date = new DateTime(1979, 8, 11)}
      };

      var finder = new Finder(things.ToList());
      
      var f1 = finder.Find(FT.One);
      Console.WriteLine(f1.Duration);
      Console.WriteLine(f1.First.Moniker);
      Console.WriteLine(f1.Second.Moniker);

      var f2 = finder.Find(FT.Two);
      Console.WriteLine(f2.Duration);
      Console.WriteLine(f2.First.Moniker);
      Console.WriteLine(f2.Second.Moniker);
    }
  }
}
