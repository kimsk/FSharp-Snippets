using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using numl.Model;

namespace TennisData
{
    public enum Digit
    {
        Zero,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine
    }

    public class DigitRecognizerRecord
    {
        [Label]
        public Digit Digit { get; set; }

        [EnumerableFeature(50)]
        public float[] Pixels { get; set; }
    }

    public static class Stuff
    {
        static Random Rng = new Random();
        public static Dictionary<Digit, float[]> Pixels = new Dictionary<Digit, float[]>(); 
        static Stuff()
        {
            foreach(Digit digit in (Digit[])Enum.GetValues(typeof(Digit)))
            {
                Pixels[digit] = getRandomFloats(50);
            }
        }

        static float[] getRandomFloats(int num)
        {            
            var ret = new List<float>();
            for (int i = 0; i < num; i++)
            {
                ret.Add(Rng.Next(0,255));
            }

            return ret.ToArray();
        }

        static DigitRecognizerRecord getRecord(Digit digit)
        {
            return new DigitRecognizerRecord {Digit = digit, Pixels = Pixels[digit]};
        }

        public static IEnumerable<DigitRecognizerRecord> GetRecords()
        {
            return new[]
            {
                getRecord(Digit.One),
                getRecord(Digit.Two),
                getRecord(Digit.Three),
                getRecord(Digit.One),
                getRecord(Digit.Four),
                getRecord(Digit.Five),
                getRecord(Digit.One),
                getRecord(Digit.Eight),
                getRecord(Digit.One),
                getRecord(Digit.Nine),
                getRecord(Digit.Four),
                getRecord(Digit.Seven),
                getRecord(Digit.Seven),
                getRecord(Digit.Eight),
                getRecord(Digit.Six),
            };          
        } 
    }
}
