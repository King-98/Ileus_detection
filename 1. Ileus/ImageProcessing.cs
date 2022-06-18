using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1.Ileus
{
    class ImageProcessing
    {
        public int[,] GrayArray(Bitmap bitmap)
        {
            /*
                비트맵을 그레이어레이로
             */
            Color gray;
            int brightness;
            int[,] grayarray = new int[bitmap.Width, bitmap.Height];

            for (int y = 0; y < bitmap.Height; y++)
                for (int x = 0; x < bitmap.Width; x++)
                {
                    gray = bitmap.GetPixel(x, y);
                    brightness = (int)(0.299 * gray.R + 0.587 * gray.G + 0.114 * gray.B);
                    grayarray[x, y] = brightness;
                }

            return grayarray;
        }

        public int[,] binary(int[,] bitmap)
        {
            int avg = 0;
            int[,] newArray = new int[bitmap.GetLength(0), bitmap.GetLength(1)];

            for (int y = 0; y < bitmap.GetLength(1); y++)
                for (int x = 0; x < bitmap.GetLength(0); x++)
                    avg += bitmap[x, y];

            avg /= bitmap.GetLength(1) * bitmap.GetLength(0);

            for (int y = 0; y < bitmap.GetLength(1); y++)
                for (int x = 0; x < bitmap.GetLength(0); x++)
                {
                    if (avg > bitmap[x, y])
                        newArray[x, y] = 0;
                    else
                        newArray[x, y] = 255;
                }

            return newArray;
        }

        public int[,] binary(int[,] bitmap, int bias)
        {
            int[,] newArray = new int[bitmap.GetLength(0), bitmap.GetLength(1)];

            for (int y = 0; y < bitmap.GetLength(1); y++)
                for (int x = 0; x < bitmap.GetLength(0); x++)
                {
                    if (bias > bitmap[x, y])
                        newArray[x, y] = 0;
                    else
                        newArray[x, y] = 255;
                }

            return newArray;
        }

        public Bitmap BiasConvert(int[,] grayarray, int bias)
        {
            // 배열을 받아서 비트맵으로
            Bitmap bitmap = new Bitmap(grayarray.GetLength(0), grayarray.GetLength(1));
            Color color;
            for (int y = 0; y < grayarray.GetLength(1); y++)
            {
                for (int x = 0; x < grayarray.GetLength(0); x++)
                {
                    color = Color.FromArgb(grayarray[x, y], grayarray[x, y], grayarray[x, y]);
                    if (grayarray[x, y] > bias)
                        color = Color.FromArgb(255, 0, 0);
                    bitmap.SetPixel(x, y, color);
                }
            }

            return bitmap;
        }

        public Bitmap BiasConvert_original(int[,] grayarray, Bitmap original, int Bias)
        {
            // 배열을 받아서 원본에 색칠
            Color color;
            Bitmap bitmap= new Bitmap(original);
            int xpad = (bitmap.Width - grayarray.GetLength(0)) / 2;
            int ypad = (bitmap.Height - grayarray.GetLength(1)) / 2;

            for (int y = 0; y < grayarray.GetLength(1); y++)
            {
                for (int x = 0; x < grayarray.GetLength(0); x++)
                {
                    if (grayarray[x, y] > Bias)
                    {
                        color = Color.FromArgb(grayarray[x, y], grayarray[x, y], grayarray[x, y]);
                        color = Color.FromArgb(255, 0, 0);
                        bitmap.SetPixel(x + xpad, y + ypad, color);
                    }
                }
            }

            return bitmap;
        }

        public Bitmap BiasConvert_rainbow(int[,] grayarray)
        {
            // HeatMap 다채롭게
            CAM cam = new CAM();
            Bitmap bitmap = new Bitmap(grayarray.GetLength(0), grayarray.GetLength(1));
            Color color;

            int HM_Color = 0;
            for (int y = 0; y < grayarray.GetLength(1); y++)
            {
                for (int x = 0; x < grayarray.GetLength(0); x++)
                {
                    if (grayarray[x, y] > 0)
                    {
                        HM_Color = cam.maxminStretch_2(grayarray[x, y], 255, 1, 1020);
                    }
                    color = Color.FromArgb(grayarray[x, y], grayarray[x, y], grayarray[x, y]);
                    if (HM_Color >= 765)
                        color = Color.FromArgb(255, 255-(HM_Color-765), 0);
                    else if(HM_Color < 765 && HM_Color >= 510)
                        color = Color.FromArgb(HM_Color - 510, 255, 0);
                    else if (HM_Color < 510 && HM_Color >= 255)
                        color = Color.FromArgb(0, 255, 255 - (HM_Color - 255));
                    bitmap.SetPixel(x, y, color);
                }
            }

            return bitmap;
        }
        
        public int[,] HorizontalMask(int[,] featuremap)
        {
            int[,] mask = { { 1, 0,-1 },
                            { 2, 0,-2 },
                            { 1,-0,-1 } };
            int x, y;
            int r, c;
            int xPad = mask.GetLength(1) / 2;
            int yPad = mask.GetLength(0) / 2;
            double sum;
            int[,] newBitArray = new int[featuremap.GetLength(0), featuremap.GetLength(1)];
            double biasValue = 0.0;

            for (y = 0; y < featuremap.GetLength(1) - 2 * yPad; y++)// 핵심 
            {
                for (x = 0; x < featuremap.GetLength(0) - 2 * xPad; x++)// 영상의 패드를 뺀모습
                {
                    sum = 0.0;
                    for (r = 0; r < mask.GetLength(0); r++)
                    {
                        for (c = 0; c < mask.GetLength(1); c++)
                        {
                            sum += featuremap[x + c, y + r] * mask[c, r];
                        }
                    }
                    sum = Math.Abs(sum);// 절대값으로 만들어줌
                    sum += biasValue; //마스크 결과값이 너무 낮아질때 명암을 올려줌

                    if (sum > 255.0) sum = 255.0;
                    if (sum < 0.0) sum = 0.0;
                    newBitArray[x + xPad, y + yPad] = (int)sum;//저장은 한칸씩앞으로
                    if (x == 400 && y == 400)
                        break;
                }
            }

            for (y = 0; y < yPad; y++) // x라인 패딩
            {
                for (x = xPad; x < featuremap.GetLength(0) - xPad; x++)
                {
                    newBitArray[x, y] = newBitArray[x, yPad];
                    newBitArray[x, featuremap.GetLength(1) - 1 - y] = newBitArray[x, featuremap.GetLength(1) - 1 - yPad];
                }
            }

            for (x = 0; x < xPad; x++) // y라인 패딩
            {
                for (y = yPad; y < featuremap.GetLength(1) - yPad; y++)
                {
                    newBitArray[x, y] = newBitArray[xPad, y];
                    newBitArray[featuremap.GetLength(0) - 1 - x, y] = newBitArray[featuremap.GetLength(0) - 1 - xPad, y];
                }
            }

            return newBitArray;
        }

        public int[,] Mask(int[,] featuremap, int[,] mask)
        {
            int x, y;
            int r, c;
            int xPad = mask.GetLength(1) / 2;
            int yPad = mask.GetLength(0) / 2;
            double sum;
            int[,] newBitArray = new int[featuremap.GetLength(0), featuremap.GetLength(1)];
            double biasValue = 0.0;

            for (y = 0; y < featuremap.GetLength(1) - 2 * yPad; y++)// 핵심 
            {
                for (x = 0; x < featuremap.GetLength(0) - 2 * xPad; x++)// 영상의 패드를 뺀모습
                {
                    sum = 0.0;
                    for (r = 0; r < mask.GetLength(0); r++)
                    {
                        for (c = 0; c < mask.GetLength(1); c++)
                        {
                            sum += featuremap[x + c, y + r] * mask[c, r];
                        }
                    }
                    sum = Math.Abs(sum);// 절대값으로 만들어줌
                    sum += biasValue; //마스크 결과값이 너무 낮아질때 명암을 올려줌

                    if (sum > 255.0) sum = 255.0;
                    if (sum < 0.0) sum = 0.0;
                    newBitArray[x + xPad, y + yPad] = (int)sum;//저장은 한칸씩앞으로
                    if (x == 400 && y == 400)
                        break;
                }
            }

            for (y = 0; y < yPad; y++) // x라인 패딩
            {
                for (x = xPad; x < featuremap.GetLength(0) - xPad; x++)
                {
                    newBitArray[x, y] = newBitArray[x, yPad];
                    newBitArray[x, featuremap.GetLength(1) - 1 - y] = newBitArray[x, featuremap.GetLength(1) - 1 - yPad];
                }
            }

            for (x = 0; x < xPad; x++) // y라인 패딩
            {
                for (y = yPad; y < featuremap.GetLength(1) - yPad; y++)
                {
                    newBitArray[x, y] = newBitArray[xPad, y];
                    newBitArray[featuremap.GetLength(0) - 1 - x, y] = newBitArray[featuremap.GetLength(0) - 1 - xPad, y];
                }
            }

            return newBitArray;
        }

        public int[,] dMask(int[,] featuremap, double[,] mask)
        {
            int x, y;
            int r, c;
            int xPad = mask.GetLength(1) / 2;
            int yPad = mask.GetLength(0) / 2;
            double sum;
            int[,] newBitArray = new int[featuremap.GetLength(0), featuremap.GetLength(1)];
            double biasValue = 0.0;

            for (y = 0; y < featuremap.GetLength(1) - 2 * yPad; y++)// 핵심 
            {
                for (x = 0; x < featuremap.GetLength(0) - 2 * xPad; x++)// 영상의 패드를 뺀모습
                {
                    sum = 0.0;
                    for (r = 0; r < mask.GetLength(0); r++)
                    {
                        for (c = 0; c < mask.GetLength(1); c++)
                        {
                            sum += featuremap[x + c, y + r] * mask[c, r];
                        }
                    }
                    sum = Math.Abs(sum);// 절대값으로 만들어줌
                    sum += biasValue; //마스크 결과값이 너무 낮아질때 명암을 올려줌

                    if (sum > 255.0) sum = 255.0;
                    if (sum < 0.0) sum = 0.0;
                    newBitArray[x + xPad, y + yPad] = (int)sum;//저장은 한칸씩앞으로
                    if (x == 400 && y == 400)
                        break;
                }
            }

            for (y = 0; y < yPad; y++) // x라인 패딩
            {
                for (x = xPad; x < featuremap.GetLength(0) - xPad; x++)
                {
                    newBitArray[x, y] = newBitArray[x, yPad];
                    newBitArray[x, featuremap.GetLength(1) - 1 - y] = newBitArray[x, featuremap.GetLength(1) - 1 - yPad];
                }
            }

            for (x = 0; x < xPad; x++) // y라인 패딩
            {
                for (y = yPad; y < featuremap.GetLength(1) - yPad; y++)
                {
                    newBitArray[x, y] = newBitArray[xPad, y];
                    newBitArray[featuremap.GetLength(0) - 1 - x, y] = newBitArray[featuremap.GetLength(0) - 1 - xPad, y];
                }
            }

            return newBitArray;
        }
        
        public Bitmap UPandDOWN(Bitmap featuremap)
        {
            int cut = featuremap.Height - (featuremap.Height / 5);
            Bitmap newbit = new Bitmap(featuremap.Width, cut);
            Color color;

            for (int y = 0; y < newbit.Height; y++)
            {
                for (int x = 0; x < newbit.Width; x++)
                {
                    color = featuremap.GetPixel(x, y + (featuremap.Height / 5) / 2);
                    newbit.SetPixel(x, y, color);
                }
            }
            return newbit;
        }

        public int[,] UPandDOWN_mat(int[,] featuremap)
        {
            int cut = featuremap.GetLength(1) - (featuremap.GetLength(1) / 5);
            int[,] mat = new int[featuremap.GetLength(0), cut];

            for (int y = 0; y < mat.GetLength(1); y++)
            {
                for (int x = 0; x < mat.GetLength(0); x++)
                {
                    mat[x, y] = featuremap[x, y + (featuremap.GetLength(1) / 5)/2];
                }
            }
            return mat;
        }

        public Bitmap Blackup(Bitmap bitmap)
        {
            Bitmap newbit = new Bitmap(bitmap, bitmap.Width, bitmap.Height-1);
            Color color;

            for (int y = 0; y < newbit.Height; y++)
            {
                for (int x = 0; x < newbit.Width; x++)
                {
                    color = bitmap.GetPixel(x, y);
                    newbit.SetPixel(x, y, color);
                }
            }

            return newbit;
        }

        public int[,] FuzzyStretch(int[,] grayarray)
        {
            /*
                gray array를 받아서 퍼지 스트레칭한  array를 다시반환
             */

            int Xmean = 0, Xmin = 255, Xmax = 0, Dmin, Dmax, a, lmax, lmin, lmid, x, y;
            double l_value, r_value, percent = 0.05;

            // 평균 명암도 구함
            for (y = 0; y < grayarray.GetLength(1); y++)
            {
                for (x = 0; x < grayarray.GetLength(0); x++)
                {
                    Xmean += grayarray[x, y];
                    if (Xmin > grayarray[x, y])
                        Xmin = grayarray[x, y]; //gray에서 가장 작은값
                    if (Xmax < grayarray[x, y])
                        Xmax = grayarray[x, y];
                }
            }
            Xmean /= (grayarray.GetLength(1) * grayarray.GetLength(0)); // 평균명암도

            // 어두운 영역과 밝은 영역의 거리값을 구함
            Dmax = Xmax - Xmean;
            Dmin = Xmean - Xmin;// 평균명암-min

            //높은 명암도에 속하면 a에넣고 낮은것도 마찬가지
            if (Xmean > 128) // 절반보다 높으면 전체에서 평균명암도를뺌
                a = 255 - Xmean;
            else if (Xmean <= Dmin) // Dmin이 더높을리없음
                a = Dmin;
            else if (Xmean >= Dmax) // 마찬가지
                a = Dmax;
            else
                a = Xmean; // a=xmean

            lmax = Xmean + a; // 평균 + 평균
            lmin = Xmean - a; // 0
            lmid = (lmax + lmin) / 2; // 소속도의평균

            if (lmin != 0) percent = (double)lmin / (double)lmax;
            l_value = (lmid - lmin) * percent + lmin;
            r_value = -(lmax - lmid) * percent + lmax;
            int alpha = (int)l_value;
            int beta = (int)r_value;
            int[] LUT = new int[256];

            for (x = 0; x < alpha; x++) LUT[x] = 0;
            for (x = 255; x > beta; x--) LUT[x] = 255;
            for (x = alpha; x <= beta; x++)
                LUT[x] = (int)((x - alpha) * 255.0 / (beta - alpha));

            for (y = 0; y < grayarray.GetLength(1); y++)
            {
                for (x = 0; x < grayarray.GetLength(0); x++)
                {
                    grayarray[x, y] = LUT[grayarray[x, y]];
                }
            }

            return grayarray;
        }

        public int[] histogram(int[,] mat)
        {
            // 히스토그램 분포
            int[] histogramArray = new int[fix.MaxColor];

            for (int y = 0; y < mat.GetLength(1); y++)
            {
                for (int x = 0; x < mat.GetLength(0); x++)
                {
                    histogramArray[mat[x, y]]++;
                }
            }
            return histogramArray;
        }

        public int[] histogramSum(int[] histogram)
        {
            // 히스토그램 누적합
            int sum =0;
            int[] histogramSumArray = new int[histogram.Length];

            for (int i = 0; i < histogram.Length; i++)
            {
                sum += histogram[i];
                histogramSumArray[i] += sum;
            }
            return histogramSumArray;
        }

        public int[] LookUpTable(int[] histogramSum, int Max)
        {
            // 평활화에 대한 룩업테이블 만들기
            int[] LUT = new int[histogramSum.Length];
            double nomalization = 0.0;

            for (int i = 0; i < histogramSum.Length; i++)
            {
                nomalization = ((fix.MaxColor - 1) / (double)Max);
                LUT[i] = (int)(histogramSum[i] * nomalization);
            }

            return LUT;
        }

        public int[,] equalization(int[,] mat)
        {
            // 히스토그램 평활화
            int[,] newmat = new int[mat.GetLength(0), mat.GetLength(1)];

            // 히스토그램 분포 구하기
            int[] histogramArray = histogram(mat);

            // 누적 히스토그램
            int[] histogramSumArray = histogramSum(histogramArray);

            // LUT 구하기
            int[] LUT = LookUpTable(histogramSumArray, histogramSumArray[fix.MaxColor - 1]);

            for (int y = 0; y < newmat.GetLength(1); y++)
            {
                for (int x = 0; x < newmat.GetLength(0); x++)
                {
                    newmat[x,y] = LUT[mat[x,y]];
                }
            }

            return newmat;
        }

        public int[,] Convolution(int[,] mask, int[,] featuremap)
        {
            int x, y;
            int r, c;
            int xPad = mask.GetLength(1) / 2;
            int yPad = mask.GetLength(0) / 2;
            double sum;
            int[,] newBitArray = new int[featuremap.GetLength(0), featuremap.GetLength(1)];
            double biasValue = 0.0;

            for (y = 0; y < featuremap.GetLength(1) - 2 * yPad; y++)// 핵심 
            {
                for (x = 0; x < featuremap.GetLength(0) - 2 * xPad; x++)// 영상의 패드를 뺀모습
                {
                    sum = 0.0;
                    for (r = 0; r < mask.GetLength(0); r++)
                    {
                        for (c = 0; c < mask.GetLength(1); c++)
                        {
                            sum += featuremap[x + c, y + r] * mask[c, r];
                        }
                    }
                    sum = Math.Abs(sum);// 절대값으로 만들어줌
                    sum += biasValue; //마스크 결과값이 너무 낮아질때 명암을 올려줌

                    if (sum > 255.0) sum = 255.0;
                    if (sum < 0.0) sum = 0.0;
                    newBitArray[x + xPad, y + yPad] = (int)sum;//저장은 한칸씩앞으로
                    if (x == 400 && y == 400)
                        break;
                }
            }

            for (y = 0; y < yPad; y++) // x라인 패딩
            {
                for (x = xPad; x < featuremap.GetLength(0) - xPad; x++)
                {
                    newBitArray[x, y] = newBitArray[x, yPad];
                    newBitArray[x, featuremap.GetLength(1) - 1 - y] = newBitArray[x, featuremap.GetLength(1) - 1 - yPad];
                }
            }

            for (x = 0; x < xPad; x++) // y라인 패딩
            {
                for (y = yPad; y < featuremap.GetLength(1) - yPad; y++)
                {
                    newBitArray[x, y] = newBitArray[xPad, y];
                    newBitArray[featuremap.GetLength(0) - 1 - x, y] = newBitArray[featuremap.GetLength(0) - 1 - xPad, y];
                }
            }

            return newBitArray;
        }

        public int[,] EdgeDetect(Bitmap Original)
        {
            // 엣지 디텍션
            Run run = new Run();
            int[,] HorizontalMask = {
                { 1, 0,-1},
                { 2, 0,-2},
                { 1, 0,-1},
            };
            int[,] VerticalityMask = {
                { 1, 2, 1},
                { 0, 0, 0},
                {-1,-2,-1},
            };

            // Convolution
            int[,] OriginalArray = GrayArray(Original);
            int[,] Conv_H = Convolution(HorizontalMask, OriginalArray);
            int[,] Conv_V = Convolution(VerticalityMask, OriginalArray);

            // fusion
            int[,] fusion = run.HaarFilter(Conv_H, Conv_V);

            return fusion;
        }

        public int[,] Integral(int[,] matrix)
        {
            int[,] Integral_mat = new int[matrix.GetLength(1), matrix.GetLength(0)];
            int sum = 0;

            for(int y=0; y< matrix.GetLength(1); y++)
            {
                for (int x = 0; x < matrix.GetLength(0); x++)
                {
                    if(y>0)
                        sum += Integral_mat[y - 1, x];

                    sum += matrix[y, x];
                    Integral_mat[y,x] = sum;

                    if (y > 0)
                        sum -= Integral_mat[y - 1, x];
                }
                sum = 0;
            }

            return Integral_mat;
        }

        public int Integral_Coord(int[,] IntImage, int[] start, int[] end)
        {
            return IntImage[end[0], end[1]] - IntImage[start[0] - 1, end[1]] - IntImage[end[0], start[1] - 1] + IntImage[start[0] - 1, start[1] - 1];
        }

        public Bitmap BoundingBox(List<int> AllXY, Bitmap Original)
        {
            // 좌표와 오리지널 사진으로 바운딩박스

            for (int i = 0; i < AllXY.Count() / 4; i++)
            {
                for (int X = AllXY[(i * 4) + 0]; X < AllXY[(i * 4) + 2]; X++)
                {
                    Original.SetPixel(X, AllXY[(i * 4) + 1], Color.Red);
                    Original.SetPixel(X, AllXY[(i * 4) + 3], Color.Red);
                }

                for (int Y = AllXY[(i * 4) + 1]; Y < AllXY[(i * 4) + 3]; Y++)
                {
                    Original.SetPixel(AllXY[(i * 4) + 0], Y, Color.Red);
                    Original.SetPixel(AllXY[(i * 4) + 2], Y, Color.Red);
                }
            }

            return Original;
        }

        public int[] Histo_init(Bitmap temp)
        {
            int[,] GA = GrayArray(temp);
            int[] HistoArray = new int[256];

            for (int y = 0; y < GA.GetLength(1); y++)
            {
                for (int x = 0; x < GA.GetLength(0); x++)
                {
                    HistoArray[GA[x, y]]++;
                }
            }

            return HistoArray;
        }

        public int[,] smoothing(int[,] total)
        {
            // 토탈 이미지를 가우시안 필터 스무딩으로 다운샘플링시킴
            int[,] mask = { { 1, 2, 1 },
                            { 2, 4, 2 },
                            { 1, 2, 1 } };
            int[,] pixtotal = new int[total.GetLength(0) / 2, total.GetLength(1) / 2];
            int sum;
            int r, c;

            for (int y = 2; y < total.GetLength(1) - mask.GetLength(1); y += 2)
                for (int x = 2; x < total.GetLength(0) - mask.GetLength(1); x += 2)
                {
                    sum = 0;
                    // -1은 짝수면 하나남아서 그냥 제거
                    for (r = 0; r < mask.GetLength(0); r++)
                    {
                        for (c = 0; c < mask.GetLength(1); c++)
                        {
                            sum += total[x + c, y + r] * mask[c, r];
                        }
                    }
                    pixtotal[x / 2, y / 2] = sum / 16;
                }

            return pixtotal;
        }

        public int[,] smoothing_1(int[,] total)
        {
            // 토탈 이미지를 가우시안 필터 스무딩으로 다운샘플링시킴
            int[,] mask = { { 1, 2, 1 },
                            { 2, 4, 2 },
                            { 1, 2, 1 } };
            int[,] pixtotal = new int[total.GetLength(0), total.GetLength(1)];
            int sum;
            int r, c;

            for (int y = 0; y < total.GetLength(1) - mask.GetLength(1); y += 1)
                for (int x = 0; x < total.GetLength(0) - mask.GetLength(1); x += 1)
                {
                    sum = 0;
                    // -1은 짝수면 하나남아서 그냥 제거
                    for (r = 0; r < mask.GetLength(0); r++)
                    {
                        for (c = 0; c < mask.GetLength(1); c++)
                        {
                            sum += total[x + c, y + r] * mask[c, r];
                        }
                    }
                    pixtotal[x, y] = sum / 16;
                }

            return pixtotal;
        }
    }
}
