using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1.Ileus
{
    class Extraction
    {
        public List<int> extend(Bitmap bitmap)
        {
            // 한번의 사이클에 모든좌표 출력
            List<int> XY = new List<int>();

            Color color;
            int flag;
            int backflag = 0;
            int count = 0;
            int maxX = int.MinValue;
            int minX = int.MaxValue;
            int startY = 0;

            for (int y = 0; y < bitmap.Height; y++)
            {
                flag = 0;

                for (int x = 0; x < bitmap.Width; x++)
                {
                    color = bitmap.GetPixel(x, y);
                    if (color.R > 200 && color.B < 50)
                    {
                        flag = 1;
                        maxX = Math.Max(x, maxX);
                        minX = Math.Min(x, minX);
                    }
                }

                if (flag != backflag) // 영역이 바꼈을때
                {
                    count += 1;
                    if (count % 2 == 0)
                    {
                        XY.Add(minX);
                        XY.Add(startY);
                        XY.Add(maxX);
                        XY.Add(y - 1); //endY

                        maxX = int.MinValue;
                        minX = int.MaxValue;
                    }
                    startY = y; // 시작y는 따로 기록
                }

                backflag = flag;
            }

            return XY;
        }

        public List<int> extend_2(Bitmap bitmap)
        {
            // 멀리있는 빨간색까지 추출하는 현상 막기
            List<int> AllXY = new List<int> { };
            Color color;

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++) {
                    color = bitmap.GetPixel(x, y);
                    if (color.R > 200 && color.B < 50)
                    {
                        // 오른쪽 좌표업데이트
                    }
                }
            }

            return AllXY;
        }

        public Bitmap normAllFilterBit(Bitmap Cleansing, Bitmap Original, List<int> AllXY, int idx)
        {
            // 하나의 사진에 대한 그냥필터 모두 저장 (Nomalization)
            // idx : 개수

            if (!System.IO.Directory.Exists(fix.DataFolder + @"\Extraction Data")) // 폴더 있는지 확인
                System.IO.Directory.CreateDirectory(fix.DataFolder + @"\Extraction Data");

            Bitmap bitmap = new Bitmap(AllXY[(idx * 4) + 2] - AllXY[(idx * 4)], AllXY[(idx * 4) + 3] - AllXY[(idx * 4) + 1] + 1);
            Color color;
            int x2 = 0;
            int y2 = 0;

            for (int y = AllXY[(idx * 4) + 1]; y < AllXY[(idx * 4) + 3]; y++)
            {
                x2 = 0;
                for (int x = AllXY[(idx * 4) + 0]; x < AllXY[(idx * 4) + 2]; x++)
                {
                    color = Original.GetPixel(x, y);
                    bitmap.SetPixel(x2, y2, color);
                    x2++;
                }
                y2++;
            }

            return bitmap;
        }

        public void Norm_2(Bitmap[][] bitmap)
        {
            // 2차원 클렌징데이터들 모두 저장
            if (!System.IO.Directory.Exists(fix.DataFolder + @"\Extraction Data")) // 폴더 있는지 확인
                System.IO.Directory.CreateDirectory(fix.DataFolder + @"\Extraction Data");

            int width = 0;
            int height = 0;
            int count = 0;

            for (int i = 0; i < bitmap.Length; i++)
            {
                for (int num = 0; num < bitmap[i].Length; num++)
                {
                    width += bitmap[i][num].Width;
                    height += bitmap[i][num].Height;
                    count++;
                }
            }

            Size resize;
            Image image;
            int label = 0;
            for (int i = 0; i < bitmap.Length; i++)
            {
                for (int num = 0; num < bitmap[i].Length; num++)
                {
                    resize = new Size(width / count, height / count);
                    bitmap[i][num] = new Bitmap(bitmap[i][num], resize);
                    image = bitmap[i][num];

                    image.Save(fix.DataFolder + @"\Extraction Data\filter" + label + ".png", System.Drawing.Imaging.ImageFormat.Jpeg);
                    label++;
                }
            }
        }

        public void Norm_2_Blackup(Bitmap[][] bitmap)
        {
            // 맨밑에 올리는 2차원 클렌징데이터들 모두 저장
            ImageProcessing ip = new ImageProcessing();

            if (!System.IO.Directory.Exists(fix.DataFolder + @"\Extraction Data")) // 폴더 있는지 확인
                System.IO.Directory.CreateDirectory(fix.DataFolder + @"\Extraction Data");

            int width = 0;
            int height = 0;
            int count = 0;

            for (int i = 0; i < bitmap.Length; i++)
            {
                for (int num = 0; num < bitmap[i].Length; num++)
                {
                    width += bitmap[i][num].Width;
                    height += bitmap[i][num].Height;
                    count++;
                }
            }

            Size resize;
            Image image;
            int label = 0;
            for (int i = 0; i < bitmap.Length; i++)
            {
                for (int num = 0; num < bitmap[i].Length; num++)
                {
                    resize = new Size(width / count, height / count);
                    bitmap[i][num] = new Bitmap(bitmap[i][num], resize);
                    bitmap[i][num] = ip.Blackup(bitmap[i][num]);
                    image = bitmap[i][num];

                    image.Save(fix.DataFolder + @"\Extraction Data\filter" + label + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    label++;
                }
            }
        }

        public int[,] BoundGA(int[,] totalGA, int[,] face, int startX, int startY)
        {
            // 원래이미지의 얼굴크기만큼 잘라서 grayarray로 바꿔줌
            int[,] obj = new int[face.GetLength(0), face.GetLength(1)];

            for (int y = startY; y < (startY + face.GetLength(1)); y++)
                for (int x = startX; x < (startX + face.GetLength(0)); x++)
                {
                    obj[x - startX, y - startY] = totalGA[x, y];
                }

            return obj;
        }
    }
}
