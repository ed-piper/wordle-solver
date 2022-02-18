using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordleSolver
{
    public class Word : IEnumerable
    {
        public Word(
            char one,
            char two,
            char three,
            char four,
            char five
        )
        {
            One = one;
            Two = two;
            Three = three;
            Four = four;
            Five = five;
        }

        public char One { get; }
        public char Two { get; }
        public char Three { get; }
        public char Four { get; }
        public char Five { get; }

        public bool HasLetter(char letter)
        {
            return One == letter || Two == letter || Three == letter || Four == letter || Five == letter;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(One);
            sb.Append(Two);
            sb.Append(Three);
            sb.Append(Four);
            sb.Append(Five);
            return sb.ToString();
        }

        public bool HasRepeats =>
            ToString()
                .GroupBy(x => x)
                .Select(x => new { Letter = x.Key, Count = x.Count() })
                .Max(x => x.Count > 1);

        public int Score => LetterScores[One] + LetterScores[Two] + LetterScores[Three] + LetterScores[Four] + LetterScores[Five];

        /// <summary>
        /// Scores based on analysis of 5 letter word score
        /// TODO: calculate dynamically on each turn
        /// </summary>
        private static Dictionary<char, int> LetterScores =>
            new Dictionary<char, int>
            {
                {'q', 1},
                {'j', 2},
                {'z', 3},
                {'x', 4},
                {'v', 5},
                {'w', 6},
                {'f', 7},
                {'k', 8},
                {'g', 9},
                {'b', 10},
                {'h', 11},
                {'m', 12},
                {'y', 13},
                {'p', 14},
                {'c', 15},
                {'u', 16},
                {'d', 17},
                {'n', 18},
                {'t', 19},
                {'l', 20},
                {'i', 21},
                {'r', 22},
                {'o', 23},
                {'a', 24},
                {'e', 25},
                {'s', 26}
            };

        public IEnumerator GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}