using header;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
#region ModbusRTU
using Modbus.Device;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;
using Microsoft.SqlServer.Server;
using System.Web;
using System.Windows.Forms.DataVisualization.Charting;
using System.CodeDom;
#endregion


namespace Thermal_imaging_camera
{
    public partial class Form1 : Form
    {
        public struct FixedValue
        {
            public const int INITIALIZEPORT = 1;
            public const int BAUDRATE = 9600;
            public const int MAX_RECV_DATA = 2;
            public const int INITIALIZENUM = 0;
            public const int CHART_BORDER = 3;
            public const int PICUTURE_WIDTH = 32;
            public const int PICUTURE_HEIGHT = 28;
        }


        #region 전역 선언
        Mutex mutex = new Mutex();
        Header DeviceSetting = new Header();
        LOG LogSetting = new LOG();
        bool blsFlag1 = false;
        public bool blsTemp;
        public int nTemp;
        string[] strTempData = new string[2];
        string strRecvData= "";
        Series Chart_1st;
        Series Chart_2nd;
        private const int HEIGHT = 4;
        private const int WIDTH = 32;
        #endregion
        public Form1()
        {
            InitializeComponent();
            DeviceSetting.blsbWorking = false;




            
            //차트 표현...
            ChRecv.Series.Clear();

            Chart_1st = ChRecv.Series.Add("첫 번째 온도");
            Chart_2nd = ChRecv.Series.Add("두 번째 온도");

            Chart_1st.LegendText = "첫 번째 온도";
            Chart_2nd.LegendText = "두 번째 온도";

            Chart_1st.ChartType = SeriesChartType.FastLine;
            Chart_2nd.ChartType = SeriesChartType.FastLine;

            Chart_1st.Color = Color.Lime;
            Chart_2nd.Color = Color.Red;

            Chart_1st.BorderWidth = FixedValue.CHART_BORDER;
            Chart_2nd.BorderWidth = FixedValue.CHART_BORDER;

            DeviceSetting.nTemp[2] = FixedValue.INITIALIZENUM;

        }
        

        #region 초기화 클릭시

        private void button1_Click(object sender, EventArgs e)
        {
            blsFlag1 = false;
            DeviceSetting.PortNum = FixedValue.INITIALIZEPORT;
            DeviceSetting.strPortName = "COM"+ DeviceSetting.PortNum.ToString();
           
            LogSetting.CreateLogFile("초기화 완료");
            MessageBox.Show("초기화 완료");
            button2.Enabled = !blsFlag1;
            GUI_Timer.Enabled = false;
        }
        #endregion


        #region Clicked Auto Connect
        private void button2_Click(object sender, EventArgs e)
        {
           

            blsFlag1 = !blsFlag1;
            //DeviceSetting.Thread_Start();
            button2.Enabled = !blsFlag1;
            LogSetting.CreateLogFile("자료수신 쓰레드 생성 완료");

            Thread RecvEngine = new Thread(bWorking);
            RecvEngine.Start();
            Thread ImageProcessThread = new Thread(ImageProcess);
            ImageProcessThread.Start();
            GUI_Timer.Enabled = true;
        }

        #endregion
        /// <summary>
        /// 텍스트 박스에 이미지 뿌리기 위한 멀티 쓰레드 준비하기 
        /// </summary>
        #region 데이터 수신 쓰레드

        SerialPort port = new SerialPort();
        private void bWorking()
        {
            DeviceSetting.PortNum = DeviceSetting.FindCom(DeviceSetting.strPortName, DeviceSetting.PortNum);
            DeviceSetting.strPortName = "COM" + DeviceSetting.PortNum.ToString();
            port.PortName = DeviceSetting.strPortName;
            port.BaudRate = FixedValue.BAUDRATE;
            Thread.Sleep(100);
            LogSetting.CreateLogFile("데이터 수신 준비 완료");
            
            

            //Temp 온도 초기화 작업
            for (int i = 0; i < strTempData.Length; i++)
            {
                strTempData[i] = "";
            }
            port.Open();
            while (blsFlag1)
            {

                
                int nReceiveData = port.ReadByte();

                mutex.WaitOne();

                strRecvData = port.ReadExisting();

              


                //LogSetting.ReceiveData(strRecvData);

                try
                {
                    RecvTemperature_Data(strRecvData);
                    DeviceSetting.nTemp[0] = Int32.Parse(strTempData[0], System.Globalization.NumberStyles.HexNumber);
                    DeviceSetting.nTemp[1] = Int32.Parse(strTempData[1], System.Globalization.NumberStyles.HexNumber);

                    float fTmp1 = DeviceSetting.nTemp[0] / 10;
                    float fTmp2 = DeviceSetting.nTemp[1] / 10;
                    DeviceSetting.nTemp[0] = (int)fTmp1;
                    DeviceSetting.nTemp[1] = (int)fTmp2;


                    //ChRecv.Series["첫번째 온도"].Points.Add(nTmp1);
                    //ChRecv.Series["두번째 온도"].Points.Add(nTmp2);
                    
                }

                catch (Exception)
                {
                    LogSetting.CreateLogFile("수신데이터 차트표기 오류");
                }
                mutex.ReleaseMutex();
                
            }





            Thread.Sleep(10);
            port.Close();

            //port.Close();
            return;
        }
        #endregion

