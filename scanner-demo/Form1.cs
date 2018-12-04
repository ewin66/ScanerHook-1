using Microsoft.Win32;
using scanner_demo.Properties;
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

namespace scanner_demo
{
    public partial class Form1 : Form
    {
        private ScanerHook listener = new ScanerHook();

        SerialPortListener m_SerialPort = null;
        public Form1()
        {
            InitializeComponent();
            listener.ScanerEvent += Listener_ScanerEvent;
        }

        private void Listener_ScanerEvent(ScanerHook.ScanerCodes codes)
        {

            //dgv_lst.Rows.Add(new object[] { codes.KeyDownCount, codes.Event.message, codes.Event.paramH, codes.Event.paramL, codes.CurrentChar, codes.Result, codes.isShift, codes.CurrentKey });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listener.Start();
            
            this.tbPort.Text = Settings.Default.port;
            this.tbFrequency.SelectedIndex = Settings.Default.freq;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            listener.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (m_SerialPort == null)
            {
                m_SerialPort = new SerialPortListener(tbPort.Text, Convert.ToInt32(tbFrequency.Text));
                m_SerialPort.Parity = Parity.None;
                m_SerialPort.StopBits = StopBits.One;
                m_SerialPort.Handshake = Handshake.None;
                m_SerialPort.DataBits = 8;
                m_SerialPort.ReadBufferSize = 100;
                m_SerialPort.ReceivedBytesThreshold = 1;
                m_SerialPort.BufferSize = 4;
                m_SerialPort.ReceiveTimeout = Convert.ToInt32(tbTimeout.Text);
                m_SerialPort.WriteBufferSize = 100;
                m_SerialPort.SendInterval = 100;
                m_SerialPort.SerialPortResult += new HandResult(SerialPort_Result);
                m_SerialPort.OnSerialPortReceived += new OnReceivedData(SerialPort_Received);
                m_SerialPort.OnSeriaPortSend += new OnSendData(SerialPort_Send);
            }

            if (m_SerialPort.IsOpen)
            {
                m_SerialPort.Stop();
                btnListen.Text = "启动监听";
            }
            else
            {
                m_SerialPort.Start();
                btnListen.Text = "关闭监听";
            }
            
        }

        void SerialPort_Result(object sender, SerialPortEvents e)
        {
            this.Invoke(new MethodInvoker(() => {
                //处理结果
                rtbResult.Text += Encoding.GetEncoding("GB2312").GetString(e.BufferData);
            }));
        }

        void SerialPort_Received(object sender, SerialPortEvents e)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                long receivedCount = Convert.ToInt64(lbReceivedCount.Text);
                if (e.BufferData != null)
                {
                    receivedCount += e.BufferData.Length;
                }
                lbReceivedCount.Text = receivedCount.ToString();
            }));
        }

        void SerialPort_Send(object sender, SerialPortEvents e)
        {
            //this.Invoke(new MethodInvoker(() =>
            //{
            //    long sendCount = Convert.ToInt64(lbSendCount.Text);
            //    if (e.BufferData != null)
            //    {
            //        sendCount += e.BufferData.Length;
            //    }
            //    lbSendCount.Text = sendCount.ToString();
            //}));
        }

        private void tbPort_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.port = tbPort.Text;
            Settings.Default.Save();
        }

        private void tbFrequency_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.freq = this.tbFrequency.SelectedIndex;
            Settings.Default.Save();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            RegistryKey keyCom = Registry.LocalMachine.OpenSubKey("Hardware\\DeviceMap\\SerialComm");
            if (keyCom != null)
            {
                string[] sSubKeys = keyCom.GetValueNames();
                foreach (string sName in sSubKeys)
                {
                    string sValue = (string)keyCom.GetValue(sName);
                }
                rtbResult.Text += string.Join("\r\n", sSubKeys);
            }
        }
    }
}
