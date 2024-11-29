using System;

namespace W4k.Either;

/// <summary>
/// Members of the <c>Either</c> type that can be generated.
/// </summary>
[Flags]
public enum Members
{
    /// <summary>
    /// All members will be generated.
    /// </summary>
    All = Case | TryPick | BindAll | MapAll | MatchAll | SwitchAll,

    /// <summary>
    /// <c>Case</c> property will be generated.
    /// </summary>
    Case = 1 << 0,

    /// <summary>
    /// <c>TryPick</c> method will be generated.
    /// </summary>
    TryPick = 1 << 1,

    /// <summary>
    /// <c>Bind</c> method will be generated.
    /// </summary>
    Bind = 1 << 2,

    /// <summary>
    /// <b>Bind&lt;TState&gt;</b> method will be generated.
    /// </summary>
    BindWithState = 1 << 3,

    /// <summary>
    /// Both <c>Bind</c> and <b>Bind&lt;TState&gt;</b> methods will be generated.
    /// </summary>
    BindAll = Bind | BindWithState,

    /// <summary>
    /// <c>Map</c> method will be generated.
    /// </summary>
    Map = 1 << 4,

    /// <summary>
    /// <c>Map&lt;TState&gt;</c> method will be generated.
    /// </summary>
    MapWithState = 1 << 5,

    /// <summary>
    /// Both <c>Map</c> and <c>Map&lt;TState&gt;</c> methods will be generated.
    /// </summary>
    MapAll = Map | MapWithState,

    /// <summary>
    /// <c>Match</c> method will be generated.
    /// </summary>
    Match = 1 << 6,

    /// <summary>
    /// <c>Match&lt;TState&gt;</c> method will be generated.
    /// </summary>
    MatchWithState = 1 << 7,

    /// <summary>
    /// <c>MatchAsync</c> method will be generated.
    /// </summary>
    MatchAsync = 1 << 8,

    /// <summary>
    /// <c>MatchAsync&lt;TState&gt;</c> method will be generated.
    /// </summary>
    MatchAsyncWithState = 1 << 9,

    /// <summary>
    /// All match methods will be generated.
    /// </summary>
    MatchAll = Match | MatchWithState | MatchAsync | MatchAsyncWithState,

    /// <summary>
    /// <c>Switch</c> method will be generated.
    /// </summary>
    /// <remarks>
    /// <c>Switch</c> implies <c>Match</c> method will be generated as well.
    /// </remarks>
    Switch = 1 << 10 | Match,

    /// <summary>
    /// <c>Switch&lt;TState&gt;</c> method will be generated.
    /// </summary>
    /// <remarks>
    /// <c>Switch&lt;TState&gt;</c> implies <c>Match&lt;TState&gt;</c> method will be generated as well.
    /// </remarks>
    SwitchWithState = 1 << 11 | MatchWithState,

    /// <summary>
    /// <c>SwitchAsync</c> method will be generated.
    /// </summary>
    /// <remarks>
    /// <c>SwitchAsync</c> implies <c>MatchAsync</c> method will be generated as well.
    /// </remarks>
    SwitchAsync = 1 << 12 | MatchAsync,

    /// <summary>
    /// <c>SwitchAsync&lt;TState&gt;</c> method will be generated.
    /// </summary>
    /// <remarks>
    /// <c>SwitchAsync&lt;TState&gt;</c> implies <c>MatchAsync&lt;TState&gt;</c> method will be generated as well.
    /// </remarks>
    SwitchAsyncWithState = 11 << 13 | MatchAsyncWithState,

    /// <summary>
    /// All switch methods will be generated.
    /// </summary>
    /// <remarks>
    /// All match methods will be generated as well.
    /// </remarks>
    SwitchAll = Switch | SwitchWithState | SwitchAsync | SwitchAsyncWithState,
}

/// <summary>
/// Extensions for the <see cref="Members"/> enum.
/// </summary>
public static class MembersExtensions
{
    /// <summary>
    /// Checks if the <paramref name="value"/> contains the <paramref name="member"/>.
    /// </summary>
    /// <param name="value">Value to be examined.</param>
    /// <param name="member">Desired flag value.</param>
    /// <returns>
    /// Returns <see langword="true"/> if the <paramref name="value"/> contains the <paramref name="member"/> flag.
    /// </returns>
    public static bool ShouldGenerate(this Members value, Members member) =>
        value == 0 || (value & member) == member;
}