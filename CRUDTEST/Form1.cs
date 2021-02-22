using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace ACK
{
    public partial class Form1 : Form
    {
        //InotifyPropertyChanged Interface 적용
        private BindingSource studentBindingSource = new BindingSource(); 

        //MySQL 접속 정의
        MySqlConnection connection = new MySqlConnection(@"Server=localhost;Port=3306;Database=ack_test;Uid=root;Pwd=1234;");

        public Form1()
        {
            InitializeComponent();

        }

        /// <summary>
        /// Select Query 실행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            connection.Open();
            string readQuery = "SELECT * FROM student;";
            MySqlCommand cmd = new MySqlCommand(readQuery, connection);
            MySqlDataReader dr = cmd.ExecuteReader();
            int rowIndex = dataGridView1.Rows.Count;
            rowIndex -= rowIndex;

            //InotifyPropertyChanged 적용
            BindingList<StudentChanged> studentList = new BindingList<StudentChanged>();
            while (dr.Read())
            {
                studentList.Add(StudentChanged.CreateNewStudent(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString()));
            }
            for(int i = 1; i < rowIndex; i++)
            {
                
                studentList[i].Grade = dr[0].ToString();
                studentList[i].CClass = dr[1].ToString();
                studentList[i].NO = dr[2].ToString();
                studentList[i].Name = dr[3].ToString();
                studentList[i].Score = dr[4].ToString();
            }

            this.studentBindingSource.DataSource = studentList;

            dataGridView1.DataSource = studentBindingSource;

            connection.Close();
        }

        /// <summary>
        /// Insert Query 실행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {

            int rowIndex = dataGridView1.CurrentRow.Index;
            int colum_Index = dataGridView1.CurrentCell.ColumnIndex;

            var queryData = new StringBuilder();
            string result;

            for (int column = 0; column <= colum_Index; column++)
            {
                string appendData = dataGridView1.Rows[rowIndex].Cells[column].FormattedValue.ToString();
                queryData.Append('"');
                queryData.Append(appendData);
                queryData.Append('"');

                if (column != colum_Index)
                {
                    queryData.Append(",");
                }
            }
            result = queryData.ToString();
            string insertQuery = "INSERT INTO student(Grade,CClass,No,Name,Score) VALUES(" + result + ");";

            try
            {
                connection.Open();
                MySqlCommand cmdDatabase = new MySqlCommand(insertQuery, connection);
                cmdDatabase.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB Connnection Error" + ex);
            }
            finally
            {
                
                connection.Close();
            }
            
        }

        /// <summary>
        /// Update Query 실행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            int rowIndex = dataGridView1.CurrentRow.Index;

            string gradeData = dataGridView1.Rows[rowIndex].Cells[0].FormattedValue.ToString();
            string cClassData = dataGridView1.Rows[rowIndex].Cells[1].FormattedValue.ToString();
            string noData = dataGridView1.Rows[rowIndex].Cells[2].FormattedValue.ToString();
            string nameData = dataGridView1.Rows[rowIndex].Cells[3].FormattedValue.ToString();
            string scoreData = dataGridView1.Rows[rowIndex].Cells[4].FormattedValue.ToString();

            if (MessageBox.Show("선택한 " + noData + "를 수정하시겠습니까?", "경고", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string updateQuery = "UPDATE student SET Grade = '" + gradeData + "', CClass = '" + cClassData +
                                    "', Name = '" + nameData + "', Score = '" + scoreData + "' WHERE No = '" + noData + "';";

                try
                {
                    connection.Open();
                    MySqlCommand cmdDatabase = new MySqlCommand(updateQuery, connection);
                    cmdDatabase.ExecuteNonQuery();
                    MessageBox.Show("수정되었습니다", "수정 성공");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("DB Connection Error" + ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Delete Query 실행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            int rowIndex = dataGridView1.CurrentRow.Index;

            //기본키인 No Column에서 선택된 행을 삭제
            string noData = dataGridView1.Rows[rowIndex].Cells[2].FormattedValue.ToString();

            if(MessageBox.Show("선택한 " + noData + "를 삭제하시겠습니까?", "경고", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string deleteQuery = "DELETE FROM student WHERE No ='" + noData +"';";

                try
                {
                    connection.Open();
                    MySqlCommand cmdDatabase = new MySqlCommand(deleteQuery, connection);
                    cmdDatabase.ExecuteNonQuery();
                    MessageBox.Show("삭제되었습니다", "삭제 성공");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("DB Connection Error" + ex);
                }
                finally
                {
                    connection.Close();
                }
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            connection.Open();
            string readQuery = "SELECT * FROM student;";
            MySqlCommand cmd = new MySqlCommand(readQuery, connection);
            MySqlDataReader dr = cmd.ExecuteReader();
            int rowIndex = dataGridView1.Rows.Count;
            rowIndex -= rowIndex;

            //InotifyProperyChanged Interface 적용
            BindingList<StudentChanged> studentList = new BindingList<StudentChanged>();
            while (dr.Read())
            {
                studentList.Add(StudentChanged.CreateNewStudent(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString()));
            }
            for (int i = 1; i < rowIndex; i++)
            {
                studentList[i].Grade = dr[0].ToString();
                studentList[i].CClass = dr[1].ToString();
                studentList[i].NO = dr[2].ToString();
                studentList[i].Name = dr[3].ToString();
                studentList[i].Score = dr[4].ToString();
            }

            this.studentBindingSource.DataSource = studentList;

            dataGridView1.DataSource = studentBindingSource;

            connection.Close();
        }

        /// <summary>
        /// InotifyPropertyChanged 적용
        /// </summary>
        class StudentChanged : INotifyPropertyChanged
        {
            private string gradeValue = String.Empty;
            private string cClassValue = String.Empty;
            private string noValue = String.Empty;
            private string nameValue = String.Empty;
            private string scoreValue = String.Empty;

            public event PropertyChangedEventHandler PropertyChanged;

            private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public static StudentChanged CreateNewStudent(string grade, string cClass, string no, string name, string score) 
            {
                
                StudentChanged stdChanged = new StudentChanged();

                stdChanged.gradeValue = grade;
                stdChanged.cClassValue = cClass;
                stdChanged.noValue = no;
                stdChanged.nameValue = name;
                stdChanged.scoreValue = score;

                return stdChanged;
            }

            public string Grade
            {
                get
                {
                    return this.gradeValue;
                }

                set
                {
                    if (value != this.gradeValue)
                    {
                        this.gradeValue = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            public string CClass
            {
                get
                {
                    return this.cClassValue;
                }

                set
                {
                    if (value != this.cClassValue)
                    {
                        this.cClassValue = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            public string NO
            {
                get
                {
                    return this.noValue;
                }

                set
                {
                    if (value != this.noValue)
                    {
                        this.noValue = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            public string Name
            {
                get
                {
                    return this.nameValue;
                }

                set
                {
                    if (value != this.nameValue)
                    {
                        this.nameValue = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            public string Score
            {
                get
                {
                    return this.scoreValue;
                }

                set
                {
                    if (value != this.scoreValue)
                    {
                        this.scoreValue = value;
                        NotifyPropertyChanged();
                    }
                }
            }
        }
    }
}

