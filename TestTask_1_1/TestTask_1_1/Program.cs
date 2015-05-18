using System;
using System.IO;
using System.Windows.Forms;
using System.Yaml.Serialization;

namespace TestTask_1_1
{
    class Program
    {
        static void Main(string[] args)
        {            
            
            Console.Write("Enter path to YAML file: ");
            string path;            

            path = Console.ReadLine();
            
            // read and convert YAML file to the array of integers
            var deserializer = new YamlSerializer();
            object obj = null;
            try
            {
                obj = deserializer.DeserializeFromFile(path)[0];
            }
            catch(Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error by reding or deserialisation file: " + ex.Message);
                Console.WriteLine("Press eny key to close...");
                Console.Read();
                return;
            }

            int[] array = null;
            if(obj != null && obj.GetType() == typeof(int[]))
            {
                array = (int[])obj;
            }

            if(array == null)
            {
                Console.WriteLine();
                Console.WriteLine("Error by reding or deserialisation file...");
                Console.WriteLine("Press eny key to close...");
                Console.Read();
                return;
            }

            int result = 0;

            // using bitwise exclusive-OR
            foreach (int item in array)
                result ^= item;
            // after that we have non-repeating value

            Console.WriteLine();
            Console.WriteLine("Non-repeating number: " + result);
            Console.WriteLine();
            Console.WriteLine("Press eny key to close...");
            Console.Read();
        }
    }
}
