using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        
            private  string ReplaceFirst( string str, string oldValue, string newValue)
            {
                int startindex = str.IndexOf(oldValue);

                if (startindex == -1)
                {
                    return str;
                }

                return str.Remove(startindex, oldValue.Length).Insert(startindex, newValue);
            }
        

        public List<int> GetPositions(string source, string searchString)
        {
            List<int> ret = new List<int>();
            int len = searchString.Length;
            int start = -len;
            while (true)
            {
                start = source.IndexOf(searchString, start + len);
                if (start == -1)
                {
                    break;
                }
                else
                {
                    ret.Add(start);
                }
            }
            return ret;
        }

    


        private void textBoxIn_Leave(object sender, EventArgs e)
        {
            string[] stringSeparators = new string[] { "\r\n" };

           // ==> :

            ArrayList param = new ArrayList();
            ArrayList paramName = new ArrayList();
            String strMerge = "";

            string[] lines = textBoxIn.Text.Trim().Split(stringSeparators, StringSplitOptions.None);



            Boolean isParamName = textBoxIn.Text.ToUpper().IndexOf("INSERT") >= 0;


            foreach (string s in lines)
            {
                string str = s.Substring(s.IndexOf("):") + 2);

                if (str.IndexOf("==> param") > 0  || str.IndexOf("==>") > 0)
                {




                    if (isParamName)
                    {
                        int inizio = s.IndexOf("==> :")+5;
                        int fine = s.IndexOf("= ",inizio);

                        string name = s.Substring(inizio, fine - inizio);
                        paramName.Add(name.Trim());

                        fine = fine + 2;

                        try
                        {

                            int fine2 = s.IndexOf("'", fine + 1);

                            string value = s.Substring(fine + 1, fine2 - fine - 1);

                            param.Add(value);

                        }
                        catch (Exception ex)
                        {
                            param.Add("null");
                        
                        }

                        }
                    else
                    {
                        string value = str.Substring(str.IndexOf(":") + 1);

                        param.Add(value);
                    }

                }
                else
                {
                    if (str.ToUpper().Contains("INSERT") 
                        || str.ToUpper().Contains("SELECT")
                        || str.ToUpper().Contains("UPDATE")
                        || str.ToUpper().Contains("DELETE"))

                        strMerge += str + "\r\n";
                }



                Console.WriteLine(s);
            }

            Console.WriteLine(strMerge);

            int i = 0;

            foreach (String value in param)
            {

                if (!isParamName)

                    strMerge = ReplaceFirst(strMerge, "?", value);
                else

                {

                    string key = ":" + paramName[i].ToString().Trim();

                    strMerge = strMerge.Replace(key+",","'"+value+"',");
                    strMerge = strMerge.Replace(key + ")", "'" + value + "')");

                }
                i++;

            }

            //List<int> list = GetPositions(strMerge, "?");

            textBoxOut.Text= strMerge;

        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBoxIn.Text = "";
            textBoxOut.Text = ""; 
        }
    }

