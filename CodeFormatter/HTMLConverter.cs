using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeFormatter
{
    public class HTMLConverter
    {
        private const string Space = "&nbsp;";
        private const string LineSpace = "<br>";
        
        public int SpacesPerTab { get; private set; }

        private readonly string Tab;
        private readonly string TabInSpaces;

        public HTMLConverter(int spacesPerTab = 4)
        {
            SpacesPerTab = spacesPerTab;

            StringBuilder sb; 

            //Build Tab
            sb = new StringBuilder();
            
            for (int i = 0; i < SpacesPerTab; i++)
            {
                sb.Append(Space);
            }

            Tab = sb.ToString();

            //Build Spaces Per Tab
            sb = new StringBuilder();
            for (int i = 0; i < spacesPerTab; i++)
            {
                sb.Append(" ");
            }

            TabInSpaces = sb.ToString();
        }

        public string Convert(string code)
        {
            StringBuilder sb = new StringBuilder();
            StringReader sr = new StringReader(code);

            string line;

            while ((line = sr.ReadLine()) != null)
            {
                string l = WebUtility.HtmlEncode(line);
                int firstNonSpace = GetIndexOfNonspace(l);

                if (firstNonSpace > 0)
                {
                    int numberOfTabs = firstNonSpace / SpacesPerTab;

                    StringBuilder sb2 = new StringBuilder();
                    for (int i = 0; i < numberOfTabs; i++ )
                        sb2.Append("\t");

                    l = sb2.ToString() + l.Substring(firstNonSpace);
                }


                //l = l.Replace(TabInSpaces, Tab);
                l = l.Replace("\t", Tab);
                l += LineSpace;

                sb.AppendLine(l);
            }

            return "<code>" + sb.ToString() + "</code>";
        }

        private int GetIndexOfNonspace(string l)
        {
            int idx = -1;

            foreach (char c in l)
            {
                if (c == ' ')
                    idx++;
                else
                    break;
            }

            return idx;
        }
    }
}
