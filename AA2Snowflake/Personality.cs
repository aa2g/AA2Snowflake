using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SB3Utility;
using System.Diagnostics;
using AA2Install;
using System.IO;
using System.Text.RegularExpressions;

namespace AA2Snowflake.Personalities
{
    public enum Gender: byte
    {
        Female = 1,
        Male = 0
    }
    public interface IPersonality
    {
        string Name { get; }
        byte Slot { get; }
        bool Custom { get; } //custom/dlc personalities have a different .lst format
        string ID { get; } // example: a01 or s34, usually same as slot with a or s at the start
        Gender Gender { get; } // you should be able to determine if it's male or female by looking at if the ID starts with an a or s but i'm not building a dynamic system
        string ICFLocation { get; } //.pp where the icfs are stored
        string LSTLocation { get; } //.pp where the personality lst is stored & "/" & actual filename of the .lst

        /*
        if you wish to build a dynamic system that loads personalities on a per .pp basis the approximate regex for each .pp is:

        jg2p05_([as]\d+)_0[01]\.pp

        the ID is captured, and you can find out the slot and name by examining the only .lst inside the .pp
        column 2 is the slot, column 7 is ID (if you didn't want to capture it in the regex) and column 8 is the name

        a much easier option than using .xml files like ReiEdit since we can view the contents of .pp files
        */
    }

    [DebuggerDisplay("{Name}: {ID}")]
    public class CustomPersonality : IPersonality
    {
        private Gender _gender;
        public Gender Gender => _gender;

        private string _icflocation;
        public string ICFLocation => _icflocation;

        private string _id;
        public string ID => _id;

        private string _lstlocation;
        public string LSTLocation => _lstlocation;

        private string _name;
        public string Name => _name;

        private byte _slot;
        public byte Slot => _slot;
        
        public bool Custom => true;

        public CustomPersonality(Gender gender, string icflocation, string id, string lstlocation, string name, byte slot)
        {
            _gender = gender;
            _icflocation = icflocation;
            _id = id;
            _lstlocation = lstlocation;
            _name = name;
            _slot = slot;
        }
    }

    public static class PersonalityFactory
    {
        private static Dictionary<string, string> AppendTranslation = new Dictionary<string, string> {
            { "曹", "Cadet" },
            { "慈", "Caring" },
            { "策", "Schemer" },
            { "軽", "Carefree" },
            { "温", "Warm" },
        };

        public static CustomPersonality LoadPersonality(ppParser pp)
        {
            if (!pp.Subfiles.Select(iw => iw.Name).Any(n => n.EndsWith(".icf")) || //check if it's a valid personality .pp which contains everything we need
                !pp.Subfiles.Select(iw => iw.Name).Any(n => n.EndsWith(".lst") && n.StartsWith("jg2p")))
                return null;

            string filename = pp.FilePath.Remove(0, pp.FilePath.LastIndexOf('\\') + 1);
            IWriteFile lst = pp.Subfiles.First(iw => iw.Name.EndsWith(".lst") && iw.Name.StartsWith("jg2p")); //you can thank a certain person for making this difficult (http://pastebin.com/3zkjpM7e)

            byte slot = byte.Parse(Tools.GetLstValue(lst, 2));

            Gender gender = (Gender)byte.Parse(Tools.GetLstValue(lst, 6)); //not sure if more accurate than grabbing ID letter, this column is set to 1 for female and 0 for male

            string ID = Tools.GetLstValue(lst, 7);

            string Name = Tools.GetLstValue(lst, 8);
            Name = AppendTranslation.GetValueOrDefault(Name, Name);

            return new CustomPersonality(gender, filename, ID, filename + "/" + lst.Name, Name, slot);
        }

        public static BasePersonality[] BasePersonalities
        {
            get
            {
                return new BasePersonality[] {
                    new Lively(),
                    new Delicate(),
                    new Cheerful(),
                    new Quiet(),
                    new Playful(),
                    new Frisky(),
                    new Kind(),
                    new Joyful(),
                    new Ordinary(),
                    new Irritated(),
                    new Harsh(),
                    new Sweet(),
                    new Creepy(),
                    new Reserved(),
                    new Dignified(),
                    new Aloof(),
                    new Smart(),
                    new Genuine(),
                    new Mature(),
                    new Lazy(),
                    new Manly(),
                    new Gentle(),
                    new Positive(),
                    new Otaku(),
                    new Savage(),
                };
            }
        }

        public static Dictionary<int, IPersonality> GetAllPersonalities()
        {
            Dictionary<int, IPersonality> pers = new Dictionary<int, IPersonality>();
            foreach (BasePersonality bp in BasePersonalities)
                pers.Add(bp.Slot, bp);

            Regex regex = new Regex(@"jg2p05_([as]\d+)_0[01]\.pp");
            foreach (string path in Directory.EnumerateFiles(Paths.AA2Play))
                if (regex.IsMatch(path))
                {
                    CustomPersonality cp = LoadPersonality(new ppParser(path, new ppFormat_AA2()));
                    if (!ReferenceEquals(cp, null)) //is there a better way to check null?
                        pers.Add(cp.Slot, cp);
                }

            return pers;
        }

        public static ppParser GetLstPP(this IPersonality personality)
        {
            string path = Paths.AA2Play + "\\" + personality.LSTLocation.RemoveFilename('/');
            if (!File.Exists(path))
                path = Paths.AA2Edit + "\\" + personality.LSTLocation.RemoveFilename('/');
            return new ppParser(path, new ppFormat_AA2());
        }

        public static IWriteFile GetLstFromPP(this ppParser pp, IPersonality personality)
        {
            string file = personality.LSTLocation.GetFilename('/');
            return pp.Subfiles.First(iw => iw.Name == file);
        }

        public static ppParser GetIcfPP(this IPersonality personality)
        {
            string path = Paths.AA2Play + "\\" + personality.ICFLocation;
            if (!File.Exists(path))
                path = Paths.AA2Edit + "\\" + personality.ICFLocation;
            return new ppParser(path, new ppFormat_AA2());
        }
    }

    #region Base Personalities

    [DebuggerDisplay("{Name}: {ID}")]
    public abstract class BasePersonality : IPersonality //because I can't be fucked writing the same thing over and over again
    {
        public abstract string Name { get; }
        public abstract byte Slot { get; }
        public abstract string ID { get; }
        public abstract Gender Gender { get; }
        public string ICFLocation => "jg2e01_00_00.pp";
        public string LSTLocation => "jg2e00_00_00.pp/jg2e_00_01_00_00.lst";
        public bool Custom => false;
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
