using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestExcelPass
{
    public static class Instrument
    {
        public static char[] symbols = new char[] {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'/*,
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ч', 'ц', 'ш', 'щ', 'ь', 'ъ', 'ы', 'э', 'ю', 'я',
            'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ч', 'Ц', 'Ш', 'Щ', 'Ь', 'Ъ', 'Ы', 'Э', 'Ю', 'Я',
            '+', '/', '\\', '*', '_', ' ', '!', ':', ';', '>', '<', '&', '%', '$', '#', '@', '^', '(', '\'', '\"', '|', '(', ')', '~'*/
        };

        public static void IncrementString(ref byte[] mass)
        {
            int i = symbols.Length - 1;

            while (true)
            {
                if (mass[i] == symbols.Length - 1)
                {
                    mass[i] = 0;
                }
                else
                {
                    mass[i]++;
                    break;
                }

                i--;
            }
        }
    }
}
