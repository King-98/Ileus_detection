using OpenCvSharp;
using OpenCvSharp.Blob;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1.Ileus
{
    static class fix
    {
        public const int NumOfImage = 120;
        public const int NumOfFilter = 654;
        // public const string DataFolder = @"E:\개인프젝자료\장폐색\1. Data";
        public const string DataFolder = @"..\..\..\1. Data";
        public const int Width = 1000;
        public const int Height = 1000;
        public const int MaxColor = 256;
    }

    class Run
    {
        Create c = new Create();
        CAM cam = new CAM();
        Extraction ex = new Extraction();
        util u = new util();
        ImageProcessing ip = new ImageProcessing();
        Learning learning = new Learning();
        Comparison com = new Comparison();
        OpenCV cv = new OpenCV();

        public void All_File_ROI_Save()
        {
            // 모든 파일의 클렌징 데이터 ROI해서 저장
            Extraction coo = new Extraction();

            Image Cleansing_Image;
            Bitmap Cleansing;
            Image Original_Image;
            Bitmap Original;
            List<int> AllXY;
            Bitmap[][] bitmap = new Bitmap[fix.NumOfImage][];

            for (int i = 0; i < fix.NumOfImage; i++)
            {
                Console.Clear();
                Console.WriteLine("진행률 : " + (float)i / (fix.NumOfImage - 1) * 100 + "%");
                // 파일 불러오기
                Cleansing_Image = Image.FromFile(fix.DataFolder + @"\Cleansing Data\ileus2 (" + (i + 1) + ").jpg");
                Original_Image = Image.FromFile(fix.DataFolder + @"\Original Data\ileus2 (" + (i + 1) + ").jpg");
                Cleansing = new Bitmap(Cleansing_Image, fix.Width, fix.Height);
                Original = new Bitmap(Original_Image, fix.Width, fix.Height);

                // 좌표구하기
                AllXY = coo.extend(Cleansing);
                bitmap[i] = new Bitmap[AllXY.Count() / 4];

                // 이미지 옮기기
                for (int num = 0; num < AllXY.Count() / 4; num++)
                    bitmap[i][num] = coo.normAllFilterBit(Cleansing, Original, AllXY, num);

                Cleansing_Image.Dispose();
                Original_Image.Dispose();
            }

            // 정규화 및 저장
            coo.Norm_2_Blackup(bitmap);
        }

        public int[,] similarity()
        {
            // 하나의 사진에 대한 필터의 유사도를 구함
            ImageProcessing IP = new ImageProcessing();
            const int NumOfImg = 6; // 이미지 개수
            Bitmap bitmap;
            Learning learning = new Learning();

            // 이미지개수만큼 지정해서 Image Load함
            Image image = Image.FromFile(fix.DataFolder + @"\Extraction Data\filter0.jpg"); // 경로는 역슬래쉬로
            int[][,] filter = new int[NumOfImg][,];
            for (int i = 0; i < NumOfImg; i++)
            {
                image = Image.FromFile(fix.DataFolder + @"\Extraction Data\filter" + i + ".jpg");
                //                     @는 utf-8로 변환해서 한글도 가능하게 해줌
                bitmap = new Bitmap(image);
                filter[i] = IP.GrayArray(bitmap);
            }

            // 한 픽셀의 빨간색부분인 평균값을 구한다
            int[,] avg = learning.Average(filter);

            // 평균제곱오차 RMSE 구하기
            int[,] rmse = learning.RMSE(avg, filter);

            // 유사도 구하기
            int[,] similality = learning.Stretch(rmse, 100);

            return similality;
        }

        public Bitmap horizontal(Bitmap bitmap)
        {
            // 수평마스크적용하고 위에 밑에 자르기
            ImageProcessing ip = new ImageProcessing();
            Extraction cd = new Extraction();

            // 피처맵 생성
            int[,] Original = ip.GrayArray(bitmap);
            int[,] featureMap = cam.pretraind_featureMap_2level(Original);
            Console.WriteLine("피처맵생성완료");

            // 255사이의 값으로 변환
            featureMap = cam.change(featureMap);

            // 수평마스크
            featureMap = ip.HorizontalMask(featureMap);

            // 빨간색 색칠
            Bitmap newbit = ip.BiasConvert(featureMap, 100);
            Console.WriteLine("이미지처리완료");

            // 위에 밑에 자르기
            newbit = ip.UPandDOWN(newbit);

            // 빨간색 개수찾기
            List<int> red = cd.extend(newbit);
            Console.WriteLine("장폐색 개수 : " + red.Count() / 4);

            return newbit;
        }

        public Bitmap Overlap(Bitmap bitmap)
        {
            // 원본이랑 오버랩
            Extraction cd = new Extraction();

            // 피처맵 생성
            int[,] Original = ip.GrayArray(bitmap);
            int[,] featureMap = cam.pretraind_featureMap_2level(Original);
            Console.WriteLine("피처맵생성완료");

            // 255사이의 값으로 변환
            featureMap = cam.change(featureMap);

            // 수평마스크
            featureMap = ip.HorizontalMask(featureMap);

            // 원본에 빨간색 색칠
            Bitmap newbit = ip.BiasConvert_original(featureMap, bitmap, 40);

            // 위에 밑에 자르기
            newbit = ip.UPandDOWN(newbit);

            // 빨간색 개수찾기
            List<int> red = cd.extend(newbit);
            Console.WriteLine("이미지처리완료");

            Console.WriteLine("장폐색 개수 : " + red.Count() / 4);

            return newbit;
        }

        public int[,] Overlap_Matrix(Bitmap bitmap)
        {
            // 원본이랑 오버랩
            ImageProcessing ip = new ImageProcessing();
            Extraction cd = new Extraction();

            // 피처맵 생성
            int[,] Original = ip.GrayArray(bitmap);
            int[,] featureMap = cam.pretraind_featureMap_2level(Original);

            // 255사이의 값으로 변환
            featureMap = cam.change(featureMap);

            // 수평마스크
            featureMap = ip.HorizontalMask(featureMap);

            return featureMap;
        }

        public int[,] Overlap_Matrix_test(Bitmap bitmap)
        {
            // 원본이랑 오버랩
            ImageProcessing ip = new ImageProcessing();
            Extraction cd = new Extraction();

            // 피처맵 생성
            int[,] Original = ip.GrayArray(bitmap);
            int[,] featureMap = cam.pretraind_featureMap_2level_test(Original);
            Console.WriteLine("피처맵생성완료");

            // 255사이의 값으로 변환
            featureMap = cam.change(featureMap);

            // 수평마스크
            featureMap = ip.HorizontalMask(featureMap);

            return featureMap;
        }

        public void Overlap_Matrix_rmse()
        {
            // 원본이랑 오버랩
            ImageProcessing ip = new ImageProcessing();
            Extraction cd = new Extraction();

            // 피처맵 생성(예측)
            int[,] similarity = {
                { 10, 15, 20, 20, 25, 40, 40, 40, 45, 50, 45, 45, 45, 50, 50, 45, 60, 65, 75, 80, 85, 85, 85, 85, 80, 80, 75, 75, 70, 70, 75, 75, 80, 80, 80, 80, 85, 85, 85, 85, 85, 85, 85, 80, 80, 80, 80, 80, 80, 85, 85, 85, 85, 90, 90, 90, 80, 85, 85, 80, 80, 80, 80, 75, 75, 80, 80, 75, 75, 75, 80, 85, 95, 95, 100, 100, 100, 95, 95, 95, 90, 90, 90, 90, 90, 85, 85, 85, 80, 80, 75, 70, 65, 60, 50, 45, 45, 45, 45, 40, 15, 10, 15, 5, 0 },
                { 0, 0, 0, 0, 5, 15, 15, 15, 20, 20, 20, 20, 20, 25, 25, 20, 25, 35, 40, 45, 50, 50, 50, 50, 50, 50, 50, 45, 40, 40, 45, 50, 50, 50, 50, 55, 55, 60, 60, 60, 70, 65,    65,    65,    65,    60,    60,    60,    60,    65,    65,    65,    70,    70,    70,    70,    65,    70,    70,    70,    70,    70,    70,    65,    65,    65,    65,    60,    60,    60,    65,    70,    65,    70,    80,    80,    85,    85,    85,    85,    85,    85,    85,    85,    85,    85,    80,    80,    75,    75,    70,    65,    60,    50,    45,    40,    40,    40,    45,    35,    10,    10, 15, 5, 5 }
            };
            int[][,] featureMap = cam.pretraind_featureMap_2level_temp(similarity);
            Console.WriteLine("피처맵생성완료");
            // 255사이의 값으로 변환(
            for (int i = 0; i < 327; i++)
                featureMap[i] = cam.change(featureMap[i]);
            // 80개 평균 배열
            int[,] avg481 = learning.Average481(featureMap);
            avg481 = cam.change(avg481);
            u.MatrixPrint(avg481);

            // 피처맵 생성(정답)
            int[][,] answer = cam.pretraind_featureMap_2level_no_sim();
            for (int i = 0; i < 327; i++)
                answer[i] = cam.change(answer[i]);
            int[,] avg40 = learning.Average40(answer);
            avg40 = cam.change(avg40);
            u.MatrixPrint(avg40);

            // 두 피처맵을 rmse
            Console.WriteLine("rmse 값은 : " + learning.featureRMSE_2(avg481, avg40));
        }

        public void average_rmse()
        {
            Image image;
            Bitmap bitmap;

            int[][,] filter = new int[fix.NumOfFilter][,];
            for (int i = 0; i < fix.NumOfFilter; i++)
            {
                image = Image.FromFile(fix.DataFolder + @"\Extraction Data\filter" + i + ".jpg");
                bitmap = new Bitmap(image);
                filter[i] = ip.GrayArray(bitmap);
            }

            // 80개 평균 배열
            int[,] avg481 = learning.Average481(filter);
            avg481 = cam.change(avg481);
            u.MatrixPrint(avg481);

            // 40개 평균 배열
            int[,] avg40 = learning.Average40(filter);
            avg40 = cam.change(avg40);
            u.MatrixPrint(avg40);

            // 두 값을 rmse
            Console.WriteLine("rmse 값은 : " + learning.featureRMSE_2(avg481, avg40));
        }

        public Bitmap Overlap_save(Bitmap bitmap)
        {
            // 원본이랑 오버랩한거 저장, 클렌징도 저장
            Extraction cd = new Extraction();

            // 피처맵 생성
            int[,] Original = ip.GrayArray(bitmap);
            int[,] featureMap = cam.pretraind_featureMap_2level(Original);
            Console.WriteLine("피처맵생성완료");

            // 255사이의 값으로 변환
            featureMap = cam.change(featureMap);

            // 수평마스크
            featureMap = ip.HorizontalMask(featureMap);

            // 원본에 빨간색 색칠
            Bitmap newbit = ip.BiasConvert_original(featureMap, bitmap, 100);

            // 위에 밑에 자르기
            newbit = ip.UPandDOWN(newbit);

            // 빨간색 개수찾기
            List<int> red = cd.extend(newbit);
            Console.WriteLine("이미지처리완료");

            Console.WriteLine("장폐색 개수 : " + red.Count() / 4);

            // 변환파일 저장
            Image image = newbit;
            image.Save(fix.DataFolder + @"\Temp Data\prediction.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

            // 클렌징 저장
            image = Image.FromFile(fix.DataFolder + @"\Cleansing Data\ileus2 (1).jpg");
            newbit = new Bitmap(image, 820, 1000);
            newbit = ip.UPandDOWN(newbit);
            image = newbit;
            image.Save(fix.DataFolder + @"\Temp Data\cleansing.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

            return newbit;
        }

        public Bitmap Overlap_Sobel(Bitmap bitmap)
        {
            // 그레이스케일
            int[,] Original = ip.GrayArray(bitmap);

            // 수평마스크
            Original = ip.HorizontalMask(Original);

            // 원본에 빨간색 색칠
            Bitmap newbit = ip.BiasConvert_original(Original, bitmap, 100);

            return newbit;
        }

        public Bitmap Sobel(Bitmap bitmap)
        {
            // 그레이스케일
            int[,] Original = ip.GrayArray(bitmap);

            // 스무딩
            Original = ip.equalization(Original);

            // 수평마스크
            int[,] mask = { { 1, 0,-1 },
                         { 2, 0,-2 },
                         { 1,-0,-1 } };
            Original = ip.Mask(Original, mask);

            // 풀링
            Original = cam.MaxPooling(Original);

            // 변환
            Bitmap newbit = ip.BiasConvert(Original, 255);

            return newbit;
        }

        public int[,] Sobel_mat(Bitmap bitmap)
        {
            // 그레이스케일
            int[,] Original = ip.GrayArray(bitmap);

            // 스무딩
            Original = ip.equalization(Original);

            // 수평마스크
            int[,] mask = {
                {1, 1, 0,-1 ,-1},
                {1, 1, 0,-1,-1},
                {1,1, 0,-1,-1},
                {1, 1, 0,-1,-1},
                {1,1,-0,-1,-1}
            };
            Original = ip.Mask(Original, mask);

            return Original;
        }

        public void Filter_Analysis(Bitmap bitmap)
        {
            // 필터 파일을 열고 Text로 그 필터를 출력
            int[,] GrayArray = ip.GrayArray(bitmap);
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Console.Write(GrayArray[x, y] + " ");
                }
            }
            Console.WriteLine();
        }

        public Bitmap randompattern(int width, int height)
        {
            // 검색, 하얀색의 정해진크기에따라 비트맵 리턴
            return Convert(c.randompattern(width, height));
        }

        public Bitmap Convert(int[,] grayarray)
        {
            // 배열을 받아서 비트맵으로
            Bitmap bitmap = new Bitmap(grayarray.GetLength(0), grayarray.GetLength(1));
            Color color;

            for (int y = 0; y < grayarray.GetLength(1); y++)
            {
                for (int x = 0; x < grayarray.GetLength(0); x++)
                {
                    color = Color.FromArgb(grayarray[x, y], grayarray[x, y], grayarray[x, y]);
                    bitmap.SetPixel(x, y, color);
                }
            }

            return bitmap;
        }

        public double similarity2x2(int[,] totalGA, int[,] objGA)
        {
            /* 전체배열과 객체배열을 받아서 유사도검사 */
            return c.sim(totalGA, objGA);
        }


        public Bitmap BoundingBox(int[,] totalGA, int startX, int startY)
        {
            // 전체와 객체의 그레이배열을 받아서 시작점에 바운딩박스 후 비트맵출력
            Bitmap bitmap = new Bitmap(totalGA.GetLength(0), totalGA.GetLength(1));
            Color color;

            int[,] objGA = c.filterㄴ(60, 20);

            for (int y = 0; y < totalGA.GetLength(1); y++)
            {
                for (int x = 0; x < totalGA.GetLength(0); x++)
                {
                    color = Color.FromArgb(totalGA[x, y], totalGA[x, y], totalGA[x, y]);
                    bitmap.SetPixel(x, y, color);
                    // 중간에 바운딩
                    if ((x >= startX && x <= (startX + objGA.GetLength(0))) && (y >= startY && y <= (startY + objGA.GetLength(1))))
                    { // 가장자리만
                        if (x == startX || y == startY || x == (startX + objGA.GetLength(0)) || y == (startY + objGA.GetLength(1)))
                            bitmap.SetPixel(x, y, Color.Red);
                    }
                }
            }

            return bitmap;
        }

        public Bitmap HaarSW(int[,] total, int[,] obj)
        {
            // 슬라이딩 윈도우로 피처맵을 콘솔에 출력
            int[,] boundGA;

            int width = total.GetLength(0) - obj.GetLength(0) + 1;
            int height = total.GetLength(1) - obj.GetLength(1) + 1;

            double[,] sim = new double[width, height];

            for (int y = 0; y <= total.GetLength(1) - obj.GetLength(1); y += 1) // stride 1
                for (int x = 0; x <= total.GetLength(0) - obj.GetLength(0); x += 1)
                {
                    boundGA = ex.BoundGA(total, obj, x, y);
                    sim[x, y] = similarity2x2(boundGA, obj);
                    Console.WriteLine(sim[x, y]);
                }

            int[,] simInt = u.changeInt(sim);

            simInt = cam.change(simInt);

            return Convert(simInt);
        }

        public Bitmap HaarImage(Bitmap total)
        {
            // 하르 피처맵을 반환

            int[,] boundGA;
            int[,] totalarray = ip.GrayArray(total);

            int[,] obj = c.filter(50, 50);

            int width = totalarray.GetLength(0) - obj.GetLength(0) + 1;
            int height = totalarray.GetLength(1) - obj.GetLength(1) + 1;

            double[,] sim = new double[width, height];

            for (int y = 0; y <= totalarray.GetLength(1) - obj.GetLength(1); y += 1)
            {
                Console.WriteLine("진행도 : " + ((double)y / (totalarray.GetLength(1) - obj.GetLength(1))) * 100);
                for (int x = 0; x <= totalarray.GetLength(0) - obj.GetLength(0); x += 1)
                {
                    boundGA = ex.BoundGA(totalarray, obj, x, y);
                    sim[x, y] = similarity2x2(boundGA, obj);
                }
            }

            int[,] simInt = u.changeInt(sim);

            simInt = cam.change(simInt);

            Bitmap newbit = ip.BiasConvert_original(simInt, total, 175);

            return newbit;
        }

        public int[,] HaarImageMat(Bitmap total)
        {
            // 하르 피처맵을 반환

            int[,] boundGA;
            int[,] totalarray = ip.GrayArray(total);

            int[,] obj = c.filter(50, 50);

            int width = totalarray.GetLength(0) - obj.GetLength(0) + 1;
            int height = totalarray.GetLength(1) - obj.GetLength(1) + 1;

            double[,] sim = new double[width, height];

            for (int y = 0; y <= totalarray.GetLength(1) - obj.GetLength(1); y += 1)
            {
                Console.WriteLine("진행도 : " + ((double)y / (totalarray.GetLength(1) - obj.GetLength(1))) * 100);
                for (int x = 0; x <= totalarray.GetLength(0) - obj.GetLength(0); x += 1)
                {
                    boundGA = ex.BoundGA(totalarray, obj, x, y);
                    sim[x, y] = similarity2x2(boundGA, obj);
                }
            }

            int[,] simInt = u.changeInt(sim);

            simInt = cam.change(simInt);

            return simInt;
        }

        public int[,] HaarFilter(int[,] HaarImage, int[,] filterImage)
        {
            // 피처맵을 합처서 퓨전이미지를 리턴
            int[,] featureMap = cam.fusion(HaarImage, filterImage);

            return featureMap;
        }

        public int[,] HaarFilter_Pooling(int[,] HaarImage, int[,] filterImage)
        {
            // 피처맵을 합처서 퓨전이미지를 리턴
            filterImage = cam.MaxPooling(filterImage);
            int[,] featureMap = cam.fusion(HaarImage, filterImage);

            return featureMap;
        }

        public int[,] BiasCombination(int[,] HaarImage, int[,] filterImage, int bias)
        {
            // 피처맵을 합처서 퓨전이미지를 리턴
            // filterImage = cam.MaxPooling(filterImage);
            int[,] featureMap = cam.fusion_bias(HaarImage, filterImage, bias);

            return featureMap;
        }

        public Bitmap HaarFilter_B(Bitmap Original)
        {
            // 피처맵을 합처서 퓨전이미지를 리턴
            int[,] HaarImage = StepByStep(Original); // 하르예측
            int[,] filterImage = Overlap_Matrix(Original); // 필터예측
            int[,] featureMap = cam.fusion(HaarImage, filterImage); // 융합

            // 색칠
            Bitmap temp;
            temp = ip.BiasConvert_original(featureMap, Original, 140);

            return temp;
        }

        public int[,] StepByStep(Bitmap total)
        {
            // 하르 피처맵을 반환
            int[,] boundGA;
            int[,] totalarray = ip.GrayArray(total);
            double Progress;

            int[,] obj = c.filter(40, 20);

            int width = totalarray.GetLength(0) - obj.GetLength(0) + 1;
            int height = totalarray.GetLength(1) - obj.GetLength(1) + 1;

            double[,] sim = new double[width, height];

            for (int y = 0; y <= totalarray.GetLength(1) - obj.GetLength(1); y += 1)
            {
                Progress = ((double)y / (totalarray.GetLength(1) - obj.GetLength(1))) * 100;
                if (Progress % 10 >= 0.0 && Progress % 10 <= 0.1)
                    Console.WriteLine("진행도 : " + Progress + "%");

                for (int x = 0; x <= totalarray.GetLength(0) - obj.GetLength(0); x += 1)
                {
                    boundGA = ex.BoundGA(totalarray, obj, x, y);
                    boundGA = ip.equalization(boundGA);
                    sim[x, y] = similarity2x2(boundGA, obj);
                }
            }

            int[,] simInt = u.changeInt(sim);

            simInt = cam.change(simInt);

            return simInt;
        }

        public int[,] StepByStep_2(Bitmap total, int[,] obj)
        {
            // 하르 피처맵을 반환
            int[,] boundGA;
            int[,] totalarray = ip.GrayArray(total);
            double Progress;

            int width = totalarray.GetLength(0) - obj.GetLength(0) + 1;
            int height = totalarray.GetLength(1) - obj.GetLength(1) + 1;

            double[,] sim = new double[width, height];

            for (int y = 0; y <= totalarray.GetLength(1) - obj.GetLength(1); y += 1)
            {
                Progress = ((double)y / (totalarray.GetLength(1) - obj.GetLength(1))) * 100;
                // if (Progress % 10 >= 0.0 && Progress % 10 <= 0.1)
                    // Console.WriteLine("진행도 : " + Progress + "%");

                for (int x = 0; x <= totalarray.GetLength(0) - obj.GetLength(0); x += 1)
                {
                    boundGA = ex.BoundGA(totalarray, obj, x, y);
                    boundGA = ip.equalization(boundGA);

                    sim[x, y] = similarity2x2(boundGA, obj);
                }
            }

            int[,] simInt = u.changeInt(sim);

            simInt = cam.change(simInt);

            return simInt;
        }

        public int[,] Haar_Integral(Bitmap total)
        {
            // 하르 피처맵을 반환
            int[,] boundGA;
            int[,] totalarray = ip.GrayArray(total);
            double Progress;

            int[,] obj = c.filter(40, 20);

            int width = totalarray.GetLength(0) - obj.GetLength(0) + 1;
            int height = totalarray.GetLength(1) - obj.GetLength(1) + 1;

            double[,] sim = new double[width, height];

            for (int y = 0; y <= totalarray.GetLength(1) - obj.GetLength(1); y += 1)
            {
                Progress = ((double)y / (totalarray.GetLength(1) - obj.GetLength(1))) * 100;
                if (Progress % 10 >= 0.0 && Progress % 10 <= 0.1)
                    Console.WriteLine("진행도 : " + Progress + "%");

                for (int x = 0; x <= totalarray.GetLength(0) - obj.GetLength(0); x += 1)
                {
                    boundGA = ex.BoundGA(totalarray, obj, x, y);
                    boundGA = ip.equalization(boundGA);
                    sim[x, y] = c.sim(boundGA, obj);
                }
            }

            int[,] simInt = u.changeInt(sim);

            simInt = cam.change(simInt);

            return simInt;
        }

        public int[,] SBS_Filter(Bitmap total, int[,] filterMat)
        {
            // 필터에 종속된 하르
            int[,] boundGA;
            int[,] totalarray = ip.GrayArray(total);
            double Progress;

            int[,] obj = c.filter(40, 20);

            int width = filterMat.GetLength(0) - obj.GetLength(0) + 1;
            int height = filterMat.GetLength(1) - obj.GetLength(1) + 1;

            double[,] sim = new double[width, height];

            // 패딩
            int xpad = (total.Width - filterMat.GetLength(0)) / 2;
            int ypad = (total.Height - filterMat.GetLength(1)) / 2;

            for (int y = 0; y <= filterMat.GetLength(1) - obj.GetLength(1); y += 1)
            {
                Progress = ((double)y / (filterMat.GetLength(1) - obj.GetLength(1))) * 100;
                if (Progress % 10 >= 0.0 && Progress % 10 <= 0.1)
                    Console.WriteLine("진행도 : " + Progress + "%");

                for (int x = 0; x <= filterMat.GetLength(0) - obj.GetLength(0); x += 1)
                {
                    if (filterMat[x, y] > 20)
                    {
                        boundGA = ex.BoundGA(totalarray, obj, x + xpad, y + ypad);
                        boundGA = ip.equalization(boundGA);
                        sim[x, y] = similarity2x2(boundGA, obj);
                    }
                    else
                        sim[x, y] = 0;
                }
            }

            int[,] simInt = u.changeInt(sim);

            simInt = cam.change(simInt);

            return simInt;
        }

        public Bitmap EdgeDetect(Bitmap Original)
        {
            // 엣지 디텍션한 행렬을 리턴
            int[,] EdgeMatrix = ip.EdgeDetect(Original);

            // 바이너리
            // EdgeMatrix = ip.binary(EdgeMatrix, 50);

            return Convert(EdgeMatrix);
        }

        public void IntegralImage()
        {
            // 적분영상 테스트
            int[,] original =
            {
                {10,15,7,9,5,6,4,2},
                {9,32,65,45,12,7,5,2},
                {7,24,66,65,41,34,4,7},
                {9,11,70,89,44,37,42,11},
                {32,78,91,78,48,65,15,24},
                {64,12,89,58,65,45,37,9},
                {1,1,96,89,56,48,59,3},
                {2,3,45,65,44,71,57,4}
            };
            u.MatrixPrint(original);
            Console.WriteLine("--------------------");

            // 적분영상 만들기
            int[,] InteImage_mat = ip.Integral(original);
            u.MatrixPrint(InteImage_mat);
            Console.WriteLine("--------------------");

            // 적분영상의 크기 구하기
            int[] start = { 1, 1 };
            int[] end = { 6, 6 };
            int Area = ip.Integral_Coord(InteImage_mat, start, end);
            Console.WriteLine("해당 좌표의 크기 : " + Area);
        }

        public void IntegralImage(Bitmap bitmap)
        {
            // 적분영상 테스트
            int[,] original = ip.GrayArray(bitmap);
            u.MatrixPrint(original);
            Console.WriteLine("--------------------");

            // 적분영상 만들기
            int[,] InteImage_mat = ip.Integral(original);
            u.MatrixPrint(InteImage_mat);
            Console.WriteLine("--------------------");

            // 적분영상의 크기 구하기
            int[] start = { 3, 3 };
            int[] end = { 5, 5 };
            int Area = ip.Integral_Coord(InteImage_mat, start, end);
            Console.WriteLine("해당 좌표의 크기 : " + Area);
        }

        public Bitmap ROI_BoundingBox(Bitmap predict, Bitmap Original)
        {
            // ROI한것을 떼냄
            Extraction coo = new Extraction();
            Console.WriteLine("--------------------");

            // 좌표구함
            List<int> AllXY = coo.extend(predict);
            Console.WriteLine("개수 : " + AllXY.Count() / 4);

            // 바운딩박스 변환
            Bitmap newbit = ip.BoundingBox(AllXY, Original);

            return newbit;
        }

        public Bitmap histogramview(Bitmap temp)
        {
            // 히스토그램 배열 만들기
            int[] HistoArray = ip.Histo_init(temp);

            // 히스토그램 맥스
            int max = 0;
            for (int x = 0; x < 256; x++)
            {
                if (max < HistoArray[x])
                    max = HistoArray[x];
            }

            // 히스토그램 뷰
            Bitmap newColor = new Bitmap(256, max);

            for (int y = max - 1; y >= 0; y--)
            {
                for (int x = 0; x < 256; x++)
                {
                    if (HistoArray[x] > max - 1 - y)
                        newColor.SetPixel(x, y, Color.Black);
                    else
                        newColor.SetPixel(x, y, Color.White);
                }
            }

            return newColor;
        }

        public Bitmap Black(int[,] bin)
        {
            int[] Blackn = Blacknum(bin);
            Bitmap newbit = new Bitmap(bin.GetLength(0), bin.GetLength(1));

            for (int y = 0; y < bin.GetLength(1); y++)
            {
                for (int x = 0; x < Blackn[y]; x++)
                {
                    newbit.SetPixel(x, y, Color.Black);
                }
            }

            return newbit;
        }

        public int[] Blacknum(int[,] bin)
        {
            int[] newarr = new int[bin.GetLength(1)];
            for (int y = 0; y < bin.GetLength(1); y++)
            {
                for (int x = 0; x < bin.GetLength(0); x++)
                {
                    if (bin[x, y] == 0)
                        newarr[y]++;
                }
            }
            return newarr;
        }

        public int[,] LUNG(int[,] bin, int[,] original, int bias)
        {
            int[] Blackn = Blacknum(bin);
            int change = 0;

            // bias가 되는 순간을 찾아
            for (int i = 0; i < Blackn.Length; i++)
            {
                if (Blackn[i] < bias)
                {
                    change = i;
                    break;
                }
            }

            int[,] newarr = new int[original.GetLength(0), original.GetLength(1) - change];

            for (int y = 0; y < newarr.GetLength(1); y++)
            {
                for (int x = 0; x < newarr.GetLength(0); x++)
                {
                    newarr[x, y] = original[x, y + change];
                }
            }

            return newarr;
        }

        public List<int> FeatureList(int[,] filterImage,Bitmap ans){
            // 비교해서 피처리스트를 반환
            List<int> result = new List<int> { };
            Color color = new Color();
            int xpad = (ans.Width - filterImage.GetLength(0)) / 2+1;
            int ypad = (ans.Height - filterImage.GetLength(1)) / 2+1;

            // 빨간색 부분을 찾음
            for (int y = ypad; y < ans.Height-ypad; y++)
            {
                for (int x = xpad; x < ans.Width-xpad; x++)
                {
                    color = ans.GetPixel(x, y);
                    if (color.R > 150 && color.B < 100)
                    {
                        result.Add(filterImage[x-xpad, y-ypad]);
                        Console.WriteLine("x:" + (x - xpad) + " y:" + (y - ypad));
                    }
                }
            }

            return result;
        }

        public Bitmap Verification(int[,] filterImage, Bitmap ans, Bitmap Original)
        {
            // 비교해서 피처리스트를 반환
            List<int> result = new List<int> { };
            Color color = new Color();
            int xpad = (ans.Width - filterImage.GetLength(0)) / 2;
            int ypad = (ans.Height - filterImage.GetLength(1)) / 2;

            // 빨간색 부분을 찾음
            for (int y = ypad; y < ans.Height - ypad; y++)
            {
                for (int x = xpad; x < ans.Width - xpad; x++)
                {
                    color = ans.GetPixel(x, y);
                    if (color.R > 150 && color.B < 100)
                    {
                        // result.Add(filterImage[x - xpad, y - ypad]);

                        // 색칠
                        Original.SetPixel(x, y, Color.Red);
                    }
                }
            }

            return Original;
        }

        public int all_filter_avg()
        {
            // 모든 이미지의 fiter 임계치 평균 구하기
            Image image;
            Bitmap Original, temp;
            int[,] filterImage;
            List<int> feature;
            List<double> feature_avg = new List<double> { };

            for (int i = 1; i <= fix.NumOfImage; i++)
            {
                // filter 특징맵 생성
                image = Image.FromFile(fix.DataFolder + @"\Original Data\ileus2 ("+i+").jpg");
                Original = new Bitmap(image, 1000, 1000);
                filterImage = Overlap_Matrix(Original);

                // 정답이미지 불러오기
                image = Image.FromFile(fix.DataFolder + @"\Cleansing Data\ileus2 ("+i+").jpg");
                temp = new Bitmap(image, 1000, 1000);

                // 정답이미지의 빨간색 부분 찾아서 특징값 불러옴
                feature = FeatureList(filterImage, temp);

                // 리스트로 편입
                feature_avg.Add(feature.Average());

                Console.WriteLine("진행률 : {0}%", (((double)i / fix.NumOfImage)*100).ToString("F1"));
            }

            return (int)feature_avg.Average();
        }

        public int all_haar_avg()
        {
            // 모든 이미지의 fiter 임계치 평균 구하기
            Image image;
            Bitmap Original, temp;
            int[,] haarImage;
            List<int> feature;
            List<double> feature_avg = new List<double> { };
            int[,] obj = c.filter(60, 10);

            for (int i = 1; i <= fix.NumOfImage; i++)
            {
                // haar 특징맵 생성
                image = Image.FromFile(fix.DataFolder + @"\Original Data\ileus2 (" + i + ").jpg");
                Original = new Bitmap(image, 500, 500);
                haarImage = StepByStep_2(Original, obj);

                // 정답이미지 불러오기
                image = Image.FromFile(fix.DataFolder + @"\Cleansing Data\ileus2 (" + i + ").jpg");
                temp = new Bitmap(image, 500, 500);

                // 정답이미지의 빨간색 부분 찾아서 특징값 불러옴
                Console.WriteLine(i + "번째");
                feature = FeatureList(haarImage, temp);

                // 리스트로 편입
                feature_avg.Add(feature.Average());

                Console.WriteLine("진행률 : {0}%", (((double)i / fix.NumOfImage) * 100).ToString("F1"));
            }

            return (int)feature_avg.Average();
        }

        public Bitmap Red_Black(Bitmap bit)
        {
            Color color;

            for (int y = 0; y < bit.Height; y++)
            {
                for (int x = 0; x < bit.Width; x++)
                {
                    color = bit.GetPixel(x,y);
                    if (color.R > 150 && color.B < 100)
                        bit.SetPixel(x, y, Color.White);
                    else
                        bit.SetPixel(x, y, Color.Black);
                }
            }

            return bit;
        }

        public int TP(Bitmap TestBimap, Bitmap AnswerBimap)
        {
            // TP를 구하는 메소드 Bitmap to TP

            // 라벨링 좌표 불러오기
            IplImage ipImage = TestBimap.ToIplImage();
            CvBlobs Test_blobs = cv.BlobInfo(ipImage);

            ipImage = AnswerBimap.ToIplImage();
            CvBlobs Answer_blobs = cv.BlobInfo(ipImage);

            cv.BlobPrint(Test_blobs);
            cv.BlobPrint(Answer_blobs);

            // TP구하기
            int TP_num = cv.TP_num(Test_blobs, Answer_blobs);

            return TP_num;
        }

        public List<int> TPFN(Bitmap TestBimap, Bitmap AnswerBimap)
        {
            // TP를 구하는 메소드 Bitmap to TP

            // 라벨링 좌표 불러오기
            IplImage ipImage = TestBimap.ToIplImage();
            CvBlobs Test_blobs = cv.BlobInfo(ipImage);

            ipImage = AnswerBimap.ToIplImage();
            CvBlobs Answer_blobs = cv.BlobInfo(ipImage);

            cv.BlobPrint(Test_blobs);
            cv.BlobPrint(Answer_blobs);

            // TP구하기
            List<int> TP_num = cv.TPFN_num(Test_blobs, Answer_blobs);

            return TP_num;
        }

        public List<int> TPFNFP(Bitmap TestBimap, Bitmap AnswerBimap)
        {
            // TP를 구하는 메소드 Bitmap(이진화) to TP

            // 라벨링 좌표 불러오기
            IplImage ipImage = TestBimap.ToIplImage();
            CvBlobs Test_blobs = cv.BlobInfo(ipImage);

            ipImage = AnswerBimap.ToIplImage();
            CvBlobs Answer_blobs = cv.BlobInfo(ipImage);

            // TP구하기
            List<int> TP_num = cv.TPFNFP_num(Test_blobs, Answer_blobs);

            return TP_num;
        }

        public Bitmap accuracy(Bitmap TestBimap, Bitmap AnswerBimap)
        {
            // 정확도구하는 함수
            int[,] fusion;
            Bitmap Extra_ori;
            int[,] filterImage;
            int[,] HaarImage;
            int[,] obj = c.filter(60, 10);

            // 장폐색 예측
            HaarImage = this.StepByStep_2(TestBimap, obj);
            Console.WriteLine("하르맵 생성");

            filterImage = this.Overlap_Matrix(TestBimap);
            Console.WriteLine("필터맵 생성");

            fusion = this.BiasCombination(HaarImage, filterImage, 5);
            fusion = ip.UPandDOWN_mat(fusion);

            Extra_ori = ip.BiasConvert_original(fusion, TestBimap, 150);

            // 장폐색을 이진화
            Extra_ori = this.Red_Black(Extra_ori);
            AnswerBimap = this.Red_Black(AnswerBimap);

            // TP & FP & FN
            List<int> TPFNFP = this.TPFNFP(Extra_ori, AnswerBimap);
            Console.WriteLine(TPFNFP[0]);
            Console.WriteLine(TPFNFP[1]);
            Console.WriteLine(TPFNFP[2]);

            int sum = TPFNFP[0] + TPFNFP[1] + TPFNFP[2];
            Console.WriteLine("정확도 : " + ((double)TPFNFP[0]/sum*100));

            return Extra_ori;
        }
    }
}
