using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1.Ileus
{
    class CAM
    {
        public int[,] pretraind_featureMap_2level(int[,] original)
        {
            // 가중치를 이용한 Feature Map 생성
            ImageProcessing IP = new ImageProcessing();
            Run run = new Run();
            Learning learning = new Learning();
            Bitmap bitmap;

            // 이미지개수만큼 지정해서 Image Load함
            Image image;
            int[][,] filter = new int[fix.NumOfFilter][,];
            for (int i = 0; i < fix.NumOfFilter; i++)
            {
                image = Image.FromFile(fix.DataFolder + @"\Extraction Data\filter" + i + ".jpg");
                bitmap = new Bitmap(image);
                filter[i] = IP.GrayArray(bitmap);
            }

            // 한 픽셀의 빨간색부분인 평균값을 구한다
            int[,] avg = learning.Average(filter);
            int[,] similarity = run.similarity();
            // Console.WriteLine("필터유사도 학습완료");

            int paddingW = avg.GetLength(0) / 2;
            int paddingH = avg.GetLength(1) / 2;
            int[,] oriTemp;
            int[,] FM_2level = new int[original.GetLength(0) - paddingW * 2, original.GetLength(1) - paddingH * 2];

            for (int y = paddingH; y < original.GetLength(1) - paddingH; y++)
            {
                for (int x = paddingW; x < original.GetLength(0) - paddingW; x++)
                {
                    oriTemp = new int[avg.GetLength(0), avg.GetLength(1)];

                    for (int i = 0; i < avg.GetLength(1); i++)
                        for (int j = 0; j < avg.GetLength(0); j++)
                            oriTemp[j, i] = original[x - paddingW + j, y - paddingH + i];

                    FM_2level[x - paddingW, y - paddingH] = MaS_3_2(oriTemp, avg, similarity); // 자른거 끼리 모아가지고 MaS
                }
            }

            return FM_2level;
        }

        public int[,] pretraind_featureMap_2level_test(int[,] original)
        {
            // 가중치를 이용한 Feature Map 생성
            ImageProcessing IP = new ImageProcessing();
            Run run = new Run();
            Learning learning = new Learning();
            Bitmap bitmap;

            // 이미지개수만큼 지정해서 Image Load함
            Image image;
            int[][,] filter = new int[fix.NumOfFilter][,];
            for (int i = 0; i < fix.NumOfFilter; i++)
            {
                image = Image.FromFile(fix.DataFolder + @"\Extraction Data\filter" + i + ".jpg");
                bitmap = new Bitmap(image);
                filter[i] = IP.GrayArray(bitmap);
            }

            // 한 픽셀의 빨간색부분인 평균값을 구한다
            int[,] avg = learning.Average(filter);
            int[,] similarity = run.similarity();
            Console.WriteLine("필터유사도 학습완료");

            int paddingW = avg.GetLength(0) / 2;
            int paddingH = avg.GetLength(1) / 2;
            int[,] oriTemp;
            int[,] FM_2level = new int[original.GetLength(0) - paddingW * 2, original.GetLength(1) - paddingH * 2];

            for (int y = paddingH; y < original.GetLength(1) - paddingH; y++)
            {
                for (int x = paddingW; x < original.GetLength(0) - paddingW; x++)
                {
                    oriTemp = new int[avg.GetLength(0), avg.GetLength(1)];

                    for (int i = 0; i < avg.GetLength(1); i++)
                        for (int j = 0; j < avg.GetLength(0); j++)
                            oriTemp[j, i] = original[x - paddingW + j, y - paddingH + i];

                    FM_2level[x - paddingW, y - paddingH] = MaS_3_2_nosim(oriTemp, avg, similarity); // 자른거 끼리 모아가지고 MaS
                }
            }

            return FM_2level;
        }

        public int[][,] pretraind_featureMap_2level_temp(int[,] similarity)
        {
            // 가중치를 이용한 Feature Map 생성
            ImageProcessing IP = new ImageProcessing();
            Run run = new Run();
            Learning learning = new Learning();
            Bitmap bitmap;

            // 이미지개수만큼 지정해서 Image Load함
            Image image;
            int[][,] filter = new int[327][,];
            for (int i = 0; i < 327; i++)
            {
                image = Image.FromFile(fix.DataFolder + @"\Extraction Data\filter" + i + ".jpg");
                bitmap = new Bitmap(image);
                filter[i] = IP.GrayArray(bitmap);
            }
            
            int[][,] FM_2level = new int[filter.Length][,];

            for (int i = 0; i < filter.Length; i++)
            {
                FM_2level[i] = new int[filter[0].GetLength(0), filter[0].GetLength(1)];
                for (int y = 0; y < filter[0].GetLength(1); y++)
                {
                    for (int x = 0; x < filter[0].GetLength(0); x++)
                    {
                        FM_2level[i][x, y] = filter[i][x, y] * similarity[y, x];
                        if (FM_2level[i][x, y] < 0)
                            Console.Write("여기");
                    }
                }
            }

            return FM_2level;
        }

        public int[][,] pretraind_featureMap_2level_no_sim()
        {
            // 가중치를 이용한 Feature Map 생성
            ImageProcessing IP = new ImageProcessing();
            Run run = new Run();
            Learning learning = new Learning();
            Bitmap bitmap;

            // 이미지개수만큼 지정해서 Image Load함
            Image image;
            int[][,] filter = new int[327][,];
            for (int i = 0; i < 327; i++)
            {
                image = Image.FromFile(fix.DataFolder + @"\Extraction Data\filter" + i + ".jpg");
                bitmap = new Bitmap(image);
                filter[i] = IP.GrayArray(bitmap);
            }

            int[,] avg = learning.Average(filter);
            int[,] similarity = run.similarity();
            Console.WriteLine("필터유사도 학습완료");

            int[][,] FM_2level = new int[filter.Length][,];

            for (int i = 0; i < filter.Length; i++)
            {
                FM_2level[i] = new int[filter[0].GetLength(0), filter[0].GetLength(1)];
                for (int y = 0; y < filter[0].GetLength(1); y++)
                {
                    for (int x = 0; x < filter[0].GetLength(0); x++)
                    {
                        FM_2level[i][x, y] = filter[i][x, y] * similarity[x,y];
                    }
                }
            }

            return FM_2level;
        }

        public int[,] change(int[,] FM_2level)
        {
            // 피처맵을 255사이값으로 바꿈
            var MaxMin = this.MaxMin(FM_2level);

            for (int y = 0; y < FM_2level.GetLength(1); y++)
                for (int x = 0; x < FM_2level.GetLength(0); x++)
                {
                    FM_2level[x, y] = maxminStretch_2(FM_2level[x, y], MaxMin.Item1, MaxMin.Item2, 255);
                    if (FM_2level[x, y] < 0)
                        Console.Write("여기");
                }

            return FM_2level;
        }

        public Tuple<int, int> MaxMin(int[,] FM_2level)
        {
            // 해당 2차원배열의 제일 큰값과 작은값을 리턴
            int max = Int32.MinValue;
            int min = Int32.MaxValue;

            for (int y = 0; y < FM_2level.GetLength(1); y++)
                for (int x = 0; x < FM_2level.GetLength(0); x++)
                {
                    if (FM_2level[x, y] > max)
                        max = FM_2level[x, y];
                    if (FM_2level[x, y] < min)
                        min = FM_2level[x, y];
                }

            return new Tuple<int, int>(max, min);
        }

        public int maxminStretch_2(int x, int max, int min, int limit)
        {
            // 해당값, 최대, 최소를 받아서 0~255까지 값으로 만들어줌
            if((max - min) == 0)
            {
                return 0;
            }
            return (int)((x - min) * ((double)limit / (max - min)));
        }

        public int MaS_3_2(int[,] original, int[,] filtering, int[,] similality)
        {
            // 원본과 필터를 곱해서 MaS (흰색255를 찾음)
            int result = 0;

            for (int i = 0; i < original.GetLength(0); i++)
                for (int j = 0; j < original.GetLength(1); j++)
                    result += (255 - Math.Abs(original[i, j] - filtering[i, j])) * similality[i, j];

            return result;
        }

        public int MaS_3_2_nosim(int[,] original, int[,] filtering, int[,] similality)
        {
            // 원본과 필터를 곱해서 MaS (흰색255를 찾음)
            int result = 0;

            for (int i = 0; i < original.GetLength(0); i++)
                for (int j = 0; j < original.GetLength(1); j++)
                    result += (255 - Math.Abs(original[i, j] - filtering[i, j]));

            return result;
        }

        public int[,] fusion(int[,] HaarImage, int[,] filterImage)
        {
            // 피처맵 두개를 합치는 메소드

            // 차이를 절대값으로 만들어줌
            int widthMin = Math.Min(HaarImage.GetLength(0),filterImage.GetLength(0));
            int heightMin = Math.Min(HaarImage.GetLength(1),filterImage.GetLength(1));

            // 초기화
            int[,] FusionImage = new int[widthMin, heightMin];
            int haarXpad = (widthMin == HaarImage.GetLength(0)) ? 0 : (HaarImage.GetLength(0)-widthMin)/2;
            int haarYpad = (widthMin == HaarImage.GetLength(1)) ? 0 : (HaarImage.GetLength(1)-heightMin)/2;
            int filterXpad = (widthMin == filterImage.GetLength(0)) ? 0 : (filterImage.GetLength(0) - widthMin) / 2;
            int filterYpad = (widthMin == filterImage.GetLength(1)) ? 0 : (filterImage.GetLength(1) - heightMin) / 2;
            double HaarTemp = 0;
            double filterTemp = 0;

            double haarbias = 0.6;
            double filterbias = 0.4;

            // 값넣기
            for (int y = 0; y < FusionImage.GetLength(1); y++)
            {
                for (int x = 0; x < FusionImage.GetLength(0); x++)
                {
                    HaarTemp = (HaarImage[x + haarXpad, y + haarYpad] * haarbias);
                    filterTemp = (filterImage[x + filterXpad, y + filterYpad] * filterbias);
                    FusionImage[x, y] = (int)(HaarTemp + filterTemp);
                }
            }

            return FusionImage;
        }

        public int[,] fusion_bias(int[,] HaarImage, int[,] filterImage, int bias)
        {
            // 피처맵 두개를 합치는 메소드

            // 차이를 절대값으로 만들어줌
            int widthMin = Math.Min(HaarImage.GetLength(0), filterImage.GetLength(0));
            int heightMin = Math.Min(HaarImage.GetLength(1), filterImage.GetLength(1));

            // 초기화
            int[,] FusionImage = new int[widthMin, heightMin];
            int haarXpad = (widthMin == HaarImage.GetLength(0)) ? 0 : (HaarImage.GetLength(0) - widthMin) / 2;
            int haarYpad = (widthMin == HaarImage.GetLength(1)) ? 0 : (HaarImage.GetLength(1) - heightMin) / 2;
            int filterXpad = (widthMin == filterImage.GetLength(0)) ? 0 : (filterImage.GetLength(0) - widthMin) / 2;
            int filterYpad = (widthMin == filterImage.GetLength(1)) ? 0 : (filterImage.GetLength(1) - heightMin) / 2;
            double HaarTemp = 0;
            double filterTemp = 0;

            // 바이어스 정제
            double filterbias = bias * 0.1;
            double haarbias = 1 - filterbias;

            // 값넣기
            for (int y = 0; y < FusionImage.GetLength(1); y++)
            {
                for (int x = 0; x < FusionImage.GetLength(0); x++)
                {
                    HaarTemp = (HaarImage[x + haarXpad, y + haarYpad] * haarbias);
                    filterTemp = (filterImage[x + filterXpad, y + filterYpad] * filterbias);
                    FusionImage[x, y] = (int)(HaarTemp + filterTemp);
                }
            }

            return FusionImage;
        }

        public int[,] MaxPooling(int[,] featuremap)
        {
            int[,] poolingmap = new int[featuremap.GetLength(0)/2,featuremap.GetLength(1)/2];
            int max = 0;

            for (int y = 0; y < featuremap.GetLength(1); y+=2)
            {
                for (int x = 0; x < featuremap.GetLength(0); x+=2)
                {
                    max = Math.Max(featuremap[x, y], Math.Max(featuremap[x + 1, y], Math.Max(featuremap[x, y + 1], featuremap[x + 1, y + 1]) ) );
                    poolingmap[x / 2, y / 2] = max;
                }
            }

            return poolingmap;
        }
    }
}
