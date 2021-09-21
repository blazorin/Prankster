using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Utils
{
	public static class IdentifierUtils
	{
        private static readonly Random _random = new Random();

        // Generates a random number within a range.      
        private static int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }

        // Generates a random string with a given size.    
        private static string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);

            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):   
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.  

            // char is a single Unicode character  
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length = 26  

            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }

        public static List<string> CreateComplexIdentifier()
		{
            List<string> parts = new();

            for (int i = 0; i < 4; i++)
                parts.Add(RandomNumber(1000, 9999).ToString());

            string exclusivePart = RandomNumber(100, 999).ToString() + RandomString(1);
            parts.Add(exclusivePart);

           return parts;
		}

    }
}
