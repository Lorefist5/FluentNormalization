using FluentNormalization.Abstraction;
using FluentNormalization.ConsoleTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentNormalization.ConsoleTest.Normalizers;

public class PersonNormalizer : AbstractNormalizer<Person>
{
    public PersonNormalizer()
    {
        // Normalize FirstName
        RuleFor(x => x.FirstName)
            .ToLower()                             // Convert to lowercase
            .RemoveWhitespace()                    // Remove all whitespace
            .NormalizeDiacritics()                 // Normalize diacritics (e.g., "é" -> "e")
            .RemovePrefix(new List<string> { "mr.", "dr." })  // Remove titles like "Mr." or "Dr."
            .ToTitleCase()                         // Capitalize the first letter of each word
            .TrimPunctuation()                     // Remove punctuation
            .Replace(new Dictionary<string, string> { { "john", "Jonathan" } });  // Replace "john" with "Jonathan"

        // Normalize LastName
        RuleFor(x => x.LastName)
            .ToLower()
            .RemovePrefix(new List<string> { "mr.", "dr." })
            .RemoveWhitespace()
            .NormalizeDiacritics()
            .TrimPunctuation()
            .ToTitleCase();
    }
}
