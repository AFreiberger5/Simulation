using System;
using System.Globalization;

public static class Utils
{
    public static bool IsBitSet<T>(this T t, int pos) where T : struct, IConvertible
    {
        var value = t.ToInt32(CultureInfo.CurrentCulture);
        return (value & (1 << pos)) != 0;
    }

    public static int SetBit<T>(this T t, int pos) where T : struct, IConvertible
    {
        var value = t.ToInt32(CultureInfo.CurrentCulture);
        value = value | (1 << pos);
        return value;
    }
}
