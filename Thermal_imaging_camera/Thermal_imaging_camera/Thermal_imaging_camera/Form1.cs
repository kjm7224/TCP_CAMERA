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
#region ModbusRTU
using Modbus.Device;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;
using Microsoft.SqlServer.Server;
#endregion


namespace Thermal_imaging_camera
{
    public partial class Form1 : Form
    {
        
        Header DeviceSetting = new Header();
        LOG LogSetting = new LOG();

        public bool blsTemp;
        public int nTemp;
        public const int BAUDRATE = 9600;
        private const int INITIALIZEPORT = 1;
        public Form1()
        {
            InitializeComponent();
            DeviceSetting.blsbWorking = false;
         }
        

        #region 시작버튼 클릭시

        private void button1_Click(object sender, EventArgs e)
        {
            blsFlag1 = false;
            DeviceSetting.PortNum = INITIALIZEPORT;
            DeviceSetting.strPortName = "COM"+ DeviceSetting.PortNum.ToString();
            while (tbDebug.Text!="")
            {
                Monitor.Enter(tbDebug);
                tbDebug.Text = "";
                LogSetting.CreateLogFile("초기화 진행중...");
                Monitor.Exit(tbDebug);
            }
            LogSetting.CreateLogFile("초기화 완료");
            MessageBox.Show("초기화 완료");
            
            
            button2.Enabled = !blsFlag1;
        }
        #endregion

        bool blsFlag1=false;
        private void button2_Click(object sender, EventArgs e)
        {
            blsFlag1 = !blsFlag1;
            //DeviceSetting.Thread_Start();
            button2.Enabled = !blsFlag1;
            LogSetting.CreateLogFile("자료수신 쓰레드 생성 완료");
            Thread DisplayThread = new Thread(bWorking);
            DisplayThread.Start();
        }


        /// <summary>
        /// 텍스트 박스에 이미지 뿌리기 위한 멀티 쓰레드 준비하기 
        /// </summary>
        SerialPort port = new SerialPort();
        private void bWorking() 
        {
            DeviceSetting.PortNum = DeviceSetting.FindCom(DeviceSetting.strPortName, DeviceSetting.PortNum);
            DeviceSetting.strPortName = "COM" + DeviceSetting.PortNum.ToString();
            port.PortName = DeviceSetting.strPortName;
            port.BaudRate = BAUDRATE;
            Thread.Sleep(100);
            LogSetting.CreateLogFile("데이터 수신 준비 완료");
            port.Open();
            while (blsFlag1) 
            {
                int nReceiveData = port.ReadByte();
                if (this.InvokeRequired ) 
                {
                    this.Invoke(new MethodInvoker(delegate () 
                    {
                        string strTemp;
                        DeviceSetting.cntData++;
                        if (DeviceSetting.cntData == 520)
                        {
                            tbDebug.Text += "\r\n";
                        }
                        tbDebug.Text += string.Format("{0:X2}", nReceiveData);
                        strTemp = tbDebug.Text;
                        LogSetting.ReceiveData(strTemp);
                    }));
                }
                
                
                
                Thread.Sleep(10);
                
            }
            port.Close();
            return;
        }
      

        private void button3_Click(object sender, EventArgs e)
        {
            Monitor.Enter(this);
            try
            {
                port.Write(tbDebug_Send.Text);
                LogSetting.CreateLogFile("데이터 송신중.. 송신내용 : "+ tbDebug_Send.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Auto Connect를 먼저 클릭하세요.");
                
            }
            
            Monitor.Exit(this);
        }
    }
}
