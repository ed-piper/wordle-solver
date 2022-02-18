namespace WordleSolver
{
    public class Guess
    {
        public Guess(char letter,
            bool isCorrect,
            bool isInPosition)
        {
            Letter = letter;
            IsCorrect = isCorrect;
            IsInPosition = isInPosition;
        }

        public char Letter { get; }
        public bool IsCorrect { get; }
        public bool IsInPosition { get; }
    }
}