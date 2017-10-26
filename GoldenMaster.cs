﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Algorithm;
using Xunit;

namespace Refactoring_1
{
  public class GoldenMaster
  {
    [Theory]
    [InlineData(SucheNach.KleinsterAltersunterschied, 286, "M", "A")]
    [InlineData(SucheNach.GrößterAltersunterschied, 19221, "W", "B")]
    public void Approve_saves_answer_to_database(SucheNach sucheNach, int altersunterschied, string name1, string name2)
    {
      var things = new[]
      {
        new Person {Name = "A", Geburtsdatum = new DateTime(1980, 5, 23)},
        new Person {Name = "B", Geburtsdatum = new DateTime(1991, 4, 27)},
        new Person {Name = "C", Geburtsdatum = new DateTime(1954, 4, 19)},
        new Person {Name = "W", Geburtsdatum = new DateTime(1938, 9, 11)},
        new Person {Name = "M", Geburtsdatum = new DateTime(1979, 8, 11)}
      };

      var finder = new Finder(things.ToList());

      Kombination savedAnswer = null;

      finder.FindForTesting(sucheNach,
        answer =>
        {
          savedAnswer = answer;
        });

      Assert.Equal(TimeSpan.FromDays(altersunterschied), savedAnswer.Altersunterschied);
      Assert.Equal(name1, savedAnswer.Person1.Name);
      Assert.Equal(name2, savedAnswer.Person2.Name);
    }

    [Fact]
    public void Approve_does_not_save_to_database_with_empty_person_list()
    {
      var things = new List<Person>();

      var finder = new Finder(things.ToList());

      var wasSavedToDatabase = false;

      finder.FindForTesting(SucheNach.KleinsterAltersunterschied,
        _ =>
        {
          wasSavedToDatabase = true;
        });

      Assert.False(wasSavedToDatabase);
    }


    [Fact]
    public void Ist_mindestens_genauso_schnell_wie_vorher()
    {
      var things = new[]
      {
        new Person {Name = "A", Geburtsdatum = new DateTime(1980, 5, 23)},
        new Person {Name = "B", Geburtsdatum = new DateTime(1991, 4, 27)},
        new Person {Name = "C", Geburtsdatum = new DateTime(1954, 4, 19)},
        new Person {Name = "W", Geburtsdatum = new DateTime(1938, 9, 11)},
        new Person {Name = "M", Geburtsdatum = new DateTime(1979, 8, 11)}
      };
      things = Enumerable.Repeat(things, 1000).SelectMany(x => x).ToArray();

      var finder = new Finder(things.ToList());

      Kombination savedAnswer = null;

      var timeTaken = Measure.Time(() => finder.FindForTesting(SucheNach.KleinsterAltersunterschied, answer => { }));

      Assert.InRange(timeTaken.TotalSeconds, 0, TimeSpan.FromSeconds(30).TotalSeconds);
    }

    static class Measure
    {
      public static TimeSpan Time(Action action)
      {
        var timer = new Stopwatch();
        timer.Start();

        action();

        timer.Stop();
        return timer.Elapsed;
      }
    }
  }
}
