using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Algorithm;
using FakeItEasy;
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
      var things = new []
      {
        New.Person.MitNamen("Alex").GeborenAm(1980, 5, 23),
        new Person {Name = "A", Geburtsdatum = new DateTime(1980, 5, 23)},
        new Person {Name = "B", Geburtsdatum = new DateTime(1991, 4, 27)},
        new Person {Name = "C", Geburtsdatum = new DateTime(1954, 4, 19)},
        new Person {Name = "W", Geburtsdatum = new DateTime(1938, 9, 11)},
        new Person {Name = "M", Geburtsdatum = new DateTime(1979, 8, 11)},

      };



      var finder = new Finder(things.ToList());

      var database = A.Fake<IDatabase>();
      Kombination savedAnswer = null;
      A.CallTo(() => database.Save(A<Kombination>._))
        .Invokes(call =>
        {
          savedAnswer = (Kombination) call.Arguments[0];
        });

      finder.FindForTesting(sucheNach, database);

      Assert.Equal(TimeSpan.FromDays(altersunterschied), savedAnswer.Altersunterschied);
      Assert.Equal(name1, savedAnswer.Person1.Name);
      Assert.Equal(name2, savedAnswer.Person2.Name);

      A.CallTo(() => database.Save(A<Kombination>._)).MustHaveHappened();
    }

    [Fact]
    public void Approve_does_not_save_to_database_with_empty_person_list()
    {
      var things = new List<Person>();

      var finder = new Finder(things.ToList());

      var wasSavedToDatabase = false;

      var database = A.Fake<IDatabase>();
      finder.FindForTesting(SucheNach.KleinsterAltersunterschied,
        database);

      A.CallTo(() => database.Save(A<Kombination>._)).MustNotHaveHappened();
    }


    [Fact]
    public void Ist_mindestens_genauso_schnell_wie_vorher()
    {
      var things = new[]
      {
        new Person() {Name = "A", Geburtsdatum = new DateTime(1980, 5, 23)},
        new Person {Name = "B", Geburtsdatum = new DateTime(1991, 4, 27)},
        new Person {Name = "C", Geburtsdatum = new DateTime(1954, 4, 19)},
        new Person {Name = "W", Geburtsdatum = new DateTime(1938, 9, 11)},
        new Person {Name = "M", Geburtsdatum = new DateTime(1979, 8, 11)}
      };
      things = Enumerable.Repeat(things, 1000).SelectMany(x => x).ToArray();

      var finder = new Finder(things.ToList());

      var timeTaken = Measure.Time(() => finder.FindForTesting(SucheNach.KleinsterAltersunterschied, A.Fake<IDatabase>()));

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

  public static class New
  {
    public static PersonBuilder Person => new PersonBuilder();
  }

  public class PersonBuilder
  {
    string _name;
    DateTime _geburtsdatum;

    public PersonBuilder MitNamen(string name)
    {
      _name = name;
      return this;
    }

    public PersonBuilder GeborenAm(int jahr, int monat, int tag)
    {
      _geburtsdatum = new DateTime(jahr, monat, tag);
      // _geburtsdatum = new Geburtsdatum(jahr, monat, tag)
      return this;

    }

    public Person Build()
    {
      return new Person(_name, _geburtsdatum);
      // return new Person {Name = _name, Geburtsdatum = _geburtsdatum};
    }

    public static implicit operator Person(PersonBuilder self)
    {
      return self.Build();
    }
  }
}
