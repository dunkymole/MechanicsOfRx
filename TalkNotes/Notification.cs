using System;

namespace TalkNotes
{
    public static class Notification
    {
        public static Notification<T> Next<T>(T value)
        {
            return new Notification<T>(value);
        }

        public static Notification<T> Complete<T>()
        {
            return new Notification<T>();
        }

        public static Notification<T> Error<T>(Exception error)
        {
            return new Notification<T>(error);
        }
    }

    public class Notification<T>
    {
        public Notification(T next)
            : this(NotificationType.Next, next)
        {
        }

        public Notification()
            : this(NotificationType.Complete, default(T))
        {
        }

        private Notification(NotificationType notificationType, T next)
        {
            NotificationType = notificationType;
            Next = next;
        }

        public Notification(Exception exception)
            : this(NotificationType.Error, default(T), exception)
        {
        }

        private Notification(NotificationType notificationType, T next, Exception exception)
        {
            Exception = exception;
            NotificationType = notificationType;
            Next = next;
        }

        public NotificationType NotificationType { get; private set; }
        public T Next { get; private set; }
        public Exception Exception { get; private set; }
    }
}