namespace W4k.Either;

/// <summary>
/// Names of generated members, used by the source generator to determine whether a member should be skipped.
/// </summary>
public static class MemberNames
{
    /// <summary>
    /// Name of the <c>Case</c> property.
    /// </summary>
    public static readonly string Case = "Case";

    /// <summary>
    /// Name of the <c>Bind</c> method.
    /// </summary>
    public static readonly string Bind = "Bind";

    /// <summary>
    /// Name of the <c>Map</c> method.
    /// </summary>
    public static readonly string Map = "Map";

    /// <summary>
    /// Name of the <c>TryPick</c> method.
    /// </summary>
    public static readonly string TryPick = "TryPick";

    /// <summary>
    /// Name pattern to match all <c>Match</c> and <c>MatchAsync</c> methods.
    /// </summary>
    public static readonly string MatchAll = "Match*";

    /// <summary>
    /// Name of the <c>Match</c> method.
    /// </summary>
    public static readonly string Match = "Match";

    /// <summary>
    /// Name of the <c>Match</c> method override with provided state.
    /// </summary>
    public static readonly string MatchWithState = "Match<TState>";

    /// <summary>
    /// Name of the <c>MatchAsync</c> method.
    /// </summary>
    public static readonly string MatchAsync = "MatchAsync";

    /// <summary>
    /// Name of the <c>MatchAsync</c> method override with provided state.
    /// </summary>
    public static readonly string MatchAsyncWithState = "MatchAsync<TState>";

    /// <summary>
    /// Name pattern to match all <c>Switch</c> and <c>SwitchAsync</c> methods.
    /// </summary>
    public static readonly string SwitchAll = "Switch*";

    /// <summary>
    /// Name of the <c>Switch</c> method.
    /// </summary>
    public static readonly string Switch = "Switch";

    /// <summary>
    /// Name of the <c>Switch</c> method override with provided state.
    /// </summary>
    public static readonly string SwitchWithState = "Switch<TState>";

    /// <summary>
    /// Name of the <c>SwitchAsync</c> method.
    /// </summary>
    public static readonly string SwitchAsync = "SwitchAsync";

    /// <summary>
    /// Name of the <c>SwitchAsync</c> method override with provided state.
    /// </summary>
    public static readonly string SwitchAsyncWithState = "SwitchAsync<TState>";
}