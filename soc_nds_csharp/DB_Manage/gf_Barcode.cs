using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HDICSoft.Message;
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
            Bitmap temp = new Bitmap(1, 1);
            temp.SetPixel(0, 0, this.BackColor);
            pic_barcode.Image = (Image)temp;
            cbEncodeType.SelectedIndex = 0;
        }

        #region btn_Encode
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
                    b.IncludeLabel = this.chkGenerateLabel.Checked;

                    //===== Encoding performed here =====
                    pic_barcode.Image = b.Encode(type, this.txtData.Text.Trim(), Color.Black, Color.White, W, H);
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

        #region btn_save
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

        #region print
        private void Print_Click(object sender, EventArgs e)
        {
            if (cbEncodeType.SelectedIndex == -1)
            {
                HDIC_Message.ShowWarnDialog(this, "请选择编码类型");
                return;
            }

            //printDocument1.Print();
            //TSCLIB_DLL.about();                                                                 //Show the DLL version

            #region 1
            //TSCLIB_DLL.openport("TSC TTP-344M Plus");                                           //Open specified printer driver
            //TSCLIB_DLL.setup("100", "63.5", "4", "8", "0", "0", "0");                           //Setup the media size and sensor type info
            //TSCLIB_DLL.clearbuffer();                                                           //Clear image buffer

            //TSCLIB_DLL.barcode("100", "100", "128", "100", "1", "0", "2", "2", this.txtData.Text.Trim()); //Drawing barcode
            //TSCLIB_DLL.printerfont("100", "250", "3", "0", "1", "1", "Print Font Test");        //Drawing printer font
            //TSCLIB_DLL.windowsfont(100, 300, 24, 0, 0, 0, "ARIAL", "Windows Arial Font Test");  //Draw windows font

            //TSCLIB_DLL.downloadpcx("UL.PCX", "UL.PCX");                                         //Download PCX file into printer
            //TSCLIB_DLL.sendcommand("PUTPCX 100,400,\"UL.PCX\"");                                //Drawing PCX graphic

            //TSCLIB_DLL.printlabel("1", "1");                                                    //Print labels
            //TSCLIB_DLL.closeport();                                                             //Close specified printer driver 
            #endregion

            #region 2
            TSCLIB_DLL.openport("TSC TTP-344M Plus");
            TSCLIB_DLL.setup("83", "58", "4", "8", "0", "0", "0");                           //Setup the media size and sensor type info
            TSCLIB_DLL.clearbuffer();

            TSCLIB_DLL.barcode("200", "100", cbEncodeType.SelectedItem.ToString().Trim(), "100", Convert.ToString((int)chkGenerateLabel.CheckState), "0", "4", "2", this.txtData.Text.Trim());
            //TSCLIB_DLL.barcode("200", "250", "128", "100", "1", "0", "4", "2", this.txtData.Text.Trim());
            TSCLIB_DLL.printerfont("200", "250", "3", "0", "1", "1", "ShangHai HDIC");        //Drawing printer font
            TSCLIB_DLL.windowsfont(200, 300, 24, 0, 2, 0, "ARIAL", "长城测试");  //Draw windows font

            //TSCLIB_DLL.windowsfont(250, 350, 160, 0, 2, 0, "Times new Roman", "邱晓淯");  //Draw windows font
            //TSCLIB_DLL.downloadpcx("UL.PCX", "UL.PCX");                                         //Download PCX file into printer
            //TSCLIB_DLL.sendcommand("PUTPCX 100,400,\"UL.PCX\"");                                //Drawing PCX graphic
            TSCLIB_DLL.printlabel("1", "1");                                                    //Print labels
            TSCLIB_DLL.closeport();
            #endregion

        }


        #endregion

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

    }
}
