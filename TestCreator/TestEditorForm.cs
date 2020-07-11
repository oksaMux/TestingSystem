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
using System.Xml;

namespace TestCreator
{
    public partial class TestEditorForm : Form
    {
        XmlTextWriter testWriter; //to write *.xml-file with test
        public TestEditorForm()
        {
            InitializeComponent();
            
            DirectoryInfo TestsDir = new DirectoryInfo("Tests"); 
            if (!TestsDir.Exists)
                TestsDir.Create();

            comboBoxCategory.Items.AddRange(TestsDir.GetDirectories());
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (comboBoxCategory.Text != "" && textBoxTheme.Text != "" && textBoxAuthor.Text != "")
            {
                try
                {
                    testWriter = new XmlTextWriter("Tests\\" 
                        + comboBoxCategory.Text 
                        + "\\" 
                        + textBoxAuthor.Text 
                        + ".xml", Encoding.UTF8);
                }
                catch (DirectoryNotFoundException)
                {
                    Directory.CreateDirectory("Tests\\" + comboBoxCategory.Text);
                    testWriter = new XmlTextWriter("Tests\\" 
                        + comboBoxCategory.Text 
                        + "\\" 
                        + textBoxTheme.Text 
                        + ".xml", Encoding.UTF8);
                }

                testWriter.Formatting = Formatting.Indented;

                testWriter.WriteStartDocument(); 
                testWriter.WriteStartElement("test"); 

                testWriter.WriteStartElement("Category"); 
                testWriter.WriteString(comboBoxCategory.Text);
                testWriter.WriteEndElement();

                testWriter.WriteStartElement("Author");
                testWriter.WriteString(textBoxAuthor.Text); 
                testWriter.WriteEndElement();

                testWriter.WriteStartElement("Theme"); 
                testWriter.WriteString(textBoxTheme.Text);
                testWriter.WriteEndElement();

                testWriter.WriteStartElement("NumberOfQuestions"); 
                testWriter.WriteStartAttribute("numbers"); 
                testWriter.WriteString(numericNQuestions.Value.ToString());
                testWriter.WriteEndAttribute();

                for (int i = 1; i <= numericNQuestions.Value; i++)
                {
                    QuestionsForm QF = new QuestionsForm(i, testWriter);
                    QF.ShowDialog();
                    this.Visible = false;
                }

                testWriter.WriteEndElement();
                testWriter.WriteEndElement(); 
                testWriter.WriteEndDocument();

                testWriter.Close();

                MessageBox.Show("All questions are created");
                Application.Exit();
            }
            else
            {
                MessageBox.Show("Fill all data");
            }
        }
    }
}
