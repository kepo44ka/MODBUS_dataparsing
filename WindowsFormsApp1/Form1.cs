using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)                   //read feed.txt file
        {
            string[] feed = new string[101];
            if (File.Exists("feed.txt"))
            {
                feed = File.ReadAllLines("feed.txt");
                this.textBox1.Lines = feed;
            }
        }

        private void Parse_Click(object sender, EventArgs e)                     //parse feed
        {
            char separator = ':';
            string[] feed = this.textBox1.Lines;
            string[] parcedData = new string[150];
            int[] dataStringNumber = new int[101];
            string[] format = {	    //array to help with recognition of the data types
				"date", 		            //have 101 strings, to present each string in file feed.txt
				"float",	                //reg1
				"float2p",	            //float 2nd part
				"float",
                "float2p",
                "float",
                "float2p",
                "float",
                "float2p",
                "long",
                "long2p",
                "float",	                //reg11
				"float2p",
                "long",
                "long2p",
                "float",
                "float2p",
                "long",
                "long2p",
                "float",
                "float2p",
                "long",	                //reg21
				"long2p",
                "float",
                "float2p",
                "long",
                "long2p",
                "float",
                "float2p",
                "long",
                "long2p",
                "float",	                //reg31
				"float2p",
                "float",
                "float2p",
                "float",
                "float2p",
                "float",
                "float2p",
                "float",
                "float2p",
                "float",	                //reg41
				"float2p",
                "float",
                "float2p",
                "float",
                "float2p",
                "float",
                "float2p",
                "bcd1p",
                "bcd2p",
                "bcd",	                //reg51
				"none",
                "bcd1pof3",	        //BCD 1st part of 3
				"bcd2pof3",
                "bcd3pof3",
                "bcd",	                //reg56
				"none",
                "none",
                "int",	                    //reg59
				"int",
                "int",	                    //reg61
				"int",
                "int",
                "none",
                "none",
                "none",
                "none",
                "none",
                "none",
                "none",
                "none",	                //reg71
				"bit",
                "none",
                "none",
                "none",
                "none",
                "float",
                "float2p",
                "float",
                "float2p",
                "float",	                //reg81
				"float2p",
                "float",
                "float2p",
                "float",
                "float2p",
                "float",
                "float2p",
                "float",
                "float2p",
                "none",	                //reg91
				"int_reg92",
                "int_rng2048",
                "int_rng2048",
                "none",
                "int",
                "float",
                "float2p",
                "float",
                "float2p"
             };

            
            textBox2.AppendText("Date:" + feed[0] + "\r\n");
            for (int i = 1; i < feed.Length; ++i)
            {
                
                int indexOfSeparator = feed[i].IndexOf(separator);
                dataStringNumber[i] = Convert.ToInt32(feed[i].Substring(0, indexOfSeparator));      //array with regist number
                feed[i] = feed[i].Substring(indexOfSeparator + 1);                                                  //array with unparsed data

                switch (format[i])                                                                                                 //writing on textbox2 depending on format 
                {
                    case "float2p":
                        parcedData[i] = Convert.ToString(ParsingForFloat(feed[i], feed[i - 1]));            
                        textBox2.AppendText(String.Format("Reg {0:000}-{1:000} |   Float\t| {2}\r\n", dataStringNumber[i-1], dataStringNumber[i], parcedData[i]));
                        break;
                    case "long2p":
                        parcedData[i] = ParsingForLong(feed[i], feed[i - 1]);
                        textBox2.AppendText(String.Format("Reg {0:000}-{1:000} |   Long\t| {2}\r\n", dataStringNumber[i-1], dataStringNumber[i], parcedData[i]));
                        break;
                    case "int":
                        parcedData[i] = feed[i];
                        textBox2.AppendText(String.Format("Reg {0:000}        |   Integer\t| {1}\r\n", dataStringNumber[i], parcedData[i]));
                        break;
                    case "int_reg92":
                        parcedData[i] = ParsingForInt_reg92(feed[i]);
                        textBox2.AppendText(String.Format("Reg {0:000}        | Integer 2 value\t| {1}\r\n", dataStringNumber[i], parcedData[i]));
                        break;
                    case "int_rng2048":
                        parcedData[i] = ParsingForInt_rng2048(feed[i]);
                        textBox2.AppendText(String.Format("Reg {0:000}        | Integer ranged\t| {1}\r\n", dataStringNumber[i], parcedData[i]));
                        break;
                    case "bit":
                        parcedData[i] = ParsingForBit(feed[i]);
                        textBox2.AppendText(String.Format("Reg {0:000}        |   Bit\t\t| {1}\r\n", dataStringNumber[i], parcedData[i]));
                        break;
                    case "bcd":
                        parcedData[i] = ParsingForBCD(feed[i]);
                        textBox2.AppendText(String.Format("Reg {0:000}        |   BCD\t| {1}\r\n", dataStringNumber[i], parcedData[i]));
                        break;
                    case "bcd2p":
                        parcedData[i] = ParsingForBCD2reg(feed[i], feed[i - 1]);
                        textBox2.AppendText(String.Format("Reg {0:000}-{1:000} |   BCD 2parts\t| {2}\r\n", dataStringNumber[i-1], dataStringNumber[i], parcedData[i]));
                        break;
                    case "bcd3pof3":
                        parcedData[i] = ParsingForBCD3reg(feed[i], feed[i - 1], feed[i - 2]);
                        textBox2.AppendText(String.Format("Reg {0:000}-{1:000} |   BCD 3parts\t| {2}\r\n", dataStringNumber[i-2], dataStringNumber[i], parcedData[i].Insert(6, " ").Insert(4, ":").Insert(2, ":")));
                        break;
                    default:
                        parcedData[i] = "none";
                        break;
                }
            }
        }

        static int REG_LENGTH = 16;
        static int RANGE_0to2048_CONST = 32768;
        public static float BinStringToSingle(string s)
        {
            int i = Convert.ToInt32(s, 2);
            byte[] b = BitConverter.GetBytes(i);
            return BitConverter.ToSingle(b, 0);
        }
        public static string BinStringToIntToString(string s)
        {

            return Convert.ToString(Convert.ToInt16(s, 2), 10);
        }

        public static float ParsingForFloat(string a, string b)
        {
            a = Convert.ToString(Convert.ToInt32(a), 2);
            b = Convert.ToString(Convert.ToInt32(b), 2);
            while (a.Length < REG_LENGTH)
            {
                a = '0' + a;
            }
            while (b.Length < REG_LENGTH)
            {
                b = '0' + b;
            }
            return BinStringToSingle(a + b);
        }
        public static string ParsingForLong(string a, string b)
        {
            a = Convert.ToString(Convert.ToInt32(a), 2);
            b = Convert.ToString(Convert.ToInt32(b), 2);
            while (a.Length < REG_LENGTH)
            {
                a = '0' + a;
            }
            while (b.Length < REG_LENGTH)
            {
                b = '0' + b;
            }
            return Convert.ToString(Convert.ToInt32(a + b, 2), 10);
        }
        public static string ParsingForInt_reg92(string a)
        {
            a = ParsingForBit(a);
            a = BinStringToIntToString(a.Substring(0, 8)) + ' ' + BinStringToIntToString(a.Substring(8, 8));
            return a;
        }
        public static string ParsingForInt_rng2048(string a)
        {

            int i = (int)Math.Floor(Convert.ToDecimal((Convert.ToInt32(a) + RANGE_0to2048_CONST) / 32));
            a = Convert.ToString(i);
            return a;
        }
        public static string ParsingForBit(string a)
        {
            a = Convert.ToString(Convert.ToInt16(a), 2);
            while (a.Length < REG_LENGTH)
            {
                a = '0' + a;
            }
            return a;
        }
        public static string ParsingForBCD(string a)
        {
            string[] BCD = new string[4];
            a = ParsingForBit(a);
            for (int i = 0; i < 4; i++)
            {
                BCD[i] = BinStringToIntToString(a.Substring(i * 4, 4));
            }
            return String.Join("", BCD);
        }
        public static string ParsingForBCD2reg(string a, string b)
        {
            string[] BCD = new string[8];
            a = ParsingForBit(a);
            b = ParsingForBit(b);
            for (int i = 0; i < 4; i++)
            {
                BCD[i] = BinStringToIntToString(a.Substring(i * 4, 4));
                BCD[i + 4] = BinStringToIntToString(b.Substring(i * 4, 4));
            }
            return String.Join("", BCD);
        }
        public static string ParsingForBCD3reg(string a, string b, string c)
        {

            string[] BCD = new string[12];
            a = ParsingForBit(a);
            b = ParsingForBit(b);
            c = ParsingForBit(c);
            for (int i = 0; i < 4; i++)
            {
                BCD[i] = BinStringToIntToString(a.Substring(i * 4, 4));
                BCD[i + 4] = BinStringToIntToString(b.Substring(i * 4, 4));
                BCD[i + 8] = BinStringToIntToString(c.Substring(i * 4, 4));
            }
            return String.Join("", BCD); ;
        }

        private void Save_Text_Click(object sender, EventArgs e)
        {
            
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName);
                streamWriter.WriteLine(textBox2.Text);
                streamWriter.Close();
            }

        }
    }

}
