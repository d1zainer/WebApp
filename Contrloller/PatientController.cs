using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.Model;
using WebApp.Services;

namespace WebApp.Contrloller
{

    [Route("api/MyApp")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly PatientService _patientService;

        public PatientController(PatientService patientService)
        {
            _patientService = patientService;

        }
        #region GET
        [HttpGet("GetAllPatient")]
        public ActionResult<IEnumerable<Patient>> GetPatients()
        {
            return Ok(_patientService.GetPatients());
        }
        #endregion

        #region POST

        [HttpPost("GetByName")] //api/MyApp/GetByName
        public ActionResult<Patient> GetPatientByName([FromBody] string name)
        {
            var patient = _patientService.GetPatient(name);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        [HttpPost("GetById")] //api/MyApp/GetById
        public ActionResult<Patient> GetPatientById([FromBody] Guid guid)
        {
            var patient = _patientService.GetPatient(guid);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        [HttpPost("AddPatient")] //api/MyApp/AddPatient
        public ActionResult<Patient> AddPatient([FromBody] Patient patient)
        {
            if (patient == null)
            {              
                return BadRequest("Данные пациента не указаны");
            }
            else
            {
                patient.Guid = Guid.NewGuid();
                _patientService.AddPatient(patient);
                DataJsonController.SavePatientsToJson(_patientService.GetPatients());
                return Ok(patient);
                
            }             
        }

        [HttpPost("DelPatient")] //api/MyApp/DelPatient
        public ActionResult<Patient> DeletePatient([FromBody] Guid guid)
        {
            Console.WriteLine($"Received GUID: {guid}");

            if (guid == Guid.Empty)
            {
                return BadRequest("GUID пациента не указан или имеет неверный формат.");
            }

            try
            {
                _patientService.DeletePatient(guid);
                DataJsonController.SavePatientsToJson(_patientService.GetPatients());
                return Ok($"Пациент с GUID '{guid}' удалён.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return NotFound(ex.Message);
            }
        }

        #endregion

        #region PUT
        [HttpPut("UpdatePatientById")] //api/MyApp/UpdatePatientById
        public ActionResult<Patient> UpdatePatientById([FromBody] Patient updatedPatient)
        {
            if (updatedPatient == null)
            {
                return BadRequest("Данные пациента не указаны.");
            }

            var existingPatient = _patientService.GetPatient(updatedPatient.Guid); // Предположим, что у пациента есть свойство Guid
            if (existingPatient == null)
            {
                return NotFound($"Пациент с идентификатором '{updatedPatient.Guid}' не найден.");
            }

            // Обновляем данные пациента
            existingPatient.Fullname = updatedPatient.Fullname;
            existingPatient.Birthday = updatedPatient.Birthday;
            existingPatient.Gender = updatedPatient.Gender;

            try
            {
                _patientService.UpdatePatient(existingPatient);
                DataJsonController.SavePatientsToJson(_patientService.GetPatients());
                return Ok(existingPatient);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
     
    }
}
