using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thermal_imaging_camera;
#region ModbusRTU
using Modbus.Device;
using System.IO.Ports;
using System.Windows.Forms;
using System.Threading;
using Thermal_imaging_camera;
using System.Data;
#endregion

namespace header
{
    internal class Header
    {
        
        
        
        
        
        public string strPortName;
        public bool blsbWorking;
        public int PortNum=1;
        private const int READTIMEOUT    =            100;
        private const int WRITETIMEOUT   =            100;
        public const int BAUDRATE        =            9600;
        
        Thread Debug_Thread = new Thread(Func);
        private static string strCpyPortName;

        //Port 객체 생성
        SerialPort _port;
        ModbusSerialMaster _master;

        public void PortOpen(string portName) 
        {
           

            //Setting Port
            _port = new SerialPort(portName,BAUDRATE);
            _port.ReadTimeout = READTIMEOUT;
            _port.WriteTimeout = WRITETIMEOUT;
            _port.Open();
            strCpyPortName = portName;
           
        }
        public int FindCom(string strPortName,int PortNum) 
        {
            try
            {
                PortOpen(strPortName);
                MessageBox.Show("포트 이름 : "+ strPortName);
                _port.Close();
            }
            catch (Exception)
            {
                string strTempPortName;
                PortNum++;
                strTempPortName = "COM"+PortNum.ToString();
                PortNum = FindCom(strTempPortName, PortNum);
                if (PortNum == 20) 
                {
                    MessageBox.Show(strTempPortName);
                    return 0;
                }
            }
            return PortNum;
        }

        public void PortClose() 
        { 
            _port.Close();
        }
        
        private static void Func() 
        {
            SerialPort port = new SerialPort(strCpyPortName, BAUDRATE);
            Form1 form1 = new Form1();
            
            //port.ReadTimeout = READTIMEOUT;
            //port.WriteTimeout = WRITETIMEOUT;
            port.Open();
            while (true)//후에 루프문 끊는것 생각해보기 
            {
                Monitor.Enter(port);
                Thread.Sleep(10);
                int nReceiveData = port.ReadByte();
                form1.nTemp = nReceiveData;
                Monitor.Exit(port);
            }
            
        }
        public bool Return_blsWorking(bool blsWorking) 
        {
            return blsWorking;
        }
        public void Thread_Start() 
        {
            Debug_Thread.Start();
        }
        
    }
}
