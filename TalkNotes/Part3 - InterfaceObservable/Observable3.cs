using System;
using System.Threading;

namespace TalkNotes
{
    //Action<Action<Notification<int>>>

    public interface IObservable3<out T>
    {
        IDisposable Subscribe(IObserver3<T> observer);
    }

    public interface IObserver3<in T>
    {
        void OnNext(T value);
        void OnError(Exception e);
        void OnComplete();
    }

    public static class Observable3
    {
        public static IDisposable Subscribe<T>(this IObservable3<T> source, Action<T> onNext, Action<Exception> onError,
                                               Action onCmplete)
        {
            return source.Subscribe(new AnonObserver<T>(onNext, onError, onCmplete));
        }

        public static IObservable3<int> Interval(TimeSpan timeout)
        {
            return Create<int>(o =>
                {
                    var i = 0;
                    var t = new Timer(_ => o.OnNext(i++), null, (long) timeout.TotalMilliseconds,
                                      (long) timeout.TotalMilliseconds);
                    return Disposable.Create(t.Dispose);
                });
        }


        public static IObservable3<int> Range(int start, int count)
        {
            return Create<int>(o =>
                {
                    var isDisposed = false;
                    for (var i = 0; i < count; i++)
                    {
                        if (isDisposed) break;
                        o.OnNext(i + start);
                    }
                    if (!isDisposed)
                        o.OnComplete();
                    return Disposable.Create(() => isDisposed = true);
                });
        }

        public static IObservable3<T> Take<T>(this IObservable3<T> source, int count)
        {
            return Create<T>(o =>
                {
                    var i = 0;
                    IDisposable t = null;
                    t = source.Subscribe(n =>
                        {
                            i++;
                            o.OnNext(n);
                            if (i == count)
                            {
                                t.Dispose();
                                o.OnComplete();
                            }
                        }, o.OnError, o.OnComplete);
                    return t;
                });
        }

        public static IObservable3<T> Where<T>(this IObservable3<T> source, Func<T, bool> predicate)
        {
            return Create<T>(o => source.Subscribe(n =>
                {
                    if (predicate(n))
                        o.OnNext(n);
                }, o.OnError, o.OnComplete));
        }

        public static IObservable3<T2> Select<T, T2>(this IObservable3<T> source, Func<T, T2> projection)
        {
            return Create<T2>(o => source.Subscribe(n => o.OnNext(projection(n)), o.OnError, o.OnComplete));
        }


        public static IObservable3<TResult> SelectMany<TOuter, TInner, TResult>(this IObservable3<TOuter> source,
                                                                                Func<TOuter, IObservable3<TInner>> innerFactory,
                                                                                Func<TOuter, TInner, TResult> resultSelector)
        {
            return Create<TResult>(o =>
                {
                    var innerSubscription = Disposable.Create(() => { });
                    var sync = new object();

                    var outerSubscription = source.Subscribe(n =>
                        {
                            lock (sync)
                            {
                                innerSubscription.Dispose();
                                innerSubscription = innerFactory(n)
                                    .Subscribe(v => o.OnNext(resultSelector(n, v)), o.OnError, () => { });
                            }
                        }, o.OnError, o.OnComplete);

                    return Disposable.Create(() =>
                        {
                            outerSubscription.Dispose();
                            lock (sync)
                            {
                                innerSubscription.Dispose();
                            }
                        });
                });
        }

        public static IObservable3<T> Create<T>(Func<IObserver3<T>, IDisposable> factory)
        {
            Func<IObserver3<T>, IDisposable> safe = o =>
                {
                    //The real Create adds more safety
                    try
                    {
                        return factory(o);
                    }
                    catch (Exception e)
                    {
                        o.OnError(e);
                    }
                    return Disposable.Create(() => { });
                };
            return new AnonObservable<T>(safe);
        }
    }
}