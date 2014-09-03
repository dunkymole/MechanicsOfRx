using System;
using System.Linq;

namespace TalkNotes
{
    public class FunctionalEnumerableProgram
    {
        private static void Main(string[] args)
        {
            var ints = Enumerable.Range(0, 10);

            foreach (var i in ints)
            {
                Console.WriteLine(i);
            }
            var enumerator = ints.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current);
            }

            var enumerable2 = Enumerable2.Range(0, 10);
            var enumerator2 = enumerable2();
            var r = enumerator2();
            while (r.NotificationType != NotificationType.Complete)
            {
                Console.WriteLine(r.Next);
                r = enumerator2();
            }

            Enumerable2.Range(0, 10)
                       .Where(i => i%2 == 0)
                       .Select(i => string.Format("str:{0}", i))
                       .ForEach(Console.WriteLine);

            var q2 = from i in Enumerable2.Range(0, 10)
                     where i%2 == 0
                     select string.Format("str:{0}", i);

            q2.ForEach(Console.WriteLine);

            Enumerable2.Keys().ForEach(Console.WriteLine);

            var q = from i in Enumerable2.Range(0, 10)
                    from j in Enumerable2.Range(0, 2)
                    where i%2 == 0
                    select i + j;

            q.ForEach(Console.WriteLine);

            Console.ReadKey(true);
        }
    }
}