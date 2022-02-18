using System.Collections.Generic;
using System.Linq;

namespace WordleSolver
{
    public class Turn
    {
        public Turn(List<Guess> guesses)
        {
            Guesses = guesses;
        }

        public List<Guess> Guesses { get; }

        public bool IsWin => Guesses.All(x => x.IsCorrect && x.IsInPosition);
    }
}