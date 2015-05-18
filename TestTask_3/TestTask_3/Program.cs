using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask_3
{
    class Program
    {

        struct IndexWord
        {
            public int index;
            public int length;
        }


        static string[] words = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "twofive", "onefour", 
                           "eighttenthreeseven", "seventenoneten", "twosixsevensixseveneightsix", "tenfourfive", "twofivetwo", 
                           "sevensevenseventwosixeightnine", "sixninetentwo", "onetwothreefour", "tenninethreeone", "twoonethree", 
                           "fivefivefivetwo", "threetwosixnineseveneighttenten", "threetwosixnineseveneighttentenelleven" };

        static void Main(string[] args)
        {
            Console.WriteLine("We have next words:");
            Array.Sort(words, StringComparer.InvariantCulture);
            foreach (string s in words)
                Console.WriteLine(s);
            Console.WriteLine();

            // list for indexes of words witch we already checked
            List<int> indexesOfLongestWords = new List<int>();
            // write structs [index - Length] of every words in list
            List<IndexWord> indexLengthList = new List<IndexWord>(); 
            for(int i = 0; i < words.Length; i++)
            {
                IndexWord indexWord = new IndexWord();
                indexWord.index = i;
                indexWord.length = words[i].Length;
                indexLengthList.Add(indexWord);
            }

            // sort descending word length
            indexLengthList = indexLengthList.OrderByDescending(ind => ind.length).ToList();
            
            string longestStr;
            bool isFound = false;

            foreach (IndexWord iw in indexLengthList)
            {
                // copy first long word in indexLengthList 
                longestStr = new string(words[iw.index].ToCharArray());
                indexesOfLongestWords.Add(iw.index); // memorize index of checked word

                // create list for subwords witch we will find in long word
                // in order to find the longest
                List<string> subStringsInWord = new List<string>();
                while (longestStr.Length > 0)
                {
                    subStringsInWord.Clear();
                    bool isGroupFound = false; // flag, told us that we entered to a group of words with the same first character

                    for (int i = 0; i < words.Length; i++)
                    {                        
                        // find the word with the same first character
                        if (words[i].StartsWith(longestStr[0].ToString()))
                        {
                            isGroupFound = true; // we entered to a group of words with the same first character
                            if (indexesOfLongestWords.Contains(i)) // if we find word with same index - skip
                                continue;
                            if (longestStr.StartsWith(words[i]))
                            {
                                // keep all subwords in that group 
                                subStringsInWord.Add(words[i]);
                            }
                            continue;
                        }
                        
                        // when we leave group of words with the same first character
                        // exit the loop
                        if (isGroupFound == true)
                            break;
                    }

                    // if we didn't find any subword
                    // go to the next long word in the list
                    if (subStringsInWord.Count == 0)                    
                        break;                    
                    
                    // find longest subword that we found
                    string subStr = "";
                    foreach (string s in subStringsInWord)
                    {
                        if (subStr.Length < s.Length)
                            subStr = s;
                    }

                    // cut out subword from the beginning of the long word
                    longestStr = longestStr.Remove(0, subStr.Length);

                    // if after that we have no characters in our long word
                    // it means that we found longest concatenated word
                    if (longestStr.Length == 0)
                        isFound = true;
                }

                if (isFound == true)
                {
                    Console.Write("The longest concatenated word is: ");
                    Console.Write(words[iw.index] + " (" + words[iw.index].Length + ")");
                    break;
                }
            }
            if (isFound == false)
                Console.WriteLine("There are no concatenated words...");
            
            Console.Read();            
        }
    }
}
