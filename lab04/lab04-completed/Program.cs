using System;
using System.Xml.Linq;

FirstTry.Demo.Run();

#region Immutable people - first try

namespace FirstTry {
  class Person {
    public string Name { get; }
    public string Password {  get; }
    public Person(string name, string password) {
      this.Name = name;
      this.Password = password;
    }
    public Person WithName(string newName) {
      return new Person(newName, this.Password);
    }
  }

  class Student : Person {
    public DateTime Enrolled {  get; }
    public Student(string name, string password, DateTime enrolled) 
      : base(name, password) { 
      this.Enrolled = enrolled;
    }
    public new Student WithName(string newName) {
      return new Student(newName, this.Password, this.Enrolled);
    }
  }

  static class Demo {
    public static Person Rename(Person p) {
      return p.WithName(p.Name.Replace("š", "sz"));
    }
    public static void Run() {
      var student = new Student("Tomáš", "tweedledum", DateTime.Now);
      // TODO: This does not work, because Rename returns a Person!
      // var renamed = Rename(student);
      var renamed = student.WithName(student.Name.Replace("š", "sz"));
      Console.WriteLine(renamed.Enrolled);
    }
  } 
}

#endregion
#region Immutable people - init / required

namespace RequiredProps {
  class Person {
    public required string Name { get; init; }
    public required string Password {  get; init; }

    public Person WithName(string newName) {
      return new Person { Name = newName, Password = this.Password };
    }
  }

  class Student : Person {
    public required DateTime Enrolled { get; init; }
    public new Student WithName(string newName) {
      return new Student { Name = newName, Password = this.Password, Enrolled = this.Enrolled };
    }
  }

  static class Demo {
    public static Person Rename(Person p) {
      return p.WithName(p.Name.Replace("š", "sz"));
    }
    public static void Run() {
      var student = new Student { Name="Tomáš", Password="tweedledum", Enrolled=DateTime.Now };
      // var renamed = Rename(student);
      var renamed = student.WithName(student.Name.Replace("š", "sz"));
      Console.WriteLine(renamed.Enrolled);
    }
  } 
}

#endregion
#region Immutable people - self type

namespace SelfType {
  abstract class Person<T> {
    public string Name { get; }
    public string Password {  get; }
    public Person(string name, string password) {
      this.Name = name;
      this.Password = password;
    }
    public T WithName(string newName) {
      return ClonePerson(newName, this.Password);
    }
    protected abstract T ClonePerson(string newName, string newPassword);
  }

  class Student : Person<Student> {
    public DateTime Enrolled {  get; }
    public Student(string name, string password, DateTime enrolled) 
      : base(name, password) { 
      this.Enrolled = enrolled;
    }
    protected override Student ClonePerson(string newName, string newPassword) {
      return new Student(newName, newPassword, this.Enrolled);
    }
  }

  static class Demo {
    public static T Rename<T>(Person<T> p) {
      return p.WithName(p.Name.Replace("š", "sz"));
    }
    public static void Run() {
      var student = new Student("Tomáš", "tweedledum", DateTime.Now);
      var renamed1 = Rename(student);
      var renamed2 = student.WithName(student.Name.Replace("š", "sz"));
      Console.WriteLine(renamed1.Enrolled);
      Console.WriteLine(renamed2.Enrolled);
    }
  } 
}
#endregion
#region Immutable people - refactoring

namespace Refactoring {
  abstract class Person {
    public string Name { get; }
    public string Password {  get; }
    public Person(string name, string password) {
      this.Name = name;
      this.Password = password;
    }
  }
  abstract class ConcretePerson<T> : Person where T : Person {
    public ConcretePerson(string name, string password) : base(name, password) { }
    public T WithName(string newName) {
      return ClonePerson(newName, this.Password);
    }
    public T WithPassword(string newPass) {
      return ClonePerson(this.Name, newPass);
    }
    protected abstract T ClonePerson(string newName, string newPassword);
  }

  class Student : ConcretePerson<Student> {
    public DateTime Enrolled {  get; }
    public Student(string name, string password, DateTime enrolled) 
      : base(name, password) { 
      this.Enrolled = enrolled;
    }
    protected override Student ClonePerson(string newName, string newPassword) {
      return new Student(newName, newPassword, this.Enrolled);
    }
  }

  static class Extensions {
    public static void PrintAll(this List<Person> people) {
      foreach(var p in people) Console.WriteLine(p.Name);
    }
    
    public static List<Person> WithPasswordResetByFirstName<T> 
      (this List<T> people, string name, string newPassword) where T:ConcretePerson<T> {
      var res = new List<Person>();
      foreach(var p in people) 
        if (p.Name == name ) res.Add(p.WithPassword(newPassword));
        else res.Add(p);
      return res;
    }
  }
  static class Demo {
    public static T Rename<T>(ConcretePerson<T> p) where T:ConcretePerson<T> {
      return p.WithName(p.Name.Replace("š", "sz"));
    }

    public static void Run() {
      var student = new Student("Tomáš", "tweedledum", DateTime.Now);
      var renamed1 = Rename(student);
      var renamed2 = student.WithName(student.Name.Replace("š", "sz"));
      Console.WriteLine(renamed1.Enrolled);
      Console.WriteLine(renamed2.Enrolled);

		  List<Person> people = new List<Person> { student };
			Console.WriteLine("+++ People:");
			people.PrintAll();
			Console.WriteLine();

      List<Student> students = new List<Student> { student };
      Console.WriteLine("+++ Students:");
      // TODO: Needs IReadOnlyCollection
			// students.PrintAll(); 
			Console.WriteLine();

			// var updatedPeople = people.WithPasswordResetByFirstName("Tomász", "tweedledee");
			// Console.WriteLine("+++ Updated people:");
			// updatedPeople.PrintAll();
    }
  } 
}
#endregion
#region Immutable people - refactoring with With

