using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1.Ileus
{
    class Comparison
    {

        public double IoU(Bitmap Extra, Bitmap Ans)
        {
            int xpad = ((Ans.Width - Extra.Width) % 2 == 1 ? (Ans.Width - Extra.Width) + 1 : (Ans.Width - Extra.Width)) / 2;
            int ypad = ((Ans.Height - Extra.Height) % 2 == 1 ? (Ans.Height - Extra.Height) + 1 : (Ans.Height - Extra.Height)) / 2;
            Color ExtC;
            Color AnsC;
            int unionCount = 0;
            int overlapCount = 0;

            for (int y = ypad; y < Ans.Height - ypad; y++)
            {
                for (int x = xpad; x < Ans.Width - xpad; x++)
                {
                    ExtC = Extra.GetPixel(x - xpad, y - ypad);
                    AnsC = Ans.GetPixel(x, y);

                    if ((ExtC.R > 150 && ExtC.B < 100) && (AnsC.R > 150 && AnsC.B < 100))// 둘다 붉은색
                        overlapCount++;
                    else if ((ExtC.R > 150 && ExtC.B < 100) || (AnsC.R > 150 && AnsC.B < 100)) // 하나만 붉은색
                        unionCount++;
                }
            }

            Console.WriteLine(overlapCount);
            Console.WriteLine(unionCount);
            return (double)overlapCount / unionCount;
        }

        public Bitmap blue(Bitmap Extra, Bitmap Ans, Bitmap Ori)
        {
            // 둘이 비교해서 같은 부분은 파란색으로 색칠하기
            int xpad = ((Ans.Width - Extra.Width) % 2 == 1 ? (Ans.Width - Extra.Width) + 1 : (Ans.Width - Extra.Width)) / 2;
            int ypad = ((Ans.Height - Extra.Height) % 2 == 1 ? (Ans.Height - Extra.Height) + 1 : (Ans.Height - Extra.Height)) / 2;
            Color ExtC;
            Color AnsC;
            Color OriC;

            int width = Ans.Width - xpad*2;
            int height = Ans.Height - ypad * 2;
            Bitmap result = new Bitmap(width, height);

            for (int y = ypad; y < Ans.Height - ypad; y++)
            {
                for (int x = xpad; x < Ans.Width - xpad; x++)
                {
                    ExtC = Extra.GetPixel(x - xpad, y - ypad);
                    AnsC = Ans.GetPixel(x, y);
                    OriC = Ori.GetPixel(x, y);

                    if ((ExtC.R > 150 && ExtC.B < 100) && (AnsC.R > 150 && AnsC.B < 100))// 둘다 붉은색
                        result.SetPixel(x - xpad, y - ypad, Color.Blue);
                    else if (AnsC.R > 150 && AnsC.B < 100) // 정답이미지만
                        result.SetPixel(x - xpad, y - ypad, Color.Red);
                    else if (ExtC.R > 150 && ExtC.B < 100) // 추출이미지만
                        result.SetPixel(x - xpad, y - ypad, Color.Yellow);
                    else
                        result.SetPixel(x - xpad, y - ypad, ExtC);
                }
            }

            return result;
        }
    }
}
