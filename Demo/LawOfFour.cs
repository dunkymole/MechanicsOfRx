using System;
using Humanizer;

namespace Demo
{
    public static class LawOfFour
    {
        private const int MaxIterations = 100;

        public static int GetMagicNumber(int input)
        {
            return Reduce(input, input, MaxIterations);
        }

        private static int Reduce(int origonal, int input, int depth)
        {
            if (input == 4) return 4;
            if (depth == 0)
                throw new Exception(string.Format("{0} did not reduce to 4 within {1} iterations", origonal, MaxIterations));
            var word = input.ToWords();
            return Reduce(input, word.Length, --depth);
        }
    }
}
