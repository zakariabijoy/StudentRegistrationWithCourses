using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentRegistration.Api.DTOs;
using StudentRegistration.DataAccess.Repository.Interfaces;
using StudentRegistration.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentRegistration.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudentsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // GET: api/<StudentsController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var students =  await _unitOfWork.Students.GetAllAsync();
            return Ok(students);
        }

        // GET api/<StudentsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<StudentsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] StudentDto studentDto)
        {
            if(studentDto.RegNo > 0)
            {
                if (_unitOfWork.Students.IfStudentExists(studentDto.RegNo))
                {
                    return BadRequest("this Student Already Exists");

                }
                else
                {
                    var courses = await _unitOfWork.Courses.GetByIdListAsync(studentDto.CourseCheckBoxList);
                    if(courses.Count > 0)
                    {
                        var student = _mapper.Map<Student>(studentDto);
                        student.CourseList = courses;

                        var id = await _unitOfWork.Students.AddAsync(student);
                        return Ok(id);
                    }
                    else
                    {
                        return BadRequest("No Course Is Selected");
                    }
                    
                }
            }

            return BadRequest("Something is wrong");
        }

        // PUT api/<StudentsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<StudentsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
