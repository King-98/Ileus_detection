using System;
using System.Drawing;

namespace _1.Ileus
{
    class Create
    {
        public int[,] randompattern(int width, int height)
        {
            // 가로세로길이를 받아서 (0,255)배열을 반환
            Random rand = new Random();
            int[,] grayarray = new int[width, height];

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    if (rand.Next(2) == 0)
                        grayarray[x, y] = 0;
                    else
                        grayarray[x, y] = 255;
                }

            return grayarray;
        }

        public float sim(int[,] totalGA, int[,] objGA)
        {
            // 전체그레이와 객체그레이을 받아서 유사도(0~1) 반환(맨 뒷칸
            float white = 0, black = 0;
            int count = 0;
            int startX = totalGA.GetLength(0) - objGA.GetLength(0);
            int startY = totalGA.GetLength(1) - objGA.GetLength(1);

            for (int y = 0; y < totalGA.GetLength(1) - startY; y++)
                for (int x = 0; x < totalGA.GetLength(0) - startX; x++)
                {
                    if (objGA[x, y] == 0) // 검은색일때
                        black += (255 - totalGA[x + startX, y + startY]) / (float)255; // 검색 0
                    else
                        white += totalGA[x + startX, y + startY] / (float)255; // 흰색 1
                    count++;
                }

            return (black + white) / count;
        }

        public int[,] randombrightP(int width, int height)
        {
            // 가로세로길이를 받아서 (0~255)배열을 반환
            Random rand = new Random();
            int[,] grayarray = new int[width, height];

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    grayarray[x, y] = rand.Next(256);

            return grayarray;
        }

        public int[,] filter(int width, int height)
        {
            // 장폐색에 들어갈 haar like feature 필터 맨들기
            int[,] grayarray = new int[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if(height/2<y)
                        grayarray[x, y] = 255;
                    else
                        grayarray[x, y] = 0;
                }
            }

            return grayarray;
        }

        public int[,] filterㄴ(int width, int height)
        {
            // 장폐색에 들어갈 haar like feature 필터 맨들기
            int[,] grayarray = new int[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (height / 2 < y && width / 2 < x)
                        grayarray[x, y] = 255;
                    else
                        grayarray[x, y] = 0;
                }
            }

            return grayarray;
        }
    }
}