using Microsoft.AspNetCore.Mvc;
using StudentAPI.Interface;
using StudentAPI.Model;
using System.Linq.Expressions;

namespace StudentAPI.Controllers
{
    [ApiVersion("2.0")]
    [ApiController]
    public class StudentV2Controller(IContainerRepository<Student> containerRepository) : ControllerBase
    {
        private readonly IContainerRepository<Student> myContainerRepository = containerRepository;

        [MapToApiVersion("2.0")]
        [HttpGet]
        [Route("/api/v{version:apiVersion}/student/{standard}")]
        public async Task<IActionResult> GetAllStudentsAsync(string standard)
        {
            if (string.IsNullOrEmpty(standard))
            {
                return BadRequest("standard cannot be empty");
            }
            Expression<Func<Student, bool>> filter = item => item.Standard == standard;
            var response = await myContainerRepository.FilterAsync(filter).ConfigureAwait(false);
            List<StudentInfo> studentInfo = new();

            foreach (var student in response.ToList())
            {
                StudentInfo _studentInfo = new()
                {
                    Name = student.Name,
                    PhoneNumber = student.PhoneNumber,
                    Address = student.Address,
                    Department = student.Department,
                    FatherName = student.Father,
                    MotherName = student.Mother,
                    Standard = student.Standard,
                };
                studentInfo.Add(_studentInfo);
            }

            return Ok(studentInfo);

        }
    }
}
