﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithm
{  
  public class Pair
  {
    public Thing First { get; set; }
    public Thing Second { get; set; }
    public TimeSpan Duration { get; set; }
  }
  
  public enum SearchType
  {
    ShortestDuration,
    LongestDuration
  }

  public class Thing
  {
    public string Moniker { get; set; }
    public DateTime Date { get; set; }
  }

  public class Finder
  {
    private readonly List<Thing> _list;

    public Finder(List<Thing> list)
    {
      _list = list;
    }

    public Pair Find(SearchType searchType)
    {
      var durations = CalculateDurationsBetweenThings(_list);

      if(!durations.Any())
      {
        return new Pair();
      }

      var strategy = MapTypeToComparisonStrategy(searchType);
      return FindShortestOrLongestDuration(strategy, durations);
    }

    static ISearchStrategy MapTypeToComparisonStrategy(SearchType searchType)
    {
      switch (searchType)
      {
        case SearchType.ShortestDuration:
          return new ShortestDurationStrategy();
          break;
        case SearchType.LongestDuration:
          return new LongestDurationStrategy();
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(searchType), searchType, null);
      }
    }

    static Pair FindShortestOrLongestDuration(ISearchStrategy strategy, IEnumerable<Pair> durations)
    {
      return durations.Aggregate(durations.First(), strategy.Select);
    }

    static IEnumerable<Pair> CalculateDurationsBetweenThings(IEnumerable<Thing> things)
    {
      return things.SelectMany((left, i) =>
      {
        return things
          .Skip(i + 1)
          .Select(right =>
          {
            var pair = EarliestFirst(left, right);
            pair.Duration = pair.Second.Date - pair.First.Date;
            return pair;
          });
      });
    }

    static Pair EarliestFirst(Thing left, Thing right)
    {
      if (left.Date < right.Date)
      {
        return new Pair {First = left, Second = right};
      }
      
      return new Pair {First = right, Second = left};
    }
  }

  class LongestDurationStrategy : ISearchStrategy
  {
    public Pair Select(Pair current, Pair candidate)
    {
      return candidate.Duration > current.Duration ? candidate : current;
    }
  }

  class ShortestDurationStrategy : ISearchStrategy
  {
    public Pair Select(Pair current, Pair candidate)
    {
      return candidate.Duration < current.Duration ? candidate : current;
    }
  }

  interface ISearchStrategy
  {
    Pair Select(Pair current, Pair candidate);
  }
}
