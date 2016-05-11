using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace lab1_Form
{
    class Table
    {
        private static List<string> listOfLexeme = new List<string>();
        private static List<int> listOfLexemeNumbers = new List<int>();
        public string[] lex = new string[listOfLexeme.Count()];
        public int[] numb = new int[listOfLexemeNumbers.Count()];


        public Table()
        {
            listOfLexeme = new List<string>();
            listOfLexemeNumbers = new List<int>();
        }
        public bool tableLex(string str)
        {
            ArrayList tableLex = new ArrayList
            {
                "program","var","begin","end"," ","do","while","(",")","!","[","]",
                "enddo","read","write",":",",","=","+","-","*","/","if","else","endif",
                "end","or","not","and","^","<","<=",">",">=","==","!=","id","const","integer","↲"};

            for (int i = 0; i < tableLex.Count; i++)
            {
                if (str.Equals(tableLex[i]))
                    return true;
            }
            return false;
        }

        public bool tableClass(string c)
        {
            string pattern = @"[a-zA-Z\s=:*^!,↲\<\>[\]\()\d+-]";
            Regex tableClass = new Regex(pattern);
            MatchCollection matchcollection = tableClass.Matches(c);
            int i = matchcollection.Count;
            if (i == c.Length)
                return true;
            else
            {
                return false;
            }
        }

        public string[] tableSeparators(string c, int numberOfSeparator)
        {
            char[] tableSeparator = new char[17] { ' ', ',', '=', '+', '-', '*', '/','[',']','!',
                '^', ':','(',')' ,'<','>','↲'};

            for (int i = 0; i < c.Length; i++)
            {
                for (int j = 0; j < tableSeparator.Length; j++)
                {
                    if (c[i].Equals(tableSeparator[j]))
                    {
                        string str = "";
                        bool povtor = true;
                        if (c[i] == '↲') { listOfLexeme.Add("↲"); listOfLexemeNumbers.Add(numberOfSeparator); break; }
                        if (c[i] == '=' && c[i + 1] == '='){ i++; listOfLexeme.Add("=="); listOfLexemeNumbers.Add(numberOfSeparator); break; }
                        if (c[i] == '!' && c[i + 1] == '=') { i++; listOfLexeme.Add("!="); listOfLexemeNumbers.Add(numberOfSeparator); }
                        if (c[i] == '=') { listOfLexeme.Add("="); listOfLexemeNumbers.Add(numberOfSeparator); }
                        if (c[i] == ':') { listOfLexeme.Add(":"); listOfLexemeNumbers.Add(numberOfSeparator); }
                        if (c[i] == '^') { listOfLexeme.Add("^"); listOfLexemeNumbers.Add(numberOfSeparator); break; }
                        if (c[i] == '<') { listOfLexeme.Add("<"); listOfLexemeNumbers.Add(numberOfSeparator); break; }
                        if (c[i] == '>') { listOfLexeme.Add(">"); listOfLexemeNumbers.Add(numberOfSeparator); break; }
                        if (c[i] == '+') { listOfLexeme.Add("+"); listOfLexemeNumbers.Add(numberOfSeparator); break; }
                        if (c[i] == '-') { listOfLexeme.Add("-"); listOfLexemeNumbers.Add(numberOfSeparator); break; }
                        if (c[i] == '*') { listOfLexeme.Add("*"); listOfLexemeNumbers.Add(numberOfSeparator); break; }
                        if (c[i] == '/') { listOfLexeme.Add("/"); listOfLexemeNumbers.Add(numberOfSeparator); break; }
                        if (c[i] == ',') { listOfLexeme.Add(","); listOfLexemeNumbers.Add(numberOfSeparator); }
                        if (c[i] == '(') { listOfLexeme.Add("("); listOfLexemeNumbers.Add(numberOfSeparator); }
                        if (c[i] == ')') { listOfLexeme.Add(")"); listOfLexemeNumbers.Add(numberOfSeparator); break; }
                        if (c[i] == '[') { listOfLexeme.Add("["); listOfLexemeNumbers.Add(numberOfSeparator); break; }
                        if (c[i] == ']') { listOfLexeme.Add("]"); listOfLexemeNumbers.Add(numberOfSeparator); break; }
                        
                      
                        while (povtor)
                        {
                            if (i != c.Length)
                                i++;
                            for (int k = 0; k < tableSeparator.Length; k++)
                            {
                                if (c[i] == '>' && c[i + 1] == '=')
                                { i += 3; listOfLexeme.Add(">="); listOfLexemeNumbers.Add(numberOfSeparator); }
                                if (c[i] == '<' && c[i + 1] == '=') { i += 3; listOfLexeme.Add("<="); listOfLexemeNumbers.Add(numberOfSeparator); }
                                if (c[i].Equals(tableSeparator[k]))
                                    povtor = false;
                            }
                            if (povtor) str = str + c[i];
                        }
                        listOfLexeme.Add(str);
                        listOfLexemeNumbers.Add(numberOfSeparator);
                        i--;
                    }
                }
            }
            lex = listOfLexeme.ToArray<string>();
            numb = listOfLexemeNumbers.ToArray<int>();
            return lex;
        }       
    }
}

