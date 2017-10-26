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
        new Person {Name = "A", Geburtsdatum = new DateTime(1980, 5, 23)},
        new Person {Name = "B", Geburtsdatum = new DateTime(1991, 4, 27)},
        new Person {Name = "C", Geburtsdatum = new DateTime(1954, 4, 19)},
        new Person {Name = "W", Geburtsdatum = new DateTime(1938, 9, 11)},
        new Person {Name = "M", Geburtsdatum = new DateTime(1979, 8, 11)}
      };

      var finder = new Finder(things.ToList());

finder.Find(SucheNach.KleinsterAltersunterschied);
    }
  }
}
