using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WATSerialcom
{
    public partial class main : Form
    {
        Engine engine = new Engine();
        public main()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            this.OpenComm();
        }

        private void OpenComm()
        {
            try
            {
                // sp1 값이 null 일때만 새로운 SerialPort 를 생성합니다.
                if (!sp1.IsOpen)
                {
                    sp1.PortName = this.cbPortName.Text;
                    sp1.BaudRate = GetSelectedBaudRate();

                    sp1.RtsEnable = true;
                    sp1.Open();

                    RefreshViewControl(true);

                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private Int32 GetSelectedBaudRate()
        {
            if (this.rdb600.Checked)
                return 600;
            else if (this.rdb1200.Checked)
                return 1200;
            else if (this.rdb2400.Checked)
                return 2400;
            else if (this.rdb4800.Checked)
                return 4800;
            else if (this.rdb9600.Checked)
                return 9600;

            else if (this.rdb14400.Checked)
                return 14400;
            else if (this.rdb19200.Checked)
                return 19200;
            else if (this.rdb28800.Checked)
                return 28800;
            else if (this.rdb38400.Checked)
                return 38400;
            else if (this.rdb56000.Checked)
                return 56000;

            else if (this.rdb57600.Checked)
                return 57600;
            else if (this.rdb115200.Checked)
                return 115200;
            else if (this.rdb128000.Checked)
                return 128000;
            else if (this.rdb256000.Checked)
                return 256000;
            else
            {
                try
                {
                    return Convert.ToInt32(this.txbBaudRate.Text);
                }
                catch
                {
                    return 9600;

                }
            }
        }


        private void RefreshViewControl(bool _opened)
        {
            if (btnOpen.Enabled == _opened)
                btnOpen.Enabled = !_opened;
            
            
            
            //if (btnClose.Enabled != _opened)
            //    btnClose.Enabled = _opened;
            //if (ctrlTxData1.Enabled != _opened)
            //    ctrlTxData1.Enabled = _opened;

            //if (ctrlTxData2.Enabled != _opened)
            //    ctrlTxData2.Enabled = _opened;
            //if (ctrlTxData3.Enabled != _opened)
            //    ctrlTxData3.Enabled = _opened;
            //if (ctrlTxData4.Enabled != _opened)
            //    ctrlTxData4.Enabled = _opened;

            //     if(splitContainer1.Enabled != _opened) splitContainer1.Enabled = _opened;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.CloseComm();
        }
        private void CloseComm()
        {
            // sp1 이 null 아닐때만 close 처리를 해준다.
            if (null != sp1)
            {
                if (sp1.IsOpen)
                {
                    sp1.Close();
                }
            }
            RefreshViewControl(false);
        }

        private void main_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            //////////////////////////////////////////////////////////////////////////
            // 사용가능한 시리얼 포트 얻기
            try
            {
                cbPortName.Items.AddRange(SerialPort.GetPortNames());
                // timer1.Tick += timer1_Tick;
                timer1.Interval = 1;
                timer_clear.Interval = 10;
                // Y-축 zoom : 마우스 휠 이벤트 이용
                //chart1.MouseWheel += MouseWheelOnChart;
            }
            catch { }

            if (cbPortName.Items.Count > 0)
                cbPortName.SelectedIndex = 0;  // 컴포트 정보가 없을 경우 컴포트의 0번째를 사용
        }
    }
}
