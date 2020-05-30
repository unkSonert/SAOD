using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace List
{
    public static class ExtMemes
    {
        public static int IndexOf<T>(this LinkedList<T> list, T find)
        {
            int memes = -1;
            foreach (var iter in list)
            {
                ++memes;

                if (iter.Equals(find))
                    break;
            }
            return memes;
        }
    }
}
