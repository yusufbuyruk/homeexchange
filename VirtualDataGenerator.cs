using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ConsoleApp3
{
    class VirtualDataGenerator
    {
        public void Generate()
        {
            Random rnd = new Random();
            StringBuilder builder = new StringBuilder();

            for (int k = 0; k < 7; k++)
            {
                using (StreamReader sr = new StreamReader("distance_matrix.txt"))
                {
                    for (int i = 0; i < 180; i++)
                    {
                        List<int> generatedValues = new List<int>();

                        string line = sr.ReadLine();
                        line = sr.ReadLine();
                        string[] tokens = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

                        List<int> values = new List<int>();

                        foreach (var token in tokens)
                            values.Add(Convert.ToInt32(token));


                        for (int j = 0; j < 40; j++)
                        {

                            foreach (var value in values)
                            {
                                int t = value + rnd.Next(-300, 300);
                                if (t < 300) t = 300;
                                generatedValues.Add(t);
                            }
                        }

                        builder.AppendLine(String.Format("{0:0000},{1:0000}", k * 180 + i, k * 180 + i));
                        builder.AppendLine(String.Join(" ", generatedValues));
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter("distance_matrix_generated.txt", append: false))
                sw.WriteLine(builder.ToString());
            Console.WriteLine("ok");

            Console.ReadKey();
        }
    }
}
