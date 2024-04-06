using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary.GameObject
{
    public interface IMap
    {
        TypeCell this [int x, int y] { get; }
    }
}
