using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Yaml.Serialization;

namespace TestTask_1_0
{
    class Program
    {
        [STAThreadAttribute] // for copy to clipboard
        static void Main(string[] args)
        {
            Console.WriteLine("To create array of pairs of integers");
            Console.WriteLine("enter the number (how many pairs you want to create): ");
            int count; // count of numbers which have to be repeated
            string input = Console.ReadLine();
            while(Int32.TryParse(input, out count) == false)
            {
                Console.WriteLine();
                Console.WriteLine("You entered the wrong number!");
                Console.Write("Enter the number of unique integers: ");
                input = Console.ReadLine();
            }

            Console.WriteLine();
            Console.WriteLine("Enter minimum and maximum number.");
            Console.Write("min: ");
            input = Console.ReadLine();
            int min; // the minimum value of random integers
            while (Int32.TryParse(input, out min) == false || min < 0)
            {
                Console.WriteLine();
                Console.WriteLine("You entered the wrong number!");
                Console.Write("min: ");
                input = Console.ReadLine();
            }

            Console.WriteLine();            
            Console.Write("max: ");
            input = Console.ReadLine();
            int max; // the maximum value of random integers
            while (Int32.TryParse(input, out max) == false || max < 0 || max - min < count)
            {
                Console.WriteLine();
                Console.WriteLine("You entered the wrong number!");
                Console.Write("max: ");
                input = Console.ReadLine();
            }

            // HashSet allows us to add a unique random value to the array
            HashSet<int> set = new HashSet<int>();
            Random rand = new Random();
            while(set.Count < count)
            {
                set.Add(rand.Next(min, max));
            }

            // create array for our numbers (size double "count" minus 1)
            int[] array = new int[count * 2 - 1];

            // pick a random index of value for the non-repeating number in the array
            int index = rand.Next(0, set.Count - 1);
            int setCounter = 0;
            for(int i = 0; i < array.Length;)
            {
                if (setCounter == index)
                {
                    array[i++] = set.ElementAt(setCounter++);
                }
                else
                {
                    array[i++] = set.ElementAt(setCounter);
                    array[i++] = set.ElementAt(setCounter++);
                }
            }

            // shuffle 
            for (int i = array.Length; i > 1; i--)
            {
                // Pick random element to swap.
                int j = rand.Next(i); // 0 <= j <= i-1
                // Swap.
                int tmp = array[j];
                array[j] = array[i - 1];
                array[i - 1] = tmp;
            }

            // convert array to YAML text
            var serializer = new YamlSerializer();
            string yaml = serializer.Serialize(array);

            bool isSuccess = true;
            string fileName = "numbers.yml";
            // write file to the current directory
            try
            {
                File.WriteAllText(Environment.CurrentDirectory + @"\" + fileName, yaml);
            }
            catch(Exception ex)
            {
                Console.WriteLine("File writing exception: " + ex.Message);
                isSuccess = false;
            }

            if (isSuccess)
            {
                Console.WriteLine();
                Console.WriteLine("File successfully created!");
                Console.WriteLine();
                Console.WriteLine("path to YAML file: " + Environment.CurrentDirectory + @"\" + fileName);
                Console.WriteLine();
                Console.WriteLine("Non-repeating number: " + set.ElementAt(index));
            }

            // copy file path to clipboard for convenience
            Clipboard.SetText(Environment.CurrentDirectory + @"\" + fileName);

            Console.WriteLine();
            Console.WriteLine("Path of YAML file was copyed to the clipboard.");
            Console.WriteLine();
            Console.WriteLine("Press eny key to close...");
            Console.Read();
        }


    }
}
