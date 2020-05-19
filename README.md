##C#开发人工智能应用：使用DlibDotNet实现目标追踪
 
##背景知识
(1)DlibDotNet

Dlib是一个包含机器学习算法的开源工具包，帮助开发者创建复杂的图像处理、机器学习算法解决实际问题，已经被广泛的用在行业和学术领域，包括机器人控制，图像识别系统，嵌入式装置，移动应用和大型高性能计算环境。Dlib.NET是Dlib的.NET跨平台开源封装，支持Windows, MacOS and Linux。


(2)目标追踪

目标追踪（Object Tracking）是计算机视觉领域的一个重要问题。它有多种用途，其中一些用途是：人机交互、安全监控、视频通信和压缩、增强现实、交通管制、医学成像。

开发环境
1.安装Visual Studio

安装Visual Studio 2017 更高版本，建议安装 Visual Studio 2019（https://visualstudio.microsoft.com/zh-hans/vs/community/），社区版是免费的，安装的时候需要选择安装.NetCore功能。


2.新建C# 命令行项目 通过NuGet添加程序包
新建一个.NET Core命令行程序项目


在解决方案资源管理器项目名称上单击鼠标右键，选择“管理NuGet程序包”


分别搜索安装 DlibDotNet 和 OpenCVSharp

注意 Windows下选择 OpenCvSharp4.Windows


##应用实例
本示例将会教会大家使用DlibDotNet实现目标选取和追踪。
