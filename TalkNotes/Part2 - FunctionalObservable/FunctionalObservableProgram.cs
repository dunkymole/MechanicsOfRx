using System;

namespace TalkNotes
{
    public class FunctionalObservableProgram
    {
        private static void Main(string[] args)
        {
            Action<Notification<int>> observer = n =>
                {
                    if (n.NotificationType == NotificationType.Next)
                        Console.WriteLine(n.Next);
                    else
                        Console.WriteLine("done");
                };

            Observable2.Range(0, 10)(observer);

            Action<Notification<char>> observer2 = n =>
                {
                    if (n.NotificationType == NotificationType.Next)
                        Console.WriteLine(n.Next);
                    else
                        Console.WriteLine("done");
                };

            Observable2.Keys()(observer2);

            Observable2.Keys().ForEach(Console.WriteLine);
            Observable2.Interval(TimeSpan.FromSeconds(1)).ForEach(Console.WriteLine);

            Observable2.Interval(TimeSpan.FromSeconds(1), 3)
                       .Subscribe(Console.WriteLine, e => Console.WriteLine(e.Message), () => Console.WriteLine("done"));

            Console.ReadKey(true);
        }
    }
}