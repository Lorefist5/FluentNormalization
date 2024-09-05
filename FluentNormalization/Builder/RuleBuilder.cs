using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FluentNormalization.Builder;
public class RuleBuilder<T, TProperty>
{
    private readonly Expression<Func<T, TProperty>> _propertySelector;
    private readonly List<Func<TProperty, TProperty>> _normalizationActions = new();
    private readonly PropertyInfo _propertyInfo;

    public RuleBuilder(Expression<Func<T, TProperty>> propertySelector)
    {
        _propertySelector = propertySelector;
        _propertyInfo = GetPropertyInfo(_propertySelector);
    }

    // Generic Custom method for all normalizations
    public RuleBuilder<T, TProperty> Custom(Func<TProperty, TProperty> customNormalization)
    {
        _normalizationActions.Add(customNormalization);
        return this;
    }

    // Use Custom to implement ToLower
    public RuleBuilder<T, TProperty> ToLower()
    {
        return Custom(value =>
        {
            if (value is string strValue)
            {
                return (TProperty)(object)strValue.ToLowerInvariant();
            }
            return value;
        });
    }

    // Use Custom to implement RemoveWhitespace
    public RuleBuilder<T, TProperty> RemoveWhitespace()
    {
        return Custom(value =>
        {
            if (value is string strValue)
            {
                return (TProperty)(object)string.Concat(strValue.Where(c => !char.IsWhiteSpace(c)));
            }
            return value;
        });
    }

    // Use Custom to implement NormalizeDiacritics
    public RuleBuilder<T, TProperty> NormalizeDiacritics()
    {
        return Custom(value =>
        {
            if (value is string strValue)
            {
                var normalizedString = strValue.Normalize(NormalizationForm.FormD);
                var stringBuilder = new System.Text.StringBuilder();

                foreach (var c in normalizedString)
                {
                    var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                    if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                    {
                        stringBuilder.Append(c);
                    }
                }
                return (TProperty)(object)stringBuilder.ToString().Normalize(NormalizationForm.FormC);
            }
            return value;
        });
    }
    public RuleBuilder<T, TProperty> ToTitleCase()
    {
        return Custom(value =>
        {
            if (value is string strValue)
            {
                var textInfo = CultureInfo.CurrentCulture.TextInfo;
                return (TProperty)(object)textInfo.ToTitleCase(strValue.ToLowerInvariant());
            }
            return value;
        });
    }

    public RuleBuilder<T, TProperty> RemovePrefix(IEnumerable<string> prefixes)
    {
        return Custom(value =>
        {
            if (value is string strValue)
            {
                foreach (var prefix in prefixes)
                {
                    if (strValue.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    {
                        return (TProperty)(object)strValue.Substring(prefix.Length).Trim();
                    }
                }
            }
            return value;
        });
    }
    public RuleBuilder<T, TProperty> Trim()
    {
        return Custom(value =>
        {
            if (value is string strValue)
            {
                return (TProperty)(object)strValue.Trim();
            }
            return value;
        });
    }

    public RuleBuilder<T, TProperty> ToUpper()
    {
        return Custom(value =>
        {
            if (value is string strValue)
            {
                return (TProperty)(object)strValue.ToUpperInvariant();
            }
            return value;
        });
    }

    public RuleBuilder<T, TProperty> TrimPunctuation()
    {
        return Custom(value =>
        {
            if (value is string strValue)
            {
                return (TProperty)(object)new string(strValue.Where(c => !char.IsPunctuation(c)).ToArray());
            }
            return value;
        });
    }

    public RuleBuilder<T, TProperty> ReplaceAccentedCharacters()
    {
        return Custom(value =>
        {
            if (value is string strValue)
            {
                var normalizedString = strValue.Normalize(NormalizationForm.FormD);
                return (TProperty)(object)new string(normalizedString
                    .Where(c => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark)
                    .ToArray());
            }
            return value;
        });
    }

    public RuleBuilder<T, TProperty> CollapseSpaces()
    {
        return Custom(value =>
        {
            if (value is string strValue)
            {
                return (TProperty)(object)System.Text.RegularExpressions.Regex.Replace(strValue, @"\s+", " ");
            }
            return value;
        });
    }

    public RuleBuilder<T, TProperty> LimitLength(int maxLength)
    {
        return Custom(value =>
        {
            if (value is string strValue)
            {
                return (TProperty)(object)(strValue.Length > maxLength ? strValue.Substring(0, maxLength) : strValue);
            }
            return value;
        });
    }
    public RuleBuilder<T, TProperty> RemoveWords(IEnumerable<string> words)
    {
        return Custom(value =>
        {
            if (value is string strValue)
            {
                foreach (var word in words)
                {
                    strValue = strValue.Replace(word, "", StringComparison.OrdinalIgnoreCase).Trim();
                }
                return (TProperty)(object)strValue;
            }
            return value;
        });
    }

    public RuleBuilder<T, TProperty> Replace(Dictionary<string, string> replacements)
    {
        return Custom(value =>
        {
            if (value is string strValue)
            {
                foreach (var kvp in replacements)
                {
                    strValue = strValue.Replace(kvp.Key, kvp.Value, StringComparison.OrdinalIgnoreCase);
                }
                return (TProperty)(object)strValue;
            }
            return value;
        });
    }
    public Action<T> Build()
    {
        return instance =>
        {
            if (_propertyInfo == null)
            {
                throw new InvalidOperationException("Unable to find property.");
            }

            // Get the current value of the property
            var propertyValue = _propertySelector.Compile()(instance);

            // Apply normalization actions
            foreach (var action in _normalizationActions)
            {
                propertyValue = action(propertyValue);
            }

            // Set the normalized value back to the original entity
            _propertyInfo.SetValue(instance, propertyValue);
        };
    }

    // Method to extract PropertyInfo from the lambda expression
    private PropertyInfo GetPropertyInfo(Expression<Func<T, TProperty>> propertyLambda)
    {
        if (propertyLambda.Body is MemberExpression member)
        {
            if (member.Member is PropertyInfo propertyInfo)
            {
                return propertyInfo;
            }
        }
        throw new ArgumentException("Invalid property selector.");
    }
}
