using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using W4k.Either.Abstractions;

namespace W4k.Either
{
    [Serializable]
    public readonly struct Either<T1, T2> : IEquatable<Either<T1, T2>>, ISerializable
    {
        private readonly byte _idx;
        private readonly T1? _v1;
        private readonly T2? _v2;

        public Either(T1 value)
        {
            ThrowHelper.ThrowIfNull(value);
            _idx = 0;
            _v1 = value;
        }

        public Either(T2 value)
        {
            ThrowHelper.ThrowIfNull(value);
            _idx = 1;
            _v2 = value;
        }

        private Either(SerializationInfo info, StreamingContext context)
        {
            _idx = info.GetByte(nameof(_idx));
            switch (_idx)
            {
                case 1:
                    _v1 = (T1?)info.GetValue(nameof(_v1), typeof(T1));
                    break;

                case 2:
                    _v2 = (T2?)info.GetValue(nameof(_v2), typeof(T2));
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
                    default:
                        return ThrowHelper.ThrowOnInvalidState<object?>();
                }
            }
        }

        [Pure]
        public static bool operator ==(Either<T1, T2> left, Either<T1, T2> right) => left.Equals(right);

        [Pure]
        public static bool operator !=(Either<T1, T2> left, Either<T1, T2> right) => !left.Equals(right);

        [Pure]
        public static implicit operator Either<T1, T2>(T1 value) => new(value);

        [Pure]
        public static implicit operator Either<T1, T2>(T2 value) => new(value);

        [Pure]
        public override int GetHashCode()
        {
            switch (_idx)
            {
                case 1:
                    return _v1?.GetHashCode() ?? 0;
                case 2:
                    return _v2?.GetHashCode() ?? 0;
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
                    return _v1?.ToString() ?? string.Empty;
                case 2:
                    return _v2?.ToString() ?? string.Empty;
                default:
                    return ThrowHelper.ThrowOnInvalidState<string>();
            }
        }

        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is not Either<T1, T2> other)
            {
                return false;
            }

            return Equals(other);
        }

        [Pure]
        public bool Equals(Either<T1, T2> other)
        {
            if (_idx != other._idx)
            {
                return false;
            }

            switch (_idx)
            {
                case 1:
                    return _v1?.Equals(other._v1) ?? false;
                case 2:
                    return _v2?.Equals(other._v2) ?? false;
                default:
                    return ThrowHelper.ThrowOnInvalidState<bool>();
            }
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(_idx), _idx);
            switch (_idx)
            {
                case 1:
                    info.AddValue(nameof(_v1), _v1);
                    break;

                case 2:
                    info.AddValue(nameof(_v2), _v2);
                    break;

                default:
                    ThrowHelper.ThrowOnInvalidState();
                    break;
            }
        }

        [Pure]
        public bool TryPick([NotNullWhen(true)] out T1? value)
        {
            if (_idx == 1)
            {
                value = _v1!;
                return true;
            }

            value = default;
            return false;
        }

        [Pure]
        public bool TryPick([NotNullWhen(true)] out T2? value)
        {
            if (_idx == 2)
            {
                value = _v2!;
                return true;
            }

            value = default;
            return false;
        }

        public TResult Match<TResult>(Func<T1, TResult> f1, Func<T2, TResult> f2)
        {
            ThrowHelper.ThrowIfNull(f1);
            ThrowHelper.ThrowIfNull(f2);

            switch (_idx)
            {
                case 1: 
                    return f1(_v1!);
                case 2: 
                    return f2(_v2!);
                default: 
                    return ThrowHelper.ThrowOnInvalidState<TResult>();
            }
        }

        public TResult Match<TState, TResult>(TState state, Func<TState, T1, TResult> f1, Func<TState, T2, TResult> f2)
        {
            ThrowHelper.ThrowIfNull(f1);
            ThrowHelper.ThrowIfNull(f2);

            switch (_idx)
            {
                case 1:
                    return f1(state, _v1!);
                case 2:
                    return f2(state, _v2!);
                default:
                    return ThrowHelper.ThrowOnInvalidState<TResult>();
            }
        }

        public Task<TResult> MatchAsync<TResult>(
            Func<T1, CancellationToken, Task<TResult>> f1,
            Func<T2, CancellationToken, Task<TResult>> f2,
            CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfNull(f1);
            ThrowHelper.ThrowIfNull(f2);

            switch (_idx)
            {
                case 1:
                    return f1(_v1!, cancellationToken);
                case 2:
                    return f2(_v2!, cancellationToken);
                default:
                    return ThrowHelper.ThrowOnInvalidState<Task<TResult>>();
            }
        }

        public Task<TResult> MatchAsync<TState, TResult>(
            TState state,
            Func<TState, T1, CancellationToken, Task<TResult>> f1,
            Func<TState, T2, CancellationToken, Task<TResult>> f2,
            CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfNull(f1);
            ThrowHelper.ThrowIfNull(f2);

            switch (_idx)
            {
                case 1:
                    return f1(state, _v1!, cancellationToken);
                case 2:
                    return f2(state, _v2!, cancellationToken);
                default:
                    return ThrowHelper.ThrowOnInvalidState<Task<TResult>>();
            }
        }

        public void Switch(Action<T1> a1, Action<T2> a2)
        {
            Match(
                value => { a1(value); return Unit.Default; },
                value => { a2(value); return Unit.Default; });
        }

        public void Switch<TState>(TState state, Action<TState, T1> a1, Action<TState, T2> a2)
        {
            Match(
                state,
                (s, v) => { a1(s, v); return Unit.Default; },
                (s, v) => { a2(s, v); return Unit.Default; });
        }

        public Task SwitchAsync(
            Func<T1, CancellationToken, Task> a1,
            Func<T2, CancellationToken, Task> a2,
            CancellationToken cancellationToken = default)
        {
            return MatchAsync(
                async (v, ct) => { await a1(v, ct); return Unit.Default; },
                async (v, ct) => { await a2(v, ct); return Unit.Default; },
                cancellationToken);
        }

        public Task SwitchAsync<TState>(
            TState state,
            Func<TState, T1, CancellationToken, Task> a1,
            Func<TState, T2, CancellationToken, Task> a2,
            CancellationToken cancellationToken = default)
        {
            return MatchAsync(
                state,
                async (s, v, ct) => { await a1(s, v, ct); return Unit.Default; },
                async (s, v, ct) => { await a2(s, v, ct); return Unit.Default; },
                cancellationToken);
        }
    }
}
