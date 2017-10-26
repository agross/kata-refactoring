using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Algorithm
{
  public class Kombination
  {
    public Person Person1 { get; set; }
    public Person Person2 { get; set; }
    public TimeSpan Altersunterschied { get; set; }
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
          var r = SortiereNachGeburtsdatum(_personen[i], _personen[j]);
          r.Altersunterschied = r.Person2.Geburtsdatum - r.Person1.Geburtsdatum;
          ergebnis.Add(r);
        }
      }
      return ergebnis;
    }

    static Kombination SortiereNachGeburtsdatum(Person links, Person rechts)
    {
      if (links.Geburtsdatum < rechts.Geburtsdatum)
      {
        return new Kombination
        {
          Person1 = links,
          Person2 = rechts,
        };
      }

      return new Kombination
      {
        Person1 = rechts,
        Person2 = links
      };
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
