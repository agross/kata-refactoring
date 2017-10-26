using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Algorithm
{
  public class Paar
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
    private readonly List<Person> _p;

    public Finder(List<Person> p)
    {
      _p = p;
    }

    public void Find(FT ft)
    {
      FindForTesting(ft, (answer) => Database.Save(answer));
    }

    public void FindForTesting(FT ft, Action<Paar> databaseAction)
    {
      var paare = To_be_clarified();
      if(paare.Count < 1)
      {
        return;
      }

      var answer = ErmittleErgebnisAnhandFt(ft, paare);
      databaseAction(answer);
    }

    static Paar ErmittleErgebnisAnhandFt(FT ft, List<Paar> paare)
    {
      Paar answer = paare[0];
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

    List<Paar> To_be_clarified()
    {
      var tr = new List<Paar>();

      for (var i = 0; i < _p.Count - 1; i++)
      {
        for (var j = i + 1; j < _p.Count; j++)
        {
          var r = new Paar();
          if (_p[i].Geburtsdatum < _p[j].Geburtsdatum)
          {
            r.Person1 = _p[i];
            r.Person2 = _p[j];
          }
          else
          {
            r.Person1 = _p[j];
            r.Person2 = _p[i];
          }
          r.Altersunterschied = r.Person2.Geburtsdatum - r.Person1.Geburtsdatum;
          tr.Add(r);
        }
      }
      return tr;
    }
  }

  public class Database
  {
    public static void Save(Paar answer)
    {
      Console.WriteLine("Saved to database!");
    }
  }
}
