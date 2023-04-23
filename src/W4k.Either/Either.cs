﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using W4k.Either.Abstractions;

namespace W4k.Either;

[Either]
public readonly partial struct Either<TLeft, TRight>
    where TLeft : notnull
    where TRight : notnull
{
}

[Either]
public readonly partial struct Either<TLeft, TMiddle, TRight>
    where TLeft : notnull
    where TMiddle : notnull
    where TRight : notnull
{
}

[Either]
public readonly partial struct Either<T1, T2, T3, T4>
    where T1 : notnull
    where T2 : notnull
    where T3 : notnull
    where T4 : notnull
{
}

[Either]
public readonly partial struct Either<T1, T2, T3, T4, T5>
    where T1 : notnull
    where T2 : notnull
    where T3 : notnull
    where T4 : notnull
    where T5 : notnull
{
}

[Either]
public readonly partial struct Either<T1, T2, T3, T4, T5, T6>
    where T1 : notnull
    where T2 : notnull
    where T3 : notnull
    where T4 : notnull
    where T5 : notnull
    where T6 : notnull
{
}

[Either]
public readonly partial struct Either<T1, T2, T3, T4, T5, T6, T7>
    where T1 : notnull
    where T2 : notnull
    where T3 : notnull
    where T4 : notnull
    where T5 : notnull
    where T6 : notnull
    where T7 : notnull
{
}

[Either]
public readonly partial struct Either<T1, T2, T3, T4, T5, T6, T7, T8>
    where T1 : notnull
    where T2 : notnull
    where T3 : notnull
    where T4 : notnull
    where T5 : notnull
    where T6 : notnull
    where T7 : notnull
    where T8 : notnull
{
}

//[Either]
public partial struct MyEither<TAny, TNonNullRef, TNullRef, TStruct, TNotNull, TObj, TNullObj, TIFace, TNullIFace, TUnmanaged>
    where TNonNullRef : class // 2
    where TNullRef : class? // 3
    where TStruct : struct // 4
    where TNotNull : notnull // 5
    where TObj : LeObject
    where TNullObj : LeObject?
    where TIFace : IAmInterface
    where TNullIFace : IAmInterface?
    where TUnmanaged : unmanaged
{
}

public class LeObject
{
}

