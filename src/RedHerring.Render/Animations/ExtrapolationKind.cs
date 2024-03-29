namespace RedHerring.Render.Animations;

public enum ExtrapolationKind
{
    /// <summary>
    /// The value from the default node transformation is taken.
    /// </summary>
    Default,
    /// <summary>The nearest key value is used without interpolation.</summary>
    Constant,
    /// <summary>
    /// The value of the nearest two keys is linearly extrapolated for the current
    /// time value.
    /// </summary>
    Linear,
    /// <summary>
    /// The animation is repeated. If the animation key goes from n to m
    /// and the current time is t, use the value at (t - n ) % (|m-n|).
    /// </summary>
    Repeat,
}