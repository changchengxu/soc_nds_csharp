using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HDICSoft.Message;
using HDICSoft.DB;
using HDICSoft.Func;
using HDICSoft.Command;
using BarcodeLib;

namespace soc_nds_csharp.DB_Manage
{
    public partial class gf_Barcode : Form
    {
        BarcodeLib.Barcode b = new BarcodeLib.Barcode();
        string BarcodeLibType = string.Empty;//add encode type variable

        public gf_Barcode()
        {
            InitializeComponent();
        }

        private void gf_Barcode_Load(object sender, EventArgs e)
        {
            this.panel_Detail.Visible = false;

            Bitmap temp = new Bitmap(1, 1);
            temp.SetPixel(0, 0, this.BackColor);
            pic_barcode.Image = (Image)temp;
            cbEncodeType.SelectedIndex = 0;
        }

        #region 为TSC TTP-344M Plus打印机参数变量赋值 并将值赋给相应控件
        string tscSensor = "";
        string tscVertical = "";
        string tscOffset = "";
        string tscFontType = "";
        string tscFontRotation = "";
        string tscBarcodeHeight = "";
        string tscBarWide = "";
        string tscPrintLabelSetNum = "";
        string tscPrintLabelCopeNum = "";
        /// <summary>
        /// 为条形码参数变量赋值
        /// </summary>
        private void TSCVariableInit()
        {

            DataTable dt = HDIC_Func.XMLToDataSet(HDIC_Func.GetRunningPath() + @"config\BarcodeXml.xml").Tables["root"];
              if (dt.Rows.Count > 0)
              {
                  txt_OutPutPort.Text = dt.Rows[0]["tscOutPutPort"].ToString().Trim();
                  txt_tscWidth.Text = dt.Rows[0]["tscWidth"].ToString().Trim();
                  txt_tscHeight.Text = dt.Rows[0]["tscHeight"].ToString().Trim();
                  txt_tscSpeed.Text = dt.Rows[0]["tscPrintSpeed"].ToString().Trim();
                  txt_tscDensity.Text = dt.Rows[0]["tscDensity"].ToString().Trim();
                  txt_tscX.Text = dt.Rows[0]["tscX"].ToString().Trim();
                  txt_tscY.Text = dt.Rows[0]["tscY"].ToString().Trim();
                  cbEncodeType.Text = dt.Rows[0]["tscEncodeType"].ToString().Trim();
                  chk_PrintBarCode.Checked = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["tscPrintCode"].ToString().Trim()));
                  cbox_tscRotate.Text = dt.Rows[0]["tscRotate"].ToString().Trim();
                  txt_tscBarNarrow.Text = dt.Rows[0]["tscBarNarrow"].ToString().Trim();

                  txt_codeInterval.Text = dt.Rows[0]["tscCodeInterval"].ToString().Trim();
                  txt_barCodeInterval.Text = dt.Rows[0]["tscBarCodeInterval"].ToString().Trim();
                  txt_FontMagnify1.Text = dt.Rows[0]["tscFontMagnify1"].ToString().Trim();
                  txt_FontMagnify2.Text = dt.Rows[0]["tscFontMagnify2"].ToString().Trim();