public interface IAmInterface
{
}

    [Serializable]
    readonly partial struct MyEither<TAny, TNonNullRef, TNullRef, TStruct, TNotNull, TObj, TNullObj, TIFace, TNullIFace, TUnmanaged> : IEquatable<MyEither<TAny, TNonNullRef, TNullRef, TStruct, TNotNull, TObj, TNullObj, TIFace, TNullIFace, TUnmanaged>>, ISerializable
    {
        private readonly byte _idx;
        private readonly TAny? _v1;
        private readonly TNonNullRef? _v2;
        private readonly TNullRef? _v3;
        private readonly TStruct _v4;
        private readonly TNotNull? _v5;
        private readonly TObj? _v6;
        private readonly TNullObj? _v7;
        private readonly TIFace? _v8;
        private readonly TNullIFace? _v9;
        private readonly TUnmanaged _v10;

        public MyEither(TAny value)
        {
            ThrowHelper.ThrowIfNull(value);
            _idx = 1;
            _v1 = value;
        }

        public MyEither(TNonNullRef value)
        {
            ThrowHelper.ThrowIfNull(value);
            _idx = 2;
            _v2 = value;
        }

        public MyEither(TNullRef? value)
        {
            _idx = 3;
            _v3 = value;
        }

        public MyEither(TStruct value)
        {
            _idx = 4;
            _v4 = value;
        }

        public MyEither(TNotNull value)
        {
            ThrowHelper.ThrowIfNull(value);
            _idx = 5;
            _v5 = value;
        }

        public MyEither(TObj value)
        {
            ThrowHelper.ThrowIfNull(value);
            _idx = 6;
            _v6 = value;
        }

        public MyEither(TNullObj value)
        {
            ThrowHelper.ThrowIfNull(value);
            _idx = 7;
            _v7 = value;
        }

        public MyEither(TIFace value)
        {
            ThrowHelper.ThrowIfNull(value);
            _idx = 8;
            _v8 = value;
        }

        public MyEither(TNullIFace value)
        {
            ThrowHelper.ThrowIfNull(value);
            _idx = 9;
            _v9 = value;
        }

        public MyEither(TUnmanaged value)
        {
            _idx = 10;
            _v10 = value;
        }

        private MyEither(SerializationInfo info, StreamingContext context)
        {
            _idx = info.GetByte(nameof(_idx));
            switch (_idx)
            {
                case 1:
                    _v1 = (TAny?)info.GetValue("_v1", typeof(TAny));
                    break;
                case 2:
                    _v2 = (TNonNullRef?)info.GetValue("_v2", typeof(TNonNullRef));
                    break;
                case 3:
                    _v3 = (TNullRef?)info.GetValue("_v3", typeof(TNullRef));
                    break;
                case 4:
                    _v4 = (TStruct)info.GetValue("_v4", typeof(TStruct))!;
                    break;
                case 5:
                    _v5 = (TNotNull?)info.GetValue("_v5", typeof(TNotNull));
                    break;
                case 6:
                    _v6 = (TObj?)info.GetValue("_v6", typeof(TObj));
                    break;
                case 7:
                    _v7 = (TNullObj?)info.GetValue("_v7", typeof(TNullObj));
                    break;
                case 8:
                    _v8 = (TIFace?)info.GetValue("_v8", typeof(TIFace));
                    break;
                case 9:
                    _v9 = (TNullIFace?)info.GetValue("_v9", typeof(TNullIFace));
                    break;
                case 10:
                    _v10 = (TUnmanaged)info.GetValue("_v10", typeof(TUnmanaged))!;
                    break;
                default:
                    ThrowHelper.ThrowOnInvalidState();
                    break;
            }
        }

        [Pure]
        public object? Case
        {
            get
            {
                switch (_idx)
                {
                    case 1:
                        return _v1;
                    case 2:
                        return _v2;
                    case 3:
                        return _v3;
                    case 4:
                        return _v4;
                    case 5:
                        return _v5;
                    case 6:
                        return _v6;
                    case 7:
                        return _v7;
                    case 8:
                        return _v8;
                    case 9:
                        return _v9;
                    case 10:
                        return _v10;
                    default:
                        return ThrowHelper.ThrowOnInvalidState<object?>();
                }
            }
        }

        [Pure]
        public static bool operator ==(MyEither<TAny, TNonNullRef, TNullRef, TStruct, TNotNull, TObj, TNullObj, TIFace, TNullIFace, TUnmanaged> left, MyEither<TAny, TNonNullRef, TNullRef, TStruct, TNotNull, TObj, TNullObj, TIFace, TNullIFace, TUnmanaged> right) => left.Equals(right);

        [Pure]
        public static bool operator !=(MyEither<TAny, TNonNullRef, TNullRef, TStruct, TNotNull, TObj, TNullObj, TIFace, TNullIFace, TUnmanaged> left, MyEither<TAny, TNonNullRef, TNullRef, TStruct, TNotNull, TObj, TNullObj, TIFace, TNullIFace, TUnmanaged> right) => !left.Equals(right);

        [Pure]
        public static implicit operator MyEither<TAny, TNonNullRef, TNullRef, TStruct, TNotNull, TObj, TNullObj, TIFace, TNullIFace, TUnmanaged>(TAny value) => new(value);

        [Pure]
        public static implicit operator MyEither<TAny, TNonNullRef, TNullRef, TStruct, TNotNull, TObj, TNullObj, TIFace, TNullIFace, TUnmanaged>(TNonNullRef value) => new(value);

        [Pure]
        public static implicit operator MyEither<TAny, TNonNullRef, TNullRef, TStruct, TNotNull, TObj, TNullObj, TIFace, TNullIFace, TUnmanaged>(TNullRef? value) => new(value);

        [Pure]
        public static implicit operator MyEither<TAny, TNonNullRef, TNullRef, TStruct, TNotNull, TObj, TNullObj, TIFace, TNullIFace, TUnmanaged>(TStruct value) => new(value);

        [Pure]
        public static implicit operator MyEither<TAny, TNonNullRef, TNullRef, TStruct, TNotNull, TObj, TNullObj, TIFace, TNullIFace, TUnmanaged>(TNotNull value) => new(value);

        [Pure]
        public static implicit operator MyEither<TAny, TNonNullRef, TNullRef, TStruct, TNotNull, TObj, TNullObj, TIFace, TNullIFace, TUnmanaged>(TObj value) => new(value);

        [Pure]
        public static implicit operator MyEither<TAny, TNonNullRef, TNullRef, TStruct, TNotNull, TObj, TNullObj, TIFace, TNullIFace, TUnmanaged>(TNullObj value) => new(value);

        [Pure]
        public static implicit operator MyEither<TAny, TNonNullRef, TNullRef, TStruct, TNotNull, TObj, TNullObj, TIFace, TNullIFace, TUnmanaged>(TIFace value) => new(value);

        [Pure]
        public static implicit operator MyEither<TAny, TNonNullRef, TNullRef, TStruct, TNotNull, TObj, TNullObj, TIFace, TNullIFace, TUnmanaged>(TNullIFace value) => new(value);

        [Pure]
        public static implicit operator MyEither<TAny, TNonNullRef, TNullRef, TStruct, TNotNull, TObj, TNullObj, TIFace, TNullIFace, TUnmanaged>(TUnmanaged value) => new(value);

        [Pure]
        public override int GetHashCode()
        {
            switch (_idx)
            {
                case 1:
                    return _v1!.GetHashCode();
                case 2:
                    return _v2!.GetHashCode();
                case 3:
                    return _v3?.GetHashCode() ?? 0;
                case 4:
                    return _v4.GetHashCode();
                case 5:
                    return _v5!.GetHashCode();
                case 6:
                    return _v6!.GetHashCode();
                case 7:
                    return _v7!.GetHashCode();
                case 8:
                    return _v8!.GetHashCode();
                case 9:
                    return _v9!.GetHashCode();
                case 10:
                    return _v10.GetHashCode();
                default:
                    return ThrowHelper.ThrowOnInvalidState<int>();
            }
        }

        [Pure]
        public override string ToString()
        {
            switch (_idx)
            {
                case 1:
                    return _v1!.ToString() ?? string.Empty;
                case 2:
                    return _v2!.ToString() ?? string.Empty;
                case 3:
                    return _v3?.ToString() ?? string.Empty;
                case 4:
                    return _v4.ToString() ?? string.Empty;
                case 5:
                    return _v5!.ToString() ?? string.Empty;
                case 6:
                    return _v6!.ToString() ?? string.Empty;
                case 7:
                    return _v7!.ToString() ?? string.Empty;
                case 8:
                    return _v8!.ToString() ?? string.Empty;
                case 9:
                    return _v9!.ToString() ?? string.Empty;
                case 10:
                    return _v10.ToString() ?? string.Empty;
                default:
                    return ThrowHelper.ThrowOnInvalidState<string>();
            }
        }

        [Pure]
        public override bool Equals(object? obj)
        {
            if (obj is not MyEither<TAny, TNonNullRef, TNullRef, TStruct, TNotNull, TObj, TNullObj, TIFace, TNullIFace, TUnmanaged> other)
            {
                return false;
            }

            return Equals(other);
        }

        [Pure]
        public bool Equals(MyEither<TAny, TNonNullRef, TNullRef, TStruct, TNotNull, TObj, TNullObj, TIFace, TNullIFace, TUnmanaged> other)
        {
            if (_idx != other._idx)
            {
                return false;
            }

            switch (_idx)
            {
                case 1:
                    return _v1!.Equals(other._v1);
                case 2:
                    return _v2!.Equals(other._v2);
                case 3:
                    return _v3?.Equals(other._v3) ?? false;
                case 4:
                    return _v4.Equals(other._v4);
                case 5:
                    return _v5!.Equals(other._v5);
                case 6:
                    return _v6!.Equals(other._v6);
                case 7:
                    return _v7!.Equals(other._v7);
                case 8:
                    return _v8!.Equals(other._v8);
                case 9:
                    return _v9!.Equals(other._v9);
                case 10:
                    return _v10.Equals(other._v10);
                default:
                    return ThrowHelper.ThrowOnInvalidState<bool>();
            }
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_idx", _idx);
            switch (_idx)
            {
                case 1:
                    info.AddValue("_v1", _v1);
                    break;
                case 2:
                    info.AddValue("_v2", _v2);
                    break;
                case 3:
                    info.AddValue("_v3", _v3);
                    break;
                case 4:
                    info.AddValue("_v4", _v4);
                    break;
                case 5:
                    info.AddValue("_v5", _v5);
                    break;
                case 6:
                    info.AddValue("_v6", _v6);
                    break;
                case 7:
                    info.AddValue("_v7", _v7);
                    break;
                case 8:
                    info.AddValue("_v8", _v8);
                    break;
                case 9:
                    info.AddValue("_v9", _v9);
                    break;
                case 10:
                    info.AddValue("_v10", _v10);
                    break;
                default:
                    ThrowHelper.ThrowOnInvalidState();
                    break;
            }
        }

        [Pure]
        public bool TryPick([NotNullWhen(true)] out TAny? value)
        {
            if (_idx == 1)
            {
                value = _v1!;
                return true;
            }

            value = default!;
            return false;
        }

        [Pure]
        public bool TryPick([NotNullWhen(true)] out TNonNullRef? value)
        {
            if (_idx == 2)
            {
                value = _v2!;
                return true;
            }

            value = default!;
            return false;
        }

        [Pure]
        public bool TryPick(out TNullRef? value)
        {
            if (_idx == 3)
            {
                value = _v3;
                return true;
            }

            value = default;
            return false;
        }

        [Pure]
        public bool TryPick(out TStruct value)
        {
            if (_idx == 4)
            {
                value = _v4;
                return true;
            }

            value = default;
            return false;
        }

        [Pure]
        public bool TryPick([NotNullWhen(true)] out TNotNull? value)
        {
            if (_idx == 5)
            {
                value = _v5!;
                return true;
            }

            value = default!;
            return false;
        }

        [Pure]
        public bool TryPick([NotNullWhen(true)] out TObj? value)
        {
            if (_idx == 6)
            {
                value = _v6!;
                return true;
            }

            value = default!;
            return false;
        }

        [Pure]
        public bool TryPick([NotNullWhen(true)] out TNullObj? value)
        {
            if (_idx == 7)
            {
                value = _v7!;
                return true;
            }

            value = default!;
            return false;
        }

        [Pure]
        public bool TryPick([NotNullWhen(true)] out TIFace? value)
        {
            if (_idx == 8)
            {
                value = _v8!;
                return true;
            }

            value = default!;
            return false;
        }

        [Pure]
        public bool TryPick([NotNullWhen(true)] out TNullIFace? value)
        {
            if (_idx == 9)
            {
                value = _v9!;
                return true;
            }

            value = default!;
            return false;
        }

        [Pure]
        public bool TryPick(out TUnmanaged value)
        {
            if (_idx == 10)
            {
                value = _v10;
                return true;
            }

            value = default;
            return false;
        }

        public TResult Match<TResult>(
            Func<TAny, TResult> f1,
            Func<TNonNullRef, TResult> f2,
            Func<TNullRef?, TResult> f3,
            Func<TStruct, TResult> f4,
            Func<TNotNull, TResult> f5,
            Func<TObj, TResult> f6,
            Func<TNullObj, TResult> f7,
            Func<TIFace, TResult> f8,
            Func<TNullIFace, TResult> f9,
            Func<TUnmanaged, TResult> f10)
        {
            ThrowHelper.ThrowIfNull(f1);
            ThrowHelper.ThrowIfNull(f2);
            ThrowHelper.ThrowIfNull(f3);
            ThrowHelper.ThrowIfNull(f4);
            ThrowHelper.ThrowIfNull(f5);
            ThrowHelper.ThrowIfNull(f6);
            ThrowHelper.ThrowIfNull(f7);
            ThrowHelper.ThrowIfNull(f8);
            ThrowHelper.ThrowIfNull(f9);
            ThrowHelper.ThrowIfNull(f10);

            switch(_idx)
            {
                case 1:
                    return f1(_v1!);
                case 2:
                    return f2(_v2!);
                case 3:
                    return f3(_v3);
                case 4:
                    return f4(_v4);
                case 5:
                    return f5(_v5!);
                case 6:
                    return f6(_v6!);
                case 7:
                    return f7(_v7!);
                case 8:
                    return f8(_v8!);
                case 9:
                    return f9(_v9!);
                case 10:
                    return f10(_v10);
                default:
                    return ThrowHelper.ThrowOnInvalidState<TResult>();
            }
        }

        public TResult Match<TState, TResult>(
            TState state,
            Func<TState, TAny, TResult> f1,
            Func<TState, TNonNullRef, TResult> f2,
            Func<TState, TNullRef?, TResult> f3,
            Func<TState, TStruct, TResult> f4,
            Func<TState, TNotNull, TResult> f5,
            Func<TState, TObj, TResult> f6,
            Func<TState, TNullObj, TResult> f7,
            Func<TState, TIFace, TResult> f8,
            Func<TState, TNullIFace, TResult> f9,
            Func<TState, TUnmanaged, TResult> f10)
        {
            ThrowHelper.ThrowIfNull(f1);
            ThrowHelper.ThrowIfNull(f2);
            ThrowHelper.ThrowIfNull(f3);
            ThrowHelper.ThrowIfNull(f4);
            ThrowHelper.ThrowIfNull(f5);
            ThrowHelper.ThrowIfNull(f6);
            ThrowHelper.ThrowIfNull(f7);
            ThrowHelper.ThrowIfNull(f8);
            ThrowHelper.ThrowIfNull(f9);
            ThrowHelper.ThrowIfNull(f10);

            switch(_idx)
            {
                case 1:
                    return f1(state, _v1!);
                case 2:
                    return f2(state, _v2!);
                case 3:
                    return f3(state, _v3);
                case 4:
                    return f4(state, _v4);
                case 5:
                    return f5(state, _v5!);
                case 6:
                    return f6(state, _v6!);
                case 7:
                    return f7(state, _v7!);
                case 8:
                    return f8(state, _v8!);
                case 9:
                    return f9(state, _v9!);
                case 10:
                    return f10(state, _v10);
                default:
                    return ThrowHelper.ThrowOnInvalidState<TResult>();
            }
        }

        public Task<TResult> MatchAsync<TResult>(
            Func<TAny, CancellationToken, Task<TResult>> f1,
            Func<TNonNullRef, CancellationToken, Task<TResult>> f2,
            Func<TNullRef?, CancellationToken, Task<TResult>> f3,
            Func<TStruct, CancellationToken, Task<TResult>> f4,
            Func<TNotNull, CancellationToken, Task<TResult>> f5,
            Func<TObj, CancellationToken, Task<TResult>> f6,
            Func<TNullObj, CancellationToken, Task<TResult>> f7,
            Func<TIFace, CancellationToken, Task<TResult>> f8,
            Func<TNullIFace, CancellationToken, Task<TResult>> f9,
            Func<TUnmanaged, CancellationToken, Task<TResult>> f10,
            CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfNull(f1);
            ThrowHelper.ThrowIfNull(f2);
            ThrowHelper.ThrowIfNull(f3);
            ThrowHelper.ThrowIfNull(f4);
            ThrowHelper.ThrowIfNull(f5);
            ThrowHelper.ThrowIfNull(f6);
            ThrowHelper.ThrowIfNull(f7);
            ThrowHelper.ThrowIfNull(f8);
            ThrowHelper.ThrowIfNull(f9);
            ThrowHelper.ThrowIfNull(f10);

            switch(_idx)
            {
                case 1:
                    return f1(_v1!, cancellationToken);
                case 2:
                    return f2(_v2!, cancellationToken);
                case 3:
                    return f3(_v3, cancellationToken);
                case 4:
                    return f4(_v4, cancellationToken);
                case 5:
                    return f5(_v5!, cancellationToken);
                case 6:
                    return f6(_v6!, cancellationToken);
                case 7:
                    return f7(_v7!, cancellationToken);
                case 8:
                    return f8(_v8!, cancellationToken);
                case 9:
                    return f9(_v9!, cancellationToken);
                case 10:
                    return f10(_v10, cancellationToken);
                default:
                    return ThrowHelper.ThrowOnInvalidState<Task<TResult>>();
            }
        }

        public Task<TResult> MatchAsync<TState, TResult>(
            TState state,
            Func<TState, TAny, CancellationToken, Task<TResult>> f1,
            Func<TState, TNonNullRef, CancellationToken, Task<TResult>> f2,
            Func<TState, TNullRef?, CancellationToken, Task<TResult>> f3,
            Func<TState, TStruct, CancellationToken, Task<TResult>> f4,
            Func<TState, TNotNull, CancellationToken, Task<TResult>> f5,
            Func<TState, TObj, CancellationToken, Task<TResult>> f6,
            Func<TState, TNullObj, CancellationToken, Task<TResult>> f7,
            Func<TState, TIFace, CancellationToken, Task<TResult>> f8,
            Func<TState, TNullIFace, CancellationToken, Task<TResult>> f9,
            Func<TState, TUnmanaged, CancellationToken, Task<TResult>> f10,
            CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfNull(f1);
            ThrowHelper.ThrowIfNull(f2);
            ThrowHelper.ThrowIfNull(f3);
            ThrowHelper.ThrowIfNull(f4);
            ThrowHelper.ThrowIfNull(f5);
            ThrowHelper.ThrowIfNull(f6);
            ThrowHelper.ThrowIfNull(f7);
            ThrowHelper.ThrowIfNull(f8);
            ThrowHelper.ThrowIfNull(f9);
            ThrowHelper.ThrowIfNull(f10);

            switch(_idx)
            {
                case 1:
                    return f1(state, _v1!, cancellationToken);
                case 2:
                    return f2(state, _v2!, cancellationToken);
                case 3:
                    return f3(state, _v3, cancellationToken);
                case 4:
                    return f4(state, _v4, cancellationToken);
                case 5:
                    return f5(state, _v5!, cancellationToken);
                case 6:
                    return f6(state, _v6!, cancellationToken);
                case 7:
                    return f7(state, _v7!, cancellationToken);
                case 8:
                    return f8(state, _v8!, cancellationToken);
                case 9:
                    return f9(state, _v9!, cancellationToken);
                case 10:
                    return f10(state, _v10, cancellationToken);
                default:
                    return ThrowHelper.ThrowOnInvalidState<Task<TResult>>();
            }
        }

        public void Switch(
            Action<TAny> a1,
            Action<TNonNullRef> a2,
            Action<TNullRef?> a3,
            Action<TStruct> a4,
            Action<TNotNull> a5,
            Action<TObj> a6,
            Action<TNullObj> a7,
            Action<TIFace> a8,
            Action<TNullIFace> a9,
            Action<TUnmanaged> a10)
        {
            Match(
                v => { a1(v); return Unit.Default; },
                v => { a2(v); return Unit.Default; },
                v => { a3(v); return Unit.Default; },
                v => { a4(v); return Unit.Default; },
                v => { a5(v); return Unit.Default; },
                v => { a6(v); return Unit.Default; },
                v => { a7(v); return Unit.Default; },
                v => { a8(v); return Unit.Default; },
                v => { a9(v); return Unit.Default; },
                v => { a10(v); return Unit.Default; });
        }

        public void Switch<TState>(
            TState state,
            Action<TState, TAny> a1,
            Action<TState, TNonNullRef> a2,
            Action<TState, TNullRef?> a3,
            Action<TState, TStruct> a4,
            Action<TState, TNotNull> a5,
            Action<TState, TObj> a6,
            Action<TState, TNullObj> a7,
            Action<TState, TIFace> a8,
            Action<TState, TNullIFace> a9,
            Action<TState, TUnmanaged> a10)
        {
            Match(
                state,
                (s, v) => { a1(s, v); return Unit.Default; },
                (s, v) => { a2(s, v); return Unit.Default; },
                (s, v) => { a3(s, v); return Unit.Default; },
                (s, v) => { a4(s, v); return Unit.Default; },
                (s, v) => { a5(s, v); return Unit.Default; },
                (s, v) => { a6(s, v); return Unit.Default; },
                (s, v) => { a7(s, v); return Unit.Default; },
                (s, v) => { a8(s, v); return Unit.Default; },
                (s, v) => { a9(s, v); return Unit.Default; },
                (s, v) => { a10(s, v); return Unit.Default; });
        }

        public Task SwitchAsync(
            Func<TAny, CancellationToken, Task> a1,
            Func<TNonNullRef, CancellationToken, Task> a2,
            Func<TNullRef?, CancellationToken, Task> a3,
            Func<TStruct, CancellationToken, Task> a4,
            Func<TNotNull, CancellationToken, Task> a5,
            Func<TObj, CancellationToken, Task> a6,
            Func<TNullObj, CancellationToken, Task> a7,
            Func<TIFace, CancellationToken, Task> a8,
            Func<TNullIFace, CancellationToken, Task> a9,
            Func<TUnmanaged, CancellationToken, Task> a10,
            CancellationToken cancellationToken = default)
        {
            return MatchAsync(
                async (v, ct) => { await a1(v, ct); return Unit.Default; },
                async (v, ct) => { await a2(v, ct); return Unit.Default; },
                async (v, ct) => { await a3(v, ct); return Unit.Default; },
                async (v, ct) => { await a4(v, ct); return Unit.Default; },
                async (v, ct) => { await a5(v, ct); return Unit.Default; },
                async (v, ct) => { await a6(v, ct); return Unit.Default; },
                async (v, ct) => { await a7(v, ct); return Unit.Default; },
                async (v, ct) => { await a8(v, ct); return Unit.Default; },
                async (v, ct) => { await a9(v, ct); return Unit.Default; },
                async (v, ct) => { await a10(v, ct); return Unit.Default; },
                cancellationToken);
        }

        public Task SwitchAsync<TState>(
            TState state,
            Func<TState, TAny, CancellationToken, Task> a1,
            Func<TState, TNonNullRef, CancellationToken, Task> a2,
            Func<TState, TNullRef?, CancellationToken, Task> a3,
            Func<TState, TStruct, CancellationToken, Task> a4,
            Func<TState, TNotNull, CancellationToken, Task> a5,
            Func<TState, TObj, CancellationToken, Task> a6,
            Func<TState, TNullObj, CancellationToken, Task> a7,
            Func<TState, TIFace, CancellationToken, Task> a8,
            Func<TState, TNullIFace, CancellationToken, Task> a9,
            Func<TState, TUnmanaged, CancellationToken, Task> a10,
            CancellationToken cancellationToken = default)
        {
            return MatchAsync(
                state,
                async (s, v, ct) => { await a1(s, v, ct); return Unit.Default; },
                async (s, v, ct) => { await a2(s, v, ct); return Unit.Default; },
                async (s, v, ct) => { await a3(s, v, ct); return Unit.Default; },
                async (s, v, ct) => { await a4(s, v, ct); return Unit.Default; },
                async (s, v, ct) => { await a5(s, v, ct); return Unit.Default; },
                async (s, v, ct) => { await a6(s, v, ct); return Unit.Default; },
                async (s, v, ct) => { await a7(s, v, ct); return Unit.Default; },
                async (s, v, ct) => { await a8(s, v, ct); return Unit.Default; },
                async (s, v, ct) => { await a9(s, v, ct); return Unit.Default; },
                async (s, v, ct) => { await a10(s, v, ct); return Unit.Default; },
                cancellationToken);
        }
    }