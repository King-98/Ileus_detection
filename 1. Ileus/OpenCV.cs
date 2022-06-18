using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenCvSharp.Blob;

namespace _1.Ileus
{
    class OpenCV : IDisposable
    {
        IplImage bin;
        IplImage blob;

        public IplImage Binary(IplImage src)
        {
            bin = new IplImage(src.Size, BitDepth.U8, 1);
            Cv.CvtColor(src, bin, ColorConversion.BgrToGray);
            Cv.Threshold(bin, bin, 50, 255, ThresholdType.Binary);

            return bin;
        }

        public IplImage BlobImage(IplImage src)
        {
            blob = new IplImage(src.Size, BitDepth.U8, 3);
            bin = this.Binary(src);

            CvBlobs blobs = new CvBlobs();
            blobs.Label(bin);

            blobs.RenderBlobs(src, blob);

            foreach (KeyValuePair<int, CvBlob> item in blobs)
            {
                CvBlob b = item.Value;

                blob.PutText(Convert.ToString(b.Label), b.Centroid, new CvFont(FontFace.HersheyComplex, 1, 1), CvColor.Red);

            }

            return blob;
        }

        public CvBlobs BlobInfo(IplImage src)
        {
            blob = new IplImage(src.Size, BitDepth.U8, 3);
            bin = this.Binary(src);

            CvBlobs blobs = new CvBlobs();
            blobs.Label(bin);

            blobs.RenderBlobs(src, blob);

            return blobs;
        }

        public void Dispose()
        {
            if (bin != null) Cv.ReleaseImage(bin);
            if (blob != null) Cv.ReleaseImage(blob);
        }

        public void BlobPrint(CvBlobs blobs)
        {
            foreach (KeyValuePair<int, CvBlob> item in blobs)
            {
                CvBlob b = item.Value;

                blob.PutText(Convert.ToString(b.Label), b.Centroid, new CvFont(FontFace.HersheyComplex, 1, 1), CvColor.Red);

                Console.WriteLine(b.Label);
                Console.WriteLine(b.Centroid);
                Console.WriteLine(b.Area);
                Console.WriteLine(b.Rect);
                Console.WriteLine(b.Contour.StartingPoint);
                Console.WriteLine();
            }
        }

        public int TP_num(CvBlobs Test_blobs, CvBlobs Answer_blobs)
        {
            // CvBlobs to TP_num
            int W_Min, H_Min;
            int W_Max, H_Max;
            int total;
            int overlay;
            bool Result;
            CvBlob Answer;
            CvBlob Test;
            int count=0;

            foreach (KeyValuePair<int, CvBlob> Answer_blob in Answer_blobs)
            {
                foreach (KeyValuePair<int, CvBlob> Test_blob in Test_blobs)
                {
                    Answer = Answer_blob.Value;
                    Test = Test_blob.Value;
                    Result = false;

                    // 가로
                    W_Min = Math.Min(Answer.Rect.X, Test.Rect.X);
                    W_Max = Math.Max(Answer.Rect.X + Answer.Rect.Width, Test.Rect.X + Test.Rect.Width);

                    total = Test.Rect.Width + Answer.Rect.Width;

                    overlay = W_Max - W_Min;

                    Result = (total>overlay);

                    // 세로
                    H_Min = Math.Min(Answer.Rect.Y, Test.Rect.Y);
                    H_Max = Math.Max(Answer.Rect.Y + Answer.Rect.Height, Test.Rect.Y + Test.Rect.Height);

                    total = Test.Rect.Height + Answer.Rect.Height;

                    overlay = H_Max - H_Min;

                    Result = Result && (total > overlay);

                    if (Result == true) {
                        count++;
                        break;
                    }
                }
            }

            return count;
        }

