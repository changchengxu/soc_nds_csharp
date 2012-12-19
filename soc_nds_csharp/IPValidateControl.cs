using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace soc_nds_csharp
{
    public partial class IPValidateControl : UserControl
    {
        private const int DEFAULT_WIDTH_SIZE = 136;
        private const int DEFAULT_HEIGHT_SIZE = 20;
        private string _text = "";
        private bool _isAllowWarn = true;
        private Languages _language = Languages.English;//默认英语

        public IPValidateControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 语言设置
        /// </summary>
        public enum Languages
        {
            /// <summary>
            /// 英文
            /// </summary>
            English,
            /// <summary>
            /// 中文
            /// </summary>
            Chinese
        }
        /// <summary>
        /// 设置IP
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="e"></param>
        private void MaskIPAddress(TextBox textBox, KeyPressEventArgs e)
        {
            
            if (Char.IsDigit(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == 8)
            {
                if (e.KeyChar != 8)
                {
                    if (textBox.Text.Length == 2 && e.KeyChar != '.')
                    {
                        Validate(textBox, textBox.Text + e.KeyChar);
                        e.Handled = true;
                    }
                    else if (e.KeyChar == '.')
                    {
                        if (textBox.Text.Length == 0)
                        {
                            textBox.Text = "";
                        }
                        else
                        {
                            FocusNext(textBox);
                        }
                        e.Handled = true;
                    }
                }
                else if (textBox.Text.Length == 0)
                {
                    FocusPreviouse(textBox);
                    e.Handled = false;
                }
            }
            else
            {
                e.Handled = true; // 不考虑其它的键输入
            }
        }

        /// <summary>
        /// 设置上一个textBox的焦点
        /// </summary>
        /// <param name="textBox"></param>
        private void FocusPreviouse(TextBox textBox)
        {
            switch (textBox.Name)
            {
                case "textBox2":
                    textBox1.Focus();
                    textBox1.SelectionStart = textBox1.Text.Length;
                    break;
                case "textBox3":
                    textBox2.Focus();
                    textBox2.SelectionStart = textBox2.Text.Length;
                    break;
                case "textBox4":
                    textBox3.Focus();
                    textBox3.SelectionStart = textBox3.Text.Length;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 设置：焦点移动下一个textBox，并处于选中状态
        /// </summary>
        /// <param name="textBox"></param>
        private void FocusNext(TextBox textBox)
        {
            switch (textBox.Name)
            {
                case "textBox1":
                    textBox2.Focus();
                    textBox2.SelectAll();
                    break;
                case "textBox2":
                    textBox3.Focus();
                    textBox3.SelectAll();
                    break;
                case "textBox3":
                    textBox4.Focus();
                    textBox4.SelectAll();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 当在当前的textbox中输入数据后开始验证，然后下一个textbox获取鼠标焦点
        /// </summary>
        /// <param name="currentTextBox">当前的textbox</param>
        /// <param name="str">在textbox中数据的字符</param>
        private void Validate(TextBox currentTextBox, string str)
        {
            //如果在第一个textbox1中输入的数据超过233，那么为其设置默认字符为233，
            //其它textbox中一旦超过255，那么设置默认值为255
            int maxValue = currentTextBox.Name.Equals("textBox1") ? 233 : 255;

            TextBox nextTextBox = currentTextBox;
            switch (currentTextBox.Name)
            {
                case "textBox1":
                    nextTextBox = textBox2;
                    break;
                case "textBox2":
                    nextTextBox = textBox3;
                    break;
                case "textBox3":
                    nextTextBox = textBox4;
                    break;
                case "textBox4":
                    nextTextBox = textBox4;
                    break;
                default:
                    break;
            }

            if (Int32.Parse(str) > maxValue)
            {
                currentTextBox.Text = maxValue.ToString();
                currentTextBox.Focus();
                if (AllowWarn)
                {
                    switch (Language)
                    {
                        case Languages.English:
                            MessageBox.Show(str + " is not a valid entry. Please specify a value between 1 and " +
                                maxValue, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            break;
                        case Languages.Chinese:
                            MessageBox.Show(str + " 不是一个有效值。请指定一个介于 1 和 " + maxValue +
                                " 之间的数值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            break;
                    }
                }
            }
            else
            {
                currentTextBox.Text = str;
                if (!nextTextBox.Equals(currentTextBox))
                {
                    nextTextBox.Focus();
                    nextTextBox.SelectAll();
                }
                else
                {
                    currentTextBox.SelectionStart = currentTextBox.Text.Length;
                }
            }
        }

        /// <summary>
        /// 继承Text方法，同时返回值
        /// </summary>
        public override string Text
        {
            get
            {
                if (textBox1.Text.Length == 0 || textBox2.Text.Length == 0 || textBox3.Text.Length == 0 || textBox4.Text.Length == 0)
                {
                    _text = "";
                }
                else
                {
                    _text = textBox1.Text + "." + textBox2.Text +
                        "." + textBox3.Text + "." + textBox4.Text;
                }
                return _text;
            }
        }

        /// <summary>
        /// 返回：当输入有误时，返回true;
        /// </summary>
        public bool AllowWarn
        {
            get { return _isAllowWarn; }
            set { _isAllowWarn = value; }
        }

        /// <summary>
        /// 返回警告信息的语言版本
        /// </summary>
        public Languages Language
        {
            get { return _language; }
            set { _language = value; }
        }

        /// <summary>
        /// 继承Focused
        /// </summary>
        public override bool Focused
        {
            get
            {
                if (textBox1.Focused || textBox2.Focused || textBox3.Focused || textBox4.Focused)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 默认焦点在textBox1
        /// </summary>
        /// <returns></returns>
        public new bool Focus()
        {
            return textBox1.Focus();
        }

        /// <summary>
        ///清空所有textBox的文本
        /// </summary>
        public void Clear()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox1.Focus();
        }

        public void SetText(string ip)
        {
            if (ip != "")
            {
                string[] MIp = ip.Split('.');
                textBox1.Text = MIp[0];
                textBox2.Text = MIp[1];
                textBox3.Text = MIp[2];
                textBox4.Text = MIp[3];
            }
        }

        /// <summary>
        /// 禁止当窗体改变时控件也会开发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IPValidateControl_SizeChanged(object sender, System.EventArgs e)
        {
            this.Width = DEFAULT_WIDTH_SIZE;
            this.Height = DEFAULT_HEIGHT_SIZE;
        }

        private void textBox1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            MaskIPAddress(textBox1, e);
        }

        private void textBox2_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            MaskIPAddress(textBox2, e);
        }

        private void textBox3_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            MaskIPAddress(textBox3, e);
        }

        private void textBox4_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            MaskIPAddress(textBox4, e);
        }
    }
}