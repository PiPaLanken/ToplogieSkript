using System;
using System.Collections.Generic;
using System.Text;

namespace Topologie
{
    class DatenHandler
    {
        private string _input;
        List<int> RawInputList = new List<int>();
        List<int> NList = new List<int>();
        int RowSize = 0;

        public DatenHandler(string input)
        {
            _input = input;
        }
        public void GenerateTopologie()
        {
            SplitDataToIntList();
            var myItems = FirstInitialisierung();
            var verticalSumList = myItems;
            var horziontalSumList = GetHorizontalSum();
            DataRenderer(myItems);
            NextIteration(NList, verticalSumList, horziontalSumList);
        }

        void SplitDataToIntList()
        {
            try
            {
                foreach (char c in _input)
                    RawInputList.Add(Convert.ToInt32(c.ToString()));
            }
            catch
            {
                throw new Exception("Falsche Format... Bitte nur als MathML eingeben (Rechtsklick auf Matrix und anders Rendern lassen)\nBeispielaussehen: 010101000000111000000");
            }
        }

        List<int> FirstInitialisierung()
        {
            var UpDownSum = new List<int>();
            RowSize = (int)Math.Sqrt(RawInputList.Count);
            for (int k= 0; k < RowSize; k++)
            {
                int currentIndex = k;
                int currentSum = 0;
                for (int i = 0; i< RowSize; i++)
                {
                    if (currentIndex <= RawInputList.Count)
                    {
                        currentIndex = RowSize * i + k;
                        currentSum += RawInputList[currentIndex];
                    }
                }
                NList.Add(k+1);
                UpDownSum.Add(currentSum);
            }
            return UpDownSum;
        }
        List<int> GetHorizontalSum()
        {
            var LeftRight = new List<int>();
            int currentSum = 0;
            int currentRow = 1;
            for (int k = 0; k < RawInputList.Count; k++)
            {
                currentSum += RawInputList[k];
                if (k+1==currentRow*(RowSize))
                {
                    LeftRight.Add(currentSum);
                    currentSum = 0;
                    currentRow++;
                }
            }
            return LeftRight;
        }

        List<int> NextIteration(List<int> currentNList, List<int> verticalSumList, List<int> horizontalSumList, int it = 1)
        {
            if (it == verticalSumList.Count+1)
                return null;
            int highestIndex = -1;
            int lowestNumber = verticalSumList.Count;
            for (int a=0; a< verticalSumList.Count; a++)
            {
                if (currentNList.Contains(a+1))
                {
                    if (verticalSumList[a] < lowestNumber)
                    {
                        lowestNumber = verticalSumList[a];
                        highestIndex = a;
                    }
                    else if (verticalSumList[a] == lowestNumber && highestIndex<a)
                        highestIndex = a;
                }
            }
            highestIndex += 1;
            for (int a=0; a < RawInputList.Count; a++)
            {
                if (a>= (highestIndex - 1) * (RowSize)&& a<(highestIndex-1)*RowSize+RowSize)
                {
                    if (RawInputList[a] == 1&&verticalSumList[a-((highestIndex - 1) * 10)]!=0)
                        verticalSumList[a- ((highestIndex - 1) * 10)] -= 1;
                }
            }
            currentNList = RemoveNListItems(currentNList, highestIndex);
            DataRenderer(verticalSumList, currentNList, it, highestIndex);
            NextIteration(currentNList, verticalSumList, horizontalSumList, it + 1);
            return null;
        }
        List<int> RemoveNListItems(List<int> nList, int item)
        {
            var newList = nList;
            newList[newList.IndexOf(item)] = 0;
            return newList;
        }

        void DataRenderer(List<int> myItems,List<int>currentNList=null, int iterNr=0, int removedNumberK=0)
        {
            Console.WriteLine();
            string startInit = "[";
            string n = "";
            for (int i = 0; i < myItems.Count; i++)
            {
                startInit += myItems[i];
                n += (i + 1);
                if (i != myItems.Count - 1)
                {
                    n += ",";
                    startInit += ",";
                }
                else startInit += "]";
            }
            if (iterNr!=0)
                Console.Write($"Init {string.Format("{0:00}",iterNr)}");
            else
                Console.Write($"Init   ");
            if (removedNumberK != 0)
                Console.Write($"  K{string.Format("{0:00}",removedNumberK)} ");
            else Console.Write($" Start");
            Console.Write($"  {startInit}");
            if (currentNList!=null)
            {
                n = "";
                for (int i=0; i < currentNList.Count; i++)
                {
                    if (currentNList[i] != 0)
                        n += (i + 1)+",";
                }
                if (n!=""&&n[n.Length-1].Equals(','))
                    n= n.Remove(n.Length-1);
            }
            Console.Write("  N={" + n + "}");
        }
    }
}
