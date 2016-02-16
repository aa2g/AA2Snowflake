using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA2Snowflake.Personalities
{
    public enum Gender
    {
        Female,
        Male
    }
    interface IPersonality
    {
        string Name { get; }
        byte Slot { get; }
        string ID { get; } // example: a01 or s34, usually same as slot with a or s at the start
        Gender Gender { get; } // you should be able to determine if it's male or female by looking at if the ID starts with an a or s but i'm not building a dynamic system
        string ICFLocation { get; } //.pp where the icfs are stored
        string LSTLocation { get; } //.pp where the personality lst is stored & "/" & actual filename of the .lst

        /*
        if you wish to build a dynamic system that loads personalities on a per .pp basis the approximate regex for each .pp is:

        jg2p05_([as]\d+)_0[01].pp

        the ID is captured, and you can find out the slot and name by examining the only .lst inside the .pp
        column 2 is the slot, column 7 is ID (if you didn't want to capture it in the regex) and column 8 is the name

        a much easier option than using .xml files like ReiEdit since we can view the contents of .pp files
        */
    }

    #region Base Personalities

    public abstract class BasePersonality : IPersonality //because I can't be fucked writing the same thing over and over again
    {
        public abstract string Name { get; }
        public abstract byte Slot { get; }
        public abstract string ID { get; }
        public abstract Gender Gender { get; }
        public string ICFLocation => "jg2e01_00_00.pp";
        public string LSTLocation => "jg2e00_00_00.pp/jg2e_00_01_00_00.lst";
    }

    public class Lively : BasePersonality
    {
        public override string ID => "a00";
        public override string Name => "Lively";
        public override byte Slot => 0;
        public override Gender Gender => Gender.Female;
    }

    public class Delicate : BasePersonality
    {
        public override string ID => "a01";
        public override string Name => "Delicate";
        public override byte Slot => 1;
        public override Gender Gender => Gender.Female;
    }

    public class Cheerful : BasePersonality
    {
        public override string ID => "a02";
        public override string Name => "Cheerful";
        public override byte Slot => 2;
        public override Gender Gender => Gender.Female;
    }

    public class Quiet : BasePersonality
    {
        public override string ID => "a03";
        public override string Name => "Quiet";
        public override byte Slot => 3;
        public override Gender Gender => Gender.Female;
    }

    public class Playful : BasePersonality
    {
        public override string ID => "a04";
        public override string Name => "Playful";
        public override byte Slot => 4;
        public override Gender Gender => Gender.Female;
    }

    public class Frisky : BasePersonality
    {
        public override string ID => "a05";
        public override string Name => "Frisky";
        public override byte Slot => 5;
        public override Gender Gender => Gender.Female;
    }

    public class Kind : BasePersonality
    {
        public override string ID => "a06";
        public override string Name => "Kind";
        public override byte Slot => 6;
        public override Gender Gender => Gender.Female;
    }

    public class Joyful : BasePersonality
    {
        public override string ID => "a07";
        public override string Name => "Joyful";
        public override byte Slot => 7;
        public override Gender Gender => Gender.Female;
    }

    public class Ordinary : BasePersonality
    {
        public override string ID => "a08";
        public override string Name => "Ordinary";
        public override byte Slot => 8;
        public override Gender Gender => Gender.Female;
    }

    public class Irritated : BasePersonality //best
    {
        public override string ID => "a09";
        public override string Name => "Irritated";
        public override byte Slot => 9;
        public override Gender Gender => Gender.Female;
    }

    public class Harsh : BasePersonality //worst
    {
        public override string ID => "a10";
        public override string Name => "Harsh";
        public override byte Slot => 10;
        public override Gender Gender => Gender.Female;
    }

    public class Sweet : BasePersonality
    {
        public override string ID => "a11";
        public override string Name => "Sweet";
        public override byte Slot => 11;
        public override Gender Gender => Gender.Female;
    }

    public class Creepy : BasePersonality
    {
        public override string ID => "a12";
        public override string Name => "Creepy";
        public override byte Slot => 12;
        public override Gender Gender => Gender.Female;
    }

    public class Reserved : BasePersonality
    {
        public override string ID => "a13";
        public override string Name => "Reserved";
        public override byte Slot => 13;
        public override Gender Gender => Gender.Female;
    }

    public class Dignified : BasePersonality
    {
        public override string ID => "a14";
        public override string Name => "Dignified";
        public override byte Slot => 14;
        public override Gender Gender => Gender.Female;
    }

    public class Aloof : BasePersonality
    {
        public override string ID => "a15";
        public override string Name => "Aloof";
        public override byte Slot => 15;
        public override Gender Gender => Gender.Female;
    }

    public class Smart : BasePersonality
    {
        public override string ID => "a16";
        public override string Name => "Smart";
        public override byte Slot => 16;
        public override Gender Gender => Gender.Female;
    }

    public class Genuine : BasePersonality
    {
        public override string ID => "a17";
        public override string Name => "Genuine";
        public override byte Slot => 17;
        public override Gender Gender => Gender.Female;
    }

    public class Mature : BasePersonality
    {
        public override string ID => "a18";
        public override string Name => "Mature";
        public override byte Slot => 18;
        public override Gender Gender => Gender.Female;
    }

    public class Lazy : BasePersonality
    {
        public override string ID => "a19";
        public override string Name => "Lazy";
        public override byte Slot => 19;
        public override Gender Gender => Gender.Female;
    }

    public class Manly : BasePersonality
    {
        public override string ID => "a20";
        public override string Name => "Manly";
        public override byte Slot => 20;
        public override Gender Gender => Gender.Female;
    }

    public class Gentle : BasePersonality
    {
        public override string ID => "s00";
        public override string Name => "Gentle";
        public override byte Slot => 21;
        public override Gender Gender => Gender.Male;
    }

    public class Positive : BasePersonality
    {
        public override string ID => "s01";
        public override string Name => "Positive";
        public override byte Slot => 22;
        public override Gender Gender => Gender.Male;
    }

    public class Otaku : BasePersonality
    {
        public override string ID => "s02";
        public override string Name => "Otaku";
        public override byte Slot => 23;
        public override Gender Gender => Gender.Male;
    }

    public class Savage : BasePersonality
    {
        public override string ID => "s03";
        public override string Name => "Savage";
        public override byte Slot => 24;
        public override Gender Gender => Gender.Male;
    }
    #endregion
}
