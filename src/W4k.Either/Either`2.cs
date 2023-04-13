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
    public readonly struct Either<T0, T1> : IEquatable<Either<T0, T1>>, ISerializable
        where T0 : notnull
        where T1 : notnull
    {
        private readonly byte _idx;
        private readonly T0? _v0;
        private readonly T1? _v1;

        public Either(T0 v0)
        {
            ThrowHelper.ThrowIfNull(v0);
            _idx = 0;
            _v0 = v0;
        }

        public Either(T1 v1)
        {
            ThrowHelper.ThrowIfNull(v1);
            _idx = 1;
            _v1 = v1;
        }

        private Either(SerializationInfo info, StreamingContext context)
        {
            _idx = info.GetByte(nameof(_idx));
            switch (_idx)
            {
                case 0:
                    _v0 = (T0?)info.GetValue(nameof(_v0), typeof(T0));
                    break;

                case 1:
                    _v1 = (T1?)info.GetValue(nameof(_v1), typeof(T1));
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
                    case 0:
                        return _v0;
                    case 1:
                        return _v1;
                    default:
                        return ThrowHelper.ThrowOnInvalidState<object?>();
                }
            }
        }

        [Pure]
        public static bool operator ==(Either<T0, T1> left, Either<T0, T1> right) => left.Equals(right);

        [Pure]
        public static bool operator !=(Either<T0, T1> left, Either<T0, T1> right) => !left.Equals(right);

        [Pure]
        public static implicit operator Either<T0, T1>(T0 v0) => new(v0);

        [Pure]
        public static implicit operator Either<T0, T1>(T1 v1) => new(v1);

        [Pure]
        public override int GetHashCode()
        {
            switch (_idx)
            {
                case 0:
                    return _v0!.GetHashCode();
                case 1:
                    return _v1!.GetHashCode();
                default:
                    return ThrowHelper.ThrowOnInvalidState<int>();
            }
        }

        [Pure]
        public override string ToString()
        {
            switch (_idx)
            {
                case 0:
                    return $"Either<{typeof(T0).Name}, _>({_v0})";
                case 1:
                    return $"Either<_, {typeof(T1).Name}>({_v1})";
                default:
                    return ThrowHelper.ThrowOnInvalidState<string>();
            }
        }

        [Pure]
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is not Either<T0, T1> other)
            {
                return false;
            }

            return Equals(other);
        }

        [Pure]
        public bool Equals(Either<T0, T1> other)
        {
            if (_idx != other._idx)
            {
                return false;
            }

            switch (_idx)
            {
                case 0:
                    return _v0!.Equals(other._v0);
                case 1:
                    return _v1!.Equals(other._v1);
                default:
                    return ThrowHelper.ThrowOnInvalidState<bool>();
            }
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(_idx), _idx);
            switch (_idx)
            {
                case 0:
                    info.AddValue(nameof(_v0), _v0);
                    break;

                case 1:
                    info.AddValue(nameof(_v1), _v1);
                    break;

                default:
                    ThrowHelper.ThrowOnInvalidState();
                    break;
            }
        }

        [Pure]
        public bool TryPick([NotNullWhen(true)] out T0? value)
        {
            if (_idx == 0)
            {
                value = _v0!;
                return true;
            }

            value = default;
            return false;
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

        public TResult Match<TResult>(Func<T0, TResult> f0, Func<T1, TResult> f1)
        {
            ThrowHelper.ThrowIfNull(f0);
            ThrowHelper.ThrowIfNull(f1);

            switch (_idx)
            {
                case 0: 
                    return f0(_v0!);
                case 1: 
                    return f1(_v1!);
                default: 
                    return ThrowHelper.ThrowOnInvalidState<TResult>();
            }
        }

        public TResult Match<TState, TResult>(TState state, Func<TState, T0, TResult> f0, Func<TState, T1, TResult> f1)
        {
            ThrowHelper.ThrowIfNull(f0);
            ThrowHelper.ThrowIfNull(f1);

            switch (_idx)
            {
                case 0:
                    return f0(state, _v0!);
                case 1:
                    return f1(state, _v1!);
                default:
                    return ThrowHelper.ThrowOnInvalidState<TResult>();
            }
        }

        public Task<TResult> MatchAsync<TResult>(Func<T0, CancellationToken, Task<TResult>> f0, Func<T1, CancellationToken, Task<TResult>> f1, CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfNull(f0);
            ThrowHelper.ThrowIfNull(f1);

            switch (_idx)
            {
                case 0:
                    return f0(_v0!, cancellationToken);
                case 1:
                    return f1(_v1!, cancellationToken);
                default:
                    return ThrowHelper.ThrowOnInvalidState<Task<TResult>>();
            }
        }

        public Task<TResult> MatchAsync<TState, TResult>(TState state, Func<TState, T0, CancellationToken, Task<TResult>> f0, Func<TState, T1, CancellationToken, Task<TResult>> f1, CancellationToken cancellationToken = default)
        {
            ThrowHelper.ThrowIfNull(f0);
            ThrowHelper.ThrowIfNull(f1);

            switch (_idx)
            {
                case 0:
                    return f0(state, _v0!, cancellationToken);
                case 1:
                    return f1(state, _v1!, cancellationToken);
                default:
                    return ThrowHelper.ThrowOnInvalidState<Task<TResult>>();
            }
        }

        public void Switch(Action<T0> a0, Action<T1> a1)
        {
            Match(
                value => { a0(value); return Unit.Default; },
                value => { a1(value); return Unit.Default; });
        }

        public void Switch<TState>(TState state, Action<TState, T0> a0, Action<TState, T1> a1)
        {
            Match(
                state,
                (s, v0) => { a0(s, v0); return Unit.Default; },
                (s, v1) => { a1(s, v1); return Unit.Default; });
        }

        public Task SwitchAsync(
            Func<T0, CancellationToken, Task> a0,
            Func<T1, CancellationToken, Task> a1,
            CancellationToken cancellationToken = default)
        {
            return MatchAsync(
                async (v0, ct) => { await a0(v0, ct); return Unit.Default; },
                async (v1, ct) => { await a1(v1, ct); return Unit.Default; },
                cancellationToken);
        }

        public Task SwitchAsync<TState>(
            TState state,
            Func<TState, T0, CancellationToken, Task> a0,
            Func<TState, T1, CancellationToken, Task> a1,
            CancellationToken cancellationToken = default)
        {
            return MatchAsync(
                state,
                async (s, v0, ct) => { await a0(s, v0, ct); return Unit.Default; },
                async (s, v1, ct) => { await a1(s, v1, ct); return Unit.Default; },
                cancellationToken);
        }
    }
}
