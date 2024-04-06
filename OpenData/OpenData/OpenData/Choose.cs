using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;

namespace OpenData
{
    public static class Choose
    {
        static List<string[]> list = new List<string[]>();

        static void Read()
        {
            using (TextFieldParser parser = new TextFieldParser(@"C:\Users\Ermol\Google Диск\Учёба\4 Курс\1 триместр\Распределёнка\Лаба №4\Численность постоянного городского населения.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");
                string[] fields = parser.ReadFields(); // сразу считываем, чтобы пропустить строку с заголовками
                while (!parser.EndOfData)
                {
                    //Обработка строки
                    fields = parser.ReadFields();
                    string[] newFields = new string[3];
                    newFields[0] = fields[1];
                    newFields[1] = fields[3];
                    newFields[2] = fields[4];
                    list.Add(newFields);
                }
            }
        }

        static double GetMas(int value, string name)
        {
            foreach(var i in list)
            {
                var year = i[0].Substring(8);
                if (Int32.Parse(year) == value)
                    if (name == i[1])
                        return Double.Parse(i[2]); ;
            }

            return 0;
        }

        static bool Make()
        {
            var count = list.Count;
            Random rnd = new Random();
            var value = rnd.Next(0, count - 1);
            var year = list[value][0].Substring(8);
            var name = list[value][1];
            var num = Double.Parse(list[value][2]);

            value = rnd.Next(0, 16);
            while (value == Int32.Parse(year))
            {
                value = rnd.Next(0, 16);
            }

            var newNum = GetMas(value, name);

            Console.WriteLine($"{num} {newNum}");

            if (num >= newNum)
                return true;
            else
                return false;
        }
        
        public static bool Get()
        {
            Read();
            return Make();
        }
    }
}
