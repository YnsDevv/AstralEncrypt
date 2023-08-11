using System;
using System.Linq;

namespace AstralEncrypt
{
    public class RandomString
    {
        public static class RandomStringGenerator
        {
            private static readonly Random Random = new Random();
            private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            public static string Generate(int length)
            {
                return new string(Enumerable.Repeat(Chars, length)
                    .Select(s => s[Random.Next(s.Length)]).ToArray());
            }
        }

    }
}