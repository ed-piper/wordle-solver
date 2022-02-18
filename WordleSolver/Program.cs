using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace WordleSolver
{
    class Program
    {
        private static Word answer;
        private static List<Turn> turns;
        private static List<Word> possibleWords;

        static void Main(string[] args)
        {
            Console.WriteLine("Enter your target word: ");
            var input = Console.ReadLine();

            if (string.IsNullOrEmpty(input) || input.Length != 5)
            {
                Console.WriteLine("*** Must be a 5 letter word ***");
                Main(null);
            }

            answer = new Word(input[0],input[1], input[2], input[3], input[4]);
            turns = new List<Turn>();
            var guessCount = 1;

            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"data/words.txt");
            //Build Words
            possibleWords = File.ReadLines(path).Select(line => new Word(line[0], line[1], line[2], line[3], line[4]))
                .OrderByDescending(x => x.Score).ToList();
            //Choose first word

            var possibleFirstWords = GetPossibleFirstWords();
            var rand = new Random();
            var firstWord = possibleFirstWords[rand.Next(10)];
            var turn = EvaluateTurn(firstWord);
            //todo - pick next best starting word if number 1 strikes out
            turns.Add(turn);
            if (turn.IsWin)
            {
                Console.WriteLine(firstWord + " Is Correct! In the first guess");
                possibleWords.Remove(firstWord);
            }
            else
            {
                Console.WriteLine(firstWord + " Is Incorrect!");
                while (turns.All(x => !x.IsWin))
                {
                    guessCount++;
                    var nextWord = GetNextWord();
                    var nextTurn = EvaluateTurn(nextWord);
                    if (nextTurn.IsWin)
                    {
                        Console.WriteLine(nextWord + " Is Correct! It took " + guessCount + " guesses");
                    }
                    else
                    {
                        Console.WriteLine(nextWord + " Is Incorrect!");
                        possibleWords.Remove(nextWord);
                    }
                    turns.Add(nextTurn);
                }
            }

            Main(null);
        }

        private static Word GetNextWord()
        {
            foreach (var dw in DeadLetters.Select(deadLetter => possibleWords.Where(x => x.HasLetter(deadLetter))).SelectMany(dws => dws).ToList())
            {
                possibleWords.Remove(dw);
            }

            //Next discard words with letters not at known positions
            foreach (var turn in turns)
            {
                for (var i = 0; i < 5; i++)
                {
                    if (turn.Guesses[i].IsCorrect && turn.Guesses[i].IsInPosition)
                    {
                        possibleWords = possibleWords.Where(x => x.ToString()[i] == turn.Guesses[i].Letter).ToList();
                    }
                }
            }

            foreach (var turn in turns)
            {
                for (var i = 0; i < 5; i++)
                {
                    if (turn.Guesses[i].IsCorrect && !turn.Guesses[i].IsInPosition)
                    {
                        possibleWords = possibleWords.Where(x => x.ToString()[i] != turn.Guesses[i].Letter).ToList();
                    }
                }
            }


            //Next discard words with letters in proven incorrect positions
            var possibleNoRepeats = possibleWords.Where(x => !x.HasRepeats).OrderByDescending(x => x.Score).FirstOrDefault();

            if (possibleNoRepeats != null) return possibleNoRepeats;

            return possibleWords.OrderByDescending(x => x.Score).First();
        }

        private static List<char> DeadLetters => turns.SelectMany(x => x.Guesses).Where(x => !x.IsCorrect).Select(x => x.Letter).ToList();

        private static Turn EvaluateTurn(Word guessWord)
        {
            var guesses = new List<Guess>
            {
                new Guess(guessWord.One, answer.HasLetter(guessWord.One), answer.One == guessWord.One),
                new Guess(guessWord.Two,  answer.HasLetter(guessWord.Two), answer.Two == guessWord.Two),
                new Guess(guessWord.Three,  answer.HasLetter(guessWord.Three), answer.Three == guessWord.Three),
                new Guess(guessWord.Four,  answer.HasLetter(guessWord.Four), answer.Four == guessWord.Four),
                new Guess(guessWord.Five,  answer.HasLetter(guessWord.Five), answer.Five == guessWord.Five)
            };
            return new Turn(guesses);
        }

        private static List<Word> GetPossibleFirstWords()
        {
            var maxScore = possibleWords.First(x => !x.HasRepeats && x.Five != 's').Score;
            var possibleFirstWords =
                possibleWords.Where(x => x.Score <= maxScore &&
                                         !x.HasRepeats &&
                                         x.Five != 's')
                    .Take(10).ToList();
            return possibleFirstWords;
        }
    }
}