        public List<int> TPFN_num(CvBlobs Test_blobs, CvBlobs Answer_blobs)
        {
            // CvBlobs to TP, FN
            int W_Min, H_Min;
            int W_Max, H_Max;
            int total;
            int overlay;
            bool Result;
            CvBlob Answer;
            CvBlob Test;
            int count = 0;
            int TP_count = 0;
            int FN_count = 0;

            foreach (KeyValuePair<int, CvBlob> Answer_blob in Answer_blobs)
            {
                count = 0;

                foreach (KeyValuePair<int, CvBlob> Test_blob in Test_blobs)
                {
                    Answer = Answer_blob.Value;
                    Test = Test_blob.Value;
                    Result = false;

                    // 가로
                    W_Min = Math.Min(Answer.Rect.X, Test.Rect.X);
                    W_Max = Math.Max(Answer.Rect.X + Answer.Rect.Width, Test.Rect.X + Test.Rect.Width);

                    total = Test.Rect.Width + Answer.Rect.Width;

                    overlay = W_Max - W_Min;

                    Result = (total > overlay);

                    // 세로
                    H_Min = Math.Min(Answer.Rect.Y, Test.Rect.Y);
                    H_Max = Math.Max(Answer.Rect.Y + Answer.Rect.Height, Test.Rect.Y + Test.Rect.Height);

                    total = Test.Rect.Height + Answer.Rect.Height;

                    overlay = H_Max - H_Min;

                    Result = Result && (total > overlay);

                    if (Result == true)
                    {
                        count++;
                        TP_count++;
                        break;
                    }
                }
                if (count == 0)
                    FN_count++; // 한번도 예측에 성공한적이 없을때
            }

            List<int> TPFN= new List<int>{ };
            TPFN.Add(TP_count);
            TPFN.Add(FN_count);

            return TPFN;
        }

        public List<int> TPFNFP_num(CvBlobs Test_blobs, CvBlobs Answer_blobs)
        {
            // CvBlobs to TP, FN
            int W_Min, H_Min;
            int W_Max, H_Max;
            int total;
            int overlay;
            bool Result;
            CvBlob Answer;
            CvBlob Test;
            int count = 0;
            int TP_count = 0;
            int FN_count = 0;
            int FP_count = 0;
            int[] Test_array = Enumerable.Repeat<int>(0, Test_blobs.Count).ToArray<int>();

            foreach (KeyValuePair<int, CvBlob> Answer_blob in Answer_blobs)
            {
                count = 0;

                foreach (KeyValuePair<int, CvBlob> Test_blob in Test_blobs)
                {
                    Answer = Answer_blob.Value;
                    Test = Test_blob.Value;
                    Result = false;

                    // 가로
                    W_Min = Math.Min(Answer.Rect.X, Test.Rect.X);
                    W_Max = Math.Max(Answer.Rect.X + Answer.Rect.Width, Test.Rect.X + Test.Rect.Width);

                    total = Test.Rect.Width + Answer.Rect.Width;

                    overlay = W_Max - W_Min;

                    Result = (total > overlay);

                    // 세로
                    H_Min = Math.Min(Answer.Rect.Y, Test.Rect.Y);
                    H_Max = Math.Max(Answer.Rect.Y + Answer.Rect.Height, Test.Rect.Y + Test.Rect.Height);

                    total = Test.Rect.Height + Answer.Rect.Height;

                    overlay = H_Max - H_Min;

                    Result = Result && (total > overlay);

                    // 이미 확인되면 패스
                    if (Test_array[Test_blob.Key - 1] == 1)
                        continue;

                    if (Result == true)
                    {
                        count++;
                        TP_count++;
                        Test_array[Test_blob.Key-1]=1;

                        break;
                    }
                }
                if (count == 0)
                    FN_count++; // 한번도 예측에 성공한적이 없을때
            }

            foreach (int i in Test_array)
                if (i == 0)
                    FP_count++;

            List<int> TPFNFP = new List<int> { };
            TPFNFP.Add(TP_count);
            TPFNFP.Add(FN_count);
            TPFNFP.Add(FP_count);

            return TPFNFP;
        }
    }
}