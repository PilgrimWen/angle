using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace angle
{
    class HandleFile
    {
        string path;
        double []A = new double[2];//已知点一
        double []B = new double[2];//已知点二
        AngleOperation[] angles;//观测角集合
        AngleOperation[] Azimuths;//方位角集合
        double[] dx;
        double[] dy;
        public double[] x;
        public double[] y;
        double[] distances;
        int count;
        public HandleFile(string path)
        {
            this.path = path;
            handle();
            updateAngles();
            fillArr();
        }
        public void handle()
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            string firstLine = sr.ReadLine();
            string[] firstPoint = firstLine.Split(',');
            A[0] = Convert.ToDouble(firstPoint[1]);
            A[1] = Convert.ToDouble(firstPoint[2]);
            string secondLine = sr.ReadLine();
            string[] secondPoint = secondLine.Split(',');
            B[0] = Convert.ToDouble(secondPoint[1]);
            B[1] = Convert.ToDouble(secondPoint[2]);
            string thirdLine = sr.ReadLine();
            string[] sAngle = thirdLine.Split(',');
            angles= new AngleOperation[sAngle.Length];//观测角集合
            for(int i = 0; i < sAngle.Length; i++)
            {
                angles[i] = new AngleOperation(sAngle[i]);
            }

            string forthLine = sr.ReadLine();
            string[] sDistance = forthLine.Split(',');
            count = sDistance.Length+1;
            distances = new double[sDistance.Length];
            for(int i = 0; i < sDistance.Length; i++)
            {
                distances[i] = Convert.ToDouble(sDistance[i]);
            }
            Console.WriteLine(forthLine);
            sr.Close();
        }
        public AngleOperation Azimuth
        {
            get
            {
                AngleOperation an180 = new AngleOperation("180.0000");
                AngleOperation an90 = new AngleOperation("90.0000");
                AngleOperation b= new AngleOperation(AngleOperation.radianTosAngle(Math.Atan((B[0] - A[0]) / (B[1] - A[1]))));
                AngleOperation a = an180 - an90 * Math.Sign(B[1] - A[1]) - b;
                return a;
            }
        }
        public int closure()//求闭合差
        {
            AngleOperation sum=new AngleOperation("0.0000");
            foreach(AngleOperation e in angles)
            {
                sum += e;
            }
            AngleOperation f = sum - new AngleOperation("180.0000") * (count-1);
            return f.Seconds; 
        }
        public double[] xyClosure(double []d)//求dxdy闭合差
        {
            double[] clo = new double[d.Length];
            double sum=0;
            double s = 0;
            for(int i = 0; i < distances.Length; i++)
            {
                s += distances[i];
            }
            foreach (double e in d)
            {
                sum += e;
            }
 
            for (int i = 0; i < clo.Length; i++)
            {
                clo[i] = -sum / s * distances[i];
            }
            return clo;
        }
        public void updateAngles()//闭合差改正
        {
            int f = closure();
            int v =Math.Abs(f / count);
            int i = Math.Abs(f) - v * count;
            for (int j = 0; j < angles.Length; j++)
            {
                if (f > 0)
                {
                    angles[j] = angles[j] - new AngleOperation("0.000" + v.ToString());
                }
                else
                {
                    angles[j] = angles[j] + new AngleOperation("0.000" + v.ToString());
                }
            }
            //foreach (AngleOperation e in angles)
            //{
            //    e.second = e.second - v;
            //}
            for (int j=0;j< i; j++)
            {
                if (f > 0)
                {
                    angles[j] = angles[j] - new AngleOperation("0.0001");
                }
                else
                {
                    angles[j] = angles[j] + new AngleOperation("0.0001");
                }
                
            }
        }
        public void fillArr()//计算方位角，dx,dy
        {
            Azimuths = new AngleOperation[count];
            double tempx=B[0];
            double tempy = B[1];
            AngleOperation temp = new AngleOperation("0.0000");
            dx = new double[count-1];
            dy = new double[count-1];
            x = new double[count-1];
            y = new double[count-1];
            for(int i = 0; i < count; i++)
            {
                temp += angles[i];
                Azimuths[i] = Azimuth + temp - new AngleOperation("180.0000")*(i+1);
                

            }
            for(int i = 0; i < count - 1; i++)
            {
                dx[i] = distances[i] * Math.Cos(Azimuths[i].Radian);
                dy[i] = distances[i] * Math.Sin(Azimuths[i].Radian);               
            }
            double[] cloX = xyClosure(dx);
            double[] cloY = xyClosure(dy);
            for(int i = 0; i < count - 1; i++)
            {
                x[i] = tempx + dx[i] + cloX[i];
                tempx = x[i];
                y[i] = tempy + dy[i] + cloY[i];
                tempy = y[i];
            }
        }
    }
}
