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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ACK
{
    public partial class Form1 : Form
    {
        //InotifyPropertyChanged Interface 적용
        private BindingSource studentBindingSource = new BindingSource();
        string apiUrl = "https://localhost:44368/api/New_Student";

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
            Get();

        }

        /// <summary>
        /// Insert Query 실행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Post();
        }

        /// <summary>
        /// Update Query 실행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            Put();
        }

        /// <summary>
        /// Delete Query 실행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            Delete();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Get();
        }

        public async Task Get()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(apiUrl))
                {
                    using (HttpContent httpContent = httpResponseMessage.Content)
                    {
                        string readContent = await httpContent.ReadAsStringAsync();
                        int rowIndex = dataGridView1.Rows.Count;
                        rowIndex -= rowIndex;

                        BindingList<StudentChanged> studentList = new BindingList<StudentChanged>();

                        JArray jArray = JArray.Parse(readContent);
                        var i = jArray.SelectToken("");
                        foreach (var data in i)
                        {
                            var grade = data.SelectToken("Grade").ToString();
                            var cClass = data.SelectToken("CClass").ToString();
                            var no = data.SelectToken("No").ToString();
                            var name = data.SelectToken("Name").ToString();
                            var score = data.SelectToken("Score").ToString();

                            studentList.Add(StudentChanged.CreateNewStudent(grade, cClass, no, name, score));
                        }
                        this.studentBindingSource.DataSource = studentList;

                        dataGridView1.DataSource = studentBindingSource;
                    }
                }


            }
        }

        public async Task Post()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(apiUrl))
                {
                    using (HttpContent httpContent = httpResponseMessage.Content)
                    {

                        int rowIndex = dataGridView1.CurrentRow.Index;

                        StudentChanged student = new StudentChanged();

                        student.Grade = dataGridView1.Rows[rowIndex].Cells[0].FormattedValue.ToString();
                        student.CClass = dataGridView1.Rows[rowIndex].Cells[1].FormattedValue.ToString();
                        student.NO = dataGridView1.Rows[rowIndex].Cells[2].FormattedValue.ToString();
                        student.Name = dataGridView1.Rows[rowIndex].Cells[3].FormattedValue.ToString();
                        student.Score = dataGridView1.Rows[rowIndex].Cells[4].FormattedValue.ToString();

                        string jsonData = JsonConvert.SerializeObject(student);

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                        request.ContentType = "application/json";
                        request.Method = "POST";
                        using (StreamWriter stream = new StreamWriter(request.GetRequestStream()))
                        {
                            stream.Write(jsonData);
                            stream.Flush();
                            stream.Close();
                        }
                        WebResponse response = request.GetResponse();
                        using (Stream stream = response.GetResponseStream())
                        {
                            StreamReader reader = new StreamReader(stream);
                            string reServer = reader.ReadToEnd();
                        }
                    }
                }
            }
        }


        public async Task Put()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(apiUrl))
                {
                    using (HttpContent httpContent = httpResponseMessage.Content)
                    {
                        int rowIndex = dataGridView1.CurrentRow.Index;

                        StudentChanged student = new StudentChanged();

                        student.Grade = dataGridView1.Rows[rowIndex].Cells[0].FormattedValue.ToString();
                        student.CClass = dataGridView1.Rows[rowIndex].Cells[1].FormattedValue.ToString();
                        student.NO = dataGridView1.Rows[rowIndex].Cells[2].FormattedValue.ToString();
                        student.Name = dataGridView1.Rows[rowIndex].Cells[3].FormattedValue.ToString();
                        student.Score = dataGridView1.Rows[rowIndex].Cells[4].FormattedValue.ToString();

                        if (MessageBox.Show("선택한 " + student.NO + "를 수정하시겠습니까?", "경고", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            try
                            {
                                string jsonData = JsonConvert.SerializeObject(student);

                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                                request.ContentType = "application/json";
                                request.Method = "PUT";
                                using (StreamWriter stream = new StreamWriter(request.GetRequestStream()))
                                {
                                    stream.Write(jsonData);
                                    stream.Flush();
                                    stream.Close();
                                }
                                WebResponse response = request.GetResponse();
                                using (Stream stream = response.GetResponseStream())
                                {
                                    StreamReader reader = new StreamReader(stream);
                                    string reServer = reader.ReadToEnd();
                                }
                                MessageBox.Show("수정되었습니다", "수정 성공");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("DB Connection Error" + ex);
                            }
                        }
                    }
                }
            }
        }

        public async Task Delete()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(apiUrl))
                {
                    using (HttpContent httpContent = httpResponseMessage.Content)
                    {
                        int rowIndex = dataGridView1.CurrentRow.Index;

                        StudentChanged student = new StudentChanged();

                        student.NO = dataGridView1.Rows[rowIndex].Cells[2].FormattedValue.ToString();


                        if (MessageBox.Show("선택한 " + student.NO + "를 삭제하시겠습니까?", "경고", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            try
                            {
                                string jsonData = JsonConvert.SerializeObject(student);

                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                                request.ContentType = "application/json";
                                request.Method = "DELETE";

                                using (StreamWriter stream = new StreamWriter(request.GetRequestStream()))
                                {
                                    stream.Write(jsonData);
                                    stream.Flush();
                                    stream.Close();
                                }
                                WebResponse response = request.GetResponse();
                                using (Stream stream = response.GetResponseStream())
                                {
                                    StreamReader reader = new StreamReader(stream);
                                    string reServer = reader.ReadToEnd();
                                }
                                MessageBox.Show(student.NO + "가 삭제되었습니다", "삭제 성공");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("DB Connection Error" + ex);
                            }
                        }
                    }
                }
            }
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

