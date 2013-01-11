using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace TSC
{
public class TSCLIB_DLL
{
    /// <summary>
    /// 显示DLL 版本号码
    /// </summary>
    /// <returns></returns>
    [DllImport("TSCLIB.dll", EntryPoint = "about")]
    public static extern int about();

    /// <summary>
    /// 指定计算机端的输出端口
    /// </summary>
    /// <param name="printername">a.单机打印时，请指定打印机驱动程序名称，例如: TSC TTP/TDP-243(E) b.若连接打印机服务器，请指定服务器路径及共享 打印机 名称，例如:\\SERVER\TTP243</param>
    /// <returns></returns>
    [DllImport("TSCLIB.dll", EntryPoint = "openport")]
    public static extern int openport(string printername);
    
    /// <summary>
    /// 使用条形码机内建条形码打印
    /// </summary>
    /// <param name="x">条形码X方向起始点，以点(point)表示</param>
    /// <param name="y">条形码Y方向起始点，以点(point)表示</param>
    /// <param name="type">128,128M,EAN128,25,25C,39,39C,93,EAN13,EAN13+2,EAN13+5,EAN8,EAN8+2,EAN8+5,CODA,POST,UPCA,UPCA+2,UPCA+5,UPCE,UPCE+2,UPCE+5</param>
    /// <param name="height">设定条形码高度，高度以点来表示</param>
    /// <param name="readable">设定是否打印条形码码文:1打印0不打印</param>
    /// <param name="rotation">设定条形码旋转角度</param>
    /// <param name="narrow">设定条形码宽窄bar 比例因子</param>
    /// <param name="wide">设定条形码宽窄bar 比例因子</param>
    /// <param name="code">条形码内容</param>
    /// <returns></returns>
    [DllImport("TSCLIB.dll", EntryPoint = "barcode")]
    public static extern int barcode(string x, string y, string type,
                string height, string readable, string rotation,
                string narrow, string wide, string code);

    /// <summary>
    /// 清除
    /// </summary>
    /// <returns></returns>
    [DllImport("TSCLIB.dll", EntryPoint = "clearbuffer")]
    public static extern int clearbuffer();

    /// <summary>
    /// 关闭指定的计算机端输出端口
    /// </summary>
    /// <returns></returns>
    [DllImport("TSCLIB.dll", EntryPoint = "closeport")]
    public static extern int closeport();

    /// <summary>
    /// 下载单色PCX 格式图文件至打印机
    /// </summary>
    /// <param name="filename">文件名(可包含路径)</param>
    /// <param name="image_name">下载至打印机内存内之文件名(请使用大写档名)</param>
    /// <returns></returns>
    [DllImport("TSCLIB.dll", EntryPoint = "downloadpcx")]
    public static extern int downloadpcx(string filename, string image_name);

    /// <summary>
    /// 跳页，该函式需在setup后使用
    /// </summary>
    /// <returns></returns>
    [DllImport("TSCLIB.dll", EntryPoint = "formfeed")]
    public static extern int formfeed();

    /// <summary>
    /// 设定纸张不回吐
    /// </summary>
    /// <returns></returns>
    [DllImport("TSCLIB.dll", EntryPoint = "nobackfeed")]
    public static extern int nobackfeed();

    /// <summary>
    /// 使用条形码机内建文字打印
    /// </summary>
    /// <param name="x">文字X方向起始点</param>
    /// <param name="y">文字Y方向起始点</param>
    /// <param name="fonttype">内建字型名称，共12种</param>
    /// <param name="rotation">设定文字旋转角度</param>
    /// <param name="xmul">设定文字X方向放大倍率，1~8</param>
    /// <param name="ymul">设定文字Y方向放大倍率，1~8</param>
    /// <param name="text">列印文字內容</param>
    /// <returns></returns>
    [DllImport("TSCLIB.dll", EntryPoint = "printerfont")]
    public static extern int printerfont(string x, string y, string fonttype,
                    string rotation, string xmul, string ymul,
                    string text);

    /// <summary>
    /// 打印卷标内容
    /// </summary>
    /// <param name="set">设定打印卷标式数</param>
    /// <param name="copy">设定打印卷标份数(copy)</param>
    /// <returns></returns>
    [DllImport("TSCLIB.dll", EntryPoint = "printlabel")]
    public static extern int printlabel(string set, string copy);

    /// <summary>
    /// 送内建指令到条形码打印机,详细指令请参考TSPL
    /// </summary>
    /// <param name="printercommand"></param>
    /// <returns></returns>
    [DllImport("TSCLIB.dll", EntryPoint = "sendcommand")]
    public static extern int sendcommand(string printercommand);

    /// <summary>
    /// 设定卷标的宽度、高度、打印速度、打印浓度、感应器类别、gap/black mark 垂直间距、gap/black mark 偏移距离)
    /// </summary>
    /// <param name="width">字符串型别，设定卷标宽度，单位 mm</param>
    /// <param name="height">字符串型别，设定卷标高度，单位 mm</param>
    /// <param name="speed">字符串型别，设定打印速度.1.0: 每秒1.0吋打印速度  1.5: 每秒1.5吋打印速度  2.0: 每秒2.0吋打印速度 3.0: 每秒3.0吋打印速度  4.0: 每秒4.0吋打印速度   5.0: 每秒5.0吋打印速度   6.0: 每秒6.0吋打印速度</param>
    /// <param name="density">字符串型别，设定打印浓度，0~15，数字愈大打印结果愈黑</param>
    /// <param name="sensor">符串型别，设定使用感应器类别.0 表示使用垂直间距传感器(gap sensor)   1表示使用黑标传感器(black mark sensor)</param>
    /// <param name="vertical">设定gap/black mark 垂直间距高度，单位: mm</param>
    /// <param name="offset">字符串型别，设定gap/black mark 偏移距离，单位: mm，此参数若使用一般卷标时均设为0</param>
    /// <returns></returns>
    [DllImport("TSCLIB.dll", EntryPoint = "setup")]
    public static extern int setup(string width, string height,
              string speed, string density,
              string sensor, string vertical,
              string offset);

    /// <summary>
    /// 使用Windows TTF字型打印文字
    /// </summary>
    /// <param name="x">文字X方向起始点，以点(point)表示</param>
    /// <param name="y">文字Y方向起始点，以点(point)表示</param>
    /// <param name="fontheight">字体高度，以点(point)表示</param>
    /// <param name="rotation">转角度，逆时钟方向旋转</param>
    /// <param name="fontstyle">整数型别，字体外形0-> 标准(Normal);1-> 斜体(Italic);2-> 粗体(Bold);3-> 粗斜体(Bold and Italic)</param>
    /// <param name="fontunderline">整数型别, 底线 0-> 无底线 ;1-> 加底线</param>
    /// <param name="szFaceName">字符串型别，字体名称\n如: Arial, Times new Roman, 细名体, 标楷体</param>
    /// <param name="content">字符串型别，打印文字内容</param>
    /// <returns></returns>
    [DllImport("TSCLIB.dll", EntryPoint = "windowsfont")]
    public static extern int windowsfont(int x, int y, int fontheight,
                    int rotation, int fontstyle, int fontunderline,
                    string szFaceName, string content);

}
}