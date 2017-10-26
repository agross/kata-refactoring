using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Algorithm
{
  public class Kombination
  {
    public Person Person1 { get; }
    public Person Person2 { get; }
    public TimeSpan Altersunterschied { get; }

    public Kombination(Person links, Person rechts)
    {
      var sortiert = new[] {links, rechts}
        .OrderBy(x => x.Geburtsdatum);

      Person1 = sortiert.First();
      Person2 = sortiert.Last();

      Altersunterschied = Person2.Geburtsdatum - Person1.Geburtsdatum;
    }
  }

  public enum SucheNach
  {
    KleinsterAltersunterschied,
    GrößterAltersunterschied
  }

  public class Person
  {
    public string Name { get; set; }
    public DateTime Geburtsdatum { get; set; }
  }

  public class Finder
  {
    private readonly List<Person> _personen;

    public Finder(List<Person> personen)
    {
      _personen = personen;
    }

    public void Find(SucheNach sucheNach)
    {
      FindForTesting(sucheNach, (answer) => Database.Save(answer));
    }

    public void FindForTesting(SucheNach sucheNach, Action<Kombination> databaseAction)
    {
      var paare = ErzeugeKombinationen(_personen).ToList();
      if(paare.Count < 1)
      {
        return;
      }

      var answer = ErmittleErgebnisAnhandFt(sucheNach, paare);
      databaseAction(answer);
    }

    static IEnumerable<Kombination> ErzeugeKombinationen(IEnumerable<Person> personen)
    {
      return personen.SelectMany((person1, index) =>
        {
          return personen.Skip(index + 1)
            .Select(person2 => new Kombination(person1, person2));
        }
      );
    }

    static Kombination ErmittleErgebnisAnhandFt(SucheNach sucheNach, IEnumerable<Kombination> kombinationen)
    {
      var sortiert = kombinationen.OrderBy(x => x.Altersunterschied);

      switch (sucheNach)
      {
        case SucheNach.KleinsterAltersunterschied:
          return sortiert.First();

        case SucheNach.GrößterAltersunterschied:
          return sortiert.Last();
      }

      return kombinationen.First();
    }
  }

  public class Database
  {
    public static void Save(Kombination answer)
    {
      Console.WriteLine("Saved to database!");
    }
  }
}
