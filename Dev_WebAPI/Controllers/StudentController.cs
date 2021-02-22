using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using Dapper;
using Dev_WebAPI.Models;
using System.Data;

namespace Dev_WebAPI.Controllers
{
    public class StudentController : ApiController
    {
        public static string connectionStr = @"Server=localhost;Port=3306;Database=ack_test;Uid=root;Pwd=1234;";

        
        public List<Student> Get()
        {
            using (IDbConnection connection = new MySqlConnection(connectionStr))
            {
                connection.Open();
                string readQuery = "select * from student;";
                return connection.Query<Student>(readQuery).ToList();
            }
        }

        /*
        public IEnumerable<string> Get()
        {
            return new string[] {"안녕하세요", "test중" };
        }
        */
        public string Get(int id)
        {
            return "입력한 값 : " + id.ToString();
        }
    }
}
