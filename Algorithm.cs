using System;
using System.Collections.Generic;

namespace Algorithm
{  
  public class F
  {
    public Thing P1 { get; set; }
    public Thing P2 { get; set; }
    public TimeSpan D { get; set; }
  }
  
  public enum FT
  {
    One,
    Two
  }

  public class Thing
  {
    public string Moniker { get; set; }
    public DateTime Date { get; set; }
  }

  public class Finder
  {
    private readonly List<Thing> _p;

    public Finder(List<Thing> p)
    {
      _p = p;
    }

    public F Find(FT ft)
    {
      var tr = new List<F>();

      for(var i = 0; i < _p.Count - 1; i++)
      {
        for(var j = i + 1; j < _p.Count; j++)
        {
          var r = new F();
          if(_p[i].Date < _p[j].Date)
          {
            r.P1 = _p[i];
            r.P2 = _p[j];
          }
          else
          {
            r.P1 = _p[j];
            r.P2 = _p[i];
          }
          r.D = r.P2.Date - r.P1.Date;
          tr.Add(r);
        }
      }

      if(tr.Count < 1)
      {
        return new F();
      }

      F answer = tr[0];
      foreach(var result in tr)
      {
        switch(ft)
        {
          case FT.One:
            if(result.D < answer.D)
            {
              answer = result;
            }
            break;

          case FT.Two:
            if(result.D > answer.D)
            {
              answer = result;
            }
            break;
        }
      }

      return answer;
    }
  }
}
