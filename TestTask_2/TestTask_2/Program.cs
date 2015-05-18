using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter a positive integer: ");
            string input = Console.ReadLine(); // read entered string
            byte[] array;
            bool isWrongString;
            
            do
            {                
                array = new byte[input.Length];
                isWrongString = false;

                if (input.Length == 0) // you have to enter number
                    isWrongString = true;

                // fill an array of numbers
                for (int i = 0; i < input.Length; i++)
                {
                    // try convert chars to digits
                    if (!Byte.TryParse(input.Substring(i, 1), out array[i])) 
                    {                        
                        isWrongString = true;  
                        break;
                    }
                    else
                    {
                        // here we have a digit from 0 to 9
                        if (i == 0 && array[i] == 0) // numbers can not start with zero
                        {
                            isWrongString = true;
                            break;
                        }
                    }
                }

                if(isWrongString == true)
                {
                    Console.WriteLine();
                    Console.WriteLine("You entered the wrong integer!");
                    Console.WriteLine();
                    Console.Write("Enter a positive integer: ");
                    input = Console.ReadLine();
                }

            } while (isWrongString == true);

            int index; 
            bool done = false;
            byte temp; 

            // here we are looking for a digit that is lower then last and then swap
            // if such digit not found we are looking for a digit that lower then next to last and so on ...
            // when we swapped numbers we sort the remaining part of array
            for (int i = 0; i < array.Length; i++)
            {
                if (done)
                    break;
                index = array.Length - 1 - i;
                for (int j = index; j >= 0; j--)
                {
                    if (array[index] > array[j]) // find lower number
                    {
                        temp = array[j];            //
                        array[j] = array[index];    // swap
                        array[index] = temp;        //
                        if (array.Length - 1 - j > 1) // if we swap not the last two digits
                            Array.Sort(array, j + 1, array.Length - j - 1); // sort the the remaining part of array
                        done = true; // it's ok
                        break;
                    }
                }
            }

            Console.WriteLine();
            if(done)
            {                
                Console.Write("your number:   ");
                Console.WriteLine(input);
                Console.Write("result number: ");
                if (array.Length < 29) // if number has less digits then decimal type we can have it like number
                {
                    decimal number = 0;
                    int count = 0;
                    for(int i = array.Length - 1; i >= 0; i--)
                    {
                        number += (decimal)(array[i] * Math.Pow(10, count++));
                    }
                    Console.Write(number);
                }
                else
                {
                    // if number has too many digits
                    foreach (byte item in array)
                        Console.Write(item);
                }
            }
            else
            {                
                Console.WriteLine("There is no greater number.");                
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to close...");
            Console.Read();
        }
    }
}
