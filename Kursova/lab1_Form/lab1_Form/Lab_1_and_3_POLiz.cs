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
    public partial class Lab_1_and_3_POLiz : Form
    {
        public List<String> enterString = new List<String>();//входящая строка для автомата
        public List<String> enterStringForPolIZ = new List<String>();//входящая строка для полиза

        public List<String> numberOfChangedLexeme = new List<string>();//перменные и цифры которые мы заменили
        //на id i const
        public List<String> stack = new List<string>();//стэк       
        public Lab4_Relation_Table f2 = new Lab4_Relation_Table();

        public List<String> label = new List<string>();//список меток
        public List<PrioritetsTable> Priorities { get; set; }
        public bool checkForIf = false;
        public List<string> resultPOLIZ = new List<string>();
        private int labelCounter = 1;
        int commas = 0;//подсчет количества запятых в операторах рид врайт

        public void InputString()
        {
            var displayEnterString = "";//отобразить входную цепочку в датагрид
            int step = 0;//шаг
           String relation = "";//знак отношения

            var displayStack = "";//отобразить стэк в датагрид
            bool replaceSmth = false;
            stack.Add("#");
            stack.Add(enterString[0]);
            stack.Add(enterString[1]);
            stack.Add(enterString[2]);
            enterString.RemoveRange(0, 3);
            var stepCount = 0;//количество пройденных шагов
            bool repeatBeforeNotMoreThanGate = false;

            while (!repeatBeforeNotMoreThanGate)
            {
                if (stack[stack.Count - 1] == "<прогр>")
                    repeatBeforeNotMoreThanGate = true;
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
                dataGridView1.Rows.Add(step, displayStack, relation, displayEnterString);
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
                step++;
                if (!replaceSmth)
                {
                    stack.Add(enterString[0]);
                    enterString.RemoveAt(0);
                }
                displayEnterString = "";
                displayStack = "";
                replaceSmth = false;
                stepCount++;
            }
        }


        public String Compare(List<String> s, int step)//замена
        {
            for (int i = 0; i < f2.myLanguage.Length; i++)
            {
                int length = 0;
                int check = 0;
                if (f2.myLanguage[i].Length - 1 == s.Count)
                {
                    length = s.Count;
                    for (int j = 0; j < length; j++)
                    {
                        if (f2.myLanguage[i][j + 1] == s[j])
                        {
                            check++;
                        }
                    }
                    if ((check == s.Count) && ((s.Count + 1) == f2.myLanguage[i].Length))
                    {
                        return f2.myLanguage[i][0];
                    }
                }
            }
            return "Error!!";
        }


        public String Compare(String s1, String s2)//s1 - stack, s2 - enterString
        {
            int allRelationsCount = 0;
            foreach (string str in f2.relation[0])//stack
            {
                if (s1 == "#")
                {
                    return "<";
                }
                if (str == s1)
                {
                    if (s2 == f2.relation[1][allRelationsCount])//enterString
                    {
                        return f2.relation[2][allRelationsCount];
                    }
                }
                allRelationsCount++;
            }
            return "=";
        }


       
        public void AddColumn(ref int columnCount)
        {
            dataGridView2.ColumnCount++;
            DataGridViewColumn column = dataGridView2.Columns[columnCount];
            dataGridView2.Columns[columnCount].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            column = dataGridView2.Columns[columnCount];
            columnCount++;    
        }

        //главная функциия
        public List<String> AddToGrid(int enterStringCounter, int columnCount)
        {
            Boolean find = false;//якщо операнд у списку пріорітетів
            List<String> polizTemporary = new List<string>();
            List<String> poliz = new List<string>();
            int counterForChangedLexeme =0;
            bool outFromWhile = false;
            string str1 = "";
            while (!outFromWhile)
            {
                AddColumn(ref columnCount);
                if (enterStringForPolIZ[enterStringCounter] == "end")
                { outFromWhile = true; break; }
                 
                dataGridView2.Rows[2].Cells[columnCount].Value = enterStringForPolIZ[enterStringCounter];
                if(enterStringForPolIZ[enterStringCounter]=="id"||enterStringForPolIZ[enterStringCounter]=="const")
                {
                    dataGridView2.Rows[2].Cells[columnCount].Value = numberOfChangedLexeme[counterForChangedLexeme];
                }
                string str = enterStringForPolIZ[enterStringCounter];
                
                if (str == "do"||str == "enddo") str1 = "do";
                if (str == "if"||str == "else"||str == "endif") str1 = "if";
                if (str == "=") str1 ="=";
                if (str == "read")
                    str1 = "read";
                if (str == "write") str1 = "write";

                for (int i = 0; i < Priorities.Count; i++)
                {
                    if (Priorities[i].Symbol.Equals(str))
                    {
                        find = true;//нашли приоритет

                        if (str == "if")
                        {
                            checkForIf = true;
                            stack.Add(str);
                            i = Priorities.Count;
                        }
                        else
                            if (str == "do")
                            {
                                i = Priorities.Count;
                            }
                            else
                                if (str == "while")
                                {
                                    String labelName = "m" + labelCounter.ToString() + ":";
                                    labelCounter++;
                                    label.Add(labelName);
                                    stack.Add(labelName.Remove(labelName.Length-1));
                                    stack.Add(str);
                                    polizTemporary.Add(labelName);                                   
                                    i = Priorities.Count;
                                }
                                else
                                    
                                    if (stack.Count == 0)
                                    {
                                        stack.Add(str);
                                        i = Priorities.Count;//чудо выход
                                    }
                                    else
                                    {
                                        Boolean addToStack = false;//добавили ли в стек

                                        while (!addToStack)
                                        {
                                            Int32 stackPiority = 0;
                                            stackPiority = SearchStackPriority();//нашли приоритет символа в стеке

                                            if (Priorities[i].Priotity <= stackPiority)//выталкиваем символ из стека и дальше чекаем 
                                            {
                                                if (str != "(" && str != "[")
                                                {

                                        if (str == "read" || str == "write") {
                                            stack.RemoveAt(stack.Count - 1);
                                            stack.Add(str);
                                            i = Priorities.Count-1;//костылище!
                                        }
                                        else {
                                            polizTemporary.Add(stack[stack.Count - 1]);
                                            stack.RemoveAt(stack.Count - 1);
                                        }     
                                                }
                                            
                                        }

                                if (Priorities[i].Priotity > stackPiority || str == "(" || str == "[")
                                            {
                                                if (str1 == "if" || str1 == "id"||str1=="=")
                                                    AddToStackIfId(str, polizTemporary, ref i, ref addToStack);
                                                else
                                                    if (str1 == "do")
                                                        AddToStackDoWhile(str, polizTemporary, ref i, ref addToStack);
                                                    else if(str1 == "read" )
                                                    {
                                                        AddToStackRead(str, polizTemporary, ref i, ref addToStack);
                                                    }
                                                    else if (str1 == "write")
                                                        {
                                                            AddToStackWrite(str, polizTemporary, ref i, ref addToStack);
                                                        }
                                                }
                                                if (stack.Count == 0)
                                            {
                                                addToStack = true;
                                                i = Priorities.Count;
                                            }
                                        }
                                    }
                    }
                }
                if (stack.Count > 0)
                    if (stack[stack.Count - 1] == "]" || stack[stack.Count - 1] == ")")
                    {
                        stack.RemoveRange(stack.Count - 2, 2);
                       
                    }
                        
                if (polizTemporary.Count > 0)
                {
                    String printOperand = "";
                    polizTemporary.Reverse();//меняем все елемнеты местами для правильного вывода
                    for (int j = polizTemporary.Count - 1; j >= 0; j--)
                    {
                        printOperand += polizTemporary[j] + "\n";
                        poliz.Add(polizTemporary[j]);
                    }
                    dataGridView2.Rows[0].Cells[columnCount].Value = printOperand;
                }
                if (!find)
                {
                    dataGridView2.Rows[0].Cells[columnCount].Value = numberOfChangedLexeme[counterForChangedLexeme];
                    poliz.Add(numberOfChangedLexeme[counterForChangedLexeme]);
                    counterForChangedLexeme++;
                }
                String printStack = "";
                for (int i = stack.Count - 1; i >= 0; i--)
                    printStack += stack[i] + "\n";
                dataGridView2.Rows[1].Cells[columnCount].Value = printStack;
                polizTemporary.Clear();
                enterStringCounter++;
                find = false;
                if (str == "enddo")
                {
                    label.RemoveRange(label.Count - 2, 2);
                    enterStringCounter++;
                }
                if (str == "endif")
                {
                    label.RemoveRange(label.Count - 2, 2);
                    enterStringCounter++;
                }               
            }
            return poliz;
        }

        private void AddToStackRead(String str, List<String> poliz, ref int i, ref Boolean addToStack)
        {
            if (str == "read"||stack[stack.Count - 1] == "read")//костыль
                commas = 0;
            //MessageBox.Show(str);
            addToStack = true;
            if (str == ",")
            {
                ++commas;//имитациия какого-то r1
                if (commas==1)//в стеке нету r1
                {
                    if (stack[stack.Count - 1] != "read")//костыль
                        stack.Add("read");

                    stack.Add(commas.ToString());
                }
                else
                {
                    stack.RemoveAt(stack.Count - 1);
                    stack.Add(commas.ToString());
                }
            }
                
            i = Priorities.Count;
            return;
        }

        private void AddToStackWrite(String str, List<String> poliz, ref int i, ref Boolean addToStack)
        {
            if(str=="write"|| stack[stack.Count - 1] == "write")
                commas = 0;
            //MessageBox.Show(str);
            addToStack = true;
            if (str == ",")
            {
                ++commas;
                if (commas==1)
                {
                    if (stack[stack.Count - 1] != "write")//костыль
                        stack.Add("write");

                    stack.Add(commas.ToString());
                }
                else
                {
                    stack.RemoveAt(stack.Count - 1);
                    stack.Add(commas.ToString());
                }
            }
                
            i = Priorities.Count;
            return;
        }

        private void AddToStackIfId(String str, List<String> poliz, ref int i, ref Boolean addToStack)
        {
            int amount = 0;
            for (int k = 0; k < stack.Count; k++)
                if (stack[k].Contains("if"))
                    amount++;
                if (label.Count%2==0 && str == "↲" && checkForIf)
                {
                    String labelName = "m" + labelCounter.ToString();
                    labelCounter++;
                    label.Add(labelName);
                    stack.Insert(stack.Count - 1, labelName);
                    poliz.Add(labelName);
                    poliz.Add("УПЛ");
                    addToStack = true;
                    i = Priorities.Count;
                }
                else
                    if (label.Count > 0 && str == "↲")
                    {
                        addToStack = true;
                        i = Priorities.Count;
                    }
                    else
                        if (str == "else")
                        {
                            String labelName = "m" + labelCounter.ToString();
                            labelCounter++;
                            stack.Insert(stack.Count - 2, labelName);
                            poliz.Add(labelName);
                            label.Add(labelName);
                            poliz.Add("БП");
                            labelName = label[label.Count - 2] + ":";
                            poliz.Add(labelName);
                            addToStack = true;
                            i = Priorities.Count;
                            checkForIf = false;
                        }
                        else
                            if (str == "endif")
                            {
                                String labelName = label[label.Count - 1] + ":";
                                poliz.Add(labelName);
                                stack.RemoveRange(stack.Count - 3, 3);
                                addToStack = true;
                                i = Priorities.Count;
                            }
                            else
                            {
                                stack.Add(str);
                                addToStack = true;
                                i = Priorities.Count;
                            }
        }

        private void AddToStackDoWhile(String str, List<String> poliz, ref int i, ref Boolean addToStack)
        {
            if (label.Count % 2 == 1 && str == "↲")
            {
                String labelName = "m" + labelCounter.ToString();
                labelCounter++;
                label.Add(labelName);
                stack.Insert(stack.Count - 2, labelName);
                poliz.Add(labelName);
                poliz.Add("УПЛ");
                addToStack = true;
                i = Priorities.Count;
            }
            else
                if (label.Count > 0 && str == "↲")
                {
                    addToStack = true;
                    i = Priorities.Count;
                }
                else
                    if (str == "enddo")
                    {
                        String labelName = label[label.Count - 2].Remove(label[label.Count - 2].Length-1);
                        poliz.Add(labelName);
                        poliz.Add("БП");
                        labelName = label[label.Count - 1]+":";
                        poliz.Add(labelName);
                        stack.RemoveRange(stack.Count - 3, 3);
                        addToStack = true;
                        i = Priorities.Count;
                    }
                    else
                    {
                        stack.Add(str);
                        addToStack = true;
                        i = Priorities.Count;
                    }
        }
        public Int32 SearchStackPriority()
        {
            Int32 priority = 1;
            for (int j = 0; j < Priorities.Count; j++)//ищем приоритет последнего елемента в стеке
            {
                if (Priorities[j].Symbol.Equals(stack[stack.Count - 1]))
                {
                    priority = Priorities[j].Priotity;
                    return priority;
                }
            }
            return priority;
        }

        public void CreatePriorityTable()
        {
            Priorities = new List<PrioritetsTable>
            {
                  new PrioritetsTable
                {
                    Symbol="write",
                    Priotity=1              
                },
                  new PrioritetsTable
                {
                    Symbol="read",
                    Priotity=1             
                },
                new PrioritetsTable
                {
                    Symbol="(",
                    Priotity=0              
                },
                new PrioritetsTable
                {
                    Symbol="[",
                    Priotity=0              
                },
                new PrioritetsTable
                {
                    Symbol="if",
                    Priotity=0              
                },
                 new PrioritetsTable
                {
                    Symbol="do",
                    Priotity=0              
                },
                 new PrioritetsTable
                {
                    Symbol="while",
                    Priotity=0              
                },
                new PrioritetsTable
                {
                    Symbol=")",
                    Priotity=1              
                },
                new PrioritetsTable
                {
                    Symbol="↲",
                    Priotity=1              
                }, 
                new PrioritetsTable
                {
                    Symbol="]",
                    Priotity=1              
                },
                new PrioritetsTable
                {
                    Symbol="endif",
                    Priotity=1              
                },
                new PrioritetsTable
                {
                    Symbol="enddo",
                    Priotity=1              
                },
                new PrioritetsTable
                {
                    Symbol="else",
                    Priotity=1              
                },

                new PrioritetsTable
                {
                    Symbol=",",
                    Priotity=2
                },
                new PrioritetsTable
                {
                    Symbol="=",
                    Priotity=2              
                }
                , new PrioritetsTable
                {
                    Symbol="or",
                    Priotity=3              
                }
                , new PrioritetsTable
                {
                    Symbol="and",
                    Priotity=4              
                }
                , new PrioritetsTable
                {
                    Symbol="not",
                    Priotity=5   
                }
                , new PrioritetsTable
                {
                    Symbol="<",
                    Priotity=6   
                }
                , new PrioritetsTable
                {
                    Symbol="<=",
                    Priotity=6   
                }
                , new PrioritetsTable
                {
                    Symbol=">",
                    Priotity=6   
                }
                , new PrioritetsTable
                {
                    Symbol=">=",
                    Priotity=6   
                }
                , new PrioritetsTable
                {
                    Symbol="!=",
                    Priotity=6   
                }
                , new PrioritetsTable
                {
                    Symbol="==",
                    Priotity=6   
                }
                , new PrioritetsTable
                {
                    Symbol="+",
                    Priotity=7   
                }
                , new PrioritetsTable
                {
                    Symbol="-",
                    Priotity=7   
                }
                , new PrioritetsTable
                {
                    Symbol="*",
                    Priotity=8   
                }
                , new PrioritetsTable
                {
                    Symbol="/",
                    Priotity=8   
                }   
                  
            };
        }
        public Lab_1_and_3_POLiz(List<String> a, List<String> numberOfLexeme)
        {
            InitializeComponent();
            for (int i = 0; i < a.Count; i++)
            {
                enterString.Add(a[i]);
                enterStringForPolIZ.Add(a[i]);
            }
            for (int i = 0; i < numberOfLexeme.Count; i++)
                numberOfChangedLexeme.Add(numberOfLexeme[i]);
            InputString();
            CreatePriorityTable();
            int columnCount = 0;
            int enterStringCounter = 0;//найдем вхождения if
            dataGridView2.ColumnCount++;
            dataGridView2.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            DataGridViewColumn column = dataGridView2.Columns[columnCount];
            column.Width = 70;
            dataGridView2.Rows.Add("POLIZ");
            dataGridView2.Rows.Add("Stack");
            dataGridView2.Rows.Add("EnterString");
            dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            stack = new List<string>();

            var outFromWhile = false;
            while (!outFromWhile)
            {
                if (!enterStringForPolIZ[enterStringCounter].Equals("begin"))
                    enterStringCounter++;
                else
                    outFromWhile = true;
            }
            enterStringCounter += 2;
            outFromWhile = false;
            
            List<String> allPoliz = AddToGrid(enterStringCounter, columnCount);
            foreach (var s in allPoliz)
            {
                textBox1.Text += s + " ";
                resultPOLIZ.Add(s);
            }          
        }
    }
}
