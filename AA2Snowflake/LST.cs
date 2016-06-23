using AA2Snowflake.Personalities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SB3Utility;
using AA2Data;
using System.IO;

namespace AA2Snowflake
{
    public enum LSTMode
    {
        Default,
        Custom
    }

    public static class LSTFactory
    {
        public static PersonalityLST LoadLST(this IPersonality p)
        {
            if (p.Custom)
            {
                return new CustomPersonalityLST(p.GetLst().ToStream().ToArray());
            }
            else
            {
                return new PersonalityLST(p.GetLst().ToStream().ToArray());
            }
        }

        public static PersonalityLST LoadLST(this IWriteFile iw, LSTMode mode)
        {
            if (mode == LSTMode.Custom)
            {
                return new CustomPersonalityLST(iw.ToStream().ToArray());
            }
            else
            {
                return new PersonalityLST(iw.ToStream().ToArray());
            }

        }

        public static IWriteFile ToSubfile(this BaseLST lst, string name)
        {
            return new MemSubfile(lst.raw, name);
        }
    }
}
