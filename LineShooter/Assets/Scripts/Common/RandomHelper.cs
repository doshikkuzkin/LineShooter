using System;

public static class RandomHelper
{
    private static System.Random _random = new System.Random(Guid.NewGuid().GetHashCode());

    public static int GetRandomInt(int minValue, int maxValue)
    {
        return _random.Next(minValue, maxValue);
    }

    public static float GetRandomFloat(float minValue, float maxValue)
    {
        var range = maxValue - minValue;

        var sample = _random.NextDouble();
        var scaled = (sample * range) + minValue;

        return (float)scaled;
    }

    public static T GetRandomEnum<T>() where T : Enum
    {
        var enumValues = Enum.GetValues(typeof(T));
        var valuesCount = enumValues.Length;

        return (T) enumValues.GetValue(_random.Next(0, valuesCount));
    }
}