namespace RefactoringRevisited {
  abstract class Person {
    public string Name { get; }
    public string Password {  get; }
    public Person(string name, string password) {
      this.Name = name;
      this.Password = password;
    }
    public Person WithPassword(string newPass) {
      return ClonePerson(this.Name, newPass);
    }
    public Person WithName(string newName) {
      return ClonePerson(newName, this.Password);
    }
    protected abstract Person ClonePerson(string newName, string newPassword);
  }
  abstract class ConcretePerson<T> : Person where T : Person {
    public ConcretePerson(string name, string password) : base(name, password) { }
    public new T WithName(string newName) {
      return (T)ClonePerson(newName, this.Password);
    }
    public new T WithPassword(string newPassword) {
      return (T)ClonePerson(this.Name, newPassword);
    }
  }

  class Student : ConcretePerson<Student> {
    public DateTime Enrolled {  get; }
    public Student(string name, string password, DateTime enrolled) 
      : base(name, password) { 
      this.Enrolled = enrolled;
    }
    protected override Student ClonePerson(string newName, string newPassword) {
      return new Student(newName, newPassword, this.Enrolled);
    }
  }

  static class Extensions {
    public static void PrintAll(this IReadOnlyCollection<Person> people) {
      foreach(var p in people) Console.WriteLine(p.Name);
    }
    public static List<Person> WithPasswordResetByFirstName 
      (this List<Person> people, string name, string newPassword) {
      var res = new List<Person>();
      foreach(var p in people) 
        if (p.Name == name ) res.Add(p.WithPassword(newPassword));
        else res.Add(p);
      return res;
    }
    /*
    public static List<T> WithPasswordResetByFirstName<T> 
      (this List<T> people, string name, string newPassword) where T:Person {
      var res = new List<T>();
      foreach(var p in people) 
        if (p.Name == name ) res.Add((T)p.WithPassword(newPassword));
        else res.Add(p);
      return res;
    }
    */
  }

  static class Demo {
    public static T Rename<T>(ConcretePerson<T> p) {
      return p.WithName(p.Name.Replace("š", "sz"));
    }
    public static void Run() {
      var student = new Student("Tomáš", "tweedledum", DateTime.Now);
      var renamed1 = Rename(student);
      var renamed2 = student.WithName(student.Name.Replace("š", "sz"));
      Console.WriteLine(renamed1.Enrolled);
      Console.WriteLine(renamed2.Enrolled);

		  List<Person> people = new List<Person> { student };
			Console.WriteLine("+++ People:");
			people.PrintAll();
			Console.WriteLine();

			var updatedPeople = people.WithPasswordResetByFirstName("Tomász", "tweedledee");
			Console.WriteLine("+++ Updated people:");
			updatedPeople.PrintAll();
    }
  } 
}
#endregion
#region Immutable people - builder

namespace Builder {
  class Person {
    public required string Name { get; init; }
    public required string Password {  get; init; }
  }


  class Student : Person {
    public required DateTime Enrolled {  get; init; }
  }

  abstract class PersonBuilder {
  	protected string name = "";
	  protected string password = "";
	  public PersonBuilder WithName(string name) {
		  this.name = name;
		  return this;
	  }
    public PersonBuilder WithPassword(string password) {
		  this.password = password;
		  return this;
	  }
  }

  class StudentBuilder : PersonBuilder {
	  private DateTime enrolled;
    public StudentBuilder WithDateEnrolled(DateTime enrolled) {
		  this.enrolled = enrolled;
	  	return this;
  	}
	  public Student Build() => 
      new Student { Enrolled = enrolled, Name = name, Password = password };
  }

  static class Demo {
    public static void Run() {
      var student = 
         new StudentBuilder()
          .WithDateEnrolled(DateTime.Now)
          .WithName("Tomáš")
          .WithPassword("tweedledum")
          //.Build()
          ;

      //Console.WriteLine(student.Name);
    }
  } 
}

#endregion
#region Immutable people - generic builder

namespace GenericBuilder {
  class Person {
    public required string Name { get; init; }
    public required string Password {  get; init; }
  }


  class Student : Person {
    public required DateTime Enrolled {  get; init; }
  }

  abstract class PersonBuilder<T> where T : PersonBuilder<T> {
  	protected string name = "";
	  protected string password = "";
	  public T WithName(string name) {
		  this.name = name;
		  return (T)this;
	  }
    public T WithPassword(string password) {
		  this.password = password;
		  return (T)this;
	  }
  }

  class StudentBuilder : PersonBuilder<StudentBuilder> {
	  private DateTime enrolled;
    public StudentBuilder WithDateEnrolled(DateTime enrolled) {
		  this.enrolled = enrolled;
	  	return this;
  	}
	  public Student Build() => new Student { Enrolled = enrolled, Name = name, Password = password };
  }

  static class Demo {
    public static void Run() {
      var student = 
         new StudentBuilder()
          .WithName("Tomáš")
          .WithDateEnrolled(DateTime.Now)
          .WithPassword("tweedledum")
          .Build();

      Console.WriteLine(student.Name);
    }
  } 
}

#endregion
