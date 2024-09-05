
PersonNormalizer README
=======================

This project demonstrates how to use a custom normalizer for cleaning up and transforming string properties in a class. The normalizer provides flexible, chainable normalization rules that can be applied to properties of the `Person` class.

Table of Contents
-----------------

*   [Overview](#overview)
*   [PersonNormalizer](#personnormalizer)
*   [Normalization Rules](#normalization-rules)
*   [Examples](#examples)

Overview
--------

The project includes two main components:

*   **AbstractNormalizer<T>**: This class serves as the base for creating custom normalizers for any class. It allows you to apply a series of normalization rules to properties of type `T`.
*   **PersonNormalizer**: This is a concrete implementation of `AbstractNormalizer<Person>` designed to clean up and normalize the `FirstName` and `LastName` properties of a `Person` object.

PersonNormalizer
----------------

The `PersonNormalizer` applies a series of predefined rules to the `FirstName` and `LastName` properties. You can easily modify or extend these rules to fit your specific needs.

### Default Rules for `FirstName`

*   Convert the name to lowercase.
*   Remove all whitespace in the name.
*   Normalize diacritics (e.g., "é" becomes "e").
*   Remove certain prefixes like "Mr.", "Dr.".
*   Capitalize the first letter of each word (title case).
*   Trim any punctuation marks.
*   Replace specific words (e.g., "John" becomes "Jonathan").

### Default Rules for `LastName`

*   Convert the last name to lowercase.
*   Remove all whitespace.
*   Normalize diacritics.
*   Trim any punctuation marks.
*   Capitalize the first letter of each word.

Normalization Rules
-------------------

The following normalization rules are implemented and can be applied to any property in the normalizer:

*   **ToLower()**: Converts the string to lowercase.
*   **RemoveWhitespace()**: Removes all whitespace in the string.
*   **NormalizeDiacritics()**: Replaces accented characters (e.g., "é" becomes "e").
*   **ToTitleCase()**: Capitalizes the first letter of each word in the string.
*   **RemovePrefix(IEnumerable<string>)**: Removes specified prefixes from the string.
*   **RemoveWords(IEnumerable<string>)**: Removes specified words from anywhere in the string.
*   **TrimPunctuation()**: Removes punctuation from the string.
*   **Replace(Dictionary<string, string>)**: Replaces occurrences of keys in the string with their corresponding values.

Examples
--------

### Example 1: Basic Normalization

    var person = new Person
    {
        FirstName = " Jürgen ",
        LastName = "Smith "
    };
    
    var normalizer = new PersonNormalizer();
    var normalizedPerson = normalizer.Normalize(person);
    
    Console.WriteLine(normalizedPerson.FirstName); // Output: jurgen
    Console.WriteLine(normalizedPerson.LastName);  // Output: smith
    

**Output:**  
FirstName: jurgen  
LastName: smith

### Example 2: Normalizing Accented Characters

    var person = new Person
    {
        FirstName = " Élodie ",
        LastName = " García "
    };
    
    var normalizer = new PersonNormalizer();
    var normalizedPerson = normalizer.Normalize(person);
    
    Console.WriteLine(normalizedPerson.FirstName); // Output: elodie
    Console.WriteLine(normalizedPerson.LastName);  // Output: garcia
    

**Output:**  
FirstName: elodie  
LastName: garcia

### Example 3: Replacing Words and Removing Prefixes

    var person = new Person
    {
        FirstName = " Mr. John ",
        LastName = " Dr. Emily "
    };
    
    var normalizer = new PersonNormalizer();
    var normalizedPerson = normalizer.Normalize(person);
    
    Console.WriteLine(normalizedPerson.FirstName); // Output: Jonathan
    Console.WriteLine(normalizedPerson.LastName);  // Output: Emily
    

**Output:**  
FirstName: Jonathan  
LastName: Emily

### Example 4: Trimming Punctuation

    var person = new Person
    {
        FirstName = "John!",
        LastName = "Emily..."
    };
    
    var normalizer = new PersonNormalizer();
    var normalizedPerson = normalizer.Normalize(person);
    
    Console.WriteLine(normalizedPerson.FirstName); // Output: John
    Console.WriteLine(normalizedPerson.LastName);  // Output: Emily
    

**Output:**  
FirstName: John  
LastName: Emily

Conclusion
----------

This normalization system provides a flexible and extendable way to clean and standardize string properties in your classes. You can easily add more normalization rules to fit specific needs, ensuring that all your data is consistent and properly formatted.