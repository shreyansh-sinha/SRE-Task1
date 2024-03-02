using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentAPI.Interface;
using StudentAPI.Model;

namespace StudentAPI.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    public class StudentController(IContainerRepository<Student> containerRepository, IMapper mapper) : ControllerBase 
    {
        private readonly IContainerRepository<Student> myContainerRepository = containerRepository;
        private readonly IMapper myMapper = mapper;

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("api/v{version:apiVersion}/student/{standard}/{id}")]
        public async Task<IActionResult> GetStudentDetailsAsync(string standard, string id)
        {
            if(string.IsNullOrEmpty(id) || string.IsNullOrEmpty(standard))
            {
                return BadRequest();
            }
            
            var response = await myContainerRepository.GetItemAsync(id, standard).ConfigureAwait(false);
            
            StudentInfo studentInfo = new()
            {
                Name = response.Name,
                Standard = response.Standard,
                FatherName = response.Father,
                MotherName = response.Mother,
                Address = response.Address,
                PhoneNumber = response.PhoneNumber,
                Department = response.Department,
            };

            return Ok(studentInfo);
        }

        [MapToApiVersion("1.0")]
        [HttpPost]
        [Route("api/v{version:apiVersion}/student")]
        public async Task<IActionResult> AddStudentDetailsAsync([FromBody] StudentInfo studentInfo)
        {
            if(studentInfo.Name == null || studentInfo.Standard == null) 
            {
                return BadRequest("Name or Standard cannot be null");
            }

            var response = await myContainerRepository.AddAsync(new Student
            {
                Id = Guid.NewGuid().ToString(),
                Standard = studentInfo.Standard,
                Name = studentInfo.Name,
                Father = studentInfo.FatherName,
                Mother = studentInfo.MotherName,
                CreationDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                Department = studentInfo.Department,
                Address = studentInfo.Address,
                PhoneNumber = studentInfo.PhoneNumber,
            });

            return Ok(response);
        }

        [MapToApiVersion("1.0")]
        [HttpPatch]
        [Route("api/v{version:apiVersion}/student/{standard}/{id}")]
        public async Task<IActionResult> UpdateStudentAsync(string id, string standard, [FromBody] UpdateStudentDto requestDto)
        {
            try
            {
                // Validate input parameters
                if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(standard) || requestDto == null)
                {
                    return BadRequest();
                }

                // Retrieve the existing student from the repository
                Student student = await myContainerRepository.GetItemAsync(id, standard).ConfigureAwait(false);
                if (student == null)
                {
                    return NotFound("Student not found.");
                }

                student = myMapper.Map(requestDto, student); // AutoMapper mapping

                student.ModifiedDate = DateTime.UtcNow;
                var response = await myContainerRepository.UpdateAsync(student);

                var responseDto = myMapper.Map<StudentInfo>(response); // Map to response DTO

                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "An error occurred while updating the student.");
            }
        }

        [MapToApiVersion("1.0")]
        [HttpDelete]
        [Route("api/v{version:apiVersion}/student/{standard}/{Id}")]
        public async Task<IActionResult> DeleteStudentDetailAsync(string standard, string id)
        {
            await myContainerRepository.DeleteAsync(standard, id).ConfigureAwait(false);
            return Ok();
        }
    }
}
