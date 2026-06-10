using HealthAppWebApi.Services.Interface;
using SharedDto.DoctorDtos;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace HealthAppWebApi.Controllers
{
    [RoutePrefix("api/doctors")]
    public class DoctorsController : ApiController
    {
        private readonly IDoctorService _service;

        public DoctorsController(
            IDoctorService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult>
            GetAll()
        {
            var doctors =
                await _service
                    .GetAllDoctorsAsync();

            return Ok(doctors);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult>
            Get(int id)
        {
            var doctor =
                await _service
                    .GetDoctorByIdAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            return Ok(doctor);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult>
            Create(CreateDoctorDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(
                        ModelState);
                }

                await _service
                    .AddDoctorAsync(dto);

                return Ok(
                    "Doctor added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult>
            Update(
                int id,
                CreateDoctorDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(
                        ModelState);
                }

                await _service
                    .UpdateDoctorAsync(
                        id,
                        dto);

                return Ok(
                    "Doctor updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }

        [HttpPatch]
        [Route("{id:int}/status")]
        public async Task<IHttpActionResult>
            ChangeStatus(
                int id,
                bool isActive)
        {
            try
            {
                await _service
                    .ChangeStatusAsync(
                        id,
                        isActive);

                return Ok(
                    "Doctor status updated.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }

        [HttpGet]
        [Route("specialisation/{specialisation}")]
        public async Task<IHttpActionResult>
            GetBySpecialisation(
                string specialisation)
        {
            try
            {
                var doctors =
                    await _service
                        .GetDoctorsBySpecialisationAsync(
                            specialisation);

                return Ok(doctors);
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }

        [HttpGet]
        [Route("search")]
        public async Task<IHttpActionResult>
            SearchByName(
                string name)
        {
            var doctors =
                await _service
                    .SearchByNameAsync(
                        name);

            return Ok(doctors);
        }
    }
}