using HealthAppWebApi.Services.Interface;
using SharedDto.HealthRecordDtos;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace HealthAppWebApi.Controllers
{
    [RoutePrefix("api/healthrecords")]
    public class HealthRecordsController
        : ApiController
    {
        private readonly
            IHealthRecordService _service;

        public HealthRecordsController(
            IHealthRecordService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult>
            GetAll()
        {
            var records =
                await _service.GetAllAsync();

            return Ok(records);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult>
            GetById(int id)
        {
            try
            {
                var record =
                    await _service
                        .GetByIdAsync(id);

                return Ok(record);
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }

        [HttpGet]
        [Route("patient/{patientId:int}")]
        public async Task<IHttpActionResult>
            GetPatientHistory(
                int patientId)
        {
            var records =
                await _service
                    .GetPatientHistoryAsync(
                        patientId);

            return Ok(records);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult>
            Add(
                CreateHealthRecordDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(
                        ModelState);
                }

                await _service.AddAsync(dto);

                return Ok(
                    "Health Record added successfully. Appointment completed.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }
    }
}