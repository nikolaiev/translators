using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab1_Form
{
    public partial class Lab2_ariphmetic : Form
    {
        public String[][] myLanguage = new String[11][];
        public List<List<string>> relation = new List<List<string>>();

        public List<String> enterString = new List<String>();//входящая строка      
        public String[] lex = {"E", "T1","T","F", "i","(",
                                ")","+","-","*","/","E1","#"
                            };
        public List<String> stack = new List<string>();//стэк 
        public String timingID = "";
        public List<String> poliz = new List<string>();
        public List<String> calculate = new List<string>();
        public List<List<String>> variables = new List<List<String>>();
      
        public Boolean syntaxError = false;
        public List<String> constList = new List<string>();
        public List<Int32> stackForCalculate = new List<int>();
        public void Alltable()
        {
            dataGridView1.Rows.Add();
            dataGridView1.Rows[0].Cells[0].Value = "";

            for (var i = 1; i < lex.Length + 1; i++)
                dataGridView1.Rows[0].Cells[i].Value = lex[i - 1];


            for (var i = 1; i < lex.Length + 1; i++)
                dataGridView1.Rows.Add();

            for (var i = 1; i < lex.Length + 1; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = lex[i - 1];
            }

            for (var i = 1; i < lex.Length + 1; i++)
            {
                dataGridView1.Rows[i].Cells[lex.Length].Value = ">";
                dataGridView1.Rows[lex.Length].Cells[i].Value = "<";
            }
            dataGridView1.Rows[0].Frozen = true;

        }

        public String[] LastPlus(String s, String[][] gr)
        {
            var a = new String[200]; var l = 0;
            for (var i = 0; i < gr.Length; i++)
            {

                if (gr[i][0] == s)
                {
                    a[l] = gr[i][gr[i].Length - 1];
                    l++;
                    if (gr[i][gr[i].Length - 1] != gr[i][0])
                    {
                        var a2 = LastPlus(gr[i][gr[i].Length - 1], gr); var ll = 0;
                        do
                        {
                            a[l] = a2[ll];
                            l++;
                            ll++;
                        } while (a2[ll] != null);
                    }
                }
            }
            return a;
        }

        public String[] FirstPlus(String s, String[][] gr)
        {
            var a = new String[200]; var l = 0;
            for (var i = 0; i < gr.Length; i++)
            {
                if (gr[i][0] == s)
                {
                    a[l] = gr[i][1];
                    l++;
                    if ((gr[i][1] != gr[i][0]))
                    {
                        var a2 = FirstPlus(gr[i][1], gr); var ll = 0;
                        do
                        {
                            a[l] = a2[ll];
                            l++;
                            ll++;
                        } while (a2[ll] != null);
                    }
                }
            }
            return a;
        }

        public void Bigger(String[][] gr)
        {
            for (var i = 0; i < gr.Length; i++)
                for (var j = 1; j < gr[i].Length - 1; j++)
                {
                    var lp = LastPlus(gr[i][j], gr);
                    {
                        for (var ii = 0; ii < lp.Length; ii++)
                            FillTable(lp[ii], gr[i][j + 1], ">");
                    }
                }
        }


        public void FillTable(String s1, String s2, String z)
        {
            for (var i = 0; i < dataGridView1.RowCount - 1; i++)
                if (dataGridView1.Rows[i].Cells[0].Value.ToString() == s1)
                {

                    for (var j = 0; j < dataGridView1.RowCount - 1; j++)
                        if (dataGridView1.Rows[0].Cells[j].Value.ToString() == s2)
                        {
                            dataGridView1.Rows[i].Cells[j].Value = z;
                        }
                }
        }

        public void Kostil()
        {
            dataGridView1.Rows[6].Cells[5].Value = "<";
            dataGridView1.Rows[8].Cells[5].Value = "<";
            dataGridView1.Rows[9].Cells[5].Value = "<";
            dataGridView1.Rows[5].Cells[7].Value = ">";
            dataGridView1.Rows[5].Cells[8].Value = ">";
            dataGridView1.Rows[5].Cells[9].Value = ">";
            dataGridView1.Rows[5].Cells[10].Value = ">";
            dataGridView1.Rows[5].Cells[11].Value = ">";
        }
        public void Smaller(String[][] gr)
        {
            for (var i = 0; i < gr.Length; i++)
            {
                for (var j = 1; j < gr[i].Length - 1; j++)
                {
                    var fp = FirstPlus(gr[i][j + 1], gr);
                    for (var ii = 0; ii < fp.Length; ii++)
                        FillTable(gr[i][j], fp[ii], "<");
                }
            }
        }

        public void Eql(String[][] gr)
        {
            for (var i = 0; i < gr.Length; i++)
            {
                for (var j = 1; j < gr[i].Length - 1; j++)
                    FillTable(gr[i][j], gr[i][j + 1], "=");
            }
        }


        public void FillRelation()
        {
            for (int i = 1; i < dataGridView1.ColumnCount; i++)
            {
                for (int j = 1; j < dataGridView1.ColumnCount; j++)
                {
                    if (dataGridView1.Rows[i].Cells[j].Value != null)
                    {
                        String str1 = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        String str2 = dataGridView1.Rows[0].Cells[j].Value.ToString();
                        String str3 = dataGridView1.Rows[i].Cells[j].Value.ToString();
                        relation[0].Add(str1);
                        relation[1].Add(str2);
                        relation[2].Add(str3);
                    }
                }
            }
            dataGridView1.Rows[0].Cells[0].Value = relation[0].Count.ToString();
        }


        public void FillMyLanguage()
        {
            relation.Add(new List<String>());
            relation.Add(new List<String>());
            relation.Add(new List<String>());
            myLanguage[0] = new[] { "E1", "E" };
            myLanguage[1] = new[] { "E", "E", "+", "T1" };
            myLanguage[2] = new[] { "E", "E", "-", "T1" };
            myLanguage[3] = new[] { "E", "T1" };
            myLanguage[4] = new[] { "E", "-", "T1" };
            myLanguage[5] = new[] { "T1", "T" };
            myLanguage[6] = new[] { "T", "T", "*", "F" };
            myLanguage[7] = new[] { "T", "T", "/", "F" };
            myLanguage[8] = new[] { "T", "F" };
            myLanguage[9] = new[] { "F", "(", "E1", ")" };
            myLanguage[10] = new[] { "F", "i" };
        }

        public void Poliz()
        {
            var displayEnterString = "";//отобразить входную цепочку в датагрид
            int step = 0;//шаг
            String relation = "";//знак отношения

            var displayStack = "";//отобразить стэк в датагрид
            bool replaceSmth = false;
            stack.Add("#");
            var stepCount = 0;//количество пройденных шагов
            bool repeatBeforeNotMoreThanGate = false;
            while (!repeatBeforeNotMoreThanGate)
            {
                
                foreach (string str in enterString)
                {
                    displayEnterString += str + " ";
                }
                foreach (string str in stack)
                {
                    displayStack += str + " ";
                }

                int lastIndexOfStack = stack.Count - 1;
                if (enterString.Count == 0)
                    break;
                relation = Compare(stack[lastIndexOfStack], enterString[0]);

                if (relation == ">")//ищем на что заменить
                {
                    replaceSmth = true;
                    String timeRelation = "=";
                    List<String> timeReplace = new List<string>();
                    int coun = 0;
                    while (timeRelation != "<")
                    {
                        String compareRight = stack[lastIndexOfStack];
                        String compareLeft = stack[lastIndexOfStack - 1];
                        timeReplace.Add(compareRight);
                        timeRelation = Compare(compareLeft, compareRight);
                        lastIndexOfStack--;
                        coun++;
                    }
                    timeReplace.Reverse();//если нашли переворачиваем для нужного порядка
                    stack.RemoveRange(lastIndexOfStack + 1, coun);
                    stack.Add(Compare(timeReplace, step));//заменяем
                }
                
                if (!replaceSmth)
                {
                    stack.Add(enterString[0]);
                    enterString.RemoveAt(0);
                }
                dataGridView2.Rows.Add(step, displayStack, relation, displayEnterString);
                step++;
                displayEnterString = "";
                displayStack = "";
                replaceSmth = false;
                stepCount++;
                if (stack[stack.Count - 1] == "E1" && enterString[0] == "#")
                {
                    dataGridView2.Rows.Add(step, "#E1", "", "#");
                    repeatBeforeNotMoreThanGate = true;
                }
            }



        }

        public String[] operations = { "+", "-", "*", "/", "@" };
        public void PrepareToCalculate(String str)
        {
            if (!operations.Contains(str))
            {
                stack.Add(str);
            }
            else
            {               
                if (str == "+")
                {
                    //op = "(";                    
                    //op += stack[stack.Count - 2];                    
                    //op += str;
                    //op += stack[stack.Count - 1];                   
                    //op += ")";
                    int num1 = ValueFinder(stack[stack.Count - 2]);
                    int num2 = ValueFinder(stack[stack.Count - 1]);
                    //if (num1 != result && num2 != result)
                    //    result += num1 + num2;
                    //else
                    //    result = num1 + num2;                   
                    stack.RemoveRange(stack.Count-2, 2);
                    stack.Add((num1 + num2).ToString());
                    //stack.RemoveRange(stack.Count - 2, 2);
                   
                   
                }
                if (str == "-")
                {
                    //op = "(";
                    //op += stack[stack.Count - 2];
                    //op += str;
                    //op += stack[stack.Count - 1];
                    //op += ")";
                    int num1 = ValueFinder(stack[stack.Count - 2]);
                    int num2 = ValueFinder(stack[stack.Count - 1]);
                    //if (num1 != result && num2 != result)
                    //    result += num1 - num2;
                    //else
                    //    result = num1 - num2;
                   
                    //stack.RemoveRange(stack.Count - 2, 2);
                    stack.RemoveRange(stack.Count - 2, 2);
                    stack.Add((num1 - num2).ToString());

                }
                if (str == "@")
                {
                    //op = "-";
                    //op += stack[stack.Count - 1];
                    int num1 = ValueFinder(stack[stack.Count - 1]);                   
                    //result += - num1 ;
                    //stack.RemoveAt(stack.Count - 1);

                    stack.RemoveAt(stack.Count - 1);
                    stack.Add((-num1).ToString());
                }
                if(str=="*")
                {
                    //op += stack[stack.Count - 2];
                    //op += str;
                    //op += stack[stack.Count - 1];
                    int num1 = ValueFinder(stack[stack.Count - 2]);
                    int num2 = ValueFinder(stack[stack.Count - 1]);
                    //if (num1 != result && num2 != result)
                    //    result += num1 * num2;
                    //else
                    //    result = num1 * num2;
                    //stack.RemoveRange(stack.Count - 2, 2);
                   
                    stack.RemoveRange(stack.Count - 2, 2);
                    stack.Add((num1 * num2).ToString());
                }
                if (str == "/")
                {
                    //op += stack[stack.Count - 2];
                    //op += str;
                    //op += stack[stack.Count - 1];
                    int num1 = ValueFinder(stack[stack.Count - 2]);
                    int num2 = ValueFinder(stack[stack.Count - 1]);
                    //if (num1 != result && num2 != result)
                    //    result += num1 / num2;
                    //else
                    //    result = num1 / num2;
                    //stack.RemoveRange(stack.Count - 2, 2);
                   
                    stack.RemoveRange(stack.Count - 2, 2);
                    stack.Add((num1 / num2).ToString());
                }
                //stack.Add(op);
            }
            enterString.RemoveAt(0);
        }

        public int ValueFinder(String op)
        {
            int res = 0;
            //bool find = false;
            //    for(int j=0;j<variables[0].Count;j++)
            //    if(op.Equals(variables[0][j]))
            //    {
            //        res = Int32.Parse(variables[1][j]);
            //        find = true;
            //    }
           
                int r = 0;
                if (Int32.TryParse(op, out  r))
                {
                    return r;
                }
                return res;
            
            

       }
        public String Compare(List<String> s, int step)//замена
        {
            for (int i = 0; i < myLanguage.Length; i++)
            {
                int length = 0;
                int check = 0;
                if (myLanguage[i].Length - 1 == s.Count)
                {
                    length = s.Count;
                    for (int j = 0; j < length; j++)
                    {
                        if (myLanguage[i][j + 1] == s[j])
                        {
                            check++;
                        }
                    }
                    if ((check == s.Count) && ((s.Count + 1) == myLanguage[i].Length))
                    {
                        Rules(myLanguage[i]);
                        return myLanguage[i][0];
                    }
                }
            }
            String str="";
            foreach(String s1 in s )
                str+=s1;
            MessageBox.Show("Oops, i can't transform it "+str);
            syntaxError = true;
            return "Error!!";
        }

        public String PrintPoliz()
        {
            String str = "";
            foreach (String s in poliz)
            {
                str += s;
            }
            return str;
        }
        public void Rules(String[] str)
        {

            foreach (String s in str)
            {
                if (s.Equals("+"))
                {
                    poliz.Add("+");
                    dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[4].Value = PrintPoliz();
                }
                if (s.Equals("-"))
                {
                    if (str[0] == "E" && str[1] == "E")
                        poliz.Add("-");
                    else
                        poliz.Add("@");
                    dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[4].Value = PrintPoliz();
                }
                if (s.Equals("*"))
                {
                    poliz.Add("*");
                    dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[4].Value = PrintPoliz();
                }
                if (s.Equals("/"))
                {
                    poliz.Add("/");
                    dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[4].Value = PrintPoliz();
                }
                if (s.Equals("i"))
                {
                    poliz.Add(timingID);
                    dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[4].Value = PrintPoliz();
                }
            }
        }

        public String Compare(String s1, String s2)//s1 - stack, s2 - enterString
        {
            int allRelationsCount = 0;
            if (!lex.Contains(s2))
            {
                timingID = s2;
                s2 = "i";
                enterString[0] = "i";
            }
            if (!lex.Contains(s1))
            {
                timingID = s2;
                s1 = "i";
                stack[stack.Count - 1] = "i";
                poliz.Add(s1);
            }
            foreach (string str in relation[0])//stack
            {
                if (s1 == "#")
                {
                    return "<";
                }
                if (str == s1)
                {
                    if (s2 == relation[1][allRelationsCount])//enterString
                    {
                        return relation[2][allRelationsCount];
                    }
                }
                allRelationsCount++;
            }
            return "=";
        }

        public void AddToEnterString(String str)
        {
            constList = new List<string>();
            foreach (char s in str)
            {
                if (s != ' ')
                {
                    enterString.Add(s.ToString());
                }
            }

          
                foreach (String s in enterString)
                {
                    if (!lex.Contains(s))
                    {
                        int res;
                        if (!Int32.TryParse(s, out res))
                        {
                            if (!constList.Contains(s))
                            {
                                constList.Add(s);
                                dataGridView3.Rows.Add(s);
                            }
                        }

                    }
                }
        }

        public Lab2_ariphmetic()
        {
            InitializeComponent();
            FillMyLanguage();
            Alltable();
            Eql(myLanguage);
            Bigger(myLanguage);
            Smaller(myLanguage);
            Kostil();
            FillRelation();
            textBox1.Text = "a*(-b+c)";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            syntaxError = false;
            textBox2.Text = "";
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            String str = textBox1.Text;
            str += "#";
            AddToEnterString(str);
            Poliz();
            if (syntaxError)
                button2.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            if (dataGridView3.Rows[dataGridView3.Rows.Count-1].Cells[1].Value!=null)
            {
                Int32 stepCount = 0;
                dataGridView2.Rows.Add();
                stack = new List<string>();
                enterString = new List<string>();
                enterString = poliz;
                String displayStack = "";
                String displayEnterString = "";
                Boolean repeatBeforeNotMoreThanGate = false;
 
                variables.Add(new List<string>());
                variables.Add(new List<string>());
                for (int i = 0; i < dataGridView3.Rows.Count; i++)
                {
                    variables[0].Add(dataGridView3.Rows[i].Cells[0].Value.ToString());
                    variables[1].Add(dataGridView3.Rows[i].Cells[1].Value.ToString());
                }
                for (int i = 0; i < enterString.Count; i++)
                {
                    for (int j = 0; j < dataGridView3.Rows.Count; j++)
                        if (dataGridView3.Rows[j].Cells[0].Value.Equals(enterString[i]))
                            enterString[i] = dataGridView3.Rows[j].Cells[1].Value.ToString();

                }
                if (enterString[0] == "#")
                    enterString.RemoveAt(0);
                while (!repeatBeforeNotMoreThanGate)
                {
                    PrepareToCalculate(enterString[0]);
                    foreach (string str in stack)
                    {
                        displayStack += str + " ";
                    }
                    foreach (string str in enterString)
                    {
                        displayEnterString += str + " ";
                    }
                    dataGridView2.Rows.Add(stepCount, displayStack, "", displayEnterString);
                    displayEnterString = "";
                    displayStack = "";
                    stepCount++;

                    if (enterString.Count == 0)
                        repeatBeforeNotMoreThanGate = true;
                }
                textBox2.Text = stack[0].ToString();
                stack = new List<string>();
            }
            else
                MessageBox.Show("Enter values for variables");
        }
    }
}
