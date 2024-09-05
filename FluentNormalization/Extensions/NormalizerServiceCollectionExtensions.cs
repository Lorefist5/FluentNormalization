using FluentNormalization.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FluentNormalization.Extensions;


public static class NormalizerServiceCollectionExtensions
{
    public static IServiceCollection AddNormalizersFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var normalizerType = typeof(INormalizer<>);

        // Find all classes that implement INormalizer<T> in the given assembly
        var normalizers = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == normalizerType) && !t.IsAbstract && !t.IsInterface)
            .ToList();

        foreach (var normalizer in normalizers)
        {
            // Register the normalizer in the DI container
            var interfaces = normalizer.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == normalizerType);

            foreach (var @interface in interfaces)
            {
                services.AddTransient(@interface, normalizer);
            }
        }

        return services;
    }
}