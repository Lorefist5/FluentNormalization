using FluentNormalization.Abstraction;
using FluentNormalization.ConsoleTest.Models;
using FluentNormalization.ConsoleTest.Normalizers;
using FluentNormalization.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

var services = new ServiceCollection();

// Add all normalizers from the current assembly to the DI container
services.AddNormalizersFromAssembly(Assembly.GetExecutingAssembly());

var provider = services.BuildServiceProvider();

// Resolve normalizers from the DI container
var personNormalizer = provider.GetRequiredService<INormalizer<Person>>();


var people = new List<Person>
{
    new Person { FirstName = " Jürgen ", LastName = "Smith " },         // Case 1: Basic lowercasing and whitespace removal
    new Person { FirstName = "Élodie ", LastName = "García " },         // Case 2: Handling accented characters
    new Person { FirstName = " Mr. John ", LastName = "Dr. Emily " },   // Case 3: Remove prefixes
    new Person { FirstName = " John ", LastName = "Smith " },           // Case 4: Replace words or patterns
    new Person { FirstName = "mr. john", LastName = "smith" },          // Case 5: Title Case
    new Person { FirstName = "John!", LastName = "Emily..." }           // Case 6: Trim punctuation
};


foreach (var person in people)
{
    var normalizedPerson = personNormalizer.Normalize(person);
    Console.WriteLine($"{normalizedPerson.FirstName} {normalizedPerson.LastName}");
}