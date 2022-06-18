using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1.Ileus
{
    class Learning
    {
        public int[,] Average(int[][,] filter)
        {
            // 필터의 평균값구하기
            int[,] avg = new int[filter[0].GetLength(0), filter[0].GetLength(1)];
            int sum = 0;

            for (int y = 0; y < avg.GetLength(1); y++)
            {
                for (int x = 0; x < avg.GetLength(0); x++)
                {
                    sum = 0;
                    for (int fil = 0; fil < filter.Length; fil++)
                        sum += filter[fil][x, y];

                    avg[x, y] = sum / filter.Length;
                }
            }

            return avg;
        }

        public int[,] Average481(int[][,] filter)
        {
            // 필터의 평균값구하기
            int[,] avg = new int[filter[0].GetLength(0), filter[0].GetLength(1)];
            int sum = 0;

            for (int y = 0; y < avg.GetLength(1); y++)
            {
                for (int x = 0; x < avg.GetLength(0); x++)
                {
                    sum = 0;
                    for (int fil = 0; fil < 327; fil++)
                        sum += filter[fil][x, y];

                    avg[x, y] = sum / 327;
                }
            }

            return avg;
        }

        public int[,] Average40(int[][,] filter)
        {
            // 필터의 평균값구하기
            int[,] avg = new int[filter[0].GetLength(0), filter[0].GetLength(1)];
            int sum = 0;

            for (int y = 0; y < avg.GetLength(1); y++)
            {
                for (int x = 0; x < avg.GetLength(0); x++)
                {
                    sum = 0;
                    for (int fil = 0; fil < 327; fil++)
                        sum += filter[fil][x, y];

                    avg[x, y] = sum / 327;
                }
            }

            return avg;
        }

        public int[,] RMSE(int[,] avg, int[][,] filter)
        {
            // 평균 제곱 오차 구하기
            int[,] result = new int[avg.GetLength(0), avg.GetLength(1)];
            double sum = 0;

            for (int y = 0; y < avg.GetLength(1); y++)
            {
                for (int x = 0; x < avg.GetLength(0); x++)
                {
                    sum = 0;
                    for (int fil = 0; fil < filter.Length; fil++)
                    {
                        sum += Math.Pow((avg[x, y] - filter[fil][x, y]), 2);
                    }
                    result[x, y] = (int)(Math.Sqrt(sum / filter.Length));
                }
            }

            return result;
        }

        public double featureRMSE(int[][,] featureMap, int[][,] answer)
        {
            // 예측값과 정답값을 RMSE
            double result;
            double sum = 0;

            for (int y = 0; y < featureMap[0].GetLength(1); y++)
            {
                for (int x = 0; x < featureMap[0].GetLength(0); x++)
                {
                    for (int fil = 0; fil < featureMap.Length; fil++)
                    {
                        sum += Math.Abs(answer[fil][x, y] - featureMap[fil][x, y]);
                    }
                }
            }

            result = (Math.Sqrt(sum / (featureMap.Length * featureMap[0].GetLength(0) * featureMap[0].GetLength(1))));

            return result;
        }

        public double featureRMSE_2(int[,] featureMap, int[,] answer)
        {
            // 예측값과 정답값을 RMSE
            double result;
            double sum = 0;

            for (int y = 0; y < featureMap.GetLength(1); y++)
            {
                for (int x = 0; x < featureMap.GetLength(0); x++)
                {
                    sum += Math.Abs(answer[x, y] - featureMap[x, y]);
                }
            }

            result = (Math.Sqrt(sum / (featureMap.GetLength(0) * featureMap.GetLength(1))));

            return result;
        }

        public int[,] Stretch(int[,] dim2_array, int total)
        {
            // 2차원 배열의 전체값에 대한 스트레칭(낮을수록 높은값을 반환)
            // 입력 : 2차원배열, 늘릴 최대값
            int[,] result = new int[dim2_array.GetLength(0), dim2_array.GetLength(1)];
            int max = 0;
            int min = Int32.MaxValue;

            foreach (int i in dim2_array)
            {
                if (max < i)
                    max = i;
                if (min > i)
                    min = i;
            }

            for (int y = 0; y < result.GetLength(1); y++)
                for (int x = 0; x < result.GetLength(0); x++)
                    result[x, y] = total - (int)(((double)(dim2_array[x, y] - min) / (max - min)) * total);

            return result;
        }
    }
}