        #region ImageProcessThread
        string[,] strTemp = new string[FixedValue.PICUTURE_WIDTH, FixedValue.PICUTURE_HEIGHT];
        int[,] nTempData = new int[FixedValue.PICUTURE_WIDTH, FixedValue.PICUTURE_HEIGHT];
        float[,] fTempData = new float[FixedValue.PICUTURE_WIDTH, FixedValue.PICUTURE_HEIGHT];
        string[,] strTemp2 = new string[FixedValue.PICUTURE_WIDTH, FixedValue.PICUTURE_HEIGHT];
        string strTempp;
        private void ImageProcess() 
        {

            
            string strTemp_RecvData;
            // 각 픽셀 초기화
            for (int i = 0; i < FixedValue.PICUTURE_HEIGHT; i++) 
            {
                for (int j = 0; j < FixedValue.PICUTURE_WIDTH; j++)
                {
                    strTemp[j,i] = "";
                }
            }
            strTempp= "";
            Thread ImageProcess2 = new Thread(ImageProcess22);
            ImageProcess2.Start();
            while (blsFlag1)
            {
                mutex.WaitOne();
                strTemp_RecvData = strRecvData;
                mutex.ReleaseMutex();
                
                ScaterdTemperatureData(strTemp_RecvData);
               
               

            }


        }
        private void ImageProcess22()
        {
            
            while (blsFlag1)
            {
                try
                {
                    for (int i = 0; i < FixedValue.PICUTURE_HEIGHT; i++)
                    {
                        for (int j = 0; j < FixedValue.PICUTURE_WIDTH; j++)
                        {
                            nTempData[j, i] = Int32.Parse(strTemp[j, i], System.Globalization.NumberStyles.HexNumber);
                            fTempData[j, i] = nTempData[j, i] / 10;
                            nTempData[j, i] = (int)fTempData[j, i];
                            strTemp2[j, i] = nTempData[j, i].ToString();
                            strTempp += strTemp2[j, i] + " /";
                            
                        }
                        strTempp += "\r\n";

                    }
                    strTempp = "";

                }
                catch (Exception)
                {


                }
            }

        }
        public void ScaterdTemperatureData(string strData)
        {

            try
            {
                string[] strTempData = strData.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < strData.Length; i++)
                {
                    string strTempStart = strTempData[i].Substring(5, 1);

                    if (strTempStart == "1")
                    {
                        for (int j = HEIGHT * 0; j < HEIGHT * 1; j++)
                        {
                            for (int k = 0; k < WIDTH; k++)
                            {
                                strTemp[k, j] = strTempData[i].Substring(6 + 4 * k, 4);
                            }
                        }
                    }
                    if (strTempStart == "2")
                    {
                        for (int j = HEIGHT * 1; j < HEIGHT * 2; j++)
                        {
                            for (int k = 0; k < WIDTH; k++)
                            {
                                strTemp[k, j] = strTempData[i].Substring(6 + 4 * k, 4);
                            }
                        }
                    }
                    if (strTempStart == "3")
                    {
                        for (int j = HEIGHT * 2; j < HEIGHT * 3; j++)
                        {
                            for (int k = 0; k < WIDTH; k++)
                            {
                                strTemp[k, j] = strTempData[i].Substring(6 + 4 * k, 4);
                            }
                        }
                    }
                    if (strTempStart == "4")
                    {
                        for (int j = HEIGHT * 3; j < HEIGHT * 4; j++)
                        {
                            for (int k = 0; k < WIDTH; k++)
                            {
                                strTemp[k, j] = strTempData[i].Substring(6 + 4 * k, 4);
                            }
                        }
                    }
                    if (strTempStart == "5")
                    {
                        for (int j = HEIGHT * 4; j < HEIGHT * 5; j++)
                        {
                            for (int k = 0; k < WIDTH; k++)
                            {
                                strTemp[k, j] = strTempData[i].Substring(6 + 4 * k, 4);
                            }
                        }
                    }
                    if (strTempStart == "6")
                    {
                        for (int j = HEIGHT * 5; j < HEIGHT * 6; j++)
                        {
                            for (int k = 0; k < WIDTH; k++)
                            {
                                strTemp[k, j] = strTempData[i].Substring(6 + 4 * k, 4);
                            }
                        }
                    }
                    if (strTempStart == "7")
                    {
                        for (int j = HEIGHT * 6; j < HEIGHT * 7; j++)
                        {
                            for (int k = 0; k < WIDTH; k++)
                            {
                                strTemp[k, j] = strTempData[i].Substring(6 + 4 * k, 4);
                            }
                        }
                    }

                }
            }
            catch (Exception)
            {

                
            }
            



        }



