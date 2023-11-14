using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RedHerring.Numbers;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
public readonly struct HsvColor : IEquatable<HsvColor>
{
    public readonly float H;
    public readonly float S;
    public readonly float V;
    public readonly float A;

    #region Lifecycle

    public HsvColor(float hue, float saturation, float value, float alpha)
    {
        H = float.Clamp(hue, 0, 360f);
        S = float.Clamp(saturation, 0, 100f);
        V = float.Clamp(value, 0, 100f);
        A = float.Clamp(alpha, 0, 1);

        H = (H >= 360.0 - float.Epsilon ? 0f : H);
    }

    public HsvColor(float hue, float saturation, float value, float alpha, bool clamp)
    {
        if (clamp)
        {
            H = float.Clamp(hue, 0, 360f);
            S = float.Clamp(saturation, 0, 100f);
            V = float.Clamp(value, 0, 100f);
            A = float.Clamp(alpha, 0, 1);

            H = (H >= 360.0 - float.Epsilon ? 0f : H);
        }
        else
        {
            H = hue;
            S = saturation;
            V = value;
            A = alpha;
        }
    }

    public HsvColor(Color4 color)
    {
        var hsv = FromRgba(color.R, color.G, color.B, color.A);

        H = hsv.H;
        S = hsv.S;
        V = hsv.V;
        A = hsv.A;
    }

    #endregion Lifecycle

    #region Equality

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(HsvColor left, HsvColor right)
    {
        return left.H == right.H && left.S == right.S && left.V == right.V && left.A == right.A;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(HsvColor left, HsvColor right)
    {
        return !(left == right);
    }

    public bool Equals(HsvColor other)
    {
        return H.Equals(other.H) && S.Equals(other.S) && V.Equals(other.V) && A.Equals(other.A);
    }

    public override bool Equals(object? obj) => obj is HsvColor other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(H, S, V, A);

    #endregion Equality

    #region Queries

    public void Deconstruct(out float hue, out float saturation, out float value, out float alpha)
    {
        hue = H;
        saturation = S;
        value = V;
        alpha = A;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public HsvColor WithHue(float hue) => new(hue, S, V, A);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public HsvColor WithSaturation(float saturation) => new(H, saturation, V, A);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public HsvColor WithValue(float value) => new(H, S, value, A);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public HsvColor WithAlpha(float alpha) => new(H, S, V, alpha);

    public Vector3 ToVector3() => new Vector3(H, S, V);

    public Vector4 ToVector4() => new Vector4(H, S, V, A);

    public Color4 ToColor4() => ToRgba(H, S, V, A);

    public static Color4 ToRgba(float hue, float saturation, float value, float alpha = 1.0f)
    {
        // Note: Conversion code is originally based on the C++ in WinUI (licensed MIT)
        // https://github.com/microsoft/microsoft-ui-xaml/blob/main/dev/Common/ColorConversion.cpp
        // This was used because it is the best documented and likely most optimized for performance
        // Alpha support was added

        // We want the hue to be between 0 and 359,
        // so we first ensure that that's the case.
        while (hue >= 360.0f)
        {
            hue -= 360.0f;
        }

        while (hue < 0.0f)
        {
            hue += 360.0f;
        }

        // We similarly clamp saturation, value and alpha between 0 and 1.
        saturation = saturation < 0.0f ? 0.0f : saturation;
        saturation = saturation > 100.0f ? 100.0f : saturation;

        value = value < 0.0f ? 0.0f : value;
        value = value > 100.0f ? 100.0f : value;

        alpha = alpha < 0.0f ? 0.0f : alpha;
        alpha = alpha > 1.0f ? 1.0f : alpha;

        // The first thing that we need to do is to determine the chroma (see above for its definition).
        // Remember from above that:
        //
        // 1. The chroma is the difference between the maximum and the minimum of the RGB channels,
        // 2. The value is the maximum of the RGB channels, and
        // 3. The saturation comes from dividing the chroma by the maximum of the RGB channels (i.e., the value).
        //
        // From these facts, you can see that we can retrieve the chroma by simply multiplying the saturation and the value,
        // and we can retrieve the minimum of the RGB channels by subtracting the chroma from the value.
        float chroma = saturation * value;
        float min = value - chroma;

        // If the chroma is zero, then we have a greyscale color.  In that case, the maximum and the minimum RGB channels
        // have the same value (and, indeed, all of the RGB channels are the same), so we can just immediately return
        // the minimum value as the value of all the channels.
        if (chroma == 0)
        {
            return new Color4(min, min, min, alpha);
        }

        // If the chroma is not zero, then we need to continue.  The first step is to figure out
        // what section of the color wheel we're located in.  In order to do that, we'll divide the hue by 60.
        // The resulting value means we're in one of the following locations:
        //
        // 0 - Between red and yellow.
        // 1 - Between yellow and green.
        // 2 - Between green and cyan.
        // 3 - Between cyan and blue.
        // 4 - Between blue and purple.
        // 5 - Between purple and red.
        //
        // In each of these sextants, one of the RGB channels is completely present, one is partially present, and one is not present.
        // For example, as we transition between red and yellow, red is completely present, green is becoming increasingly present, and blue is not present.
        // Then, as we transition from yellow and green, green is now completely present, red is becoming decreasingly present, and blue is still not present.
        // As we transition from green to cyan, green is still completely present, blue is becoming increasingly present, and red is no longer present.  And so on.
        //
        // To convert from hue to RGB value, we first need to figure out which of the three channels is in which configuration
        // in the sextant that we're located in.  Next, we figure out what value the completely-present color should have.
        // We know that chroma = (max - min), and we know that this color is the max color, so to find its value we simply add
        // min to chroma to retrieve max.  Finally, we consider how far we've transitioned from the pure form of that color
        // to the next color (e.g., how far we are from pure red towards yellow), and give a value to the partially present channel
        // equal to the minimum plus the chroma (i.e., the max minus the min), multiplied by the percentage towards the new color.
        // This gets us a value between the maximum and the minimum representing the partially present channel.
        // Finally, the not-present color must be equal to the minimum value, since it is the one least participating in the overall color.
        int sextant = (int)(hue / 60);
        float intermediateColorPercentage = (hue / 60) - sextant;
        float max = chroma + min;

        float r = 0;
        float g = 0;
        float b = 0;

        switch (sextant)
        {
            case 0:
                r = max;
                g = min + (chroma * intermediateColorPercentage);
                b = min;
                break;
            case 1:
                r = min + (chroma * (1 - intermediateColorPercentage));
                g = max;
                b = min;
                break;
            case 2:
                r = min;
                g = max;
                b = min + (chroma * intermediateColorPercentage);
                break;
            case 3:
                r = min;
                g = min + (chroma * (1 - intermediateColorPercentage));
                b = max;
                break;
            case 4:
                r = min + (chroma * intermediateColorPercentage);
                g = min;
                b = max;
                break;
            case 5:
                r = max;
                g = min;
                b = min + (chroma * (1 - intermediateColorPercentage));
                break;
        }

        return new Color4(r, g, b, alpha);
    }

    public static HsvColor FromRgba(float r, float g, float b, float a = 1.0f)
    {
        // Note: Conversion code is originally based on the C++ in WinUI (licensed MIT)
        // https://github.com/microsoft/microsoft-ui-xaml/blob/main/dev/Common/ColorConversion.cpp
        // This was used because it is the best documented and likely most optimized for performance
        // Alpha support was added

        float hue;
        float saturation;
        float value;

        float max = r >= g ? (r >= b ? r : b) : (g >= b ? g : b);
        float min = r <= g ? (r <= b ? r : b) : (g <= b ? g : b);

        // The value, a number between 0 and 1, is the largest of R, G, and B (divided by 255).
        // Conceptually speaking, it represents how much color is present.
        // If at least one of R, G, B is 255, then there exists as much color as there can be.
        // If RGB = (0, 0, 0), then there exists no color at all - a value of zero corresponds
        // to black (i.e., the absence of any color).
        value = max;

        // The "chroma" of the color is a value directly proportional to the extent to which
        // the color diverges from greyscale.  If, for example, we have RGB = (255, 255, 0),
        // then the chroma is maximized - this is a pure yellow, no gray of any kind.
        // On the other hand, if we have RGB = (128, 128, 128), then the chroma being zero
        // implies that this color is pure greyscale, with no actual hue to be found.
        float chroma = max - min;

        // If the chrome is zero, then hue is technically undefined - a greyscale color
        // has no hue.  For the sake of convenience, we'll just set hue to zero, since
        // it will be unused in this circumstance.  Since the color is purely gray,
        // saturation is also equal to zero - you can think of saturation as basically
        // a measure of hue intensity, such that no hue at all corresponds to a
        // nonexistent intensity.
        if (chroma == 0f)
        {
            hue = 0.0f;
            saturation = 0.0f;
        }
        else
        {
            // In this block, hue is properly defined, so we'll extract both hue
            // and saturation information from the RGB color.

            // Hue can be thought of as a cyclical thing, between 0 degrees and 360 degrees.
            // A hue of 0 degrees is red; 120 degrees is green; 240 degrees is blue; and 360 is back to red.
            // Every other hue is somewhere between either red and green, green and blue, and blue and red,
            // so every other hue can be thought of as an angle on this color wheel.
            // These if/else statements determines where on this color wheel our color lies.
            if (r >= max - float.Epsilon)
            {
                // If the red channel is the most pronounced channel, then we exist
                // somewhere between (-60, 60) on the color wheel - i.e., the section around 0 degrees
                // where red dominates.  We figure out where in that section we are exactly
                // by considering whether the green or the blue channel is greater - by subtracting green from blue,
                // then if green is greater, we'll nudge ourselves closer to 60, whereas if blue is greater, then
                // we'll nudge ourselves closer to -60.  We then divide by chroma (which will actually make the result larger,
                // since chroma is a value between 0 and 1) to normalize the value to ensure that we get the right hue
                // even if we're very close to greyscale.
                hue = 60 * (g - b) / chroma;
            }
            else if (g >= max - float.Epsilon)
            {
                // We do the exact same for the case where the green channel is the most pronounced channel,
                // only this time we want to see if we should tilt towards the blue direction or the red direction.
                // We add 120 to center our value in the green third of the color wheel.
                hue = 120 + (60 * (b - r) / chroma);
            }
            else // blue == max
            {
                // And we also do the exact same for the case where the blue channel is the most pronounced channel,
                // only this time we want to see if we should tilt towards the red direction or the green direction.
                // We add 240 to center our value in the blue third of the color wheel.
                hue = 240 + (60 * (r - g) / chroma);
            }

            // Since we want to work within the range [0, 360), we'll add 360 to any value less than zero -
            // this will bump red values from within -60 to -1 to 300 to 359.  The hue is the same at both values.
            if (hue < 0.0)
            {
                hue += 360.0f;
            }

            // The saturation, our final HSV axis, can be thought of as a value between 0 and 1 indicating how intense our color is.
            // To find it, we divide the chroma - the distance between the minimum and the maximum RGB channels - by the maximum channel (i.e., the value).
            // This effectively normalizes the chroma - if the maximum is 0.5 and the minimum is 0, the saturation will be (0.5 - 0) / 0.5 = 1,
            // meaning that although this color is not as bright as it can be, the dark color is as intense as it possibly could be.
            // If, on the other hand, the maximum is 0.5 and the minimum is 0.25, then the saturation will be (0.5 - 0.25) / 0.5 = 0.5,
            // meaning that this color is partially washed out.
            // A saturation value of 0 corresponds to a greyscale color, one in which the color is *completely* washed out and there is no actual hue.
            saturation = chroma / value;
        }

        return new HsvColor(hue, saturation * 100f, value * 100f, a, false);
    }

    public override string ToString()
    {
        return ToString("G", CultureInfo.CurrentCulture);
    }

    public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format)
    {
        return ToString(format, CultureInfo.CurrentCulture);
    }

    public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format,
        IFormatProvider? formatProvider)
    {
        string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

        return
            $"<{H.ToString(format, formatProvider)}{separator} {S.ToString(format, formatProvider)}{separator} {V.ToString(format, formatProvider)}{separator} {A.ToString(format, formatProvider)}>";
    }

    #endregion Queries

    #region Operations

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HsvColor Lerp(HsvColor value1, HsvColor value2, float amount)
    {
        float inverseAmount = 1.0f - amount;
        float hue = value1.H * inverseAmount + value2.H * amount;
        float saturation = value1.S * inverseAmount + value2.S * amount;
        float value = value1.V * inverseAmount + value2.V * amount;
        float alpha = value1.A * inverseAmount + value2.A * amount;
        
        return new HsvColor(hue, saturation, value, alpha, false);
    }

    #endregion Operations
}