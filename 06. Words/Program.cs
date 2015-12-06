namespace Words
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class Hash
    {
        const ulong BASE = 31;
        const ulong MOD = 1000000009;

        private static ulong[] powers;

		public static void ComputePowers(int n)
		{
			powers = new ulong[n + 1];
			powers[0] = 1;
			
			for(int i = 0; i < n; i++)
			{
				powers[i + 1] = powers[i] * BASE % MOD;
			}
		}

		public ulong Value { get; private set; }

		public Hash(string str)
		{
			this.Value = 0;

			foreach(char c in str)
			{
				this.Add(c);
			}
		}

		public void Add(char c)
		{
			this.Value = (this.Value * BASE + c) % MOD;
		}

		public void Remove(char c, int n)
		{
			this.Value = (MOD + this.Value - powers[n] * c % MOD) % MOD;
		}
    }

    public class Program
    {
        //static string text;
        static Dictionary<string, int> counted = new Dictionary<string,int>();
        static int count;

        static void Main()
        {
            string pattern = Console.ReadLine();
            string text = Console.ReadLine();

            int matchTotalCount = 0;
            Hash.ComputePowers(pattern.Length);

            // Whole
           // counted.Clear();
            var hwm = Compare(pattern, text);
            matchTotalCount += hwm;

            for (int i = 1; i < pattern.Length; i++)
            {
                var prefix = pattern.Substring(0, i);
               // counted.Clear();
                var pm = Compare(prefix, text);
                var suffix = pattern.Substring(i);
             //   counted.Clear();
                var sm = Compare(suffix, text);

                matchTotalCount += pm * sm;
            }

            Console.WriteLine(matchTotalCount);
        }

        static int Compare(string pattern, string text)
        {
            if (counted.ContainsKey(pattern))
            {
                return counted[pattern];
            }

            int n = text.Length;
            int m = pattern.Length;

            if (m > n)
            {
                return 0;
            }


            Hash hPattern = new Hash(pattern);
            Hash hWindow = new Hash(text.Substring(0, m));

            int count = 0;

            if (hPattern.Value == hWindow.Value)
            {
                count++;
            }

            for (int i = 1; i <= n - m; i++)
            {
                hWindow.Add(text[i + m - 1]);
                hWindow.Remove(text[i - 1], m);

                if (hPattern.Value == hWindow.Value)
                {
                    count++;
                }
            }

            counted.Add(pattern, count);
            return count;
        }
    }
}
