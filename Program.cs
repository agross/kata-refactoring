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

finder.Find(FT.One);
    }
  }
}