                  tscSensor = dt.Rows[0]["tscSensor"].ToString().Trim();
                  tscVertical = dt.Rows[0]["tscVertical"].ToString().Trim();
                  tscOffset = dt.Rows[0]["tscOffset"].ToString().Trim();
                  tscFontType = dt.Rows[0]["tscFontType"].ToString().Trim();
                  tscFontRotation = dt.Rows[0]["tscFontRotation"].ToString().Trim();
                  tscBarcodeHeight = dt.Rows[0]["tscBarcodeHeight"].ToString().Trim();
                  tscBarWide = dt.Rows[0]["tscBarWide"].ToString().Trim();
                  tscPrintLabelSetNum = dt.Rows[0]["tscPrintLabelSetNum"].ToString().Trim();
                  tscPrintLabelCopeNum = dt.Rows[0]["tscPrintLabelCopeNum"].ToString().Trim();
              }
        }
        #endregion

        #region 博思得打印机参数变量赋值 并将值赋给条形码打印机
        string POSTEKOutPutPort = "";
        string POSTEKPrintSpeed = "";
        string POSTEKDensity = "";
        string POSTEKHeight = "";
        string POSTEKInterval = "";
        string POSTEKWidth = "";
        string POSTEKX = "";
        string POSTEKY = "";
        string POSTEKBarRotation = "";
        string POSTEKEncodeType = "";
        string POSTEKBarNarrow = "";
        string POSTEKBarWide = "";
        string POSTEKBarcodeHeight = "";
        string POSTEKPrintCode = "";
        string POSTEKBarCodeInterval = "";
        string POSTEKCodeInterval = "";
        string POSTEKFontHeight = "";
        string POSTEKFontWidth = "";
        string POSTEKFontType = "";
        string POSTEKFontAlign = "";
        string POSTEKFontWeight = "";
        string POSTEKPrintLabelSetNum = "";
        string POSTEKPrintLabelCopeNum = "";

        private void POSTEKVariableInit()
        {
            using (DataTable dt = HDIC_Func.XMLToDataSet(HDIC_Func.GetRunningPath() + @"config\BarcodeXml.xml").Tables["root"])
            {
                if (dt.Rows.Count > 0)
                {
                    POSTEKOutPutPort = dt.Rows[0]["POSTEKOutPutPort"].ToString().Trim();
                    POSTEKPrintSpeed = dt.Rows[0]["POSTEKPrintSpeed"].ToString().Trim();
                    POSTEKDensity = dt.Rows[0]["POSTEKDensity"].ToString().Trim();
                    POSTEKHeight = dt.Rows[0]["POSTEKHeight"].ToString().Trim();
                    POSTEKInterval = dt.Rows[0]["POSTEKInterval"].ToString().Trim();
                    POSTEKWidth = dt.Rows[0]["POSTEKWidth"].ToString().Trim();
                    POSTEKX = dt.Rows[0]["POSTEKX"].ToString().Trim();
                    POSTEKY = dt.Rows[0]["POSTEKY"].ToString().Trim();
                    POSTEKBarRotation = dt.Rows[0]["POSTEKBarRotation"].ToString().Trim();
                    POSTEKEncodeType = dt.Rows[0]["POSTEKEncodeType"].ToString().Trim();
                    POSTEKBarNarrow = dt.Rows[0]["POSTEKBarNarrow"].ToString().Trim();
                    POSTEKBarWide = dt.Rows[0]["POSTEKBarWide"].ToString().Trim();
                    POSTEKBarcodeHeight = dt.Rows[0]["POSTEKBarcodeHeight"].ToString().Trim();
                    POSTEKPrintCode = dt.Rows[0]["POSTEKPrintCode"].ToString().Trim();
                    POSTEKBarCodeInterval = dt.Rows[0]["POSTEKBarCodeInterval"].ToString().Trim();
                    POSTEKCodeInterval = dt.Rows[0]["POSTEKCodeInterval"].ToString().Trim();
                    POSTEKFontHeight = dt.Rows[0]["POSTEKFontHeight"].ToString().Trim();
                    POSTEKFontWidth = dt.Rows[0]["POSTEKFontWidth"].ToString().Trim();
                    POSTEKFontType = dt.Rows[0]["POSTEKFontType"].ToString().Trim();
                    POSTEKFontAlign = dt.Rows[0]["POSTEKFontAlign"].ToString().Trim();
                    POSTEKFontWeight = dt.Rows[0]["POSTEKFontWeight"].ToString().Trim();
                    POSTEKPrintLabelSetNum = dt.Rows[0]["POSTEKPrintLabelSetNum"].ToString().Trim();
                    POSTEKPrintLabelCopeNum = dt.Rows[0]["POSTEKPrintLabelCopeNum"].ToString().Trim();
                }
            }
        }
        #endregion

        #region 立像打印机参数变量赋值 并将值赋给条形码打印机
        string ArogoxOutPutPort = "";
        string ArogoxnEnable = "";
        string Arogoxhor = "";
        string Arogoxver = "";
        string Arogoxobject = "";
        string Arogoxdarkness = "";
        string ArogoxIsImmediate = "";
        string Arogoxpbuf = "";
        string ArogoxX = "";
        string ArogoxY = "";
        string Arogoxori = "";
        string ArogoxEncodeType = "";
        string ArogoxBarNarrow = "";
        string ArogoxBarWide = "";
        string ArogoxBarcodeHeight = "";
        string ArogoxPrintCode = "";
        string ArogoxBarCodeInterval = "";
        string ArogoxCodeInterval = "";
        string Arogoxfont = "";
        string Arogoxhor_factor = "";
        string Arogoxver_factor = "";
        string Arogoxmode = "";
        string ArogoxPrintLabelCopeNum = "";
        private void ArogoxVariableInit()
        {
            using (DataTable dt = HDIC_Func.XMLToDataSet(HDIC_Func.GetRunningPath() + @"config\BarcodeXml.xml").Tables["root"])
            {
                if (dt.Rows.Count > 0)
                {
                    ArogoxOutPutPort = dt.Rows[0]["ArogoxOutPutPort"].ToString().Trim();
                    Arogoxhor = dt.Rows[0]["Arogoxhor"].ToString().Trim();
                    Arogoxver = dt.Rows[0]["Arogoxver"].ToString().Trim();
                    Arogoxobject = dt.Rows[0]["Arogoxobject"].ToString().Trim();
                    Arogoxdarkness = dt.Rows[0]["Arogoxdarkness"].ToString().Trim();
                    ArogoxIsImmediate = dt.Rows[0]["ArogoxIsImmediate"].ToString().Trim();
                    Arogoxpbuf = dt.Rows[0]["Arogoxpbuf"].ToString().Trim();
                    ArogoxX = dt.Rows[0]["ArogoxX"].ToString().Trim();
                    ArogoxY = dt.Rows[0]["ArogoxY"].ToString().Trim();
                    Arogoxori = dt.Rows[0]["Arogoxori"].ToString().Trim();
                    ArogoxEncodeType = dt.Rows[0]["ArogoxEncodeType"].ToString().Trim();
                    ArogoxBarNarrow = dt.Rows[0]["ArogoxBarNarrow"].ToString().Trim();
                    ArogoxBarWide = dt.Rows[0]["ArogoxBarWide"].ToString().Trim();
                    ArogoxBarcodeHeight = dt.Rows[0]["ArogoxBarcodeHeight"].ToString().Trim();
                    ArogoxPrintCode = dt.Rows[0]["ArogoxPrintCode"].ToString().Trim();
                    ArogoxBarCodeInterval = dt.Rows[0]["ArogoxBarCodeInterval"].ToString().Trim();
                    ArogoxCodeInterval = dt.Rows[0]["ArogoxCodeInterval"].ToString().Trim();
                    Arogoxfont = dt.Rows[0]["Arogoxfont"].ToString().Trim();
                    Arogoxhor_factor = dt.Rows[0]["Arogoxhor_factor"].ToString().Trim();
                    Arogoxver_factor = dt.Rows[0]["Arogoxver_factor"].ToString().Trim();
                    Arogoxmode = dt.Rows[0]["Arogoxmode"].ToString().Trim();
                    ArogoxPrintLabelCopeNum = dt.Rows[0]["ArogoxPrintLabelCopeNum"].ToString().Trim();
                }
            }
        }
        #endregion

        #region 条形码显示
        private void btnEncode_Click(object sender, EventArgs e)
        {
            if (cbEncodeType.SelectedIndex == -1)
            {
                HDIC_Message.ShowWarnDialog(this, "请选择编码类型");
                return;
            }
            int W = Convert.ToInt32(this.txtWidth.Text.Trim());
            int H = Convert.ToInt32(this.txtHeight.Text.Trim());

            BarcodeLib.TYPE type = BarcodeLib.TYPE.UNSPECIFIED;
            switch (BarcodeLibType.Trim())
            {
                case "UPC-A": type = BarcodeLib.TYPE.UPCA; break;
                case "UPC-A (Numbered)": type = BarcodeLib.TYPE.UPCA; break;
                case "UPC-E": type = BarcodeLib.TYPE.UPCE; break;
                case "UPC 2 Digit Ext.": type = BarcodeLib.TYPE.UPC_SUPPLEMENTAL_2DIGIT; break;
                case "UPC 5 Digit Ext.": type = BarcodeLib.TYPE.UPC_SUPPLEMENTAL_5DIGIT; break;
                case "EAN-13": type = BarcodeLib.TYPE.EAN13; break;
                case "JAN-13": type = BarcodeLib.TYPE.JAN13; break;
                case "EAN-8": type = BarcodeLib.TYPE.EAN8; break;
                case "ITF-14": type = BarcodeLib.TYPE.ITF14; break;
                case "Codabar": type = BarcodeLib.TYPE.Codabar; break;
                case "PostNet": type = BarcodeLib.TYPE.PostNet; break;
                case "Bookland/ISBN": type = BarcodeLib.TYPE.BOOKLAND; break;
                case "Code 11": type = BarcodeLib.TYPE.CODE11; break;
                case "Code 39": type = BarcodeLib.TYPE.CODE39; break;
                case "Code 39 Extended": type = BarcodeLib.TYPE.CODE39Extended; break;
                case "Code 93": type = BarcodeLib.TYPE.CODE93; break;
                case "LOGMARS": type = BarcodeLib.TYPE.LOGMARS; break;
                case "MSI": type = BarcodeLib.TYPE.MSI_Mod10; break;
                case "Interleaved 2 of 5": type = BarcodeLib.TYPE.Interleaved2of5; break;
                case "Standard 2 of 5": type = BarcodeLib.TYPE.Standard2of5; break;
                case "Code 128": type = BarcodeLib.TYPE.CODE128; break;
                case "Code 128-A": type = BarcodeLib.TYPE.CODE128A; break;
                case "Code 128-B": type = BarcodeLib.TYPE.CODE128B; break;
                case "Code 128-C": type = BarcodeLib.TYPE.CODE128C; break;
                //default: MessageBox.Show("Please specify the encoding type."); break;
            }//switch

            try
            {
                if (type != BarcodeLib.TYPE.UNSPECIFIED)
                {
                    b.IncludeLabel = this.chk_PrintBarCode.Checked;

                    //===== Encoding performed here =====
                    pic_barcode.Image = b.Encode(type, this.txt_STBID.Text.Trim(), Color.Black, Color.White, W, H);
                    //===================================

                    //===== Static Encoding performed here =====
                    //barcode.Image = BarcodeLib.Barcode.DoEncode(type, this.txtData.Text.Trim(), this.btnForeColor.BackColor, this.btnBackColor.BackColor);
                    //==========================================

                    txtEncoded.Text = b.EncodedValue;

                    pic_barcode.Width = pic_barcode.Image.Width;
                    pic_barcode.Height = pic_barcode.Image.Height;

                    tsslEncodedType.Text = "Encoding Type: " + b.EncodedType.ToString();
                }//if

                pic_barcode.Width = pic_barcode.Image.Width;
                pic_barcode.Height = pic_barcode.Image.Height;

                //reposition the barcode image to the middle
                // pic_barcode.Location = new Point((this.groupBox2.Location.X + this.groupBox2.Width / 2) - pic_barcode.Width / 2, (this.groupBox2.Location.Y + this.groupBox2.Height / 2) - pic_barcode.Height / 2);

            }//try
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 条形码另存为
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "BMP (*.bmp)|*.bmp|GIF (*.gif)|*.gif|JPG (*.jpg)|*.jpg|PNG (*.png)|*.png|TIFF (*.tif)|*.tif";
            sfd.AddExtension = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                BarcodeLib.SaveTypes savetype = BarcodeLib.SaveTypes.UNSPECIFIED;
                switch (sfd.FilterIndex)
                {
                    case 1: /* BMP */  savetype = BarcodeLib.SaveTypes.BMP; break;
                    case 2: /* GIF */  savetype = BarcodeLib.SaveTypes.GIF; break;
                    case 3: /* JPG */  savetype = BarcodeLib.SaveTypes.JPG; break;
                    case 4: /* PNG */  savetype = BarcodeLib.SaveTypes.PNG; break;
                    case 5: /* TIFF */ savetype = BarcodeLib.SaveTypes.TIFF; break;
                    default: break;
                }//switch
                b.SaveImage(sfd.FileName, savetype);
            }//if
        }
        #endregion

        #region 打印
        private void Print_Click(object sender, EventArgs e)
        {
            if (cbEncodeType.SelectedIndex == -1)
            {
                HDIC_Message.ShowWarnDialog(this, "请选择编码类型");
                return;
            }
            if (!CheckDigit())//验证文本框内容填写是否完整 和 正确
            {
                txt_SmartCardID.Text = "";
                txt_CAID.Text = "";
                txt_STBID.Text = "";
                return;
            }
            //printDocument1.Print();
            //TSCLIB_DLL.about();                                                                 //Show the DLL version

            //#region 1
            ////TSCLIB_DLL.openport("TSC TTP-344M Plus");                                           //Open specified printer driver
            ////TSCLIB_DLL.setup("100", "63.5", "4", "8", "0", "0", "0");                           //Setup the media size and sensor type info
            ////TSCLIB_DLL.clearbuffer();                                                           //Clear image buffer

            ////TSCLIB_DLL.barcode("100", "100", "128", "100", "1", "0", "2", "2", this.txtData.Text.Trim()); //Drawing barcode
            ////TSCLIB_DLL.printerfont("100", "250", "3", "0", "1", "1", "Print Font Test");        //Drawing printer font
            ////TSCLIB_DLL.windowsfont(100, 300, 24, 0, 0, 0, "ARIAL", "Windows Arial Font Test");  //Draw windows font

            ////TSCLIB_DLL.downloadpcx("UL.PCX", "UL.PCX");                                         //Download PCX file into printer
            ////TSCLIB_DLL.sendcommand("PUTPCX 100,400,\"UL.PCX\"");                                //Drawing PCX graphic

            ////TSCLIB_DLL.printlabel("1", "1");                                                    //Print labels
            ////TSCLIB_DLL.closeport();                                                             //Close specified printer driver 
            //#endregion

            //#region 2
            //TSCLIB_DLL.openport("TSC TTP-344M Plus");
            //TSCLIB_DLL.setup("83", "58", "4", "8", "0", "0", "0");                           //Setup the media size and sensor type info
            //TSCLIB_DLL.clearbuffer();

            //TSCLIB_DLL.barcode("200", "100", cbEncodeType.SelectedItem.ToString().Trim(), "100", Convert.ToString((int)chkGenerateLabel.CheckState), "0", "4", "2", this.txt_STBID.Text.Trim());
            ////TSCLIB_DLL.barcode("200", "250", "128", "100", "1", "0", "4", "2", this.txtData.Text.Trim());
            //TSCLIB_DLL.printerfont("200", "250", "3", "0", "1", "1", "ShangHai HDIC");        //Drawing printer font
            //TSCLIB_DLL.windowsfont(200, 300, 24, 0, 2, 0, "ARIAL", "长城测试");  //Draw windows font

            ////TSCLIB_DLL.windowsfont(250, 350, 160, 0, 2, 0, "Times new Roman", "邱晓淯");  //Draw windows font
            ////TSCLIB_DLL.downloadpcx("UL.PCX", "UL.PCX");                                         //Download PCX file into printer
            ////TSCLIB_DLL.sendcommand("PUTPCX 100,400,\"UL.PCX\"");                                //Drawing PCX graphic
            //TSCLIB_DLL.printlabel("1", "1");                                                    //Print labels
            //TSCLIB_DLL.closeport();
            //#endregion
            int flag = 1;
            if (txt_SmartCardID.Text.Trim().Length == 12)
            {
                flag = 0;
            }

       
        /////////////////////////////////////
            string CurrentPrinter = "";
            using (DataTable dt = HDIC_Func.XMLToDataSet(HDIC_Func.GetRunningPath() + @"config\BarcodeXml.xml").Tables["root"])
            {
                if (dt.Rows.Count > 0)
                {
                    CurrentPrinter = dt.Rows[0]["CurrentPrinter"].ToString().Trim();
                }

                if (CurrentPrinter == dt.Rows[0]["tscOutPutPort"].ToString().Trim())//TSC TTP-344M Plus 打印机
                {
                    TSCVariableInit();
                    HDIC_Func.TSCPrinter(txt_OutPutPort.Text.Trim(), txt_tscWidth.Text.Trim(), txt_tscHeight.Text.Trim(), txt_tscSpeed.Text.Trim(), txt_tscDensity.Text.Trim(), tscSensor, tscVertical, tscOffset, txt_tscX.Text.Trim(), txt_tscY.Text.Trim(), tscFontType, tscFontRotation, cbEncodeType.SelectedItem.ToString().Trim(), tscBarcodeHeight, Convert.ToString((int)chk_PrintBarCode.CheckState), txt_codeInterval.Text.Trim(), txt_FontMagnify1.Text.Trim(), txt_FontMagnify2.Text.Trim(), txt_barCodeInterval.Text.Trim(), txt_STBID.Text.Trim(), txt_CAID.Text.Trim(), txt_SmartCardID.Text.Trim(), cbox_tscRotate.SelectedItem.ToString().Trim(), txt_tscBarNarrow.Text.Trim(), tscBarWide, flag, tscPrintLabelSetNum, tscPrintLabelCopeNum);

                }
                else if (CurrentPrinter == dt.Rows[0]["POSTEKOutPutPort"].ToString().Trim())//博思得打印机
                {
                    POSTEKVariableInit();
                    HDIC_Func.POSTEKPrinter(POSTEKOutPutPort, POSTEKPrintSpeed, POSTEKDensity, POSTEKHeight, POSTEKInterval, POSTEKWidth, POSTEKX, POSTEKY, POSTEKBarRotation, POSTEKEncodeType, POSTEKBarNarrow, POSTEKBarWide, POSTEKBarcodeHeight, POSTEKPrintCode, POSTEKBarCodeInterval, txt_STBID.Text.Trim(), txt_CAID.Text.Trim(), txt_SmartCardID.Text.Trim(), POSTEKCodeInterval, POSTEKFontHeight, POSTEKFontWidth, POSTEKFontType, POSTEKFontAlign, POSTEKFontWeight, flag, POSTEKPrintLabelSetNum, POSTEKPrintLabelCopeNum);
                }
                else if (CurrentPrinter == dt.Rows[0]["ArogoxOutPutPort"].ToString().Trim())//立像打印机
                {
                    ArogoxVariableInit();
                    HDIC_Func.ArogoxPrinter(ArogoxOutPutPort, ArogoxnEnable, Arogoxhor, Arogoxver, Arogoxobject, Arogoxdarkness, ArogoxIsImmediate, Arogoxpbuf, ArogoxX, ArogoxY, Arogoxori, ArogoxEncodeType, ArogoxBarNarrow, ArogoxBarWide, ArogoxBarcodeHeight, ArogoxPrintCode, ArogoxBarCodeInterval, txt_STBID.Text.Trim(), txt_CAID.Text.Trim(), txt_SmartCardID.Text.Trim(), ArogoxCodeInterval, Arogoxfont, Arogoxhor_factor, Arogoxver_factor, Arogoxmode, flag, ArogoxPrintLabelCopeNum);
                }
            }
        
        }

        #endregion

        #region TSC中编码转换为BarcodeLib可显示的编码，用于在本窗体显示
        private void cbEncodeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbEncodeType.SelectedItem.ToString().Trim())
            {
                case "128":

                    BarcodeLibType = "Code 128";
                    break;
                case "128M":
                    BarcodeLibType = "Code 128-A";
                    break;
                case "EAN128":
                    BarcodeLibType = "Code 128-B";
                    break;
                case "25":
                    BarcodeLibType = "";//
                    break;
                case "25C":
                    BarcodeLibType = "";//
                    break;
                case "39":
                    BarcodeLibType = "Code 39";
                    break;
                case "39C":
                    BarcodeLibType = "Code 39 Extended";
                    break;
                case "93":
                    BarcodeLibType = "Code 93";
                    break;
                case "EAN13":
                    BarcodeLibType = "EAN-13";
                    break;
                case "EAN13+2":
                    BarcodeLibType = "JAN-13";
                    break;
                case "EAN13+5":
                    BarcodeLibType = ""; //
                    break;
                case "EAN8":
                    BarcodeLibType = "EAN-8";
                    break;
                case "EAN8+2":
                    BarcodeLibType = "";//
                    break;
                case "EAN8+5":
                    BarcodeLibType = "";//
                    break;
                case "CODA":
                    BarcodeLibType = "Codabar";
                    break;
                case "POST":
                    BarcodeLibType = "PostNet";
                    break;
                case "UPCA":
                    BarcodeLibType = "UPC-A";
                    break;
                case "UPCA+2":
                    BarcodeLibType = "UPC 2 Digit Ext.";
                    break;
                case "UPCA+5":
                    BarcodeLibType = "UPC 5 Digit Ext.";
                    break;
                case "UPCE":
                    BarcodeLibType = "UPC-E";
                    break;
                case "UPCE+2":
                    BarcodeLibType = "";//
                    break;
                case "UPCE+5":
                    BarcodeLibType = "";//
                    break;
                //default: MessageBox.Show("Please specify the encoding type."); break;

                //"ITF-14",
                //"Interleaved 2 of 5",
                //"Standard 2 of 5",
                //"",
                //"",
                //"Bookland/ISBN",
                //"Code 11",

                //"LOGMARS",
                //"MSI"
            }
        }
        #endregion

        #region 文本框获取值 以及 清空提示信息
        private void txt_STBID_MouseDown(object sender, MouseEventArgs e)
        {
            if (!HDIC_Func.CheckObjectIsInteger(txt_STBID.Text.Trim()) || txt_STBID.Text.Trim().Length != 16)
            {
                txt_STBID.Text = "";
            }
            if (txt_CAID.Text.Trim().Length != 11)
            {
                txt_CAID.Text = "CAID是11位数字";
            }
            if (txt_SmartCardID.Text.Trim().Length != 12)
            {
                txt_SmartCardID.Text = "智能卡号是12位数字";
            }
        }
        private void txt_CAID_MouseDown(object sender, MouseEventArgs e)
        {
            if (!HDIC_Func.CheckObjectIsInteger(txt_CAID.Text.Trim()) ||txt_CAID.Text.Trim().Length != 11)
            {
                txt_CAID.Text = "";
            }
            if (txt_STBID.Text.Trim().Length != 16)
            {
                txt_STBID.Text = "STBID是16位数字";
            }
            if (txt_SmartCardID.Text.Trim().Length != 12)
            {
                txt_SmartCardID.Text = "智能卡号是12位数字";
            }
        }

        private void txt_SmartCardID_MouseDown(object sender, MouseEventArgs e)
        {
            if (!HDIC_Func.CheckObjectIsInteger(txt_SmartCardID.Text.Trim()) || txt_SmartCardID.Text.Trim().Length != 12)
            {
                txt_SmartCardID.Text = "";
            }
            if (txt_STBID.Text.Trim().Length != 16)
            {
                txt_STBID.Text = "STBID是16位数字";
            }
            if (txt_CAID.Text.Trim().Length != 11)
            {
                txt_CAID.Text = "CAID是11位数字";
            }
        }
        private void txt_STBID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //if (txt_STBID.Text.Trim().Length == 16)
                if (txt_STBID.Text.Trim().Length > 0)
                {
                    DataTable dt = HDIC_DB.GetList("select STBID,CAID,SmartCardID from STBData where STBID='" + txt_STBID.Text.Trim() + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txt_CAID.Text = dt.Rows[0]["CAID"].ToString().Trim();
                        txt_SmartCardID.Text = dt.Rows[0]["SmartCardID"].ToString().Trim();
                    }
                    else
                    {
                        txt_CAID.Text = "CAID是11位数字";
                        txt_SmartCardID.Text = "智能卡号是12位数字";
                    }
                }
                else
                {
                    ////txt_CAID.Text = "CAID是11位数字";
                    ////txt_SmartCardID.Text = "智能卡号是12位数字";
                    //txt_CAID.Text = "";
                    //txt_SmartCardID.Text = "";
                }
            }
            catch 
            {
                HDIC_Message.ShowWarnDialog(this, "数据库打开失败,请检查服务器或者网络");
            }
           
        }

        private void txt_CAID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //if (txt_CAID.Text.Trim().Length == 11)
                if (txt_CAID.Text.Trim().Length > 0)
                {
                    DataTable dt = HDIC_DB.GetList("select STBID,CAID,SmartCardID from STBData where CAID='" + txt_CAID.Text.Trim() + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txt_STBID.Text = dt.Rows[0]["STBID"].ToString().Trim();
                        txt_SmartCardID.Text = dt.Rows[0]["SmartCardID"].ToString().Trim();
                    }
                    else
                    {
                        txt_STBID.Text = "STBID是16位数字";
                        txt_SmartCardID.Text = "智能卡号是12位数字";
                    }
                }
                else
                {
                    ////txt_STBID.Text = "STBID是16位数字";
                    ////txt_SmartCardID.Text = "智能卡号是12位数字";
                    //txt_STBID.Text = "";
                    //txt_SmartCardID.Text = "";
                }
            }
            catch 
            {
                HDIC_Message.ShowWarnDialog(this, "数据库打开失败,请检查服务器或者网络");
            }
          
        }

        private void txt_SmartCardID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //if (txt_SmartCardID.Text.Trim().Length == 12)
                if (txt_SmartCardID.Text.Trim().Length >0)
                {
                    DataTable dt = HDIC_DB.GetList("select STBID,CAID,SmartCardID from STBData where SmartCardID='" + txt_SmartCardID.Text.Trim() + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txt_STBID.Text = dt.Rows[0]["STBID"].ToString().Trim();
                        txt_CAID.Text = dt.Rows[0]["CAID"].ToString().Trim();
                    }
                    else
                    {
                        txt_STBID.Text = "STBID是16位数字";
                        txt_CAID.Text = "CAID是11位数字";
                    }
                }
                else
                {
                    ////txt_STBID.Text = "STBID是16位数字";
                    ////txt_CAID.Text = "CAID是11位数字";
                    //txt_STBID.Text = "";
                    //txt_CAID.Text = "";
                }
            }
            catch
            {
                HDIC_Message.ShowWarnDialog(this, "数据库打开失败,请检查服务器或者网络");
            }
            
        }
        #endregion

        #region 判断文本框内容是否是数字
        private bool CheckDigit()
        {
            if(txt_STBID.Text.Trim()=="")
            {
                  HDIC_Message.ShowWarnDialog(this, "STB ID 不能为空");
                 return false;
            }
            if (!HDIC_Func.CheckObjectIsInteger(txt_STBID.Text.Trim()))
            {
                HDIC_Message.ShowWarnDialog(this, "您输入的<STB ID>不是数字，请重新输入");
                return false;
            }
            if (txt_STBID.Text.Trim().Length != 16)
            {
                HDIC_Message.ShowWarnDialog(this, "您输入的<STB ID>长度不是16位，请重新输入");
                return false;
            }
            if(txt_CAID.Text.Trim()=="")
            {
                HDIC_Message.ShowWarnDialog(this, "CA ID 不能为空");
                return false;
            }
            if (!HDIC_Func.CheckObjectIsInteger(txt_CAID.Text.Trim()))
            {
                HDIC_Message.ShowWarnDialog(this, "您输入的<CA ID>不是数字，请重新输入");
                return false;
            }
            if (txt_CAID.Text.Trim().Length != 11)
            {
                HDIC_Message.ShowWarnDialog(this, "您输入的<CA ID>长度不是11位，请重新输入");
                return false;
            }
            if (txt_SmartCardID.Text.Trim() != "")
            {
                if (!HDIC_Func.CheckObjectIsInteger(txt_SmartCardID.Text.Trim()))
                {
                    HDIC_Message.ShowWarnDialog(this, "您输入的<SmartCardID ID>不是数字，请重新输入");
                    return false;
                }
                if (txt_SmartCardID.Text.Trim().Length != 12)
                {
                    HDIC_Message.ShowWarnDialog(this, "您输入的<SmartCardID ID>长度不是12位，请重新输入");
                    return false;
                }
            }
            return true;
        }
        #endregion

        private void btn_Detail_Click(object sender, EventArgs e)
        {
            if (panel_Detail.Visible == false)
            {
                panel_Detail.Visible = true;
            }
            else
            {
                panel_Detail.Visible = false;
            }
        }


    }
}
