using System;
using System.Collections.Generic;

namespace Algorithm
{
  public class Pair
  {
    public Thing P1 { get; set; }
    public Thing P2 { get; set; }
    public TimeSpan Duration { get; set; }
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
    private readonly List<Thing> _things;

    public Finder(List<Thing> things)
    {
      _things = things;
    }

    public Pair Find(FT ft)
    {
      var pairs = ComputePairsFromThings();

      if(pairs.Count < 1)
      {
        return new Pair();
      }

      return GetAnswer(ft, pairs);
    }

    List<Pair> ComputePairsFromThings()
    {
      var pairs = new List<Pair>();

      for (var i = 0; i < _things.Count - 1; i++)
      {
        for (var j = i + 1; j < _things.Count; j++)
        {
          var r = new Pair();
          if (_things[i].Date < _things[j].Date)
          {
            r.P1 = _things[i];
            r.P2 = _things[j];
          }
          else
          {
            r.P1 = _things[j];
            r.P2 = _things[i];
          }

          r.Duration = r.P2.Date - r.P1.Date;
          pairs.Add(r);
        }
      }

      return pairs;
    }

    static Pair GetAnswer(FT ft, List<Pair> pairs)
    {
      var answer = pairs[0];
      foreach (var result in pairs)
      {
        switch (ft)
        {
          case FT.One:
            if (result.Duration < answer.Duration)
            {
              answer = result;
            }

            break;

          case FT.Two:
            if (result.Duration > answer.Duration)
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
