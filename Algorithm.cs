using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Algorithm;
using Xunit;

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

    [Obsolete("bitte strategie-überladung nutzen")]
    public void Find(SucheNach sucheNach)
    {
      FindForTesting(sucheNach, (answer) => Database.Save(answer));
    }

    [Obsolete("bitte strategie-überladung nutzen")]
    public void FindForTesting(SucheNach sucheNach, Action<Kombination> databaseAction)
    {
      var strategie = MappeSucheNachAufStrategie(sucheNach);
      FindForTesting(strategie, databaseAction);
    }

    public void FindForTesting(IchSelektierDasErgebis strategie, Action<Kombination> databaseAction)
    {
      var paare = ErzeugeKombinationen(_personen).ToList();
      if(paare.Count < 1)
      {
        return;
      }

      var answer = ErmittleErgebnis(strategie, paare);

      databaseAction(answer);
    }

    IchSelektierDasErgebis MappeSucheNachAufStrategie(SucheNach sucheNach)
    {
      switch (sucheNach)
      {
        case SucheNach.KleinsterAltersunterschied:
          return new KleinsterAltersunterschied();
        case SucheNach.GrößterAltersunterschied:
          return new GrößterAltersunterschied();
        default:
          throw new ArgumentOutOfRangeException(nameof(sucheNach), sucheNach, null);
      }
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

    static Kombination ErmittleErgebnis(IchSelektierDasErgebis selektor, IEnumerable<Kombination> kombinationen)
    {
      return selektor.ErmittleErgebnis(kombinationen);
    }

    public interface IchSelektierDasErgebis
    {
      Kombination ErmittleErgebnis(IEnumerable<Kombination> kombinationen);
    }

    public class KleinsterAltersunterschied : IchSelektierDasErgebis
    {
      public Kombination ErmittleErgebnis(IEnumerable<Kombination> kombinationen)
      {
        return kombinationen.OrderBy(x => x.Altersunterschied).First();
      }
    }

    public class GrößterAltersunterschied : IchSelektierDasErgebis
    {
      public Kombination ErmittleErgebnis(IEnumerable<Kombination> kombinationen)
      {
        return kombinationen.OrderBy(x => x.Altersunterschied).Last();
      }
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


public class KleinsterAltersunterschied
{
  [Fact]
  public void Selektiert_kleinsten_Unterschied()
  {
    var personen = new[]
    {
      new Kombination(new Person {Name = "Alex", Geburtsdatum = new DateTime(1900, 1, 1)},
        new Person {Name = "Peter", Geburtsdatum = new DateTime(1902, 1, 1)}),
      new Kombination(new Person {Name = "Alex", Geburtsdatum = new DateTime(1900, 1, 1)},
        new Person {Name = "Hanna", Geburtsdatum = new DateTime(1901, 1, 1)})
    };

    var selektor = new Finder.KleinsterAltersunterschied();

    var ergebis = selektor.ErmittleErgebnis(personen);

    Assert.Equal("Alex", ergebis.Person1.Name);
    Assert.Equal("Hanna", ergebis.Person2.Name);
  }
}
