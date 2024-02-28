using Microsoft.AspNetCore.Mvc;

namespace StudentAPI.Controllers
{
    public class StudentController : ControllerBase 
    {
        [HttpGet]
        [Route("api/Student/students")]
        public async Task<IActionResult> GetAllStudentsAsync()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("api/Student/student/{Id}")]
        public async Task<IActionResult> GetStudentDetailsAsync(string Id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("api/Student/student")]
        public async Task<IActionResult> AddStudentDetailsAsync()
        {
            throw new NotImplementedException();
        }

        [HttpPatch]
        [Route("api/Student/student/{Id}")]
        public async Task<IActionResult> UpdateStudentAsync()
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("api/Student/student/{Id}")]
        public async Task<IActionResult> DeleteStudentDetailAsync()
        {
            throw new NotImplementedException();
        }
    }
}
