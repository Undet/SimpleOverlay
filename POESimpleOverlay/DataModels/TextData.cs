using GameOverlay.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POESimpleOverlay.DataModels
{
    class TextData
    {
        public List<string> Text = new List<string>();

        public int LongestString;

        public List<int> WindowSize = new List<int>();

        public List<string> Headers = new List<string>();

        public int WindowMaxChars => _windowMaxChars;

        private List<int> _windowSize = new List<int>();
        private int _windowMaxChars;
        private int _windowMaxLines;

        public TextData(string filePath, int fontSize = 16)
        {
            ReadAllStrings(filePath);
            FindLongestString();
            ComputeWindowSize(fontSize);
        }

        public static string WrapText(string text, int maxCharactersPerLine)
        { 
            var sb = new StringBuilder();            

            int currentIndex = 0;
            while (currentIndex < text.Length)
            {
                int endIndex = Math.Min(currentIndex + maxCharactersPerLine, text.Length);

                if (endIndex == text.Length)
                {
                    sb.AppendLine(text.Substring(currentIndex));
                    currentIndex = endIndex;
                }
                else if (char.IsWhiteSpace(text[endIndex]))
                {
                    sb.AppendLine(text.Substring(currentIndex, endIndex - currentIndex).Trim());
                    currentIndex = endIndex + 1;
                }
                else
                {
                    int breakIndex = text.LastIndexOf(' ', endIndex);

                    if (breakIndex >= currentIndex)
                    {
                        sb.AppendLine(text.Substring(currentIndex, breakIndex - currentIndex).Trim());
                        currentIndex = breakIndex + 1;
                    }
                    else
                    {
                        sb.AppendLine(text.Substring(currentIndex, endIndex - currentIndex).Trim());
                        currentIndex = endIndex;
                    }
                }
            }

            return sb.ToString();
        }


        private void ComputeWindowSize(int fontSize)
        {
            int localWidth = _windowSize[2];
            _windowMaxChars = (int)Math.Floor((decimal)localWidth / fontSize);
            _windowMaxLines = (int)Math.Ceiling((decimal)LongestString / _windowMaxChars);

            WindowSize.Add(_windowSize[0]);
            WindowSize.Add(_windowSize[1]);
            WindowSize.Add(_windowSize[2]);
            WindowSize.Add(_windowMaxLines * fontSize);
        }

        private void FindLongestString()
        {
            int countr = 0;
            foreach (var str in Text)
            {
                if (str.Length > countr)
                {
                    countr = str.Length;
                }
            }
            LongestString = countr;
        }

        private void ReadAllStrings(string filePath)
        {
            List<string> strings = new List<string>();

            if (File.Exists(filePath))
            {
                string tmps = File.ReadAllText(filePath);

                tmps = tmps.Replace("\t", "");
                strings.AddRange(tmps.Split('\n'));

                var tmp = strings[0].Split(' ');

                foreach (var s in tmp)
                {
                    _windowSize.Add(int.Parse(s));
                }

                for (int i = 1; i < strings.Count; i++)
                {
                    if (!strings[i].Equals("\r"))
                    {
                        Text.Add(strings[i]);
                    }
                }
            }
        }
        public static bool IsStringHeader(string str)
        {
            if (str.StartsWith("[") & str.EndsWith("\r"))
            {
                return true;
            }
            return false;
        }
    }
}
