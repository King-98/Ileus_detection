using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1.Ileus
{
    class util
    {
        public int[,] changeInt(double[,] featuremap)
        {
            // 피처맵을 받아 *1000하고 INT형으로 바꾸고 리턴

            int[,] featuremapInt = new int[featuremap.GetLength(0), featuremap.GetLength(1)];
            
            for(int y=0; y<featuremapInt.GetLength(1); y++)
            {
                for (int x = 0; x < featuremapInt.GetLength(0); x++)
                {
                    featuremapInt[x, y] = (int)(featuremap[x, y] * 1000);
                }
            }

            return featuremapInt;
        }

        public void MatrixPrint(int[,] Matrix)
        {
            // 매트릭스를 출력하는 것
            String str;

            for(int y=0; y<Matrix.GetLength(1); y++)
            {
                for (int x = 0; x < Matrix.GetLength(0); x++)
                {
                    str = String.Format("{0,5}, ", Matrix[x,y]);
                    Console.Write(str);
                }
                Console.WriteLine();
            }
        }
    }
}
