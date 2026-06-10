using HealthAppWebApi.Services.Interface;
using SharedDto.AppointmentDtos;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace HealthAppWebApi.Controllers
{
    [RoutePrefix("api/appointments")]
    public class AppointmentsController
        : ApiController
    {
        private readonly
            IAppointmentService _service;

        public AppointmentsController(
            IAppointmentService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult>
            GetAll()
        {
            var appointments =
                await _service
                    .GetAllAppointmentsAsync();

            return Ok(appointments);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult>
            Get(int id)
        {
            try
            {
                var appointment =
                    await _service
                        .GetAppointmentByIdAsync(
                            id);

                return Ok(appointment);
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult>
            Book(
                CreateAppointmentDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(
                        ModelState);
                }

                await _service
                    .BookAppointmentAsync(
                        dto);

                return Ok(
                    "Appointment booked successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}/confirm")]
        public async Task<IHttpActionResult>
            Confirm(int id)
        {
            try
            {
                await _service
                    .ConfirmAppointmentAsync(
                        id);

                return Ok(
                    "Appointment confirmed.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}/cancel")]
        public async Task<IHttpActionResult>
            Cancel(
                int id,
                CancelAppointmentDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(
                        ModelState);
                }

                await _service
                    .CancelAppointmentAsync(
                        id,
                        dto.CancellationReason);

                return Ok(
                    "Appointment cancelled.");
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
            GetPatientAppointments(
                int patientId)
        {
            var appointments =
                await _service
                    .GetAppointmentsForPatientAsync(
                        patientId);

            return Ok(appointments);
        }

        [HttpGet]
        [Route("upcoming")]
        public async Task<IHttpActionResult>
            GetUpcomingAppointments()
        {
            var appointments =
                await _service
                    .GetUpcomingAppointmentsAsync();

            return Ok(appointments);
        }

        [HttpGet]
        [Route("upcoming/doctor")]
        public async Task<IHttpActionResult>
            GetUpcomingAppointmentsByDoctor(
                string doctorName)
        {
            var appointments =
                await _service
                    .GetUpcomingAppointmentsByDoctorAsync(
                        doctorName);

            return Ok(appointments);
        }

        [HttpGet]
        [Route("available-slots")]
        public async Task<IHttpActionResult>
            GetAvailableSlots(
                int doctorId,
                DateTime scheduledDate)
        {
            var slots =
                await _service
                    .GetAvailableSlotsAsync(
                        doctorId,
                        scheduledDate);

            return Ok(slots);
        }

        [HttpGet]
        [Route("search/patient")]
        public async Task<IHttpActionResult>
            SearchByPatientName(
                string patientName)
        {
            var appointments =
                await _service
                    .GetAppointmentsByPatientNameAsync(
                        patientName);

            return Ok(appointments);
        }

        [HttpGet]
        [Route("{appointmentId:int}/healthrecord-exists")]
        public async Task<IHttpActionResult>
            HealthRecordExists(
                int appointmentId)
        {
            bool exists =
                await _service
                    .HealthRecordExistsAsync(
                        appointmentId);

            return Ok(exists);
        }
    }
}
