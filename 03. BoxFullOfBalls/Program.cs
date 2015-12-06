namespace BoxFullOfBalls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    
    public class Program
    {
        static int[] possibles;
        static Dictionary<int, int> optimals;
        static int wins = 0;
        static void Main()
        {
            possibles = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            var parts = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            int startBallsCount = parts[0];
            int gamesCount = parts[1] - parts[0] + 1;

            for (int i = startBallsCount; i <= gamesCount; i++)
            {
                PlayGame(i);
            }

            Console.WriteLine(wins);
        }

        private static void PlayGame(int startBallsCount)
        {
            int ballsInBox = startBallsCount;
            int turn = 0;

            while (ballsInBox > 0)
            {
                int maxAllowed = 1;

                for (int i = 0; i < possibles.Length; i++)
                {
                    if(possibles[i] > maxAllowed && possibles[i] <= ballsInBox && Test(ballsInBox, possibles[i]))
                    {
                        maxAllowed = possibles[i];
                    }
                }
                
                ballsInBox -= maxAllowed;
                turn++;
            }

            if (turn % 2 == 1)
            {
                wins++;
            }
        }

        private static bool Test(int balls, int test)
        {
            int rest = balls - test;
            if (possibles.Contains(rest))
            {
                return false;
            }
            return true;
        }
    }
}
