using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace TestExcelPass
{
    /// <summary>
    /// Обход паролей
    /// </summary>
    public class SearcherPass
    {
        private List<byte> finderMass { get; set; }
        private int countIteration { get; set; }
        private int currIter { get; set; }

        public SearcherPass()
        {
            this.finderMass = new List<byte>() { 0 };
            currIter = 0;
        }

        /// <summary>
        ///  Заполнение массива по-умолчанию
        /// </summary>
        /// <param name="numbBit">Номер разряда</param>
        /// <param name="val">Значение старшего разряда</param>
        public void SetBitDefault(int numbBit, byte val)
        {
            if (val == 0)
            {
                this.finderMass = new List<byte>() { 0 };
                return;
            }

            List<byte> tempMass = new List<byte>();
            tempMass.Add(val);
            for (int i = 1; i < numbBit; i++)
            {
                tempMass.Add(0);
            }

            tempMass.Reverse();
            this.finderMass = tempMass;

            countIteration = (int)Math.Pow(Instrument.symbols.Count(), numbBit - 1);
        }

        /// <summary>
        /// Получение строки из массива байтов в текущем состоянии
        /// </summary>
        /// <returns>Результирующая строка</returns>
        public string GetString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte c in this.finderMass.ToArray().Reverse().ToArray())
            {
                try
                {
                    stringBuilder.Append(Instrument.symbols[c]);
                }
                catch (Exception ex)
                {
                    int jjj = 0;
                }
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Ввод начального значения пароля
        /// </summary>
        /// <param name="str">Начальный пароль, с которого будет продолжаться поиск</param>
        public void SetByteArray(string str)
        {
            List<byte> vs = new List<byte>();
            List<char> symb = Instrument.symbols.ToList();
            foreach (var c in str)
            {
                vs.Add((byte)symb.IndexOf(c));
            }

            vs.Reverse();
            this.finderMass = vs;
        }

        /// <summary>
        /// Инкрементация значения
        /// </summary>
        public bool IncrementByteArray()
        {
            bool result = false;

            if (currIter == countIteration)
            {
                return false;
            }
            int couuntArray = Instrument.symbols.Length;
            bool isNeedAdd = true;

            int iter = 0;

            while (iter < this.finderMass.Count)
            {
                if (isNeedAdd)
                {
                    if (this.finderMass[iter] == (couuntArray - 1))
                    {
                        // Если текущий код в элементе массива равен предпоследнему коду в массиве символов
                        this.finderMass[iter] = 0;  // Обнуление

                        if (iter == (this.finderMass.Count - 1))
                        {
                            // Проверка, является ли текущая позиция в массиве последней позицией.
                            // Если да, то нужно добавить в массив 0
                            this.finderMass.Add(0);
                            break;
                        }
                        else
                        {
                            isNeedAdd = true;
                            iter++;
                            continue;
                        }
                    }
                    else
                    {
                        // Если элемент в массиве не является последним из символов, то нарастить и перейти на следующий этап цикла
                        // Если нужно нарастить
                        this.finderMass[iter]++;
                        isNeedAdd = false;
                    }
                }

                iter++;
            }

            currIter++;
            result = true;
            return result;
        }

        /// <summary>
        /// Получение генерируемого массива байтов
        /// </summary>
        /// <returns>Массив байтов</returns>
        public byte[] GetByteArray()
        {
            return this.finderMass.ToArray().Reverse().ToArray();
        }
    }
}