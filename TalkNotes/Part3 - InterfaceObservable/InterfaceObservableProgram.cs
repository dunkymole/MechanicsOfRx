using System;

namespace TalkNotes
{
    public class InterfaceObservableProgram
    {
        private static void Main(string[] args)
        {
            Observable3.Range(0, 10)
                       .Subscribe(Console.WriteLine, e => Console.WriteLine(e.Message), () => Console.WriteLine("done"));

            var q = from i in Observable3.Interval(TimeSpan.FromSeconds(1))
                    where i%2 == 0
                    select "oooh:  " + i;

            var d = q.Subscribe(Console.WriteLine, e => Console.WriteLine(e.Message), () => Console.WriteLine("done"));

            Console.ReadKey(true);

            Console.WriteLine("disposing");

            d.Dispose();

            q.Take(3).Subscribe(Console.WriteLine, e => Console.WriteLine(e.Message), () => Console.WriteLine("done"));

            var q1 = from outer in Observable3.Interval(TimeSpan.FromSeconds(1))
                        from inner in Observable3.Interval(TimeSpan.FromMilliseconds(1)).Take(3)
                        select inner;

            var d1 = q1.Subscribe(Console.WriteLine, e => Console.WriteLine(e.Message), () => Console.WriteLine("done"));

            Console.ReadKey(true);

            d1.Dispose();

            Console.WriteLine("disposed");

            Console.ReadKey(true);
        }
    }
}