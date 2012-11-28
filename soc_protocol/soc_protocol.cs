using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace soc_protocol
{

    public class soc_protocol
    {
        struct _serialframe_
        {
            Byte startcode;
            Byte cmdone;
            Byte cmdtwo;
            Byte length;
            Byte[] buffer;
            Byte checksum;
        };

        enum SERCOM_TYPE
        {
            COM_NULL = 0,
            COM_START,
            COM_CONNECT = 0x10,
            COM_HANDINFO,
            COM_OK,
            COM_RESET,
            COM_ALLINFO = 0x20,	/* chipid, manufactureid, modelid, hardwareid */
            COM_CHIPID,
            COM_MFID,
            COM_MDID,
            COM_HWID,
            COM_LICENSE,		/*授权信息*/
            COM_LICENSEOK,		/*授权信息ok*/
            COM_OPENFLASH,
            COM_OPENFLASHOK,
            COM_CLOSEFLASH,
            COM_CLOSEFLASHOK,

            COM_DEBUG = 0x80,	/* 发送debug信息，下位机内存的值 addr + val*/
            COM_RETURN,
            COM_END
        };
    }
}
