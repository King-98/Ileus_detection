using OpenCvSharp;
using OpenCvSharp.Blob;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1.Ileus
{
    public partial class Form1 : Form
    {
        Image image;
        Bitmap Original;
        Run run = new Run();
        Create create = new Create();
        ImageProcessing ip = new ImageProcessing();
        Comparison com = new Comparison();
        Bitmap temp;
        int[,] tempfusion;
        int[,] tempfilter;
        int[,] temphaar;
        int[,] HaarImage_T;
        Create c = new Create();

        public Form1()
        {
            InitializeComponent();
        }

        private void 원본ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name = "All Files(*.*)|*.*|Bitmap File(*.bmp)|*.bmp|";
            name = name + "Gif File(*.gif)|*.gif|jpeg File(*.jpg)|*.jpg";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "타이틀";

            openFileDialog1.Filter = name;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string strName = openFileDialog1.FileName;
                image = Image.FromFile(strName);
                Original = new Bitmap(image, fix.Width, fix.Height);
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("width : " + Original.Width);
                Console.WriteLine("Height : " + Original.Height);
                Console.WriteLine("----------------------------------------");
            }

            pictureBox1.Image = new Bitmap(Original, pictureBox1.Width, pictureBox1.Height);

            this.Invalidate();
        }

        private void 데이터추출ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            run.All_File_ROI_Save();
        }

        private void predictionToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // 빨간색 개수 구하기
            pictureBox2.Image = new Bitmap(run.horizontal(Original), pictureBox2.Width, pictureBox2.Height);
        }

        private void 이미지불러서검은색부분확인ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name = "All Files(*.*)|*.*|Bitmap File(*.bmp)|*.bmp|";
            name = name + "Gif File(*.gif)|*.gif|jpeg File(*.jpg)|*.jpg";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "타이틀";

            openFileDialog1.Filter = name;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string strName = openFileDialog1.FileName;
                image = Image.FromFile(strName);
                Original = new Bitmap(image);
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("width : " + Original.Width);
                Console.WriteLine("Height : " + Original.Height);
                Console.WriteLine("----------------------------------------");
            }

            pictureBox1.Image = new Bitmap(Original, pictureBox1.Width, pictureBox1.Height);
            ImageProcessing ip = new ImageProcessing();
            int[,] temp = ip.GrayArray(Original);

            this.Invalidate();

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // 원본에 빨간색칠
            pictureBox2.Image = new Bitmap(run.Overlap(Original), pictureBox2.Width, pictureBox2.Height);
        }

        private void 클렌징과결과이미지저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 클렌징과결과이미지저장
            pictureBox2.Image = new Bitmap(run.Overlap_save(Original), pictureBox2.Width, pictureBox2.Height);
        }

        private void 원본에수평마스크오버랩ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = new Bitmap(run.Overlap_Sobel(Original), pictureBox2.Width, pictureBox2.Height);
        }

        private void 필터Text출력ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 필터 파일을 열고 Text로 그 필터를 출력
            string name = "All Files(*.*)|*.*|Bitmap File(*.bmp)|*.bmp|";
            name = name + "Gif File(*.gif)|*.gif|jpeg File(*.jpg)|*.jpg";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "타이틀";

            openFileDialog1.Filter = name;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string strName = openFileDialog1.FileName;
                image = Image.FromFile(strName);
                Original = new Bitmap(image);
            }

            pictureBox1.Image = new Bitmap(Original, pictureBox1.Width, pictureBox1.Height);

            run.Filter_Analysis(Original);

            this.Invalidate();
        }

        private void 랜덤패턴생성ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(run.randompattern(2,2), pictureBox1.Width, pictureBox1.Height);
        }

        private void classifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //랜덤 패턴 생성
            int[,] total = create.randompattern(3, 3);
            int[,] obj = { {0,255},{0,255} };

            // 패턴을 비트맵화 후 출력
            pictureBox1.Image = new Bitmap(run.Convert(total), pictureBox1.Width, pictureBox1.Height);
            pictureBox2.Image = new Bitmap(run.Convert(obj), pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Update();
            pictureBox2.Update();

            //유사도
            Console.WriteLine(run.similarity2x2(total, obj));
        }

        private void randomBrightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //랜덤 패턴 생성
            int[,] total = create.randombrightP(3,3);
            int[,] obj = { { 0, 255 }, { 0, 255 } };

            //유사도
            Console.WriteLine(run.similarity2x2(total, obj));

            // 객체이미지 출력
            pictureBox2.Image = new Bitmap(run.Convert(obj), pictureBox1.Width, pictureBox1.Height);

            // 전체이미지 출력
            pictureBox1.Image = new Bitmap(run.Convert(total), pictureBox1.Width, pictureBox1.Height);
        }

        private void haarSlidingWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 하르라이크피처맵
            int[,] total = create.randombrightP(5,5);
            int[,] obj = { { 0,0,255, 255 },
                           { 0,0,255, 255 },
                           { 0,0,255, 255 },
                           { 0,0,255, 255 } };

            pictureBox1.Image = new Bitmap(run.Convert(total), pictureBox1.Width, pictureBox1.Height);
            pictureBox2.Image = new Bitmap(run.Convert(obj), pictureBox1.Width, pictureBox1.Height);
            pictureBox2.Image = new Bitmap(run.HaarSW(total, obj), pictureBox1.Width, pictureBox1.Height);
            
        }

        private void 이미지에적용ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 하르라이크피처맵 이미지에 적용
            string name = "All Files(*.*)|*.*|Bitmap File(*.bmp)|*.bmp|";
            name = name + "Gif File(*.gif)|*.gif|jpeg File(*.jpg)|*.jpg";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "타이틀";

            openFileDialog1.Filter = name;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string strName = openFileDialog1.FileName;
                image = Image.FromFile(strName);
                Original = new Bitmap(image,820,1000);
            }

            pictureBox1.Image = new Bitmap(Original, pictureBox1.Width, pictureBox1.Height);
            pictureBox2.Image = new Bitmap(run.HaarImage(Original), pictureBox2.Width, pictureBox2.Height);

            this.Invalidate();
        }

        private void 필터바운딩박스ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 전체이미지 출력
            Original = new Bitmap(image, 500,500);
            pictureBox2.Image = new Bitmap(run.BoundingBox(ip.GrayArray(Original), 300,300), pictureBox2.Width, pictureBox2.Height);
        }

        private void haarmaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 앙상블 예측 haar + filter
            int[,] HaarImage = run.HaarImageMat(Original);

            temp = ip.BiasConvert_original(HaarImage, Original, 128);
            pictureBox2.Image = new Bitmap(temp, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Update();

            int[,] filterImage = run.Overlap_Matrix(Original);
            temp = ip.BiasConvert_original(filterImage, Original, 128);
            pictureBox3.Image = new Bitmap(temp, pictureBox3.Width, pictureBox3.Height);
            pictureBox3.Update();

            int[,] fusion = run.HaarFilter(HaarImage, filterImage);

            temp = ip.BiasConvert_original(fusion, Original, 150);
            pictureBox4.Image = new Bitmap(temp, pictureBox4.Width, pictureBox4.Height);
            pictureBox4.Update();

            // 피처맵
            temp = ip.BiasConvert(HaarImage, 256);
            pictureBox5.Image = new Bitmap(temp, pictureBox5.Width, pictureBox5.Height);
            pictureBox5.Update();

            temp = ip.BiasConvert(filterImage, 256);
            pictureBox6.Image = new Bitmap(temp, pictureBox6.Width, pictureBox6.Height);
            pictureBox6.Update();
        }

        private void 단계별예측ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 수평피처디텍터
            int[,] filterImage = run.Overlap_Matrix(Original);

            temp = ip.BiasConvert_original(filterImage, Original, 128);
            pictureBox2.Image = new Bitmap(temp, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Update();

            temp = ip.BiasConvert(filterImage, 256);
            pictureBox5.Image = new Bitmap(temp, pictureBox5.Width, pictureBox5.Height);
            pictureBox5.Update();

            // 단계별 하르 디텍터
            // Original = new Bitmap(image, 410, 500);
            int[,] HaarImage = run.StepByStep(Original); // filter 이미지 넣어서 그부분만 디텍팅하게 해야함

            temp = ip.BiasConvert_original(HaarImage, Original, 192);
            pictureBox3.Image = new Bitmap(temp, pictureBox3.Width, pictureBox3.Height);
            pictureBox3.Update();

            temp = ip.BiasConvert(HaarImage, 256);
            pictureBox6.Image = new Bitmap(temp, pictureBox6.Width, pictureBox6.Height);
            pictureBox6.Update();

            // 최종 예측
            int[,] fusion = run.HaarFilter(HaarImage, filterImage);

            temp = ip.BiasConvert_original(fusion, Original, 170); // 140하는게 최대한 많이잡힘
            pictureBox4.Image = new Bitmap(temp, pictureBox4.Width, pictureBox4.Height);
            pictureBox4.Update();

            temp = ip.BiasConvert(fusion, 256);
            pictureBox8.Image = new Bitmap(temp, pictureBox8.Width, pictureBox8.Height);
            pictureBox8.Update();

            // 실시간조정을 위한 temp
            tempfusion = fusion;
        }

        private void 히스토그램평활화ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name = "All Files(*.*)|*.*|Bitmap File(*.bmp)|*.bmp|";
            name = name + "Gif File(*.gif)|*.gif|jpeg File(*.jpg)|*.jpg";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "타이틀";

            openFileDialog1.Filter = name;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string strName = openFileDialog1.FileName;
                image = Image.FromFile(strName);
                Original = new Bitmap(image, 1000,1000);
            }

            pictureBox1.Image = new Bitmap(Original, pictureBox1.Width, pictureBox1.Height);


            temp = run.Convert(ip.equalization(ip.GrayArray(Original)));
            pictureBox2.Image = new Bitmap(temp, pictureBox2.Width, pictureBox2.Height);

            this.Invalidate();
        }

        private void 엣지디텍션ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[,] OriginalArray = ip.GrayArray(Original);

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
            int[,] Conv_H = ip.Convolution(HorizontalMask, OriginalArray);
            temp = run.Convert(Conv_H);
            pictureBox2.Image = new Bitmap(temp, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Update();

            int[,] Conv_V = ip.Convolution(VerticalityMask, OriginalArray);
            temp = run.Convert(Conv_V);
            pictureBox3.Image = new Bitmap(temp, pictureBox3.Width, pictureBox3.Height);
            pictureBox3.Update();

            temp = run.EdgeDetect(Original);
            pictureBox4.Image = new Bitmap(temp, pictureBox4.Width, pictureBox4.Height);
            pictureBox4.Update();
        }

        private void rOI연속실행ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 수평피처디텍터
            int[,] filterImage = run.Overlap_Matrix(Original);

            temp = ip.BiasConvert_original(filterImage, Original, 128);
            pictureBox2.Image = new Bitmap(temp, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Update();

            temp = ip.BiasConvert(filterImage, 256);
            pictureBox5.Image = new Bitmap(temp, pictureBox5.Width, pictureBox5.Height);
            pictureBox5.Update();

            // 필터에 종속된 단계별 하르 디텍터
            int[,] HaarImage = run.SBS_Filter(Original, filterImage); // filter 이미지 넣어서 그부분만 디텍팅하게 해야함

            temp = ip.BiasConvert_original(HaarImage, Original, 128);
            pictureBox3.Image = new Bitmap(temp, pictureBox3.Width, pictureBox3.Height);
            pictureBox3.Update();

            temp = ip.BiasConvert(HaarImage, 256);
            pictureBox6.Image = new Bitmap(temp, pictureBox6.Width, pictureBox6.Height);
            pictureBox6.Update();

            // 최종 예측
            int[,] fusion = run.HaarFilter(HaarImage, filterImage);

            temp = ip.BiasConvert_original(fusion, Original, 160);
            pictureBox4.Image = new Bitmap(temp, pictureBox4.Width, pictureBox4.Height);
            pictureBox4.Update();

            temp = ip.BiasConvert(fusion, 256);
            pictureBox8.Image = new Bitmap(temp, pictureBox8.Width, pictureBox8.Height);
            pictureBox8.Update();
        }

        private void 적분영상텍스트출력ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            run.IntegralImage();
        }

        private void 적분영상영상ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name = "All Files(*.*)|*.*|Bitmap File(*.bmp)|*.bmp|";
            name = name + "Gif File(*.gif)|*.gif|jpeg File(*.jpg)|*.jpg";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "타이틀";

            openFileDialog1.Filter = name;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string strName = openFileDialog1.FileName;
                image = Image.FromFile(strName);
                Original = new Bitmap(image, 50,50);
            }
            pictureBox1.Image = new Bitmap(Original, pictureBox1.Width, pictureBox1.Height);

            run.IntegralImage(Original);

            this.Invalidate();
        }

        private void 적분영상추가ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 수평피처디텍터
            int[,] filterImage = run.Overlap_Matrix(Original);

            temp = ip.BiasConvert_original(filterImage, Original, 128);
            pictureBox2.Image = new Bitmap(temp, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Update();

            temp = ip.BiasConvert(filterImage, 256);
            pictureBox5.Image = new Bitmap(temp, pictureBox5.Width, pictureBox5.Height);
            pictureBox5.Update();

            // 하르 적분영상 디텍터
            int[,] HaarImage = run.Haar_Integral(Original);

            temp = ip.BiasConvert_original(HaarImage, Original, 192);
            pictureBox3.Image = new Bitmap(temp, pictureBox3.Width, pictureBox3.Height);
            pictureBox3.Update();

            temp = ip.BiasConvert(HaarImage, 256);
            pictureBox6.Image = new Bitmap(temp, pictureBox6.Width, pictureBox6.Height);
            pictureBox6.Update();

            // 최종 예측
            int[,] fusion = run.HaarFilter(HaarImage, filterImage);
            temp = ip.BiasConvert_original(fusion, Original, 192);
            pictureBox4.Image = new Bitmap(temp, pictureBox4.Width, pictureBox4.Height);
            pictureBox4.Update();

            temp = ip.BiasConvert(fusion, 256);
            pictureBox8.Image = new Bitmap(temp, pictureBox8.Width, pictureBox8.Height);
            pictureBox8.Update();
        }

        private void 이진화ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            temp=run.Convert(ip.binary(ip.GrayArray(Original), 20));
            pictureBox2.Image = new Bitmap(temp, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Update();
        }

        private void 선데이터추출ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 원본에 바운딩박스

            // 예측이미지
            temp = run.HaarFilter_B(Original);
            pictureBox2.Image = new Bitmap(temp, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Update();

            // 예측 바운딩박스
            temp = run.ROI_BoundingBox(temp, Original);
            pictureBox3.Image = new Bitmap(temp, pictureBox3.Width, pictureBox3.Height);
            pictureBox3.Update();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            decimal C = numericUpDown1.Value; // 정확하지만 느린 부동소수점 decimal
            int bias = (int)C;

            Original = new Bitmap(image, 1000, 1000);
            temp = ip.BiasConvert_original(tempfilter, Original, bias);
            pictureBox2.Image = new Bitmap(temp, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Update();

            Original = new Bitmap(image, 500, 500);
            //Original = new Bitmap(image, 1000, 1000);
            temp = ip.BiasConvert_original(temphaar, Original, bias);
            pictureBox3.Image = new Bitmap(temp, pictureBox3.Width, pictureBox3.Height);
            pictureBox3.Update();
            
            Original = new Bitmap(image, 500,500);
            temp = ip.BiasConvert_original(tempfusion, Original, bias);
            pictureBox4.Image = new Bitmap(temp, pictureBox4.Width, pictureBox4.Height);
            pictureBox4.Update();
        }

        private void 단계별예측SpeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Create c = new Create();

            // 수평피처디텍터
            int[,] filterImage = run.Overlap_Matrix(Original);

            temp = ip.BiasConvert_original(filterImage, Original, 128);
            pictureBox2.Image = new Bitmap(temp, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Update();

            temp = ip.BiasConvert(filterImage, 256);
            pictureBox5.Image = new Bitmap(temp, pictureBox5.Width, pictureBox5.Height);
            pictureBox5.Update();

            // 단계별 하르 디텍터
            Original = new Bitmap(image, 500,500); // 부담을 줄이기위해 사이즈 반으로
            int[,] obj = c.filter(60, 10);
            int[,] HaarImage = run.StepByStep_2(Original, obj); // filter 이미지 넣어서 그부분만 디텍팅하게 해야함

            temp = ip.BiasConvert_original(HaarImage, Original, 192);
            pictureBox3.Image = new Bitmap(temp, pictureBox3.Width, pictureBox3.Height);
            pictureBox3.Update();

            temp = ip.BiasConvert(HaarImage, 256);
            pictureBox6.Image = new Bitmap(temp, pictureBox6.Width, pictureBox6.Height);
            pictureBox6.Update();

            // 최종 예측
            int[,] fusion = run.HaarFilter_Pooling(HaarImage, filterImage);

            temp = ip.BiasConvert_original(fusion, Original, 170); // 140하는게 최대한 많이잡힘
            pictureBox4.Image = new Bitmap(temp, pictureBox4.Width, pictureBox4.Height);
            pictureBox4.Update();

            temp = ip.BiasConvert(fusion, 256);
            pictureBox8.Image = new Bitmap(temp, pictureBox8.Width, pictureBox8.Height);
            pictureBox8.Update();

            // 실시간조정을 위한 temp
            tempfusion = fusion;
            HaarImage_T = HaarImage;
        }

        private void 결과피처맵히스토그램분석ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Create c = new Create();

            // 수평피처디텍터
            int[,] filterImage = run.Overlap_Matrix(Original);

            temp = ip.BiasConvert_original(filterImage, Original, 128);
            pictureBox2.Image = new Bitmap(temp, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Update();

            temp = ip.BiasConvert(filterImage, 256);
            pictureBox5.Image = new Bitmap(temp, pictureBox5.Width, pictureBox5.Height);
            pictureBox5.Update();

            // 단계별 하르 디텍터
            Original = new Bitmap(image, 500, 500); // 부담을 줄이기위해 사이즈 반으로
            int[,] obj = c.filter(20, 10);
            int[,] HaarImage = run.StepByStep_2(Original, obj); // filter 이미지 넣어서 그부분만 디텍팅하게 해야함

            temp = ip.BiasConvert_original(HaarImage, Original, 192);
            pictureBox3.Image = new Bitmap(temp, pictureBox3.Width, pictureBox3.Height);
            pictureBox3.Update();

            temp = ip.BiasConvert(HaarImage, 256);
            pictureBox6.Image = new Bitmap(temp, pictureBox6.Width, pictureBox6.Height);
            pictureBox6.Update();

            // 최종 예측
            int[,] fusion = run.HaarFilter_Pooling(HaarImage, filterImage);

            temp = ip.BiasConvert_original(fusion, Original, 170); // 140하는게 최대한 많이잡힘
            pictureBox4.Image = new Bitmap(temp, pictureBox4.Width, pictureBox4.Height);
            pictureBox4.Update();

            temp = ip.BiasConvert(fusion, 256);
            pictureBox8.Image = new Bitmap(temp, pictureBox8.Width, pictureBox8.Height);
            pictureBox8.Update();
            
            // 실시간조정을 위한 temp
            tempfusion = fusion;

            // histogram view
            temp = run.histogramview(temp);
            pictureBox7.Image = new Bitmap(temp, pictureBox7.Width, pictureBox7.Height);
            pictureBox7.Update();
        }

        private void 강력한수평ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = new Bitmap(run.Sobel(Original), pictureBox2.Width, pictureBox2.Height);
        }

        private void 수평하르앙상블ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // 수평피처디텍터
            int[,] filterImage = run.Sobel_mat(Original);

            temp = ip.BiasConvert_original(filterImage, Original, 128);
            pictureBox2.Image = new Bitmap(temp, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Update();

            temp = ip.BiasConvert(filterImage, 256);
            pictureBox5.Image = new Bitmap(temp, pictureBox5.Width, pictureBox5.Height);
            pictureBox5.Update();

            // 단계별 하르 디텍터
            Original = new Bitmap(image, 500, 500); // 부담을 줄이기위해 사이즈 반으로
            int[,] obj = c.filter(20, 10);
            int[,] HaarImage = run.StepByStep_2(Original, obj); // filter 이미지 넣어서 그부분만 디텍팅하게 해야함

            temp = ip.BiasConvert_original(HaarImage, Original, 192);
            pictureBox3.Image = new Bitmap(temp, pictureBox3.Width, pictureBox3.Height);
            pictureBox3.Update();

            temp = ip.BiasConvert(HaarImage, 256);
            pictureBox6.Image = new Bitmap(temp, pictureBox6.Width, pictureBox6.Height);
            pictureBox6.Update();

            // 최종 예측
            int[,] fusion = run.HaarFilter_Pooling(HaarImage, filterImage);

            temp = ip.BiasConvert_original(fusion, Original, 170); // 140하는게 최대한 많이잡힘
            pictureBox4.Image = new Bitmap(temp, pictureBox4.Width, pictureBox4.Height);
            pictureBox4.Update();

            temp = ip.BiasConvert(fusion, 256);
            pictureBox8.Image = new Bitmap(temp, pictureBox8.Width, pictureBox8.Height);
            pictureBox8.Update();

            // 실시간조정을 위한 temp
            tempfusion = fusion;
        }

        private void 더뛰어난수평ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[,] original = ip.GrayArray(Original);

            // 평활화
            original = ip.equalization(original);
            temp = ip.BiasConvert(original, 256);
            pictureBox2.Image = new Bitmap(temp, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Update();
            /*
            // 이진화
            original = ip.binary(original,128);
            temp = ip.BiasConvert(original, 256);
            pictureBox3.Image = new Bitmap(temp, pictureBox3.Width, pictureBox3.Height);
            pictureBox3.Update();

            // 수평마스크
            int[,] mask = {
                {1, 1, 0,-1 ,-1},
                {1, 1, 0,-1,-1},
                {1,1, 0,-1,-1},
                {1, 1, 0,-1,-1},
                {1,1,-0,-1,-1}
            };
            original = ip.Mask(original, mask);
            temp = ip.BiasConvert(original, 256);
            pictureBox4.Image = new Bitmap(temp, pictureBox4.Width, pictureBox4.Height);
            pictureBox4.Update();
            */
            // haar 적용
            temp = new Bitmap(temp, 500, 500); // 부담을 줄이기위해 사이즈 반으로
            int[,] obj = c.filter(60, 10);
            int[,] HaarImage = run.StepByStep_2(temp, obj); // filter 이미지 넣어서 그부분만 디텍팅하게 해야함

            temp = ip.BiasConvert_original(HaarImage, temp, 192);
            pictureBox3.Image = new Bitmap(temp, pictureBox3.Width, pictureBox3.Height);
            pictureBox3.Update();

            temp = ip.BiasConvert(HaarImage, 256);
            pictureBox6.Image = new Bitmap(temp, pictureBox6.Width, pictureBox6.Height);
            pictureBox6.Update();
        }

        private void 히스토그램세로출력ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[,] binary = ip.binary(ip.GrayArray(Original));
            temp = ip.BiasConvert(binary, 256);
            pictureBox2.Image = new Bitmap(temp, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Update();

            // 검은색 출력
            temp = run.Black(binary);
            pictureBox3.Image = new Bitmap(temp, pictureBox3.Width, pictureBox3.Height);
            pictureBox3.Update();
        }

        private void lUNGROIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[,] original = ip.GrayArray(Original);
            int[,] binary = ip.binary(original);
            temp = ip.BiasConvert(binary, 256);
            pictureBox2.Image = new Bitmap(temp, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Update();

            // 검은색 출력
            temp = run.Black(binary);
            pictureBox3.Image = new Bitmap(temp, pictureBox3.Width, pictureBox3.Height);
            pictureBox3.Update();

            // ROI
            int[,] LUNG = run.LUNG(binary, original, 500);
            temp = ip.BiasConvert(LUNG, 256);
            pictureBox4.Image = new Bitmap(temp, pictureBox4.Width, pictureBox4.Height);
            pictureBox4.Update();
        }

        private void lUNG예측ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 수평피처디텍터
            int[,] filterImage = run.Overlap_Matrix(Original);

            temp = ip.BiasConvert_original(filterImage, Original, 60);
            pictureBox2.Image = new Bitmap(temp, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Update();

            temp = ip.BiasConvert(filterImage, 256);
            pictureBox5.Image = new Bitmap(temp, pictureBox5.Width, pictureBox5.Height);
            pictureBox5.Update();

            // 단계별 하르 디텍터
            Original = new Bitmap(image, 500, 500); // 부담을 줄이기위해 사이즈 반으로
            int[,] obj = c.filter(60, 10);
            int[,] HaarImage = run.StepByStep_2(Original, obj); // filter 이미지 넣어서 그부분만 디텍팅하게 해야함

            temp = ip.BiasConvert_original(HaarImage, Original, 210);
            pictureBox3.Image = new Bitmap(temp, pictureBox3.Width, pictureBox3.Height);
            pictureBox3.Update();

            temp = ip.BiasConvert(HaarImage, 256);
            pictureBox6.Image = new Bitmap(temp, pictureBox6.Width, pictureBox6.Height);
            pictureBox6.Update();

            // 최종 예측
            int[,] fusion = run.HaarFilter_Pooling(HaarImage, filterImage);

            temp = ip.BiasConvert_original(fusion, Original, 170); // 140하는게 최대한 많이잡힘
            pictureBox4.Image = new Bitmap(temp, pictureBox4.Width, pictureBox4.Height);
            pictureBox4.Update();

            temp = ip.BiasConvert(fusion, 256);
            pictureBox8.Image = new Bitmap(temp, pictureBox8.Width, pictureBox8.Height);
            pictureBox8.Update();

            // LUNG
            int[,] original = ip.GrayArray(Original);
            int[,] binary = ip.binary(original);
            int[,] LUNG = run.LUNG(binary, fusion, 250);
            int[,] LUNG_ori = run.LUNG(binary, original, 250);

            Original = ip.BiasConvert(LUNG_ori, 256);
            temp = ip.BiasConvert_original(LUNG, Original, 170);
            pictureBox4.Image = new Bitmap(temp, pictureBox4.Width, pictureBox4.Height);
            pictureBox4.Update();

            // 실시간조정을 위한 temp
            tempfusion = fusion;
            temphaar = HaarImage;
            tempfilter = filterImage;
        }

        private void testsetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 40개에 80개의 가중치를 적용
            run.Overlap_Matrix_rmse();
        }

        private void avgtestsetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            run.average_rmse();
        }

        private void 적용전후표시ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 수평피처디텍터
            int[,] filterImage = run.Overlap_Matrix(Original);

            temp = ip.BiasConvert_original(filterImage, Original, 60);
            pictureBox2.Image = new Bitmap(temp, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Update();

            temp = ip.BiasConvert(filterImage, 256);
            pictureBox5.Image = new Bitmap(temp, pictureBox5.Width, pictureBox5.Height);
            pictureBox5.Update();

            int[,] filterImage2 = run.Overlap_Matrix_test(Original);

            temp = ip.BiasConvert_original(filterImage2, Original, 60);
            pictureBox3.Image = new Bitmap(temp, pictureBox3.Width, pictureBox3.Height);
            pictureBox3.Update();

            temp = ip.BiasConvert(filterImage2, 256);
            pictureBox6.Image = new Bitmap(temp, pictureBox6.Width, pictureBox6.Height);
            pictureBox6.Update();

            temphaar = filterImage2;
            tempfilter = filterImage;
        }

        private void 앙상블예측ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void loadFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 정답과 비교해서 특징값을 불러옴

            // 원본이미지 불러오기
            image = Image.FromFile(fix.DataFolder + @"\Original Data\ileus2 (1).jpg");
            Original = new Bitmap(image, 1000, 1000);
            pictureBox1.Image = new Bitmap(Original, pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Update();

            // filter 특징맵 생성
            int[,] filterImage = run.Overlap_Matrix(Original);
            temp = ip.BiasConvert(filterImage, 256);
            pictureBox7.Image = new Bitmap(temp, pictureBox7.Width, pictureBox7.Height);
            pictureBox7.Update();

            // 정답이미지 불러오기
            image = Image.FromFile(fix.DataFolder + @"\Cleansing Data\ileus2 (1).jpg");
            temp = new Bitmap(image, 1000, 1000);
            pictureBox2.Image = new Bitmap(temp, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Update();

            // 리스트 출력
            List<int> feature = run.FeatureList(filterImage, temp);
            Console.WriteLine("1번사진의 Feature평균 : "+feature.Average());

            // 이미지 닫기
            image.Dispose();
        }

        private void 원본검증ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 정답에 원본을 색칠함

            // 원본이미지 불러오기
            image = Image.FromFile(fix.DataFolder + @"\Original Data\ileus2 (1).jpg");
            Original = new Bitmap(image, 1000, 1000);
            pictureBox1.Image = new Bitmap(Original, pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Update();

            // filter 특징맵 생성
            int[,] filterImage = run.Overlap_Matrix(Original);
            Original = new Bitmap(image, 500, 500);
            int[,] obj = c.filter(60, 10);
            int[,] HaarImage = run.StepByStep_2(Original, obj);
            int[,] fusion = run.HaarFilter_Pooling(HaarImage, filterImage);

            temp = ip.BiasConvert(fusion, 256);
            pictureBox7.Image = new Bitmap(temp, pictureBox7.Width, pictureBox7.Height);
            pictureBox7.Update();

            // 정답이미지 불러오기
            image = Image.FromFile(fix.DataFolder + @"\Cleansing Data\ileus2 (1).jpg");
            temp = new Bitmap(image, 500, 500);
            pictureBox2.Image = new Bitmap(temp, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Update();

            // 비트맵 출력
            Original = run.Verification(fusion, temp, Original);
            pictureBox5.Image = new Bitmap(Original, pictureBox5.Width, pictureBox5.Height);
            pictureBox5.Update();

            // 퓨전 출력
            image = Image.FromFile(fix.DataFolder + @"\Original Data\ileus2 (1).jpg");
            Original = new Bitmap(image, 500, 500);
            temp = ip.BiasConvert_original(fusion, Original, 170);
            pictureBox3.Image = new Bitmap(temp, pictureBox3.Width, pictureBox3.Height);
            pictureBox3.Update();

            // 이미지 닫기
            image.Dispose();
        }

        private void 모든Filter임계치ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 모든 사진의 filter의 임계치 평균을 구함

            Console.WriteLine(run.all_filter_avg());
        }

        private void 모든Haar임계치ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine(run.all_haar_avg());
        }

        private void 비교알고리즘ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 결합 비교알고리즘
            Bitmap ans;
            Bitmap Extra;
            int[,] fusion;
            int[,] obj = c.filter(60, 10);
            int[,] filterImage;
            int[,] HaarImage;
            double temp = 0;
            List<double> bias_collection = new List<double> { };

            for (int pic = 1; pic <= fix.NumOfImage; pic++)
            {
                Console.WriteLine(pic + "번째 사진-----------------------");
                // 원본이미지 불러오기
                image = Image.FromFile(fix.DataFolder + @"\Original Data\ileus2 (" + pic + ").jpg");
                Original = new Bitmap(image, 1000, 1000);
                pictureBox1.Image = new Bitmap(Original, pictureBox1.Width, pictureBox1.Height);
                pictureBox1.Update();

                // 정답이미지 불러오기
                image = Image.FromFile(fix.DataFolder + @"\Cleansing Data\ileus2 (" + pic + ").jpg");
                ans = new Bitmap(image, 500, 500);
                pictureBox2.Image = new Bitmap(ans, pictureBox2.Width, pictureBox2.Height);
                pictureBox2.Update();

                for (int bias=1; bias<=99; bias++)
                {
                    Console.WriteLine("바이어스 비율 : "+bias + ":" +(100-bias));
                    filterImage = run.Overlap_Matrix(Original);
                    HaarImage = run.StepByStep_2(Original, obj);

                    // 바이어스값 받게
                    fusion = run.BiasCombination(HaarImage, filterImage,bias);

                    Extra = ip.BiasConvert(fusion, 110);
                    temp = com.IoU(Extra, ans);
                    Console.WriteLine("{0:N2}%", (temp * 100));

                    // IoU값을 리스트에 추가
                    if (pic == 1)
                        bias_collection.Add(temp);
                    else
                        bias_collection[bias - 1] += temp;

                    pictureBox7.Image = new Bitmap(Extra, pictureBox7.Width, pictureBox7.Height);
                    pictureBox7.Update();
                }
                
                Console.WriteLine("진행률: {0:N2}%", (((double)pic / fix.NumOfImage) * 100));
            }

            // 출력
            Console.WriteLine("---------------전체 결과값---------------");
            for(int i=0; i<fix.NumOfImage; i++)
                Console.WriteLine(bias_collection[i] / fix.NumOfImage);
        }

        private void all임계치ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 하나의 사진에 임계치를 모두 적용하는 알고리즘
            Bitmap Extra;
            int maxbias=-1;
            double maxIoU = 0.0;
            int count = 0;

            // 원본이미지 불러오기
            image = Image.FromFile(fix.DataFolder + @"\Original Data\ileus2 (1).jpg");
            Original = new Bitmap(image, 1000, 1000);
            pictureBox1.Image = new Bitmap(Original, pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Update();
            List<double> IoU = new List<double> { };

            // 정답이미지 불러오기
            image = Image.FromFile(fix.DataFolder + @"\Cleansing Data\ileus2 (1).jpg");
            Bitmap ans = new Bitmap(image, 1000, 1000);
            pictureBox2.Image = new Bitmap(ans, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Update();

            // filter 특징맵 생성
            int[,] filterImage = run.Overlap_Matrix(Original);
            for (int i = 0; i < 256; i++)
            {
                Extra = ip.BiasConvert(filterImage, i);
                IoU.Add(com.IoU(Extra, ans));
                Console.WriteLine("{0:N2}%",((IoU[i] / 256) * 100));

                if (IoU[i] > maxIoU)
                {
                    maxIoU = IoU[i];
                    maxbias = i;
                    count = 0;
                }
                else
                    count++;

                if (count > 10)
                    break;

                pictureBox7.Image = new Bitmap(Extra, pictureBox7.Width, pictureBox7.Height);
                pictureBox7.Update();

                Console.WriteLine("진행률: {0:N2}%", (((double)i/256)*100));
            }

            Console.WriteLine("가장 높은 임계치 : " + maxbias);
        }

        private void 모든임계치ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 모든 사진에 임계치를 모두 적용하는 알고리즘
            Bitmap Extra;
            Bitmap ans;
            int maxbias;
            double maxIoU;
            int count;
            double temp=0;
            List<int> bias = new List<int> { };

            for (int pic = 1; pic <= fix.NumOfImage; pic++)
            {
                maxbias = -1;
                maxIoU = 0.0;
                count = 0;

                // 원본이미지 불러오기
                image = Image.FromFile(fix.DataFolder + @"\Original Data\ileus2 ("+pic+").jpg");
                Original = new Bitmap(image, 1000, 1000);
                pictureBox1.Image = new Bitmap(Original, pictureBox1.Width, pictureBox1.Height);
                pictureBox1.Update();

                // 정답이미지 불러오기
                image = Image.FromFile(fix.DataFolder + @"\Cleansing Data\ileus2 (" + pic + ").jpg");
                ans = new Bitmap(image, 1000, 1000);
                pictureBox2.Image = new Bitmap(ans, pictureBox2.Width, pictureBox2.Height);
                pictureBox2.Update();

                // filter 특징맵 생성
                int[,] filterImage = run.Overlap_Matrix(Original);
                for (int i = 0; i < 256; i++)
                {
                    Extra = ip.BiasConvert(filterImage, i);
                    temp = com.IoU(Extra, ans);
                    Console.WriteLine("{0:N2}%", ((temp / 256) * 100));

                    if (temp > maxIoU)
                    {
                        maxIoU = temp;
                        maxbias = i;
                        count = 0;
                    }
                    else
                        count++;

                    if (count > 10)
                        break;

                    pictureBox7.Image = new Bitmap(Extra, pictureBox7.Width, pictureBox7.Height);
                    pictureBox7.Update();

                    // Console.WriteLine("진행률: {0:N2}%", (((double)i / 256) * 100));
                }

                // 모두 maxbias 리스트에 추가
                bias.Add(maxbias);

                Console.WriteLine("가장 높은 임계치 : " + maxbias);
                Console.WriteLine("진행률: {0:N2}%", (((double)pic / fix.NumOfImage) * 100));
            }

            Console.WriteLine("--------------------------------");
            Console.WriteLine("임계치의 평균 : " + bias.Average());
        }

        private void 모든Haar임계치ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Bitmap Extra;
            Bitmap ans;
            int maxbias;
            double maxIoU;
            int count;
            double temp = 0;
            List<int> bias = new List<int> { };
            int[,] obj = c.filter(60, 10);

            for (int pic = 1; pic <= fix.NumOfImage; pic++)
            {
                maxbias = -1;
                maxIoU = 0.0;
                count = 0;

                // 원본이미지 불러오기
                image = Image.FromFile(fix.DataFolder + @"\Original Data\ileus2 (" + pic + ").jpg");
                Original = new Bitmap(image, 500, 500);
                pictureBox1.Image = new Bitmap(Original, pictureBox1.Width, pictureBox1.Height);
                pictureBox1.Update();

                // 정답이미지 불러오기
                image = Image.FromFile(fix.DataFolder + @"\Cleansing Data\ileus2 (" + pic + ").jpg");
                ans = new Bitmap(image, 500, 500);
                pictureBox2.Image = new Bitmap(ans, pictureBox2.Width, pictureBox2.Height);
                pictureBox2.Update();

                // haar 특징맵 생성
                int[,] haarImage = run.StepByStep_2(Original, obj);
                for (int i = 0; i < 256; i++)
                {
                    Extra = ip.BiasConvert(haarImage, i);
                    temp = com.IoU(Extra, ans);
                    Console.WriteLine("{0:N3}%", ((temp / 256) * 100));

                    if (temp > maxIoU)
                    {
                        maxIoU = temp;
                        maxbias = i;
                        count = 0;
                    }
                    else
                        count++;

                    if (count > 10)
                        break;

                    pictureBox7.Image = new Bitmap(Extra, pictureBox7.Width, pictureBox7.Height);
                    pictureBox7.Update();

                    // Console.WriteLine("진행률: {0:N2}%", (((double)i / 256) * 100));
                }

                // 모두 maxbias 리스트에 추가
                bias.Add(maxbias);

                Console.WriteLine("가장 높은 임계치 : " + maxbias);
                Console.WriteLine("진행률: {0:N2}%", (((double)pic / fix.NumOfImage) * 100));
            }

            Console.WriteLine("--------------------------------");
            Console.WriteLine("임계치의 평균 : " + bias.Average());
        }

        private void 모든결합임계치ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap ans;
            Bitmap Extra;
            int[,] fusion;
            int[,] obj = c.filter(60, 10);
            int[,] filterImage;
            int[,] HaarImage;
            double maxIoU;
            int maxbias;
            int count;
            double temp = 0;
            List<int> bias = new List<int> { };

            for (int pic = 1; pic <= fix.NumOfImage; pic++)
            {
                maxbias = -1;
                maxIoU = 0.0;
                count = 0;

                // 원본이미지 불러오기
                image = Image.FromFile(fix.DataFolder + @"\Original Data\ileus2 (" + pic + ").jpg");
                Original = new Bitmap(image, 1000, 1000);
                pictureBox1.Image = new Bitmap(Original, pictureBox1.Width, pictureBox1.Height);
                pictureBox1.Update();

                // 정답이미지 불러오기
                image = Image.FromFile(fix.DataFolder + @"\Cleansing Data\ileus2 (" + pic + ").jpg");
                ans = new Bitmap(image, 500, 500);
                pictureBox2.Image = new Bitmap(ans, pictureBox2.Width, pictureBox2.Height);
                pictureBox2.Update();

                filterImage = run.Overlap_Matrix(Original);
                Original = new Bitmap(image, 500, 500);
                HaarImage = run.StepByStep_2(Original, obj);
                fusion = run.HaarFilter_Pooling(HaarImage, filterImage);

                for (int i = 0; i < 256; i++)
                {
                    Extra = ip.BiasConvert(fusion, i);
                    temp = com.IoU(Extra, ans);
                    Console.WriteLine("{0:N2}%", (temp * 100));

                    if (temp > maxIoU)
                    {
                        maxIoU = temp;
                        maxbias = i;
                        count = 0;
                    }
                    else
                        count++;

                    if (count > 20)
                        break;

                    pictureBox7.Image = new Bitmap(Extra, pictureBox7.Width, pictureBox7.Height);
                    pictureBox7.Update();
                }

                bias.Add(maxbias);

                Console.WriteLine("가장 높은 임계치 : " + maxbias);
                Console.WriteLine("진행률: {0:N2}%", (((double)pic / fix.NumOfImage) * 100));
            }

            Console.WriteLine("--------------------------------");
            Console.WriteLine("임계치의 평균 : " + bias.Average());
        }

        private void 비율과임계치ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 비율과 임계치를 같이 값을 넣음으로써 최적의 임계치를 구한다.
            int[,] fusion;
            Bitmap ans;
            int[,] obj = c.filter(60, 10);
            int[,] filterImage;
            int[,] HaarImage;
            Bitmap Extra;
            double temp = 0;
            double maxIoU;
            int maxbias;
            List<int> Mostbias = new List<int> { };
            double flag;
            int count;

            for (int pic = 1; pic <= fix.NumOfImage; pic++)
            {
                maxbias = 20;
                maxIoU = 0.0;

                // 원본이미지 불러오기
                image = Image.FromFile(fix.DataFolder + @"\Original Data\ileus2 (" + pic + ").jpg");
                Original = new Bitmap(image, 1000, 1000);
                pictureBox1.Image = new Bitmap(Original, pictureBox1.Width, pictureBox1.Height);
                pictureBox1.Update();

                // 정답이미지 불러오기
                image = Image.FromFile(fix.DataFolder + @"\Cleansing Data\ileus2 (" + pic + ").jpg");
                ans = new Bitmap(image, 500, 500);
                pictureBox2.Image = new Bitmap(ans, pictureBox2.Width, pictureBox2.Height);
                pictureBox2.Update();

                for (int bias = 1; bias <= 99; bias++)
                {
                    Console.WriteLine("바이어스 비율 = " + bias + ":" + (100 - bias));
                    filterImage = run.Overlap_Matrix(Original);
                    HaarImage = run.StepByStep_2(Original, obj);
                    flag = 0;
                    count = 0;

                    // 바이어스값 받게
                    fusion = run.BiasCombination(HaarImage, filterImage, bias);

                    for (int i = maxbias-20; i < 256; i++)
                    {
                        Extra = ip.BiasConvert(fusion, i);
                        temp = com.IoU(Extra, ans);
                        Console.WriteLine("IoU값 : {0:N2}%", (temp * 100));
                        
                        if (temp > maxIoU)
                        {
                            maxIoU = temp;
                            maxbias = i;
                            Console.WriteLine("최대 IoU값의 바이어스 : {0}", maxbias);
                        }

                        // 20번 이상 하향일시 스톱
                        if (temp > flag)
                        {
                            flag = temp;
                            count = 0;
                        }
                        else
                            count++;

                        if (count > 20)
                            break;

                        pictureBox7.Image = new Bitmap(Extra, pictureBox7.Width, pictureBox7.Height);
                        pictureBox7.Update();
                    }
                }

                Mostbias.Add(maxbias);

                Console.WriteLine("가장 높은 임계치 : " + maxbias);
                Console.WriteLine("진행률: {0:N2}%", (((double)pic / fix.NumOfImage) * 100));
            }

            Console.WriteLine("--------------------------------");
            Console.WriteLine("임계치의 평균 : " + Mostbias.Average());
        }

        private void haar좋은거따로ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Haar만 좋은거 따로 돌려서 Haar가 높을수록 좋아지는지 확인하기
            Bitmap ans;
            Bitmap Extra;
            Bitmap Extra_ori;
            int[,] fusion;
            int[,] obj = c.filter(60, 10);
            int[,] filterImage;
            int[,] HaarImage;
            double maxIoU;
            int maxbias;
            int count;
            double temp = 0;
            List<int> Mostbias = new List<int> { };

            maxbias = -1;
            maxIoU = 0.0;
            count = 0;
            maxbias = 0;
            maxIoU = 0.0;
            double flag;

            // 원본이미지 불러오기
            image = Image.FromFile(fix.DataFolder + @"\Original Data\ileus2 (9).jpg");
            Original = new Bitmap(image, 500, 500);
            pictureBox1.Image = new Bitmap(Original, pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Update();

            // 정답이미지 불러오기
            image = Image.FromFile(fix.DataFolder + @"\Cleansing Data\ileus2 (9).jpg");
            ans = new Bitmap(image, 500, 500);
            pictureBox2.Image = new Bitmap(ans, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Update();

            for (int bias = 1; bias <= 9; bias++)
            {
                Console.WriteLine("바이어스 비율 = " + bias + ":" + (10 - bias));
                filterImage = run.Overlap_Matrix(Original);
                HaarImage = run.StepByStep_2(Original, obj);
                flag = 0;
                count = 0;

                // 바이어스값 받게
                fusion = run.BiasCombination(HaarImage, filterImage, bias);

                // 위아래 자르기
                fusion = ip.UPandDOWN_mat(fusion);

                Extra_ori = ip.BiasConvert_original(fusion, Original, 140); // 140하는게 최대한 많이잡힘
                pictureBox5.Image = new Bitmap(Extra_ori, pictureBox5.Width, pictureBox5.Height);
                pictureBox5.Update();

                for (int i = 0; i < 256; i++)
                {
                    Extra = ip.BiasConvert(fusion, i);
                    temp = com.IoU(Extra, ans);
                    Console.WriteLine("IoU값 : {0:N2}%", (temp * 100));

                    if (temp > maxIoU)
                    {
                        maxIoU = temp;
                        maxbias = i;
                        Console.WriteLine("최대 IoU값의 바이어스 : {0}", maxbias);
                    }

                    // 20번 이상 하향일시 스톱
                    if (temp > flag)
                    {
                        flag = temp;
                        count = 0;
                    }
                    else
                        count++;

                    if (count > 20)
                        break;

                    pictureBox7.Image = new Bitmap(Extra, pictureBox7.Width, pictureBox7.Height);
                    pictureBox7.Update();
                }
            }

            Console.WriteLine("가장 높은 임계치 : " + maxbias);
        }

        private void 결합추력ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void filter파란색시가고하ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap Extra;
            Bitmap ans;
            int maxbias;
            double maxIoU;
            int count;
            int[,] obj = c.filter(60, 10);
            double temp = 0;

            // 원본이미지 불러오기
            image = Image.FromFile(fix.DataFolder + @"\Original Data\ileus2 (9).jpg");
            Original = new Bitmap(image, 500, 500);
            pictureBox1.Image = new Bitmap(Original, pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Update();

            // 정답이미지 불러오기
            image = Image.FromFile(fix.DataFolder + @"\Cleansing Data\ileus2 (9).jpg");
            ans = new Bitmap(image, 500, 500);
            pictureBox2.Image = new Bitmap(ans, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Update();

            // filter 특징맵 생성
            int[,] filterImage = run.StepByStep_2(Original, obj); // 하르
            // int[,] filterImage = run.Overlap_Matrix(Original);

            maxbias = -1;
            maxIoU = 0.0;
            count = 0;

            for (int i = 0; i < 256; i++)
            {
                Extra = ip.BiasConvert(filterImage, i);
                temp = com.IoU(Extra, ans);
                Console.WriteLine("{0:N2}%", (temp * 100));

                if (temp > maxIoU)
                {
                    maxIoU = temp;
                    maxbias = i;
                    count = 0;
                }
                else
                    count++;

                if (count > 10)
                    break;

                Extra = com.blue(Extra, ans,Original);
                pictureBox7.Image = new Bitmap(Extra, pictureBox7.Width, pictureBox7.Height);
                pictureBox7.Update();
            }
        }

        private void 블롭라벨링예제ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenCV cv = new OpenCV();
            IplImage ip = new IplImage(@"E:\개인프젝자료\장폐색\1. Data\Test Data\16. BlobTest2.png");
            
            IplImage result = cv.BlobImage(ip);

            Bitmap bit = result.ToBitmap();

            pictureBox1.Image = new Bitmap(bit, pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Update();
        }

        private void 원본이진화ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[,] fusion;
            Bitmap Extra_ori;
            int[,] filterImage;
            int[,] HaarImage;
            int[,] obj = c.filter(60, 10);

            // 정답
            image = Image.FromFile(fix.DataFolder + @"\Cleansing Data\ileus2 (9).jpg");
            Bitmap ans = new Bitmap(image, 500, 500);

            ans = run.Red_Black(ans);

            pictureBox1.Image = new Bitmap(ans, pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Update();

            // 예측
            image = Image.FromFile(fix.DataFolder + @"\Original Data\ileus2 (9).jpg");
            Original = new Bitmap(image, 500, 500);

            filterImage = run.Overlap_Matrix(Original);
            HaarImage = run.StepByStep_2(Original, obj);

            fusion = run.BiasCombination(HaarImage, filterImage, 5);
            fusion = ip.UPandDOWN_mat(fusion);

            Extra_ori = ip.BiasConvert_original(fusion, Original, 140);

            Extra_ori = run.Red_Black(Extra_ori);
            pictureBox5.Image = new Bitmap(Extra_ori, pictureBox5.Width, pictureBox5.Height);
            pictureBox5.Update();
        }

        private void 이진화를라벨링ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 기본이미지를 받아서 라벨링하기
            int[,] fusion;
            Bitmap Extra_ori;
            int[,] filterImage;
            int[,] HaarImage;
            int[,] obj = c.filter(60, 10);

            // 기본 이미지 불러오기
            image = Image.FromFile(fix.DataFolder + @"\Original Data\ileus2 (9).jpg");
            Original = new Bitmap(image, 500, 500);

            // 장폐색을 예측
            filterImage = run.Overlap_Matrix(Original);
            HaarImage = run.StepByStep_2(Original, obj);

            fusion = run.BiasCombination(HaarImage, filterImage, 5);
            fusion = ip.UPandDOWN_mat(fusion);

            Extra_ori = ip.BiasConvert_original(fusion, Original, 150);

            // 빨간색 부분을 기반으로 이진화
            Extra_ori = run.Red_Black(Extra_ori);

            // 예측 이미지를 블롭라벨링
            OpenCV cv = new OpenCV();
            IplImage ipImage = Extra_ori.ToIplImage();
            IplImage result = cv.BlobImage(ipImage);

            // 블롭정보를 출력
            IplImage blob;
            blob = new IplImage(ipImage.Size, BitDepth.U8, 3);

            CvBlobs blobs = cv.BlobInfo(ipImage);
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

            // 비트맵화 후에 출력
            Bitmap bit = result.ToBitmap();

            pictureBox1.Image = new Bitmap(bit, pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Update();
        }

        private void 블롭정보를리턴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Test Image 불러오기
            image = Image.FromFile(fix.DataFolder + @"\Test Data\18. Test Image2.png");
            Bitmap TestBimap = new Bitmap(image);

            IplImage ipImage = TestBimap.ToIplImage();
            pictureBox1.Image = new Bitmap(ipImage.ToBitmap(), pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Update();

            image = Image.FromFile(fix.DataFolder + @"\Test Data\17_2. Answer Image.png");
            Bitmap AnswerBimap = new Bitmap(image);

            ipImage = AnswerBimap.ToIplImage();
            pictureBox2.Image = new Bitmap(ipImage.ToBitmap(), pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Update();

            int TP = run.TP(TestBimap, AnswerBimap);
            Console.WriteLine(TP);
        }

        private void tPFNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Test Image 불러오기
            image = Image.FromFile(fix.DataFolder + @"\Test Data\19. Test Image3.png");
            Bitmap TestBimap = new Bitmap(image);

            IplImage ipImage = TestBimap.ToIplImage();
            pictureBox1.Image = new Bitmap(ipImage.ToBitmap(), pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Update();

            image = Image.FromFile(fix.DataFolder + @"\Test Data\19. Answer Image3.png");
            Bitmap AnswerBimap = new Bitmap(image);

            ipImage = AnswerBimap.ToIplImage();
            pictureBox2.Image = new Bitmap(ipImage.ToBitmap(), pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Update();

            List<int> TPFN = run.TPFN(TestBimap, AnswerBimap);
            Console.WriteLine(TPFN[0]);
            Console.WriteLine(TPFN[1]);
        }

        private void tPFNFPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            image = Image.FromFile(fix.DataFolder + @"\Test Data\20. Test Image4.png");
            Bitmap TestBimap = new Bitmap(image);

            IplImage ipImage = TestBimap.ToIplImage();
            pictureBox1.Image = new Bitmap(ipImage.ToBitmap(), pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Update();

            image = Image.FromFile(fix.DataFolder + @"\Test Data\20. Answer Image4.png");
            Bitmap AnswerBimap = new Bitmap(image);

            ipImage = AnswerBimap.ToIplImage();
            pictureBox2.Image = new Bitmap(ipImage.ToBitmap(), pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Update();

            List<int> TPFNFP = run.TPFNFP(TestBimap, AnswerBimap);
            Console.WriteLine(TPFNFP[0]);
            Console.WriteLine(TPFNFP[1]);
            Console.WriteLine(TPFNFP[2]);
        }

        private void 라벨링정보를대조ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            image = Image.FromFile(fix.DataFolder + @"\Original Data\ileus2 (1).jpg");
            Bitmap TestBimap = new Bitmap(image, 1000, 1000);

            IplImage ipImage = TestBimap.ToIplImage();
            pictureBox1.Image = new Bitmap(ipImage.ToBitmap(), pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Update();

            image = Image.FromFile(fix.DataFolder + @"\Cleansing Data\ileus2 (1).jpg");
            Bitmap AnswerBimap = new Bitmap(image, 1000, 1000);

            ipImage = AnswerBimap.ToIplImage();
            pictureBox2.Image = new Bitmap(ipImage.ToBitmap(), pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Update();

            pictureBox3.Image = new Bitmap(run.accuracy(TestBimap, AnswerBimap), pictureBox2.Width, pictureBox2.Height);
            pictureBox3.Update();
        }

        private void 임계치조절ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            image = Image.FromFile(fix.DataFolder + @"\Original Data\ileus2 (50).jpg");
            Bitmap TestBimap = new Bitmap(image, 1000, 1000);

            pictureBox1.Image = new Bitmap(TestBimap, pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Update();

            image = Image.FromFile(fix.DataFolder + @"\Cleansing Data\ileus2 (50).jpg");
            Bitmap AnswerBimap = new Bitmap(image, 1000, 1000);

            pictureBox2.Image = new Bitmap(AnswerBimap, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Update();

            int[,] fusion;
            Bitmap Extra_ori;
            int[,] filterImage;
            int[,] HaarImage;
            int[,] obj = c.filter(60, 10);
            OpenCV cv = new OpenCV();
            IplImage ipImage;
            IplImage result;
            List<int> TPFNFP;

            // 장폐색 예측
            HaarImage = run.StepByStep_2(TestBimap, obj);
            Console.WriteLine("하르맵 생성");

            filterImage = run.Overlap_Matrix(TestBimap);
            Console.WriteLine("필터맵 생성");

            fusion = run.BiasCombination(HaarImage, filterImage, 5);
            fusion = ip.UPandDOWN_mat(fusion);

            AnswerBimap = run.Red_Black(AnswerBimap);

            for (int i=50; i<256; i++)
            {
                // 임계치
                Extra_ori = ip.BiasConvert_original(fusion, TestBimap, i);

                // 장폐색을 이진화
                Extra_ori = run.Red_Black(Extra_ori);

                ipImage = Extra_ori.ToIplImage();
                result = cv.BlobImage(ipImage);

                // 출력
                pictureBox3.Image = new Bitmap(result.ToBitmap(), pictureBox3.Width, pictureBox3.Height);
                pictureBox3.Update();

                // TP & FP & FN
                TPFNFP = run.TPFNFP(Extra_ori, AnswerBimap);
                Console.WriteLine("---------임계치:" + i + "---------");
                Console.WriteLine(TPFNFP[0]);
                Console.WriteLine(TPFNFP[1]);
                Console.WriteLine(TPFNFP[2]/5);

                int sum = TPFNFP[0] + TPFNFP[1] + TPFNFP[2]/5;
                Console.WriteLine("정확도 : " + ((double)TPFNFP[0] / sum * 100));
            }
        }

        private void 가중치조절ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            image = Image.FromFile(fix.DataFolder + @"\Original Data\ileus2 (23).jpg");
            Bitmap TestBimap = new Bitmap(image, 1000, 1000);

            pictureBox1.Image = new Bitmap(TestBimap, pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Update();

            image = Image.FromFile(fix.DataFolder + @"\Cleansing Data\ileus2 (23).jpg");
            Bitmap AnswerBimap = new Bitmap(image, 1000, 1000);

            pictureBox2.Image = new Bitmap(AnswerBimap, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Update();

            int[,] fusion;
            Bitmap Extra_ori;
            int[,] filterImage;
            int[,] HaarImage;
            int[,] obj = c.filter(60, 10);
            OpenCV cv = new OpenCV();
            IplImage ipImage;
            IplImage result;
            List<int> TPFNFP;
            int[] accuracy = new int[9];
            int max_accuracy = 0;

            // 장폐색 예측
            HaarImage = run.StepByStep_2(TestBimap, obj);
            Console.WriteLine("하르맵 생성");

            filterImage = run.Overlap_Matrix(TestBimap);
            Console.WriteLine("필터맵 생성");

            AnswerBimap = run.Red_Black(AnswerBimap);

            // 가중치 조정
            for (int w = 1; w <= accuracy.Length; w++)
            {
                fusion = run.BiasCombination(HaarImage, filterImage, w);
                fusion = ip.UPandDOWN_mat(fusion);

                max_accuracy = 0;

                for (int i = 0; i < 256; i++)
                {
                    // 임계치
                    Extra_ori = ip.BiasConvert_original(fusion, TestBimap, i);

                    // 장폐색을 이진화
                    Extra_ori = run.Red_Black(Extra_ori);

                    ipImage = Extra_ori.ToIplImage();
                    result = cv.BlobImage(ipImage);

                    // 출력
                    pictureBox3.Image = new Bitmap(result.ToBitmap(), pictureBox3.Width, pictureBox3.Height);
                    pictureBox3.Update();

                    // TP & FP & FN
                    TPFNFP = run.TPFNFP(Extra_ori, AnswerBimap);

                    int sum = TPFNFP[0] + TPFNFP[1] + TPFNFP[2]/5;
                    double accuracy_now = (double)TPFNFP[0] / sum * 100;
                    Console.WriteLine("정확도 : " + accuracy_now);

                    // 가장 높은값 저장
                    if (max_accuracy < accuracy_now)
                    {
                        max_accuracy = (int)accuracy_now;
                        accuracy[w - 1] = max_accuracy;
                    }

                    if (TPFNFP[0] == 0)
                        break;
                }
            }

            foreach (int j in accuracy)
                Console.WriteLine(j);
        }

        private void 출력ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            image = Image.FromFile(fix.DataFolder + @"\Original Data\ileus2 (1).jpg");
            Bitmap TestBimap = new Bitmap(image, 1000, 1000);

            pictureBox1.Image = new Bitmap(TestBimap, pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Update();

            image = Image.FromFile(fix.DataFolder + @"\Cleansing Data\ileus2 (1).jpg");
            Bitmap AnswerBimap = new Bitmap(image, 1000, 1000);

            pictureBox2.Image = new Bitmap(AnswerBimap, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Update();

            int[,] fusion;
            Bitmap Extra_ori;
            int[,] filterImage;
            int[,] HaarImage;
            int[,] obj = c.filter(60, 10);
            OpenCV cv = new OpenCV();
            IplImage ipImage;
            IplImage result;
            List<int> TPFNFP;
            double filter_max = 0;
            double fusion_max = 0;
            double haar_max = 0; 

            // 장폐색 예측
            HaarImage = run.StepByStep_2(TestBimap, obj);
            Console.WriteLine("하르맵 생성");

            filterImage = run.Overlap_Matrix(TestBimap);
            Console.WriteLine("필터맵 생성");

            // filter_max
            fusion = run.BiasCombination(HaarImage, filterImage, 9);
            fusion = ip.UPandDOWN_mat(fusion);

            AnswerBimap = run.Red_Black(AnswerBimap);

            for (int i = 0; i < 256; i++)
            {
                // 임계치
                Extra_ori = ip.BiasConvert_original(fusion, TestBimap, i);

                // 장폐색을 이진화
                Extra_ori = run.Red_Black(Extra_ori);

                ipImage = Extra_ori.ToIplImage();
                result = cv.BlobImage(ipImage);

                // 출력
                pictureBox3.Image = new Bitmap(result.ToBitmap(), pictureBox3.Width, pictureBox3.Height);
                pictureBox3.Update();

                // TP & FP & FN
                TPFNFP = run.TPFNFP(Extra_ori, AnswerBimap);

                int sum = TPFNFP[0] + TPFNFP[1] + TPFNFP[2]/5;
                Console.WriteLine("정확도 : " + ((double)TPFNFP[0] / sum * 100));

                // filter max
                filter_max = Math.Max(filter_max, (double)TPFNFP[0] / sum * 100);

                if (TPFNFP[0] == 0)
                    break;
            }

            // haar max
            fusion = run.BiasCombination(HaarImage, filterImage, 1);
            fusion = ip.UPandDOWN_mat(fusion);

            for (int i = 0; i < 256; i++)
            {
                // 임계치
                Extra_ori = ip.BiasConvert_original(fusion, TestBimap, i);

                // 장폐색을 이진화
                Extra_ori = run.Red_Black(Extra_ori);

                ipImage = Extra_ori.ToIplImage();
                result = cv.BlobImage(ipImage);

                // 출력
                pictureBox3.Image = new Bitmap(result.ToBitmap(), pictureBox3.Width, pictureBox3.Height);
                pictureBox3.Update();

                // TP & FP & FN
                TPFNFP = run.TPFNFP(Extra_ori, AnswerBimap);

                int sum = TPFNFP[0] + TPFNFP[1] + TPFNFP[2] / 5;
                Console.WriteLine(TPFNFP[0]);
                Console.WriteLine(TPFNFP[1]);
                Console.WriteLine(TPFNFP[2]);
                Console.WriteLine(sum);
                Console.WriteLine("정확도 : " + ((double)TPFNFP[0] / sum * 100));

                // max
                haar_max = Math.Max(haar_max, (double)TPFNFP[0] / sum * 100);

                if (TPFNFP[0] == 0)
                    break;
            }

            // fusion
            fusion = run.BiasCombination(HaarImage, filterImage, 1);
            fusion = ip.UPandDOWN_mat(fusion);
            
            for (int i = 0; i < 256; i++)
            {
                // 임계치
                Extra_ori = ip.BiasConvert_original(fusion, TestBimap, i);

                // 장폐색을 이진화
                Extra_ori = run.Red_Black(Extra_ori);

                ipImage = Extra_ori.ToIplImage();
                result = cv.BlobImage(ipImage);

                // 출력
                pictureBox3.Image = new Bitmap(result.ToBitmap(), pictureBox3.Width, pictureBox3.Height);
                pictureBox3.Update();

                // TP & FP & FN
                TPFNFP = run.TPFNFP(Extra_ori, AnswerBimap);

                int sum = TPFNFP[0] + TPFNFP[1] + TPFNFP[2] / 5;
                Console.WriteLine("정확도 : " + ((double)TPFNFP[0] / sum * 100));

                // max
                fusion_max = Math.Max(fusion_max, (double)TPFNFP[0] / sum * 100);

                if (TPFNFP[0] == 0)
                    break;
            }

            // 출력
            Console.WriteLine("-------------------------------");
            Console.WriteLine("filter max 정확도 : " + filter_max);
            Console.WriteLine("haar max 정확도 : " + haar_max);
            Console.WriteLine("fusion max 정확도 : " + fusion_max);
        }

        private void 모든사진에적용ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[,] fusion;
            Bitmap Extra_ori;
            int[,] filterImage;
            int[,] HaarImage;
            int[,] obj = c.filter(60, 10);
            OpenCV cv = new OpenCV();
            IplImage ipImage;
            IplImage result;
            List<int> TPFNFP;
            int[] accuracy = new int[9];
            int max_accuracy = 0;

            for (int pic = 1; pic <= fix.NumOfImage; pic++)
            {
                image = Image.FromFile(fix.DataFolder + @"\Original Data\ileus2 ("+pic+").jpg");
                Bitmap TestBimap = new Bitmap(image, 1000, 1000);

                pictureBox1.Image = new Bitmap(TestBimap, pictureBox1.Width, pictureBox1.Height);
                pictureBox1.Update();

                image = Image.FromFile(fix.DataFolder + @"\Cleansing Data\ileus2 (" + pic + ").jpg");
                Bitmap AnswerBimap = new Bitmap(image, 1000, 1000);

                pictureBox2.Image = new Bitmap(AnswerBimap, pictureBox2.Width, pictureBox2.Height);
                pictureBox2.Update();

                // 장폐색 예측
                HaarImage = run.StepByStep_2(TestBimap, obj);
                Console.WriteLine("하르맵 생성");

                filterImage = run.Overlap_Matrix(TestBimap);
                Console.WriteLine("필터맵 생성");

                AnswerBimap = run.Red_Black(AnswerBimap);

                // 가중치 조정
                for (int w = 1; w <= accuracy.Length; w++)
                {
                    fusion = run.BiasCombination(HaarImage, filterImage, w);
                    fusion = ip.UPandDOWN_mat(fusion);

                    max_accuracy = 0;

                    for (int i = 0; i < 256; i++)
                    {
                        // 임계치
                        Extra_ori = ip.BiasConvert_original(fusion, TestBimap, i);

                        // 장폐색을 이진화
                        Extra_ori = run.Red_Black(Extra_ori);

                        ipImage = Extra_ori.ToIplImage();
                        result = cv.BlobImage(ipImage);

                        // 출력
                        pictureBox3.Image = new Bitmap(result.ToBitmap(), pictureBox3.Width, pictureBox3.Height);
                        pictureBox3.Update();

                        // TP & FP & FN
                        TPFNFP = run.TPFNFP(Extra_ori, AnswerBimap);

                        int sum = TPFNFP[0] + TPFNFP[1] + TPFNFP[2] / 5;
                        double accuracy_now = (double)TPFNFP[0] / sum * 100;
                        Console.WriteLine("정확도 : " + accuracy_now);

                        // 가장 높은값 저장
                        if (max_accuracy < accuracy_now)
                            max_accuracy = (int)accuracy_now;

                        if (TPFNFP[0] == 0)
                            break;
                    }

                    accuracy[w - 1] += max_accuracy;
                }
            }

            foreach (int j in accuracy)
                Console.WriteLine(j);
        }

        private void 스무딩적용ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 스무딩 적용해서 비교하기
            int[,] filterImage;
            Bitmap Extra_ori;
            OpenCV cv = new OpenCV();
            IplImage ipImage;
            IplImage result;

            // 원본사진 불러오기
            int pic = 9;
            image = Image.FromFile(fix.DataFolder + @"\Original Data\ileus2 (" + pic + ").jpg");
            Bitmap TestBimap = new Bitmap(image, 1000, 1000);

            pictureBox1.Image = new Bitmap(TestBimap, pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Update();

            // Filter 장폐색 예측
            filterImage = run.Overlap_Matrix(TestBimap);
            Console.WriteLine("필터맵 생성");

            // 임계치
            int i = 130;
            Extra_ori = ip.BiasConvert_original(filterImage, TestBimap, i);

            // 블롭라벨링
            Extra_ori = run.Red_Black(Extra_ori);

            ipImage = Extra_ori.ToIplImage();
            result = cv.BlobImage(ipImage);

            pictureBox2.Image = new Bitmap(result.ToBitmap(), pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Update();

            // 스무딩 먹이기
            int[,] filter_smoothing = ip.smoothing_1(filterImage);
            Extra_ori = ip.BiasConvert_original(filter_smoothing, TestBimap, i);

            // 블롭라벨링
            Extra_ori = run.Red_Black(Extra_ori);

            ipImage = Extra_ori.ToIplImage();
            result = cv.BlobImage(ipImage);

            pictureBox3.Image = new Bitmap(result.ToBitmap(), pictureBox3.Width, pictureBox3.Height);
            pictureBox3.Update();
        }

        private void 스무딩전체ㅣㅂ교ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[,] fusion;
            Bitmap Extra_ori;
            int[,] filterImage;
            int[,] HaarImage;
            int[,] obj = c.filter(60, 10);
            OpenCV cv = new OpenCV();
            IplImage ipImage;
            IplImage result;
            List<int> TPFNFP;
            int[] accuracy = new int[9];
            int max_accuracy = 0;

            for (int pic = 1; pic <= fix.NumOfImage; pic++)
            {
                image = Image.FromFile(fix.DataFolder + @"\Original Data\ileus2 (" + pic + ").jpg");
                Bitmap TestBimap = new Bitmap(image, 500, 500);

                pictureBox1.Image = new Bitmap(TestBimap, pictureBox1.Width, pictureBox1.Height);
                pictureBox1.Update();

                image = Image.FromFile(fix.DataFolder + @"\Cleansing Data\ileus2 (" + pic + ").jpg");
                Bitmap AnswerBimap = new Bitmap(image, 500, 500);

                pictureBox2.Image = new Bitmap(AnswerBimap, pictureBox2.Width, pictureBox2.Height);
                pictureBox2.Update();

                // 장폐색 예측
                HaarImage = run.StepByStep_2(TestBimap, obj);
                HaarImage = ip.smoothing_1(HaarImage);
                Console.WriteLine("하르맵 생성");

                filterImage = run.Overlap_Matrix(TestBimap);
                filterImage = ip.smoothing_1(filterImage);
                Console.WriteLine("필터맵 생성");

                AnswerBimap = run.Red_Black(AnswerBimap);

                // 가중치 조정
                for (int w = 1; w <= accuracy.Length; w++)
                {
                    fusion = run.BiasCombination(HaarImage, filterImage, w);
                    fusion = ip.UPandDOWN_mat(fusion);

                    max_accuracy = 0;

                    for (int i = 0; i < 256; i++)
                    {
                        // 임계치
                        Extra_ori = ip.BiasConvert_original(fusion, TestBimap, i);

                        // 장폐색을 이진화
                        Extra_ori = run.Red_Black(Extra_ori);

                        ipImage = Extra_ori.ToIplImage();
                        result = cv.BlobImage(ipImage);

                        // 출력
                        pictureBox3.Image = new Bitmap(result.ToBitmap(), pictureBox3.Width, pictureBox3.Height);
                        pictureBox3.Update();

                        // TP & FP & FN
                        TPFNFP = run.TPFNFP(Extra_ori, AnswerBimap);

                        int sum = TPFNFP[0] + TPFNFP[1] + TPFNFP[2] / 5;
                        double accuracy_now = (double)TPFNFP[0] / sum * 100;
                        Console.WriteLine("정확도 : " + accuracy_now);

                        // 가장 높은값 저장
                        if (max_accuracy < accuracy_now)
                            max_accuracy = (int)accuracy_now;

                        if (TPFNFP[0] == 0)
                            break;
                    }

                    accuracy[w - 1] += max_accuracy;
                }
            }

            foreach (int j in accuracy)
                Console.WriteLine(j);
        }
    }
}