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
    public Person(string name, DateTime geburtsdatum)
    {
      Name = name;
      Geburtsdatum = geburtsdatum;
    }

    public string Name { get; set; }
    public DateTime Geburtsdatum { get; set; }
  }

  public class Finder
  {
    readonly IEnumerable<Person> _personen;

    public Finder(IEnumerable<Person> personen)
    {
      _personen = personen;
    }

    [Obsolete("bitte strategie-überladung nutzen")]
    public void Find(SucheNach sucheNach)
    {
      Find(MappeSucheNachAufStrategie(sucheNach),
           new DatabaseAdapter());
    }

    public void FindForTesting(SucheNach sucheNach, IDatabase db)
    {
      Find(MappeSucheNachAufStrategie(sucheNach),
           db);
    }

    public void Find(IchSelektierDasErgebis strategie, IDatabase database)
    {
      var paare = ErzeugeKombinationen(_personen);
      if(!paare.Any())
      {
        return;
      }

      var answer = ErmittleErgebnis(strategie, paare);

      database.Save(answer);
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

  public interface IDatabase
  {
     void Save(object irgendwas);
  }

  public class DatabaseAdapter : IDatabase
  {
    public void Save(object irgendwas)
    {
      Database.Save((Kombination)irgendwas);
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
