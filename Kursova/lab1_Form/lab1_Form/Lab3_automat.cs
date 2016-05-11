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
    public partial class Lab3_automat : Form
    {
        public List<String> enterString = new List<String>();//входящая строка
        public List<List<String>> lexeme = new List<List<String>>();//список всех лексем
        public List<String> stack = new List<String>();
        public List<AutomateRow> Automate { get; set; }

        public bool err = false;

        public int GetCurrentIndex(int alpha)
        {
            if (alpha == -1)
            {
                var ret = 0;
                ret = Int32.Parse(stack[stack.Count - 1]);
                stack.RemoveAt(stack.Count - 1);
                for (int j = 0; j < Automate.Count; j++)
                {
                    if (Automate[j].Alpha == ret)
                    {
                        return j;
                    }

                }
            }
            for (int j = 0; j < Automate.Count; j++)
            {
                if (Automate[j].Alpha == alpha)
                {
                    return j;
                }

            }
            return 777;
        }



        public void AutomateInUse()
        {
            int nextMarker = 0;
            int counterForEnterString = 0;
            int counterForCycle = 0;
            stack.Add("");
            String stackOut = "";

            while (counterForEnterString != enterString.Count - 1 )
            {
                stackOut = "";
                if (counterForCycle > 100)
                {
                    err = true;
                    break;
                }
                counterForCycle++;                
                if (counterForEnterString == 52)
                { }
                for (var i = 0; i < stack.Count; i++)
                    stackOut += " " + stack[i];
                var counterForRepeatedElments = nextMarker;
                var repeat = 0;
                if (nextMarker < Automate.Count - 1)
                    for (int i = counterForRepeatedElments; i < counterForRepeatedElments + 1; i++)
                    {
                        if (Automate[counterForRepeatedElments].Alpha == Automate[counterForRepeatedElments + 1].Alpha)
                        {
                            counterForRepeatedElments++;
                            repeat++;
                        }
                    }
                for (int ii = 0; ii <= repeat; ii++)
                {                   
                    if (enterString[counterForEnterString] == Automate[nextMarker + ii].Label)
                    {
                        dataGridView1.Rows.Add(Automate[nextMarker + ii].Alpha, Automate[nextMarker + ii].Label,
                            Automate[nextMarker + ii].Beta, Automate[nextMarker + ii].StackInput, stackOut);
                        if (Automate[nextMarker + ii].StackInput != null)
                        {
                            stack.Add(Automate[nextMarker + ii].StackInput.ToString());
                        }
                        nextMarker = GetCurrentIndex(Int32.Parse(Automate[nextMarker + ii].Beta.ToString()));
                        if (nextMarker == -1)
                        {
                            nextMarker = Int32.Parse(stack[stack.Count - 1]);
                        }
                        counterForEnterString++;
                        repeat = -1;
                        counterForCycle=0;
                    }
                    else
                        if (Automate[nextMarker + ii].Label == "¢")
                        {
                            dataGridView1.Rows.Add(Automate[nextMarker + ii].Alpha, Automate[nextMarker + ii].Label,
                           Automate[nextMarker + ii].Beta, Automate[nextMarker + ii].StackInput, stackOut);
                            if (Automate[nextMarker + ii].StackInput != null)
                            {
                                stack.Add(Automate[nextMarker + ii].StackInput.ToString());
                            }
                            nextMarker = GetCurrentIndex(Int32.Parse(Automate[nextMarker + ii].Beta.ToString()));
                            if (nextMarker == -1)
                            {
                                nextMarker = Int32.Parse(stack[stack.Count - 1]);
                            }
                        }
                        else if (repeat == -1 || counterForCycle > 100)
                        {
                           MessageBox.Show("Error!!! Expecting " + Automate[nextMarker + ii].Label + " Row # " + 
                               lexeme[2][(counterForEnterString - 1)]);
                            repeat = -1;
                        }             
                }
            }
        }

        public Lab3_automat(List<String> a, List<List<String>> lex)
        {
            InitializeComponent();
            lexeme = new List<List<string>>();
            enterString = new List<string>();
            lexeme.Add(new List<string>());//добавление № лексеми
            lexeme.Add(new List<string>());//добавление лексеми
            lexeme.Add(new List<string>());//добавление № рядка     
            for (int i = 0; i < a.Count - 1; i++)
                enterString.Add(a[i]);

            for (var i = 0; i < lex[0].Count; i++)
            {
                lexeme[0].Add(lex[0][i]);
                lexeme[1].Add(lex[1][i]);
                lexeme[2].Add(lex[2][i]);
            }

            Automate = new List<AutomateRow>           
            {
                 new AutomateRow()
                {
                    Alpha = 0,
                    Label = "program",
                    Beta = 1,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 1,
                    Label = "id",
                    Beta = 2,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 2,
                    Label = "↲",
                    Beta = 3,
                    StackInput = null
                },
                 new AutomateRow()
                {
                    Alpha = 3,
                    Label = "var",
                    Beta = 4,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 4,
                    Label = ",",
                    Beta = 5,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 5,
                    Label = "id",
                    Beta = 6,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 6,
                    Label = ",",
                    Beta = 5,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 6,
                    Label = ":",
                    Beta = 7,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 7,
                    Label = "integer",
                    Beta = 8,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 8,
                    Label = "↲",
                    Beta = 9,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 9,
                    Label = "begin",
                    Beta = 10,
                    StackInput = null
                },
               
                 new AutomateRow()
                {
                    Alpha = 10,
                    Label = "↲",
                    Beta = 100,
                    StackInput = 11
                },
                new AutomateRow()
                {
                    Alpha = 11,
                    Label = "↲",
                    Beta = 12,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 12,
                    Label = "end",
                    Beta = 777,
                    StackInput = null
                },
                 new AutomateRow()
                {
                    Alpha = 12,
                    Label = "¢",
                    Beta = 100,
                    StackInput = 11
                },
           

                //operator
            
                new AutomateRow()
                {
                    Alpha = 100,
                    Label = "id",
                    Beta = 101,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 100,
                    Label = "read",
                    Beta = 110,
                    StackInput = null
                },


                new AutomateRow()
                {
                    Alpha = 100,
                    Label = "write",
                    Beta = 110,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 100,
                    Label = "do",
                    Beta = 120,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 100,
                    Label = "if",
                    Beta = 300,
                    StackInput = 131
                },                
                new AutomateRow()
                {
                    Alpha = 101,
                    Label = "=",
                    Beta = 200,
                    StackInput = 102
                },
                new AutomateRow()
                {
                    Alpha = 102,
                    Label = "¢",
                    Beta = -1,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 110,
                    Label = "(",
                    Beta = 111,
                    StackInput = null
                },
                /*
                    changes!
                */
                new AutomateRow()
                {
                    Alpha=111,
                    Label=",",
                    Beta=1121,
                    StackInput=null
                },
              
                new AutomateRow()
                {
                    Alpha = 1121,//111,
                    Label = "id",
                    Beta = 1131,
                    StackInput = null
                },

                new AutomateRow()
                {
                    Alpha=1131,
                    Label=",",
                    Beta=1121,
                    StackInput=null
                },

                new AutomateRow()
                {
                    Alpha = 1131,
                    Label = ")",
                    Beta = -1,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 112,
                    Label = ",",
                    Beta = 111,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 120,
                    Label = "while",
                    Beta = 300,
                    StackInput = 121
                },
                new AutomateRow()
                {
                    Alpha = 121,
                    Label = "↲",
                    Beta = 100,
                    StackInput = 122
                },
                new AutomateRow()
                {
                    Alpha = 122,
                    Label = "↲",
                    Beta = 123,
                    StackInput = null
                },  new AutomateRow()
                {
                    Alpha = 123,
                    Label = "enddo",
                    Beta = -1,
                    StackInput = null
                },new AutomateRow()
                    {
                    Alpha = 123,
                    Label = "¢",
                    Beta = 100,
                    StackInput = 122
                },               
                                new AutomateRow()
                {
                    Alpha = 131,
                    Label = "↲",
                    Beta = 100,
                    StackInput = 132
                },
                new AutomateRow()
                {
                    Alpha = 132,
                    Label = "↲",
                    Beta = 133,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 133,
                    Label = "else",
                    Beta = 134,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 133,
                    Label = "¢",
                    Beta = 100,
                    StackInput = 132
                },
                new AutomateRow()
                {
                    Alpha = 134,
                    Label = "↲",
                    Beta = 100,
                    StackInput = 135
                },
                new AutomateRow()
                {
                    Alpha = 135,
                    Label = "↲",
                    Beta = 136,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 136,
                    Label = "endif",
                    Beta = -1,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 136,
                    Label = "¢",
                    Beta = 100,
                    StackInput = 135
                },
          
                //expression

                new AutomateRow()
                {
                    Alpha = 200,
                    Label = "id",
                    Beta = 201,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 200,
                    Label = "const",
                    Beta = 201,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 200,
                    Label = "(",
                    Beta = 200,
                    StackInput = 202
                },
                new AutomateRow()
                {
                    Alpha = 201,
                    Label = "+",
                    Beta = 200,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 201,
                    Label = "-",
                    Beta = 200,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 201,
                    Label = "*",
                    Beta = 200,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 201,
                    Label = "/",
                    Beta = 200,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 201,
                    Label = "^",
                    Beta = 200,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 201,
                    Label = "¢",
                    Beta = -1,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 202,
                    Label = ")",
                    Beta = 201,
                    StackInput = null
                },
                //log expression
                new AutomateRow()
                {
                    Alpha = 300,
                    Label = "[",
                    Beta = 300,
                    StackInput = 303
                },
                new AutomateRow()
                {
                    Alpha = 300,
                    Label = "not",
                    Beta = 300,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 300,
                    Label = "¢",//c/
                    Beta = 200,
                    StackInput = 301
                },
                new AutomateRow()
                {
                    Alpha = 301,
                    Label = "==",//c/
                    Beta = 200,
                    StackInput = 302
                },
                new AutomateRow()
                {
                    Alpha = 301,
                    Label = "!=",//c/
                    Beta = 200,
                    StackInput = 302
                },
                new AutomateRow()
                {
                    Alpha = 301,
                    Label = ">",//c/
                    Beta = 200,
                    StackInput = 302
                },
                new AutomateRow()
                {
                    Alpha = 301,
                    Label = ">=",//c/
                    Beta = 200,
                    StackInput = 302
                },
                new AutomateRow()
                {
                    Alpha = 301,
                    Label = "<",//c/
                    Beta = 200,
                    StackInput = 302
                },
                new AutomateRow()
                {
                    Alpha = 301,
                    Label = "<=",//c/
                    Beta = 200,
                    StackInput = 302
                },
                new AutomateRow()
                {
                    Alpha = 302,
                    Label = "and",
                    Beta = 300,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 302,
                    Label = "or",
                    Beta = 300,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 302,
                    Label = "¢",
                    Beta = -1,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 303,
                    Label = "]",
                    Beta = 302,
                    StackInput = null
                }

            };
            AutomateInUse();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }
}

