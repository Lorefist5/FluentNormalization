namespace FluentNormalization.Abstraction;

public interface INormalizer<T>
{
    T Normalize(T instance);
}
