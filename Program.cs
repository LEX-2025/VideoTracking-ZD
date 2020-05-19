/*
 * This sample program is ported by C# from examples\video_tracking_ex.cpp.
 *来源 https://github.com/takuya-takeuchi/DlibDotNet/tree/master/examples/VideoTracking
 * 
 * Lex 2020-5-19 修改
*/

using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using DlibDotNet;
using OpenCvSharp; //OPENCV机器视觉库

namespace VideoTracking
{

    internal class Program
    {
        /// <summary>
        /// 追踪框X
        /// </summary>
        static double a_X = 60;
        /// <summary>
        /// 追踪框Y
        /// </summary>
        static double a_Y = 60;
        /// <summary>
        /// 追踪框 W
        /// </summary>
        static double a_W = 40;
        /// <summary>
        /// 追踪框H
        /// </summary>
        static double a_H = 40;

        private static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Call this program like this: ");
                Console.WriteLine("VideoTracking.exe <path of video_frames directory>");
                return;
            }

            var path = args[0];
            var files = new DirectoryInfo(path).GetFiles("*.jpg").Select(info => info.FullName  ).ToList();

        

            files.Sort();

            if (files.Count == 0)
            {
                Console.WriteLine($"No images found in {path}");
                return;
            }


            // 定义图像捕捉方式 从摄像头 ， 注意 Windows下需要选择 VideoCaptureAPIs.DSHOW
            var cap = new VideoCapture(0, VideoCaptureAPIs.DSHOW);

            // 定义图像捕捉方式 从摄像头 视频文件
            //var cap = new VideoCapture("video.webm");

            //判断捕捉设备是否打开
            if (!cap.IsOpened())
            {
                Console.WriteLine("Unable to connect to camera");
                return;
            }

            Mat temp = null;
            var tracker = new CorrelationTracker();

            int init = 0;

            //定义显示窗口
            using(var win = new ImageWindow())
            {
                Console.WriteLine("对象追踪程序启动");
                Console.WriteLine("选择命令行为当前窗口，通过按键选择需要追踪的区域Width: [A,Z] Height:[S,X] X:[right,left] Y:[up,down] ,点击Enter开始追踪");
                Console.WriteLine("注意：切换命令行窗口输入法为英文输入状态");
                //选择追踪对象
                while (!win.IsClosed())
                {
                    //获得1帧图片
                    temp = cap.RetrieveMat();// new Mat();


                    if (temp == null)
                    {
                        Console.WriteLine("图像获取错误!");
                        return;
                    }

                    var array = new byte[temp.Width * temp.Height * temp.ElemSize()];
                    Marshal.Copy(temp.Data, array, 0, array.Length);
                    using (var cimg = Dlib.LoadImageData<BgrPixel>(array, (uint)temp.Height, (uint)temp.Width, (uint)(temp.Width * temp.ElemSize())))
                    {

                        init++;
                        if (init > 1)
                        {
                            var KK = Console.ReadKey();
                            if (KK.Key == ConsoleKey.Enter)
                            {
                                Console.WriteLine("开始追踪目标!");

                                //确定 追踪 位置
                                var rect2 = DRectangle.CenteredRect(a_X, a_Y, a_W, a_H);
                                //开始追踪
                                tracker.StartTrack(cimg, rect2);
                                win.SetImage(cimg);
                                win.ClearOverlay();
                                win.AddOverlay(rect2);
                                break;
                            }

                            //选择 追踪区域
                            if (KK.Key == ConsoleKey.RightArrow || KK.Key == ConsoleKey.LeftArrow || KK.Key == ConsoleKey.UpArrow || KK.Key == ConsoleKey.DownArrow || KK.Key == ConsoleKey.A || KK.Key == ConsoleKey.Z || KK.Key == ConsoleKey.S || KK.Key == ConsoleKey.X)
                            {
                                if (KK.Key == ConsoleKey.RightArrow)
                                {
                                    a_X++;
                                    if (a_X > cimg.Rect.Width - a_W)
                                    {
                                        a_X = cimg.Rect.Width - a_W;
                                    }
                                }
                                if (KK.Key == ConsoleKey.LeftArrow)
                                {
                                    a_X--;
                                    if (a_X < 0)
                                    {
                                        a_X = 0;
                                    }
                                }

                                if (KK.Key == ConsoleKey.UpArrow)
                                {
                                    a_Y--;
                                    if (a_Y < 0)
                                    {
                                        a_Y = 0;
                                    }

                                }
                                if (KK.Key == ConsoleKey.DownArrow)
                                {
                                    a_Y++;
                                    if (a_Y > cimg.Rect.Height - a_H)
                                    {
                                        a_Y = cimg.Rect.Height - a_H;
                                    }

                                }

                                if (KK.Key == ConsoleKey.A)
                                {
                                    a_W++;
                                    if (a_W >= cimg.Rect.Width - a_X)
                                    {
                                        a_W = cimg.Rect.Width - a_X;
                                    }
                                }
                                if (KK.Key == ConsoleKey.Z)
                                {
                                    a_W--;
                                    if (a_W < 10)
                                    {
                                        a_W = 10;
                                    }
                                }
                                if (KK.Key == ConsoleKey.S)
                                {
                                    a_H++;
                                    if (a_H > cimg.Rect.Height - a_Y)
                                    {
                                        a_H = cimg.Rect.Height - a_Y;
                                    }

                                }
                                if (KK.Key == ConsoleKey.X)
                                {
                                    a_H--;
                                    if (a_H < 10)
                                    {
                                        a_H = 10;
                                    }
                                }


                            }
                        }

                            var rect = DRectangle.CenteredRect(a_X, a_Y, a_W, a_H);

                            Console.WriteLine("Set RECT:" + a_X + " " + a_Y + " " + a_W + " " + a_H);

                            //显示图片
                            win.SetImage(cimg);
                            win.ClearOverlay();
                            //显示框
                            win.AddOverlay(rect);

                        

                    }


                }

                //选择追踪对象
                while (!win.IsClosed())
                {
                    //获得1帧图片
                    temp = cap.RetrieveMat();// new Mat();


                    if (temp == null)
                    {
                        Console.WriteLine("图像获取错误!");
                        return;
                    }

                    var array = new byte[temp.Width * temp.Height * temp.ElemSize()];
                    Marshal.Copy(temp.Data, array, 0, array.Length);
                    using (var cimg = Dlib.LoadImageData<BgrPixel>(array, (uint)temp.Height, (uint)temp.Width, (uint)(temp.Width * temp.ElemSize())))
                    {
                        //更新追踪图像
                        tracker.Update(cimg);

                        win.SetImage(cimg);
                        win.ClearOverlay();

                        //获得追踪到的目标位置
                        DRectangle rect = tracker.GetPosition();
                        win.AddOverlay(rect);


                        Console.WriteLine("OBJ RECT:" + (int)rect.Left + " " + (int)rect.Top + " " + (int)rect.Width + " " + (int)rect.Height);
                        
                        System.Threading.Thread.Sleep(100);
                    }
                }

            }

 
          
           
                Console.WriteLine("任意键退出");
                Console.ReadKey();

            
        }

    }

}


       