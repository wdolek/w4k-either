using System;

namespace W4k.Either;

[Flags]
public enum Members
{
    All = 0b_0,

    Case = 0b_0000_0000_0000_0000_0001,
    Bind = 0b_0000_0000_0000_0000_0010,
    Map = 0b_0000_0000_0000_0000_0100,
    TryPick = 0b_0000_0000_0000_0000_1000,

    Match = 0b_0000_0000_0000_0001_0000,
    MatchWithState = 0b_0000_0000_0000_0010_0000,
    MatchAsync = 0b_0000_0000_0000_0100_0000,
    MatchAsyncWithState = 0b_0000_0000_0000_1000_0000,
    MatchAll = Match | MatchWithState | MatchAsync | MatchAsyncWithState,

    Switch = 0b_0000_0000_0001_0000_0000 | Match,
    SwitchWithState = 0b_0000_0000_0010_0000_0000 | MatchWithState,
    SwitchAsync = 0b_0000_0000_0100_0000_0000 | MatchAsync,
    SwitchAsyncWithState = 0b_0000_0000_1000_0000_0000 | MatchAsyncWithState,
    SwitchAll = Switch | SwitchWithState | SwitchAsync | SwitchAsyncWithState,
}