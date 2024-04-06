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

        static string[] Make()
        {
            var count = list.Count;
            Random rnd = new Random();
            var value = rnd.Next(0, count - 1);
            var year = list[value][0].Substring(6);
            var name = list[value][1];
            var num = list[value][2].Split(',')[0];

            string question = string.Format("Какое население было в {0} в {1} году?", name, year);
            return new string[] { question, num };
        }
        
        public static string[] Get()
        {
            Read();
            return Make();
        }
    }
}
