using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA2Data
{
    public abstract partial class BaseLST
    {
        public byte[] raw { get; set; }

        public BaseLST(byte[] data)
        {
            raw = data;
        }
        
        public void WriteValue(int column, string replacement)
        {
            string slst = Tools.ShiftJIS.GetString(raw);

            StringBuilder str = new StringBuilder();
            foreach (string line in slst.Split(new string[] { "\r\n" }, StringSplitOptions.None))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] set = line.Split('\t');
                set[column - 1] = replacement;

                str.Append(set.Aggregate((i, j) => i + "\t" + j) + "\r\n");
            }

            raw = Tools.ShiftJIS.GetBytes(str.ToString());
        }

        public string ReadValue(int column, int row = 0)
        {
            string slst = Tools.ShiftJIS.GetString(raw);
            string line = slst.Split(new string[] { "\r\n" }, StringSplitOptions.None)[row];
            string[] set = line.Split('\t');
            return set[column - 1];
        }
    }

    public class PersonalityLST : BaseLST
    {
        public PersonalityLST(byte[] data) : base(data)
        {
            
        }
        
        public virtual int Slot
        {
            get
            {
                return int.Parse(ReadValue(1));
            }
            set
            {
                WriteValue(1, value.ToString());
            }
        }

        public virtual string ID
        {
            get
            {
                return ReadValue(2);
            }
            set
            {
                WriteValue(2, value);
            }
        }

        public virtual string Name
        {
            get
            {
                return ReadValue(3);
            }
            set
            {
                WriteValue(3, value);
            }
        }

        public virtual Gait Gait
        {
            get
            {
                return (Gait)int.Parse(ReadValue(4));
            }
            set
            {
                WriteValue(4, ((int)value).ToString());
            }
        }

        public virtual Gender Gender
        {
            get
            {
                return (Gender)byte.Parse(ReadValue(5));
            }
            set
            {
                WriteValue(5, ((byte)value).ToString());
            }
        }

        public virtual int AA2EditPose
        {
            get
            {
                return int.Parse(ReadValue(6));
            }
            set
            {
                WriteValue(6, value.ToString());
            }
        }

        public virtual int AA2EditEyebrow
        {
            get
            {
                return int.Parse(ReadValue(7));
            }
            set
            {
                WriteValue(7, value.ToString());
            }
        }

        public virtual int AA2EditEye
        {
            get
            {
                return int.Parse(ReadValue(8));
            }
            set
            {
                WriteValue(8, value.ToString());
            }
        }

        public virtual int AA2EditEyeOS
        {
            get
            {
                return int.Parse(ReadValue(9));
            }
            set
            {
                WriteValue(9, value.ToString());
            }
        }

        public virtual int AA2EditMouth
        {
            get
            {
                return int.Parse(ReadValue(10));
            }
            set
            {
                WriteValue(10, value.ToString());
            }
        }

#warning There are missing mouth width values here

        public virtual int AA2EditBlush
        {
            get
            {
                return int.Parse(ReadValue(15));
            }
            set
            {
                WriteValue(15, value.ToString());
            }
        }

        public virtual int AA2PlayPoseLowPoly
        {
            get
            {
                return int.Parse(ReadValue(16));
            }
            set
            {
                WriteValue(16, value.ToString());
            }
        }
        public virtual int AA2PlayPose
        {
            get
            {
                return int.Parse(ReadValue(17));
            }
            set
            {
                WriteValue(17, value.ToString());
            }
        }
    }

    public class CustomPersonalityLST : PersonalityLST
    {
        public CustomPersonalityLST(byte[] data) : base(data)
        {

        }

        public override int Slot
        {
            get
            {
                return int.Parse(ReadValue(2));
            }
            set
            {
                WriteValue(2, value.ToString());
            }
        }

        public override string ID
        {
            get
            {
                return ReadValue(7);
            }
            set
            {
                WriteValue(7, value);
            }
        }

        public override string Name
        {
            get
            {
                return ReadValue(8);
            }
            set
            {
                WriteValue(8, value);
            }
        }

        public override Gait Gait
        {
            get
            {
                return (Gait)int.Parse(ReadValue(9));
            }
            set
            {
                WriteValue(9, ((int)value).ToString());
            }
        }

        public override Gender Gender
        {
            get
            {
                return (Gender)byte.Parse(ReadValue(6));
            }
            set
            {
                WriteValue(6, ((byte)value).ToString());
            }
        }

        public override int AA2EditPose
        {
            get
            {
                return int.Parse(ReadValue(10));
            }
            set
            {
                WriteValue(10, value.ToString());
            }
        }

        public override int AA2EditEyebrow
        {
            get
            {
                return int.Parse(ReadValue(11));
            }
            set
            {
                WriteValue(11, value.ToString());
            }
        }

        public override int AA2EditEye
        {
            get
            {
                return int.Parse(ReadValue(12));
            }
            set
            {
                WriteValue(8, value.ToString());
            }
        }

        public override int AA2EditEyeOS
        {
            get
            {
                return int.Parse(ReadValue(13));
            }
            set
            {
                WriteValue(13, value.ToString());
            }
        }

        public override int AA2EditMouth
        {
            get
            {
                return int.Parse(ReadValue(14));
            }
            set
            {
                WriteValue(14, value.ToString());
            }
        }

#warning There are missing mouth width values here

        public override int AA2EditBlush
        {
            get
            {
                return int.Parse(ReadValue(21));
            }
            set
            {
                WriteValue(21, value.ToString());
            }
        }

        public override int AA2PlayPoseLowPoly
        {
            get
            {
                return int.Parse(ReadValue(15));
            }
            set
            {
                WriteValue(15, value.ToString());
            }
        }
        public override int AA2PlayPose
        {
            get
            {
                return int.Parse(ReadValue(16));
            }
            set
            {
                WriteValue(16, value.ToString());
            }
        }
    }

    public enum Gender : byte
    {
        Female = 1,
        Male = 0,
    }

    public enum Gait : int
    {
        Normal = 0,
        Cutesy = 1,
        Lively = 2,
        Dignified = 3,
        Modest = 4,
    }
}
