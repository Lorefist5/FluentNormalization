using FluentNormalization.Builder;
using System.Linq.Expressions;

namespace FluentNormalization.Abstraction;

public abstract class AbstractNormalizer<T> : INormalizer<T>
{
    private readonly List<Action<T>> _rules = new();

    public RuleBuilder<T, TProperty> RuleFor<TProperty>(Expression<Func<T, TProperty>> propertySelector)
    {
        var ruleBuilder = new RuleBuilder<T, TProperty>(propertySelector);
        _rules.Add(ruleBuilder.Build());
        return ruleBuilder;
    }

    public T Normalize(T instance)
    {
        foreach (var rule in _rules)
        {
            rule(instance);
        }
        return instance;
    }
}
