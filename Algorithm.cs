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

  public enum FT
  {
    One,
    Two
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

    public void Find(FT ft)
    {
      FindForTesting(ft, (answer) => Database.Save(answer));
    }

    public void FindForTesting(FT ft, Action<Kombination> databaseAction)
    {
      var paare = To_be_clarified();
      if(paare.Count < 1)
      {
        return;
      }

      var answer = ErmittleErgebnisAnhandFt(ft, paare);
      databaseAction(answer);
    }

    static Kombination ErmittleErgebnisAnhandFt(FT ft, List<Kombination> paare)
    {
      Kombination answer = paare[0];
      foreach (var result in paare)
      {
        switch (ft)
        {
          case FT.One:
            if (result.Altersunterschied < answer.Altersunterschied)
            {
              answer = result;
            }
            break;

          case FT.Two:
            if (result.Altersunterschied > answer.Altersunterschied)
            {
              answer = result;
            }
            break;
        }
      }
      return answer;
    }

    List<Kombination> To_be_clarified()
    {
      var ergebnis = new List<Kombination>();

      for (var i = 0; i < _personen.Count - 1; i++)
      {
        for (var j = i + 1; j < _personen.Count; j++)
        {
          var r = new Kombination(_personen[i], _personen[j]);
          ergebnis.Add(r);
        }
      }
      return ergebnis;
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