        #endregion





        private void button3_Click(object sender, EventArgs e)
        {
            Monitor.Enter(this);
            try
            {
                
                port.Write(tbDebug_Send.Text);
                LogSetting.CreateLogFile("데이터 송신중..");
            }
            catch (Exception)
            {
                MessageBox.Show("Auto Connect를 먼저 클릭하세요.");
                
            }
            
            Monitor.Exit(this);
        }

        ///str->hex
        public string str2hex(string strData)
        {
            string resultHex = string.Empty;
            byte[] arr_byteStr = Encoding.Default.GetBytes(strData);

            foreach (byte byteStr in arr_byteStr)
                resultHex += string.Format("{0:X2}", byteStr);

            return resultHex;
        }

        /// <summary>
        /// hex->ascii
        /// </summary>
        /// <param name="hexString"></param>
        public string ConvertHex(String hexString)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hexString.Length; i += 2)
            {
                String hs = hexString.Substring(i, i + 2);
                System.Convert.ToChar(System.Convert.ToUInt32(hexString.Substring(0, 2), 16)).ToString();
            }
            String ascii = sb.ToString();
            return ascii;
           // MessageBox.Show(ascii);
        }

        public void RecvTemperature_Data(string strData)
        {
            try
            {
                //문자열 추출
                string[] strTemp = strData.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                string strTemp2, strTemp3, strTemp4;
                strTemp4 = strTemp[strTemp.Length - 1];
                strTemp2 = strTemp4.Substring(0, 1);        //"$"
                strTemp3 = strTemp4.Substring(1, 1);        //"2"

                if (strTemp2 == "$")
                {
                    if (strTemp3 == "2")
                    {
                        strTempData[0] = strTemp4.Substring(6, 4);     //첫번째 온도
                        strTempData[1] = strTemp4.Substring(10, 4);   //2번째 온도

                        LogSetting.ReceiveData("첫번째 온도:"+strTempData[0]);
                        LogSetting.ReceiveData("두번째 온도:" + strTempData[1]);
                    }


                }
            }
            catch (Exception)
            {

                LogSetting.CreateLogFile("수신데이터 차트표기 오류");
            }
            
           
        }

        private void GUI_Timer_Tick(object sender, EventArgs e)
        {

            Chart_1st.Points.AddXY(DeviceSetting.nTemp[2], DeviceSetting.nTemp[0]);
            Chart_2nd.Points.AddXY(DeviceSetting.nTemp[2], DeviceSetting.nTemp[1]);
            DeviceSetting.nTemp[2]++;


            tbDebug.Text = strRecvData;
            strRecvData = tbDebug.Text;

            label1.Text = "첫 번째 온도 : "+DeviceSetting.nTemp[0].ToString();
            label2.Text = "두 번째 온도 : " + DeviceSetting.nTemp[1].ToString();
            LogSetting.CreateLogFile("차트 표기 성공");
            if (!blsFlag1)
            {
                tbDebug.Text = "";
                LogSetting.CreateLogFile("초기화 진행중...");
            }
           tbDisplay.Text = strTempp;
        }

        
    }
}
