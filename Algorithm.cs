using System;
using System.Collections.Generic;

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
      var temporaryList = new List<Pair>();

      for(var i = 0; i < _list.Count - 1; i++)
      {
        for(var j = i + 1; j < _list.Count; j++)
        {
          var pair = new Pair();
          if(_list[i].Date < _list[j].Date)
          {
            pair.First = _list[i];
            pair.Second = _list[j];
          }
          else
          {
            pair.First = _list[j];
            pair.Second = _list[i];
          }
          pair.Duration = pair.Second.Date - pair.First.Date;
          temporaryList.Add(pair);
        }
      }

      if(temporaryList.Count < 1)
      {
        return new Pair();
      }

      Pair answer = temporaryList[0];
      foreach(var result in temporaryList)
      {
        switch(searchType)
        {
          case SearchType.ShortestDuration:
            if(result.Duration < answer.Duration)
            {
              answer = result;
            }
            break;

          case SearchType.LongestDuration:
            if(result.Duration > answer.Duration)
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
