using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace angle
{
    class AngleOperation
    {
        int degree;
        int minute;
        public int second;
        private string sAngle;
        const double PI = 3.1415926;
        public AngleOperation(string mAngle)
        {
            sAngle = mAngle;
            splitAngle();
        }
        public override string ToString()
        {
            string mAngle = degree.ToString() + "." + minute.ToString("00") + second.ToString("00");
            return mAngle;
        }
        public void splitAngle()
        {
            string[] arrAngle = new string[2];
            arrAngle = sAngle.Split('.');
            string min = arrAngle[1].Substring(0, 2);
            string sec = arrAngle[1].Substring(2, 2);
            degree= Convert.ToInt32(arrAngle[0]);
            minute = Convert.ToInt32(min);
            second = Convert.ToInt32(sec);
        }
        public int Seconds
        {
            get
            {
                if (degree > 0)
                {
                    int sec = degree * 60 * 60 + minute * 60 + second;
                    return sec;
                }
                else
                {
                    int sec = -(Math.Abs(degree) * 60 * 60 + minute * 60 + second);
                    return sec;
                }
            }
        }
        public double Radian
        {
            get
            {
                double radian = this.Seconds * PI / 648000;
                return radian;
            }
        }
        public static AngleOperation operator + (AngleOperation an1,AngleOperation an2)
        {
            int ses = an1.Seconds + an2.Seconds;
            string mAngle = secondsTosAngle(ses);
            return new AngleOperation(mAngle);
        }
        public static string secondsTosAngle(int ses)
        {
            int de = ses / 60 / 60;
            int mi =Math.Abs((ses - de * 3600) / 60);
            int se =Math.Abs( (ses - de * 3600) % 60);
            string mAngle = de.ToString() + "." +mi.ToString("00")+ se.ToString("00");
            return mAngle;
        }
        public static string radianTosAngle(double rad)
        {
            double de = rad * 180 / PI;
            int se = (int)(de * 3600);
            return secondsTosAngle(se);
        }
        public static AngleOperation operator -(AngleOperation an1,AngleOperation an2)
        {
            int ses = an1.Seconds - an2.Seconds;
            string mAngle=secondsTosAngle(ses);
            return new AngleOperation(mAngle);
        }
        public static AngleOperation operator *(AngleOperation an1,int an2)
        {
            int ses = an1.Seconds * an2;
            string mAngle = secondsTosAngle(ses);
            return new AngleOperation(mAngle);
        }
    }
}
