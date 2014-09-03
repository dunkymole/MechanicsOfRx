using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TalkNotes;

namespace Demo
{
    public static class Observable3Ex
    {
        public static IObservable3<T> ObserveOn<T>(this IObservable3<T> source, Control control)
        {
            return Observable3.Create<T>(o => source.Subscribe(n => control.Invoke((Action)(()=>o.OnNext(n))), 
                                                               e => control.Invoke((Action)(()=>o.OnError(e))),
                                                               ()=> control.Invoke((Action)(o.OnComplete))));
        }

        public static IObservable3<T> Concat<T>(this IObservable3<T> first, IObservable3<T> second)
        {
            return Observable3.Create<T>(o =>
                {
                    var d2 = Disposable.Empty;
                    var d1 = first.Subscribe(o.OnNext, o.OnError, () =>
                        {
                            d2 = second.Subscribe(o.OnNext, o.OnError, o.OnComplete);
                        });
                    return Disposable.Create(() =>
                        {
                            d1.Dispose();
                            d2.Dispose();
                        });
                    });
        }

        public static IObservable3<T> Return<T>(T value)
        {
            return Observable3.Create<T>(o =>
                {
                    o.OnNext(value);
                    o.OnComplete();
                    return Disposable.Empty;
                });
        }

        public static IObservable3<T> Switch<T>(this IObservable3<IObservable3<T>> sources)
        {
            return Observable3.Create<T>(o =>
                {
                    var innerSub = Disposable.Empty;
                    var outerSub = sources.Subscribe(inner =>
                        {
                            innerSub.Dispose();
                            innerSub = inner.Subscribe(o.OnNext, o.OnError, () => { });
                        }, o.OnError, o.OnComplete);

                    return Disposable.Create(() =>
                        {
                            innerSub.Dispose();
                            outerSub.Dispose();
                        });
                });
        }

        public static IObservable3<T> Merge<T>(this IEnumerable<IObservable3<T>> sources, int maxConcurrency)
        {
            return Observable3.Create<T>(o =>
                {
                    var enumerator = sources.GetEnumerator();
                    var disposables = new List<IDisposable>();
                    var sync = new object();
                    var completed = false;
                    var disposed = false;
                    Action trySchedule = null;
                    trySchedule = ()=>
                        {
                            lock (sync)
                            {
                                if (disposed) return;
                                if(enumerator.MoveNext())
                                {
                                    disposables.Add(enumerator.Current.Subscribe(o.OnNext, o.OnError, trySchedule));
                                }
                                else if (!completed)
                                {
                                    completed = true;
                                    o.OnComplete();
                                }
                            }
                        };
                    
                    lock (sync)
                    {
                        for (var i = 0; i < maxConcurrency; i++)
                        {
                            if (enumerator.MoveNext())
                            {
                                disposables.Add(enumerator.Current.Subscribe(o.OnNext, o.OnError, trySchedule));
                            }
                        }
                    }

                    return Disposable.Create(() =>
                        {
                            lock (sync)
                            {
                                disposed = true;
                                foreach (var disposable in disposables)
                                    disposable.Dispose();
                            }
                        });
                });
        }
    }
}