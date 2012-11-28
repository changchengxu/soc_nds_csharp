using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace soc_serialport
{
    public class Uart
    {
        String mConfigFile;
		SerialPort mSpSlot;

		String mName;
		Int32 mBaudRate;
		Parity mParity;
		Int32 mDataBits;
		StopBits mStopBit;
		Int32 mReadTimeout;

        public void Uart(string name)
        {
            SerialPort d = new SerialPort();
        }
    }
}
