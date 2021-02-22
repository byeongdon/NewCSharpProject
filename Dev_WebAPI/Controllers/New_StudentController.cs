using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using Dapper;
using MySql.Data.MySqlClient;
using Dev_WebAPI.Models;

namespace Dev_WebAPI.Controllers
{
    public class New_StudentController : ApiController 
    {
        public static string connectionStr = @"Server=localhost;Port=3306;Database=ack_test;Uid=root;Pwd=1234;";

        // GET: api/New_Student
        public List<Student> Get()
        {
            using (IDbConnection connection = new MySqlConnection(connectionStr))
            {
                connection.Open();
                string readQuery = "select * from student;";
                List<Student> students = connection.Query<Student>(readQuery).ToList();
                connection.Close();
                return students;
            }
        }

        // P: api/New_Student
        public void Post([FromBody] Student value)
        {
            using (IDbConnection connection = new MySqlConnection(connectionStr))
            {
                string grade = value.Grade;
                string cClass = value.CClass;
                string no = value.No;
                string name = value.Name;
                string score = value.Score;

                string queryData = "'" + grade + "' , '" + cClass + "' , '" + no + "' , '" + name + "' , '" + score + "'";
                string insertQuery = "INSERT INTO student(Grade, CClass, No, Name, Score) VALUES(" + queryData + "); ";

                connection.Open();

                connection.Execute(insertQuery);
                connection.Close();
            }
        }


        // PUT: api/New_Student/5
        public void Put([FromBody] Student value)
        {
            using (IDbConnection connection = new MySqlConnection(connectionStr))
            {
                string grade = value.Grade;
                string cClass = value.CClass;
                string no = value.No;
                string name = value.Name;
                string score = value.Score;

                string updateQuery = "UPDATE student SET Grade = '" + grade + "', CClass = '" + cClass +
                                    "', Name = '" + name + "', Score = '" + score + "' WHERE No = '" + no + "';";

                connection.Open();
                connection.Execute(updateQuery);
                connection.Close();

            }
        }

        // DELETE: api/New_Student
        public void Delete([FromBody] Student value)
        {
            using (IDbConnection connection = new MySqlConnection(connectionStr))
            {
                string no = value.No;

                string deleteQuery = "DELETE FROM student WHERE No ='" + no + "';";

                connection.Open();
                connection.Execute(deleteQuery);
                connection.Close();
            }
        }
    }
}

