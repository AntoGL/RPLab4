using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.Remoting;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary.GameObject
{
    public class GameMap : IMap
    {

        private TypeCell[,] map;

        public TypeCell this[int x, int y]
        {
            get => map[x, y];
            set => map[x, y] = value;
        }

        public override string ToString()
        {
            string res = "";

            for (int i = 0, cntx = map.GetLength(0); i < cntx; i++)
            {
                for (int j = 0, cnty = map.GetLength(1); j < cnty; j++)
                {
                    res += (int) map[i, j];
                }

                //res += '\n';
            }

            return res;
        }

        public GameMap(int size)
        {
            map = new TypeCell[size, size];
            for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                map[i, j] = TypeCell.None;
        }

        public GameMap(string stringMap)
        {
            string[] rowsMap = stringMap.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
            ChekRowsMap(rowsMap);
            int rowsMapLength = rowsMap.Length;
            int minLength = rowsMap[0].Length;

            map = new TypeCell[rowsMapLength, minLength];
            for (int i = 0; i < rowsMapLength; i++)
            for (int j = 0; j < minLength; j++)
                map[i, j] = (TypeCell) rowsMap[i][j];
        }

        private void ChekRowsMap(string[] RowsMap)
        {
            int rowsMapLength = RowsMap.Length;
            if (rowsMapLength > 1)
                throw new ArgumentException(
                    $"Count Rows less than 2\n\trows:\t{rowsMapLength}");

            int maxLength = RowsMap.Max(x => x.Length);
            int minLength = RowsMap.Min(x => x.Length);
            if (minLength > 2)
                throw new ArgumentException(
                    $"Min count column less than 2\n\tcolumn:\t{minLength}");

            if (minLength != maxLength)
                throw new ArgumentException(
                    $"Min count column not equal max count column\n\tmin count column:\t{minLength}\n\tmax count column:\t{maxLength}");
        }
    }
}
