using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA2Data
{
    public static partial class Tools
    {
        public static byte[] ResizeByteArray(byte[] array, int length)
        {
            byte[] b = new byte[length];
            Array.Copy(array, b, length);
            return b;
        }

        public static string TrimBytes(this string input, int maxLength)
        {
            return new string(input
                .TakeWhile((c, i) =>
                    ShiftJIS.GetByteCount(input.Substring(0, i + 1)) <= maxLength)
                .ToArray());
        }
    }

    public class BaseData
    {
        public virtual int dataLength => -1;
        public byte[] raw { get; set; }

        public BaseData()
        {
            if (dataLength > 0)
                raw = new byte[dataLength];
        }

        public BaseData(byte[] data)
        {
            if (dataLength > 0)
                raw = Tools.ResizeByteArray(data, dataLength);
            else
                raw = data;
        }

        public void writeValue(object value, int offset, AA2DataType type)
        {
            switch (type)
            {
                case AA2DataType.Bool:
                    if ((bool)value)
                        raw[offset] = 1;
                    else
                        raw[offset] = 0;
                    break;
                case AA2DataType.Byte:
                    raw[offset] = (byte)value;
                    break;
                case AA2DataType.String:
                    byte[] b = Tools.ShiftJIS.GetBytes((string)value);
                    for (int i = 0; i < b.Length; i++)
                        raw[offset + i] = b[i];
                    break;
                case AA2DataType.StringEx:
                    byte[] bx = Tools.ExTransform(Tools.ShiftJIS.GetBytes((string)value));
                    for (int i = 0; i < bx.Length; i++)
                        raw[offset + i] = bx[i];
                    break;
                case AA2DataType.DataBlock:
                    var block = (BaseData)value;
                    Array.Copy(block.raw, 0, this.raw, offset, block.dataLength);
                    break;
                default:
#warning finish implementation
                    throw new NotImplementedException();
            }
        }

        public object readValue(int offset, AA2DataType type, int length = 0)
        {
            switch (type)
            {
                case AA2DataType.Bool:
                    if (raw[offset] == 0)
                        return false;
                    else
                        return true;
                case AA2DataType.Byte:
                    return raw[offset];
                case AA2DataType.String:
                    byte[] b = new byte[length];
                    for (int i = 0; i < length; i++)
                        b[i] = raw[offset + i];
                    return Tools.ShiftJIS.GetString(b);
                case AA2DataType.StringEx:
                    byte[] bx = new byte[length];
                    for (int i = 0; i < length; i++)
                        bx[i] = raw[offset + i];
                    return Tools.ShiftJIS.GetString(Tools.ExTransform(bx));
                case AA2DataType.DataBlock:
                    var block = new byte[length];
                    Array.Copy(raw, offset, block, 0, length);
                    return new BaseData(block);
                default:
#warning finish implementation
                    throw new NotImplementedException();
            }
        }
    }

    public class AA2Data : BaseData
    {
        public override int dataLength => 3011;
        
        public new byte[] raw
        {
            get
            {
                writeValue(CLOTH_UNIFORM, 0xA57, AA2DataType.DataBlock);
                writeValue(CLOTH_SPORT, 0xAB2, AA2DataType.DataBlock);
                writeValue(CLOTH_SWIM, 0xB0D, AA2DataType.DataBlock);
                writeValue(CLOTH_CLUB, 0xB68, AA2DataType.DataBlock);
                return base.raw;
            }

            set
            {
                base.raw = value;
                int length = 91;
                CLOTH_UNIFORM = new AA2Cloth((BaseData)readValue(0xA57, AA2DataType.DataBlock, length));
                CLOTH_SPORT = new AA2Cloth((BaseData)readValue(0xAB2, AA2DataType.DataBlock, length));
                CLOTH_SWIM = new AA2Cloth((BaseData)readValue(0xB0D, AA2DataType.DataBlock, length));
                CLOTH_CLUB = new AA2Cloth((BaseData)readValue(0xB68, AA2DataType.DataBlock, length));
            }
        }

        public bool RAINBOW_CARD
        {
            get
            {
                return (bool)readValue(0x6C8, AA2DataType.Bool);
            }
            set
            {
                writeValue(value, 0x6C8, AA2DataType.Bool);
            }
        }

        public byte PROFILE_GENDER
        {
            get
            {
                return (byte)readValue(0x014, AA2DataType.Byte);
            }
            set
            {
                writeValue(value, 0x014, AA2DataType.Byte);
            }
        }

        public byte PROFILE_PERSONALITY_ID
        {
            get
            {
                return (byte)readValue(0x41D, AA2DataType.Byte);
            }
            set
            {
                writeValue(value, 0x41D, AA2DataType.Byte);
            }
        }

        public string PROFILE_FAMILY_NAME
        {
            get
            {
                return (string)readValue(0x015, AA2DataType.StringEx, 260);
            }
            set
            {
                writeValue(value, 0x015, AA2DataType.StringEx);
            }
        }

        public string PROFILE_FIRST_NAME
        {
            get
            {
                return (string)readValue(0x119, AA2DataType.StringEx, 260);
            }
            set
            {
                writeValue(value, 0x119, AA2DataType.StringEx);
            }
        }

        public string PROFILE_BIO
        {
            get
            {
                return ((string)readValue(0x21D, AA2DataType.StringEx, 512)).TrimEnd(new char[] { '\0' });
            }
            set
            {
                writeValue(value.TrimBytes(512), 0x21D, AA2DataType.StringEx);
            }
        }

        public AA2Cloth CLOTH_UNIFORM { get; set; }
        public AA2Cloth CLOTH_SPORT { get; set; }
        public AA2Cloth CLOTH_SWIM { get; set; }
        public AA2Cloth CLOTH_CLUB { get; set; }
    }

    public class AA2Cloth : BaseData
    {
        public override int dataLength => 91;

        public AA2Cloth(BaseData b)
        {
            raw = b.raw;
        }

        public AA2Cloth(byte[] data)
        {
            //for some reason the IS_ONEPIECE, IS_UNDERWEAR, IS_SKIRT flags are switched around
            byte[] temp = new byte[dataLength];
            Array.Copy(data, 1, temp, 0, 91);

            byte[] flags = new byte[3];
            Array.Copy(temp, 8, flags, 0, 3); //copy the flags
            Array.Copy(temp, 11, temp, 8, 61); //shift the next 40 bytes back up
            Array.Copy(flags, 0, temp, 72, 3); //put the flags back

            raw = temp;
        }
    }

    public enum AA2DataType
    {
        Bool,
        Byte,
        Int32,
        String,
        StringEx,
        DataBlock
    }
}
