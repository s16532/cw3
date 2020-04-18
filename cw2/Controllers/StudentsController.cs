using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using cw2.Models;
using cw2.Models.Models;
using cw2.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cw2.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private IDbService _dbService;
        public string conString = "Data Source=db-msslq;Initial Catalog=s16532;Integrated Security=true";


        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetStudents(string orderBy)
        {
            var list = new List<StudentInfoDTO>();
            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "select s.FirstName, s.LastName, s.BirthDate, st.Name, e.Semester from Student s " +
                    "join Enrollment e on e.IdEnrollment = s.IdEnrollment join Studies st on st.IdStudy = e.IdStudy";
                connection.Open();

                SqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    var student = new StudentInfoDTO
                    {
                        FirstName = dataReader["FirstName"].ToString(),
                        LastName = dataReader["LastName"].ToString(),
                        Name = dataReader["Name"].ToString(),
                        BirthDate = dataReader["BirthDate"].ToString(),
                        Semester = dataReader["Semester"].ToString()
                    };
                    list.Add(student);
                }
            }

            return Ok(list);
        }

       

        [HttpGet("{id}")]
        public IActionResult GetStudent(string id)
        {
            var student = new StudentInfoDTO();
            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "select s.FirstName, s.LastName, s.BirthDate, s.IndexNumber, st.Name, e.Semester " +
                    "from Student s " +
                    "join Enrollment e on e.IdEnrollment = s.IdEnrollment " +
                    "join Studies st on st.IdStudy = e.IdStudy " +
                    "where s.IndexNumber = @id";
                command.Parameters.AddWithValue("id", id);
                connection.Open();

                SqlDataReader dataReader = command.ExecuteReader();
                dataReader.Read();

                student.FirstName = dataReader["FirstName"].ToString();
                student.LastName = dataReader["LastName"].ToString();
                student.Name = dataReader["Name"].ToString();
                student.BirthDate = dataReader["BirthDate"].ToString();
                student.Semester = dataReader["Semester"].ToString();

            }

            return Ok(student);
        }

        [HttpGet("enrollments/{id}")]
        public IActionResult GetStudentEnrollments(string id)
        {
            var list = new List<EnrollmentInfoDTO>();
            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "select s.IndexNumber, e.Semester, st.Name, e.StartDate " +
                    "from Student s " +
                    "join Enrollment e on e.IdEnrollment = s.IdEnrollment " +
                    "join Studies st on st.IdStudy = e.IdStudy " +
                    "where s.IndexNumber = @id";
                command.Parameters.AddWithValue("id", id);
                connection.Open();

                SqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    var enrollment = new EnrollmentInfoDTO
                    {
                        Semester = dataReader["Semester"].ToString(),
                        Name = dataReader["Name"].ToString(),
                        StartDate = dataReader["StartDate"].ToString()
                    };
                    list.Add(enrollment);
                }
            }

            return Ok(list);
        }

        [HttpPost]
        public IActionResult CreateStudent(Models.Models.Student student)
        {
            //add to database
            //generating index number
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }

        [HttpPut]
        public IActionResult UpdateStudent(Student student)
        {
            return Ok("Aktualizacja dokończona");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            return Ok("Usuwanie ukończone");
        }
    }
}