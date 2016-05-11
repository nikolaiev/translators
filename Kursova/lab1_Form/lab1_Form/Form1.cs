using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab1_Form
{
    public partial class Form1 : Form
    {
        public List<String> enterString = new List<String>();
        public List<List<String>> lexemeAll = new List<List<String>>();
        public List<String> numberOfLexeme = new List<String>();
        public List<string> Ids = new List<string>();
        public List<List<string>> variables = new List<List<string>>();
        public Form1()
        {
            InitializeComponent();
            //арифметическое выражение
            //button5.Visible = false;
        }

        public void Translator(String[] str1)
        {
            String[] str = str1;
            OutFile(str);
            bool check = true;
            int count = 0;
            foreach (string ch in str)
            {
                Table tab = new Table();
                count++;
                if (tab.tableClass(ch) == false)
                {
                    MessageBox.Show("\n\nWrong symbol, row № ", count.ToString() + 1);
                    check = false;
                }

            }
            if (check)
            {
                Table tb = new Table();
                int nuberOfSeparators = 1;
                foreach (string ch in str)
                {
                    tb.tableSeparators(ch, nuberOfSeparators);
                    nuberOfSeparators++;

                }
                List<string> lexeme = new List<string>();

                List<int> previousRowCount = new List<int>();
                int em = 0;
                for (int i = 0; i < tb.lex.Length; i++)
                {
                    if (((tb.lex[i]) != "") && (tb.lex[i]) != null)
                    {
                        lexeme.Add(tb.lex[i]);
                        previousRowCount.Add(tb.numb[i]);
                        em++;
                    }
                }

                for (int i = 0; i < lexeme.Count - 1; i++)
                {
                    string tmp = lexeme[i];
                    for (int j = i + 1; j < lexeme.Count; j++)
                    {
                        if (tmp.Equals(lexeme[j]))
                        {
                            lexeme[j] = "";
                            previousRowCount[j] = 0;
                        }
                    }
                }


                List<string> lex = new List<string>();//лексеми без повтору
                List<int> row = new List<int>(); //№ рядка
                List<List<string>> listOfLexeme = new List<List<string>>(); //инициализация
                listOfLexeme.Add(new List<string>());//добавление новой строки
                listOfLexeme.Add(new List<string>());//добавление новой строки



                int sequenceNumberOfLexeme = 1;//порядковий № лексеми
                int cmd = 0;// змінна для визначення чи це число
                int numberOfConst = 1;
                int numberOfId = 1;

                for (int i = 0; i < em; i++)
                {
                    if (previousRowCount[i] != 0)
                        row.Add(previousRowCount[i]);
                }

                for (int i = 0; i < lexeme.Count; i++)
                {
                    if (lexeme[i] != "")
                    {
                        lex.Add(lexeme[i]);
                        if (int.TryParse(lex[lex.Count - 1], out  cmd) == false)
                        {
                            if (tb.tableLex(lex[lex.Count - 1]) == false)
                            {
                                dataGridView1.Rows.Add(row[lex.Count - 1], lex[lex.Count - 1].ToString(), 28, numberOfId);//add id
                                listOfLexeme[0].Add("35");//добавление столбца 
                                listOfLexeme[1].Add(lex[lex.Count - 1].ToString());//добавление столбца 
                                numberOfId++;
                            }
                            else
                            {
                                dataGridView1.Rows.Add(row[lex.Count - 1], lex[lex.Count - 1].ToString(), sequenceNumberOfLexeme);//add lexeme
                                listOfLexeme[0].Add(sequenceNumberOfLexeme.ToString());//добавление столбца 
                                listOfLexeme[1].Add(lex[lex.Count - 1].ToString());//добавление столбца 
                                sequenceNumberOfLexeme++;
                            }
                        }
                        else
                        {
                            dataGridView1.Rows.Add(row[lex.Count - 1], lex[lex.Count - 1].ToString(), 29, numberOfConst);//add const
                            listOfLexeme[0].Add("36");//добавление столбца 
                            listOfLexeme[1].Add(lex[lex.Count - 1].ToString());//добавление столбца 
                            numberOfConst++;
                        }
                    }
                }

                lex.Add("id");
                dataGridView1.Rows.Add("", lex[lex.Count - 1].ToString(), sequenceNumberOfLexeme);
                lex.Add("const");
                dataGridView1.Rows.Add("", lex[lex.Count - 1].ToString(), sequenceNumberOfLexeme + 1);

                sequenceNumberOfLexeme = 1;
                //id    
                var beforenotBegin = 0;//находим количество обьявленных id
                for (int i = 0; i < lex.Count; i++)
                {
                    if (lex[i] == "begin")
                    {
                        beforenotBegin = i;
                        i = lex.Count;
                    }
                }
                int idCount = 0;
                for (int i = 0; i < lex.Count - 3; i++)
                    if (tb.tableLex(lex[i]) == false)
                    {
                        if (Int32.TryParse(lex[i], out  cmd) == false)
                        {
                            if (i >= 1 && i <= beforenotBegin)
                            {
                                dataGridView2.Rows.Add(lex[i].ToString(), sequenceNumberOfLexeme);
                                Ids.Add(lex[i]);
                                idCount++;
                            }
                            else
                            {
                                check = false;
                            }
                        }
                        sequenceNumberOfLexeme++;
                    }
                if (check)
                {
                    sequenceNumberOfLexeme = 1;
                    //conts
                    cmd = 0;
                    for (int i = 0; i < lex.Count - 3; i++)
                        if (tb.tableLex(lex[i]) == false)
                        {
                            if (int.TryParse(lex[i], out  cmd))
                            {
                                dataGridView3.Rows.Add(lex[i].ToString(), sequenceNumberOfLexeme);
                                sequenceNumberOfLexeme++;
                            }
                        }
                }
                //-----------------------------------------------------Перевірка лексики--------------------------------------------------//

                lexemeAll.Add(new List<string>());//добавление № лексеми
                lexemeAll.Add(new List<string>());//добавление лексеми
                lexemeAll.Add(new List<string>());//добавление № рядка     
                for (int i = 0; i < tb.lex.Length; i++)
                {
                    if (((tb.lex[i]) != "") && (tb.lex[i]) != null)
                    {
                        lexemeAll[1].Add(tb.lex[i]);
                        lexemeAll[2].Add(tb.numb[i].ToString());
                        for (int j = 0; j < listOfLexeme[1].Count; j++)
                        {
                            if (tb.lex[i].Equals(listOfLexeme[1][j]))
                            {
                                lexemeAll[0].Add(listOfLexeme[0][j]);
                            }
                        }
                    }
                }


                foreach (string ones in lexemeAll[1])
                {
                    if (tb.tableLex(ones))
                    {
                        enterString.Add(ones);
                    }
                    else
                    {
                        if (int.TryParse(ones, out cmd))
                        {
                            enterString.Add("const");
                            numberOfLexeme.Add(ones);
                        }
                        else
                        {
                            enterString.Add("id");
                            numberOfLexeme.Add(ones);
                        }
                    }
                }
                enterString.Add("#");
                for (int i = 0; i < idCount; i++)
                    numberOfLexeme.RemoveAt(0);
                Lab3_automat f4 = new Lab3_automat(enterString, lexemeAll);
                if (!f4.err)
                {
                    MessageBox.Show("All lexic is correct!!!");
                    Lab_1_and_3_POLiz form = new Lab_1_and_3_POLiz(enterString, numberOfLexeme);
                    List<string> resultPoliz = form.resultPOLIZ;
                    //TestPolizInterpretator(resultPoliz);
                    try
                    {


                        PolizInterpretator(resultPoliz);
                    }
                    catch(Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }
                }
                else
                {
                    dataGridView1.Rows.Clear();
                    dataGridView2.Rows.Clear();
                    dataGridView3.Rows.Clear();
                    button1.Visible = false;
                    button2.Visible = false;
                    //button3.Visible = false;

                }
            }  
        }

     
        private void PolizInterpretator(List<string> poliz)
        {
            /*String result = "";
            foreach(String token in poliz)
            {
                result +=" "+ token;
            }
            textBox2.Text = result;*/
            
            List<string> signs = new List<string> { "+", "-", "*", "/", "=", ">", "<", ">=", "<=", "!=", "==", "and", "not", "or", "read", "write" };
            List<string> copyForDoWhile = new List<string>();//цикл 
            variables.Add(new List<string>());
            variables.Add(new List<string>());
            string thirdValue = "";
            List<List<string>> labels = new List<List<string>>();//метки
            labels.Add(new List<string>());
            labels.Add(new List<string>());
            for (int i = 0; i < poliz.Count; i++)//проверка меток
            {
                copyForDoWhile.Add(poliz[i]);
                string str = poliz[i];
                if (str[str.Length - 1].Equals(':'))
                {
                    labels[0].Add(str);
                    labels[1].Add(i.ToString());
                }
            }
            for (int i = 1; i < poliz.Count; i++)
            {
                thirdValue = poliz[i];
                if (poliz[i].Equals("БП"))
                {
                    string mark = poliz[i - 1] + ":";
                    for (int j = 0; j < labels[0].Count; j++)
                        if (labels[0][j].Equals(mark))
                        {
                            i = int.Parse(labels[1][j]);
                            break;
                        }
                    poliz = new List<string>();
                    for (int k = 0; k < copyForDoWhile.Count; k++)
                    {
                        poliz.Add(copyForDoWhile[k]);
                    }
                }

                if (signs.Contains(thirdValue))
                {
                    string secondValue;
                    string firstValue;

                    secondValue = poliz[i - 1];
                    firstValue = poliz[i - 2];

                    if (thirdValue == "=")
                    {
                        if (!variables[0].Contains(firstValue))
                        {
                            variables[0].Add(firstValue);
                            variables[1].Add(secondValue);
                        }
                        else
                            for (int j = 0; j < variables[0].Count; j++)
                                if (variables[0][j].Equals(firstValue))
                                {
                                    int b;
                                    if (Int32.TryParse(secondValue, out b))// и b число
                                    {
                                        variables[1][j] = b.ToString();
                                    }
                                    else
                                    {
                                        for (int k = 0; k < variables[0].Count; k++)
                                            if (variables[0][k].Equals(secondValue))
                                            {
                                                variables[1][j] = variables[1][k];
                                                break;
                                            }
                                    }
                                    break;
                                }
                    }
                    else
                    {
                        string caseSwitch = thirdValue;
                        switch (caseSwitch)
                        {
                            case "+":
                                poliz[i] = Ariphmetic(firstValue, secondValue, "+").ToString();

                                poliz.RemoveRange(i - 2, 2);
                                i -= 2;
                                break;
                            case "-":
                                poliz[i] = Ariphmetic(firstValue, secondValue, "-").ToString();

                                poliz.RemoveRange(i - 2, 2);
                                i -= 2;
                                break;
                            case "*":
                                poliz[i] = Ariphmetic(firstValue, secondValue, "*").ToString();

                                poliz.RemoveRange(i - 2, 2);
                                i -= 2;
                                break;
                            case "/":
                                poliz[i] = Ariphmetic(firstValue, secondValue, "/").ToString();
                                poliz.RemoveRange(i - 2, 2);
                                i -= 2;
                                break;
                            case "<":
                                if (Comparator(firstValue, secondValue, "<"))
                                {
                                    i += 2;
                                    break;
                                }
                                else
                                {
                                    string mark = poliz[i + 1] + ":";
                                    for (int j = i; j < poliz.Count; j++)
                                        if (poliz[j].Equals(mark))
                                        {
                                            i = j;
                                            break;
                                        }
                                }
                                break;
                            case "<=":
                                if (Comparator(firstValue, secondValue, "<="))
                                {
                                    i += 2;

                                    break;
                                }
                                else
                                {
                                    string mark = poliz[i + 1] + ":";
                                    for (int j = i; j < poliz.Count; j++)
                                        if (poliz[j].Equals(mark))
                                        {
                                            i = j;

                                            break;
                                        }
                                }
                                break;
                            case ">":
                                if (Comparator(firstValue, secondValue, ">"))
                                {
                                    i += 2;
                                    break;
                                }
                                else
                                {
                                    string mark = poliz[i + 1] + ":";
                                    for (int j = i; j < poliz.Count; j++)
                                        if (poliz[j].Equals(mark))
                                        {
                                            i = j;
                                            break;
                                        }
                                }
                                break;
                            case ">=":

                                if (Comparator(firstValue, secondValue, ">="))
                                {
                                    i += 2;

                                    break;
                                }
                                else
                                {
                                    string mark = poliz[i + 1] + ":";
                                    for (int j = i; j < poliz.Count; j++)
                                        if (poliz[j].Equals(mark))
                                        {
                                            i = j;

                                            break;
                                        }
                                }
                                break;
                            case "!=":

                                if (Comparator(firstValue, secondValue, "!="))
                                {
                                    i += 2;
                                    break;
                                }
                                else
                                {
                                    string mark = poliz[i + 1] + ":";
                                    for (int j = i; j < poliz.Count; j++)
                                        if (poliz[j].Equals(mark))
                                        {
                                            i = j;
                                            break;
                                        }
                                }
                                break;
                            case "==":
                                if (Comparator(firstValue, secondValue, "=="))
                                {
                                    i += 2;
                                    break;
                                }
                                else
                                {
                                    string mark = poliz[i + 1] + ":";
                                    for (int j = i; j < poliz.Count; j++)
                                        if (poliz[j].Equals(mark))
                                        {
                                            i = j;
                                            break;
                                        }
                                }
                                break;
                            case "write":
                                int counterWrite = Int32.Parse(poliz[i - 1]);
                                for (int k = counterWrite; k >0; k--)
                                {
                                    for (int j = 0; j < variables[0].Count; j++)
                                        if (variables[0][j] == poliz[i - 1 - k])//1 place for counter
                                        {
                                            textBox2.Text += variables[0][j] + " = " + variables[1][j] + "\r\n";
                                            //break;
                                        }
                                }
                                break;
                            case "read":
                                int counterRead = Int32.Parse(poliz[i - 1]);

                                for (var k = counterRead; k > 0; k--)
                                {
                                    FormEnterVules enterForm = new FormEnterVules(poliz[i - 1-k]);
                                    enterForm.ShowDialog();
                                    bool check = true;
                                    if (check)
                                    {
                                        var str = enterForm.result.ToString();
                                        for (int j = 0; j < variables[0].Count; j++)
                                            if (variables[0][j].Equals(poliz[i - 1-k]))
                                            {

                                                variables[1][j] = str;
                                                check = false;
                                                break;
                                            }
                                        if (check)
                                        {
                                            //если прежде не было такой переменной
                                            variables[0].Add(poliz[i - 1 - k]);
                                            variables[1].Add(str);
                                        }


                                    }
                                }
                                
                                break;
                            default:
                                Console.WriteLine("Default case");
                                break;
                        }
                    }
                }
            }
        }

        private int ValueFinder(string str)
        {

            int res = -1;
            try
            {
                for (int i = 0; i < variables[0].Count; i++)
                {
                    if (variables[0][i].Equals(str))
                    {
                        res = i;
                        return res;
                    }
                }
                if (res == -1)
                    throw new Exception("Undefined variable exception!");
                return res;
            }
            catch(Exception exc){
                //MessageBox.Show("Undefined variable " + str);
                throw exc;
                //return res;
                
            }
            
        }

        private int Ariphmetic(string firstValue, string secondValue, string sign)
        {
            int res = 0;
            int a = 0;
            int b = 0;
            if (Int32.TryParse(firstValue, out a))//если а число
            {
                if (Int32.TryParse(secondValue, out b))// и b число
                {
                    switch (sign)
                    {
                        case "+":
                            return a + b;
                        case "-":
                            return a - b;
                        case "*":
                            return a * b;
                        case "/":
                            return a / b;
                    }
                }
                else
                {
                    try {
                        switch (sign)//а число - b нет   
                        {
                            case "+":
                                return a + Int32.Parse(variables[1][ValueFinder(secondValue)]);

                            case "-":
                                return a - Int32.Parse(variables[1][ValueFinder(secondValue)]);

                            case "*":
                                return a * Int32.Parse(variables[1][ValueFinder(secondValue)]);

                            case "/":
                                return a / Int32.Parse(variables[1][ValueFinder(secondValue)]);

                        }
                    }
                    catch(Exception e)
                    {
                        throw e;
                    }

                }
            }
            else
                if (!Int32.TryParse(secondValue, out b))//а не число и б не число
                {
                try {
                    switch (sign)
                    {
                        case "+":
                            return Int32.Parse(variables[1][ValueFinder(firstValue)]) + Int32.Parse(variables[1][ValueFinder(secondValue)]);

                        case "-":
                            return Int32.Parse(variables[1][ValueFinder(firstValue)]) - Int32.Parse(variables[1][ValueFinder(secondValue)]);

                        case "*":
                            return Int32.Parse(variables[1][ValueFinder(firstValue)]) * Int32.Parse(variables[1][ValueFinder(secondValue)]);
                        case "/":
                            return Int32.Parse(variables[1][ValueFinder(firstValue)]) / Int32.Parse(variables[1][ValueFinder(secondValue)]);

                    }
                }
                catch(Exception e)
                {
                    throw e;
                }
                }
                else
                    if (Int32.TryParse(secondValue, out b))//а не число  б число
                    {
                try {
                    switch (sign)
                    {
                        case "+":
                            return Int32.Parse(variables[1][ValueFinder(firstValue)]) + b;
                        case "-":
                            return Int32.Parse(variables[1][ValueFinder(firstValue)]) - b;
                        case "*":
                            return Int32.Parse(variables[1][ValueFinder(firstValue)]) * b;
                        case "/":
                            return Int32.Parse(variables[1][ValueFinder(firstValue)]) / b;
                    }
                }
                catch(Exception e)
                {
                    throw e;
                }
                    }
            return res;
        }

        private bool Comparator(string firstValue, string secondValue, string connotation)//connotation знак отношения
        {
            bool res = false;
            int a = 0;
            int b = 0;
            if (Int32.TryParse(firstValue, out a))//если а число
            {
                if (Int32.TryParse(secondValue, out b))// и b число
                {
                    switch (connotation)
                    {
                        case "<":
                            return a < b;
                        case "<=":
                            return a <= b;
                        case ">":
                            return a > b;
                        case ">=":
                            return a >= b;
                        case "!=":
                            return a != b;
                        case "==":
                            return a == b;
                    }
                }
                else
                {
                    try {
                        switch (connotation)//а число - b нет   
                        {
                            case "<":
                                return a < Int32.Parse(variables[1][ValueFinder(secondValue)]);

                            case "<=":
                                return a <= Int32.Parse(variables[1][ValueFinder(secondValue)]);

                            case ">":
                                return a > Int32.Parse(variables[1][ValueFinder(secondValue)]);

                            case ">=":
                                return a >= Int32.Parse(variables[1][ValueFinder(secondValue)]);

                            case "!=":
                                return a != Int32.Parse(variables[1][ValueFinder(secondValue)]);

                            case "==":
                                return a == Int32.Parse(variables[1][ValueFinder(secondValue)]);

                        }
                    }
                    catch(Exception e)
                    {
                        throw e;
                    }

                }
            }
            else
                if (!Int32.TryParse(secondValue, out b))//а не число и б не число
                {
                try {
                    switch (connotation)
                    {
                        case "<":
                            return Int32.Parse(variables[1][ValueFinder(firstValue)]) < Int32.Parse(variables[1][ValueFinder(secondValue)]);

                        case "<=":
                            return Int32.Parse(variables[1][ValueFinder(firstValue)]) <= Int32.Parse(variables[1][ValueFinder(secondValue)]);

                        case ">":
                            return Int32.Parse(variables[1][ValueFinder(firstValue)]) > Int32.Parse(variables[1][ValueFinder(secondValue)]);

                        case ">=":
                            return Int32.Parse(variables[1][ValueFinder(firstValue)]) >= Int32.Parse(variables[1][ValueFinder(secondValue)]);

                        case "!=":
                            return Int32.Parse(variables[1][ValueFinder(firstValue)]) != Int32.Parse(variables[1][ValueFinder(secondValue)]);

                        case "==":
                            return Int32.Parse(variables[1][ValueFinder(firstValue)]) == Int32.Parse(variables[1][ValueFinder(secondValue)]);

                    }
                }
                catch(Exception e)
                {
                    throw e;
                }

                }
                else
                    if (Int32.TryParse(secondValue, out b))//а не число  б число
                    {
                try {
                    switch (connotation)
                    {
                        case "<":
                            return Int32.Parse(variables[1][ValueFinder(firstValue)]) < b;
                        case "<=":
                            return Int32.Parse(variables[1][ValueFinder(firstValue)]) <= b;
                        case ">":
                            return Int32.Parse(variables[1][ValueFinder(firstValue)]) > b;
                        case ">=":
                            return Int32.Parse(variables[1][ValueFinder(firstValue)]) >= b;
                        case "!=":
                            return Int32.Parse(variables[1][ValueFinder(firstValue)]) != b;
                        case "==":
                            return Int32.Parse(variables[1][ValueFinder(firstValue)]) == b;
                    }
                }
                catch(Exception e)
                {
                    throw e;
                }
                    }
            return res;
        }

        public void OutFile(string[] str)
        {
            String text = "";
            for (int i = 0; i < str.Length; i++)
            {
                text += (i + 1) + "  " + str[i] + "\r\n";
                str[i] = " " + str[i] + '↲';
            }
            textBox1.AppendText(text);
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Lab4_Relation_Table f = new Lab4_Relation_Table();
            f.WindowState = FormWindowState.Maximized;
            f.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Lab_1_and_3_POLiz form = new Lab_1_and_3_POLiz(enterString, numberOfLexeme);
            form.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Lab3_automat f4 = new Lab3_automat(enterString, lexemeAll);
            if (!f4.err)
                f4.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Displays an OpenFileDialog so the user can select a Cursor.
            textBox1.Clear();
            textBox2.Clear();
            dataGridView1.Rows.Clear();
            dataGridView3.Rows.Clear();
            dataGridView2.Rows.Clear();
            numberOfLexeme.Clear();
            variables = new List<List<string>>();
            lexemeAll = new List<List<string>>();
            enterString = new List<string>();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Text files|*.txt";
            openFileDialog1.Title = "Select a text File";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                button1.Visible = true;
                button2.Visible = true;
                //button3.Visible = true;
                StreamReader streamReader = new StreamReader(filename);
                string[] str = new string[100];
                int i = 0;
                while (!streamReader.EndOfStream)
                {
                    str[i] += streamReader.ReadLine();
                    i++;
                }
                streamReader.Close();
                string[] strTrue = new string[i];
                for (int j = 0; j < i; j++)
                    strTrue[j] = str[j];
                Translator(strTrue);
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Lab2_ariphmetic f = new Lab2_ariphmetic();
            f.Show();
        }
    }
}
