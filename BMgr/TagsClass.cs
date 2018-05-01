using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMgr
{
    public static class TagsClass
    {
        public static string[] GetTagListFromString(string TagString)
        {
            string[] sArray = TagString.Split('|');
            return sArray;
        }
        public static string GetTagStringFromList(string[] TagList)
        {
            string outString=string.Empty;
            foreach (string Tag in TagList)
            {
                outString += Tag + "|";
            }
            if (outString != string.Empty)
            {
                outString = outString.Substring(0, outString.Length - 1);
            }
            
            return outString;
        }
        public static bool IsContainAllTags(string[] Target, string[] condition)
        {
            List<string> cmpA = Target.ToList();
            List<string> cmpB = condition.ToList();
            return cmpB.All(b => cmpA.Any(a => a.Equals(b)));
        }
    }
}
