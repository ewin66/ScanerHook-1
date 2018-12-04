using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scanner_demo
{
    public class SerialPortEvents : EventArgs
    {
        /// <summary>
        /// 传递数据
        /// </summary>
        private byte[] m_bufferData;
        public byte[] BufferData
        {
            get { return m_bufferData; }
            set { m_bufferData = value; }
        }

        public SerialPortEvents()
            : base()
        {
        }

        public SerialPortEvents(byte[] data)
            : base()
        {
            m_bufferData = data;
        }

    }

}
