using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace CRUD
{
    public partial class Form1 : Form
    {
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

            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dataGridView1.DataSource = dt;
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
    }
}
