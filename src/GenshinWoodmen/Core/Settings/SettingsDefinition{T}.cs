using System;

namespace GenshinWoodmen.Core;

public class SettingsDefinition<T>
{
    internal static SettingsCache Cache => SettingsManager.Cache;

    public string Name { get; }
    public T DefaultValue { get; }
    public Func<object, T>? Converter { get; }

    public SettingsDefinition(string name, T defaultValue, Func<object, T>? converter = null)
    {
        Name = name;
        DefaultValue = defaultValue;
        Converter = converter ?? DefaultConverter!;
    }

    public static T? DefaultConverter(object value)
    {
        if (value is null) return default;
        try
        {
            return SettingsSerializer.DeserializeObject<T>(SettingsSerializer.SerializeObject(value));
        }
        catch
        {
            try
            {
                return (T?)typeof(T).Assembly.CreateInstance(typeof(T).FullName!);
            }
            catch
            {
                return default;
            }
        }
    }

    public T Get()
    {
        return Cache.Get(this);
    }

    public void Set(T value)
    {
        Cache.Set(this, value);
    }

    public static implicit operator T(SettingsDefinition<T> self)
    {
        return self.Get();
    }

    public void Relay()
    {
        Cache.Set(this, Get());
    }
}
