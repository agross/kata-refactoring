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
          var pair = CreatePairFromTwoThings(_things[i],
            _things[j]);

          pairs.Add(pair);
        }
      }

      return pairs;
    }

    static Pair CreatePairFromTwoThings(Thing left, Thing right)
    {
      var (oldest, newest) = OldestFirst(left, right);

      return new Pair
      {
        P1 = oldest,
        P2 = newest,
        Duration = newest.Date - oldest.Date
      };
    }

    static (Thing, Thing) OldestFirst(Thing left, Thing right)
    {
      if (left.Date < right.Date)
      {
        return (left, right);
      }

      return (right, left);
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
