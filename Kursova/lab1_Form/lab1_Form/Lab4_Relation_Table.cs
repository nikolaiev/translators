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
    public partial class Lab4_Relation_Table : Form
    {
        public List<List<string>> relation = new List<List<string>>();
        public void Alltable()
        {
            String[] lex = {"<сп. об1>", "<сп. об>","<сп. оп1>","<сп. оп>", "<оп>","<сп. id1>",
                                "<сп. id>","<врж1>","<врж>","<терм1>","<терм>","<множ>",
                                "<ЛВ1>","<ЛВ>","<ЛТ1>","<ЛТ>","<ЛМ>","<знак>",

                               "program", "id","const", "var", "begin", "end", "integer", 
                               "read", "write", "do", "while", "enddo", 
                               "if", "then", "else", "endif",

                               "↲", ":", ",", "(", ")", "[", "]", "=", "+", "-", "*", "/","not",
                               "or", "and", "<", "<=", "==", "!=", ">=", ">","#"
                            };

            dataGridView1.Rows.Add();
            dataGridView1.Rows[0].Cells[0].Value = "";

            for (var i = 1; i < lex.Length+1; i++)
                dataGridView1.Rows[0].Cells[i].Value = lex[i-1];


            for (var i = 1; i < lex.Length+1; i++)
                dataGridView1.Rows.Add();

            for (var i = 1; i < lex.Length+1; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = lex[i-1];
            }

            for (var i = 1; i < lex.Length+1; i++)
            {
                dataGridView1.Rows[i].Cells[lex.Length].Value = ">";
                dataGridView1.Rows[lex.Length].Cells[i].Value = "<";
            }
            dataGridView1.Rows[0].Frozen = true;

        }

        public Boolean IsTerm(String a)
        {
            if (a.Contains('<') && a.Contains('>')) return false;
            return true;
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

        public String[] LastPlus(String s, String[][] gr)
        {
            var a = new String[200]; var l = 0;
            for (var i = 0; i < gr.Length; i++)
            {

                if (gr[i][0] == s)
                {
                    a[l] = gr[i][gr[i].Length - 1];
                    l++;
                    if (!IsTerm(gr[i][gr[i].Length - 1]) && gr[i][gr[i].Length - 1] != gr[i][0])
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
                    if (!IsTerm(gr[i][1]) && (gr[i][1] != gr[i][0]))
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
            {
                for (var j = 1; j < gr[i].Length - 1; j++)
                    if (!IsTerm(gr[i][j]))
                    {
                        var lp = LastPlus(gr[i][j], gr);
                        if (IsTerm(gr[i][j + 1]))
                        {
                            for (var ii = 0; ii < lp.Length; ii++)
                            {
                                FillTable(lp[ii], gr[i][j + 1], ">");
                            }
                        }
                        else
                        {
                            var fp = FirstPlus(gr[i][j + 1], gr);
                            for (var ii = 0; ii < lp.Length; ii++)
                                for (var jj = 0; jj < fp.Length; jj++)
                                    FillTable(lp[ii], fp[jj], ">");
                        }

                    }
            }
        }


        public void Smaller(String[][] gr)
        {
            for (var i = 0; i < gr.Length; i++)
            {
                for (var j = 1; j < gr[i].Length - 1; j++)
                    if (!IsTerm(gr[i][j + 1]))
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

        public String[][] myLanguage = new String[41][];
               public Lab4_Relation_Table()
        {
            InitializeComponent();
            relation.Add(new List<string>());//добавление новой строки
            relation.Add(new List<string>());//добавление новой строки
            relation.Add(new List<string>());//добавление новой строки          
            myLanguage[0] = new[] {"<прогр>", "program", "id", "↲", "var", "<сп. об1>","↲","begin",
                                        "<сп. оп1>","end","↲"};
            myLanguage[1] = new[] { "<сп. об1>", "<сп. об>" };
            myLanguage[2] = new[] { "<сп. об>", "<сп. id1>", ":", "integer" };
            myLanguage[3] = new[] { "<сп. об>", "<сп. об>", "↲", "<сп. id1>", ":", "integer" };
            myLanguage[4] = new[] { "<сп. id1>", "<сп. id>" };
            myLanguage[5] = new[] { "<сп. id>", ",", "id" };
            myLanguage[6] = new[] { "<сп. id>", "<сп. id>", ",", "id" };
            myLanguage[7] = new[] { "<сп. оп1>", "<сп. оп>", "↲" };
            myLanguage[8] = new[] { "<сп. оп>", "↲", "<оп>" };
            myLanguage[9] = new[] { "<сп. оп>", "<сп. оп>", "↲", "<оп>" };
            // myLanguage[10] = new[] { "<оп>", "read", "(", "<врж1>", ")" };
            myLanguage[10] = new[] { "<оп>", "read", "(", "<сп. id1>", ")" };
            // myLanguage[11] = new[] { "<оп>", "write", "(", "<врж1>", ")" };
            myLanguage[11] = new[] { "<оп>", "write", "(", "<сп. id1>", ")" };
            myLanguage[12] = new[] { "<оп>", "id", "=", "<врж1>" };     
            myLanguage[13] = new[] { "<оп>", "if", "<ЛВ1>", "<сп. оп1>", "else", "<сп. оп1>", "endif" };
            myLanguage[14] = new[] { "<оп>", "do", "while", "<ЛВ1>", "<сп. оп1>", "enddo" };
            myLanguage[15] = new[] { "<врж1>", "<врж>" };
            myLanguage[16] = new[] { "<врж>", "<врж>", "+", "<терм1>" };
            myLanguage[17] = new[] { "<врж>", "<врж>", "-", "<терм1>" };
            myLanguage[18] = new[] { "<врж>", "<терм1>" };
            myLanguage[19] = new[] { "<терм1>", "<терм>" };
            myLanguage[20] = new[] { "<терм>", "<терм>", "*", "<множ>" };
            myLanguage[21] = new[] { "<терм>", "<терм>", "/", "<множ>" };
            myLanguage[22] = new[] { "<терм>", "<множ>" };
            myLanguage[23] = new[] { "<множ>", "(", "<врж1>", ")" };
            myLanguage[24] = new[] { "<множ>", "id" };
            myLanguage[25] = new[] { "<множ>", "const" };
            myLanguage[26] = new[] { "<ЛВ1>", "<ЛВ>" };
            myLanguage[27] = new[] { "<ЛВ>", "<ЛВ>", "or", "<ЛТ1>" };
            myLanguage[28] = new[] { "<ЛВ>", "<ЛТ1>" };           
            myLanguage[29] = new[] { "<ЛТ1>", "<ЛТ>" };
            myLanguage[30] = new[] { "<ЛТ>", "<ЛМ>" };
            myLanguage[31] = new[] { "<ЛТ>", "<ЛТ>", "and", "<ЛМ>" };
            myLanguage[32] = new[] { "<ЛМ>", "[", "<ЛВ1>", "]" };
            myLanguage[33] = new[] { "<ЛМ>", "<врж1>", "<знак>", "<врж1>" };
            myLanguage[34] = new[] { "<ЛМ>", "not", "<ЛМ>" };
            myLanguage[35] = new[] { "<знак>", "<" };
            myLanguage[36] = new[] { "<знак>", "<=" };
            myLanguage[37] = new[] { "<знак>", "==" };
            myLanguage[38] = new[] { "<знак>", "!=" };
            myLanguage[39] = new[] { "<знак>", ">=" };
            myLanguage[40] = new[] { "<знак>", ">" };
         

            //      Show all     
            Alltable();
            Eql(myLanguage);
            Bigger(myLanguage);
            Smaller(myLanguage);
            FillRelation();
        }

               private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
               {

               }
    }
}
