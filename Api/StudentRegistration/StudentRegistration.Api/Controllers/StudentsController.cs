using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentRegistration.Api.DTOs;
using StudentRegistration.Utility.Helper;
using StudentRegistration.DataAccess.Repository.Interfaces;
using StudentRegistration.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentRegistration.Utility.Extenstions;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentRegistration.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<StudentsController> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        // GET: api/<StudentsController>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] StudentsParams studentsParams)
        {
            var students = await _unitOfWork.Students.GetAllAsyncWithPaginationAsync(studentsParams);
            Response.AddPaginationHeader(students.CurrentPage, students.PageSize, students.TotalCount, students.TotalPages);
            return Ok(students);
        }

        // GET api/<StudentsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id > 0)
            {
                var student = await _unitOfWork.Students.GetByIdAsync(id);
                if (student != null)
                {
                    var stuDto = _mapper.Map<StudentDto>(student);
                    stuDto.CourseCheckBoxes = _mapper.Map<List<CourseCheckBoxDto>>(await _unitOfWork.Courses.GetAllAsync());
                    for (int i = 0; i < stuDto.CourseCheckBoxes.Count ; i++)
                    {
                        for (int j = 0; j < student.CourseList.Count; j++)
                        {
                            if (student.CourseList[j].CourseId == stuDto.CourseCheckBoxes[i].Id)
                            {
                                stuDto.CourseCheckBoxes[i].Ischecked = true;
                            }
                        }
                       
                    }
                    return Ok(stuDto);
                }
                else
                {
                    return NotFound("Student Not Found");
                }
            }
            else
            {
                return BadRequest("This Student Id is invalid");
            }
           

        }

        // POST api/<StudentsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] StudentDto studentDto)
        {
            if (studentDto.StudentId == 0)
            {
                if (studentDto.RegNo > 0)
                {
                    if (_unitOfWork.Students.IfStudentExists(studentDto.RegNo))
                    {
                        return BadRequest("this Student Already Exists");

                    }
                    else
                    {
                        var courses = await _unitOfWork.Courses.GetByIdListAsync(studentDto.CourseCheckBoxList);
                        if (courses.Count > 0)
                        {
                            var student = _mapper.Map<Student>(studentDto);
                            student.CourseList = courses;

                            try
                            {
                                var id = await _unitOfWork.Students.AddAsync(student);
                                return Ok(id);
                            }
                            catch (Exception ex)
                            {

                                _logger.LogError(ex.Message);
                            }
                            
                        }
                        else
                        {
                            return BadRequest("No Course Is Selected");
                        }

                    }
                }
            }
            else
            {
                var courses = await _unitOfWork.Courses.GetByIdListAsync(studentDto.CourseCheckBoxList);
                if (courses.Count > 0)
                {
                    var student = _mapper.Map<Student>(studentDto);
                    student.CourseList = courses;
                    try
                    {
                        var id = await _unitOfWork.Students.UpdateAsync(student);
                        return Ok(id);
                    }
                    catch (Exception ex)
                    {

                        _logger.LogError(ex.Message);
                    }
                    
                }
                else
                {
                    return BadRequest("No Course Is Selected");
                }
            }
            return BadRequest("Something is wrong");
        }


        // DELETE api/<StudentsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if(id > 0)
            {
                var deletedId = await _unitOfWork.Students.DeleteAsync(id);
                
                if(deletedId != 0)
                {
                    return Ok(deletedId);
                }
                else
                {
                    return NotFound("Student not found to delete");
                }
                
            }
            
            return BadRequest("This Student Id is invalid");
            
        }
    }
}
