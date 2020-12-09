using System;
using System.Numerics;
using System.Diagnostics;

namespace Algoritmu_Analize
{
    class Program
    {
        public static int SieveOfErathostenes (int n)
        {
            bool[] isPrime = new bool[n + 1];

            //Set all numbers as prime
            for(int i = 0; i < n; i++) isPrime[i] = true;
            
            for(int i = 2; i <= (int) Math.Sqrt(n); i++) 
            {
                //If number is prime, set multiples of it to false
                if(isPrime[i]) 
                {
                    for(int j = i + i; j <= n; j += i) isPrime[j] = false; 
                }
            }

            int primeCount = 0;

            for(int i = 2; i <= n; i++) 
            {
                if(isPrime[i]) primeCount++;
            }

            return primeCount;
        }

        //Returns a^b (mod c)
        static int modularExp(int a, int b, int c)
        {
            return (int) BigInteger.ModPow(a, b, c);
        }

        public static bool MillerRabin(int n, int times)
        {
            if(n == 1 || n == 4){
                return false;
            }

            if(n == 2 || n == 3){
                return true;
            }
            
            // Write n as 2^d * r + 1 with d odd (by factoring out powers of 2 from n - 1)
            
            int d = n - 1;
            int r = 0;

            while (d % 2 == 0){
                d /= 2;
                r += 1;
            }

            Random rand = new Random();
            int a;

            for(int i = 0; i < times; i++)
            {
                //pick a random integer a in the range [2, n-2]
                a = rand.Next(2, n - 2);
                
                int x = modularExp(a, d, n);

                if (x == 1 || x == n - 1){
                    continue;
                }

                for(int j = 0; j < r - 1; j++)
                {
                    x = modularExp(x, 2, n);

                    if(x == 1) return false;

                    if(x == n - 1) return true;
                }

                return false;
            }

            return true;
        }

        static void Main(string[] args)
        {

            if (args.Length != 2)
            {
                System.Console.WriteLine("Please pass two arguments");
                System.Environment.Exit(1);
            }

            int[] intArgs = new int[args.Length];

            for (int i = 0; i < args.Length; i++)
            {
                if (!int.TryParse(args[i], out intArgs[i]))
                {
                    Console.WriteLine($"All parameters must be integers. Could not convert {args[i]}");
                    return;
                }
            };

            int limit = intArgs[0];
            int times = intArgs[1];

            Console.WriteLine("Sieve of Erathostenes found {0} primes from 1 to {1}", SieveOfErathostenes(limit), limit);
            
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            int mrTimes = 0;

            for(int i = 1; i<limit; i++)
            {
                if(MillerRabin(i, times)){
                    //Console.WriteLine("{0}", i);
                    mrTimes++;
                }
            }

            stopWatch.Stop();

            Console.WriteLine("Miller-Rabin algorithm found {0} primes from 1 to {1}", mrTimes, limit);

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            
            Console.WriteLine("RunTime " + elapsedTime);
        }
    }
}
