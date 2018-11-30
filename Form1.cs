using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace scanner_demo
{
    public partial class Form1 : Form
    {
        private ScanerHook listener = new ScanerHook();
        public Form1()
        {
            InitializeComponent();
            listener.ScanerEvent += Listener_ScanerEvent;
        }

        private void Listener_ScanerEvent(ScanerHook.ScanerCodes codes)
        {

            dgv_lst.Rows.Add(new object[] { codes.KeyDownCount, codes.Event.message, codes.Event.paramH, codes.Event.paramL, codes.CurrentChar, codes.Result, codes.isShift, codes.CurrentKey });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listener.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            listener.Stop();
        }
    }
}
