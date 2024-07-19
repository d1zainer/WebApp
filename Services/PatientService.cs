using Newtonsoft.Json;
using System.Threading;
using System.Xml.Linq;
using WebApp.Contrloller;
using WebApp.Interfaces;
using WebApp.Model;

namespace WebApp.Services
{
    public class PatientService : IPatientService
    {
        private readonly List<Patient> _patients;
        private readonly Mutex _mutex = new Mutex();

        public PatientService()
        {
            _patients = DataJsonController.LoadPatientsFromJson();
        }

        /// <summary>
        /// Получить список пациентов
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Patient> GetPatients()
        {
            return _patients;
        }

        /// <summary>
        /// получить пациента по имени
        /// </summary>
        /// <param name="fullname"></param>
        /// <returns></returns>
        public Patient GetPatient(string fullname)
        {
            return _patients.Find(p => p.Fullname == fullname);
        }

        /// <summary>
        /// обновить пациента по GUID
        /// </summary>
        /// <param name="patient"></param>
        /// <exception cref="ArgumentException"></exception>
        public void UpdatePatient(Patient patient)
        {
            _mutex.WaitOne();
            try
            {
                var existingPatient = _patients.FirstOrDefault(p => p.Fullname == patient.Fullname);
                if (existingPatient != null)
                {
                    existingPatient.Fullname = patient.Fullname;
                    existingPatient.Birthday = patient.Birthday;
                    existingPatient.Gender = patient.Gender;
                    // Возможно, нужно обновить другие поля
                }
                else
                {
                    throw new ArgumentException("Пациент не найден");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении пациента: {ex.Message}");
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// удаляет пациента по имени
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="ArgumentException"></exception>
        public void DeletePatient(string name)
        {
            _mutex.WaitOne();
            try
            {
                var existingPatient = _patients.FirstOrDefault(p => p.Fullname == name);
                if (existingPatient != null)
                {
                    _patients.Remove(existingPatient);
                }
                else
                {
                    throw new ArgumentException("Пациент не найден");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении пациента: {ex.Message}");
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// удаляет пациента по GUID
        /// </summary>
        /// <param name="guid"></param>
        /// <exception cref="ArgumentException"></exception>
        public void DeletePatient(Guid guid)
        {
            _mutex.WaitOne();
            try
            {
                var existingPatient = _patients.FirstOrDefault(p => p.Guid == guid);
                Console.WriteLine(existingPatient != null ? $"Patient found: {existingPatient.Fullname}" : "Patient not found");
                if (existingPatient != null)
                {
                    _patients.Remove(existingPatient);
                }
                else
                {
                    throw new ArgumentException("Пациент не найден");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении пациента: {ex.Message}");
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// получает пациента по GUID
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Patient GetPatient(Guid guid)
        {
            if (guid == Guid.Empty)
            {
                throw new ArgumentException("Пациент не найден");
            }
            else
            {
                return _patients.Find(p => p.Guid == guid);
            }
        }

        /// <summary>
        /// добавляет пользователя
        /// </summary>
        /// <param name="patient"></param>
        /// <exception cref="ArgumentException"></exception>
        public void AddPatient(Patient patient)
        {
            if (patient != null)
            {
                _patients.Add(patient);
            }
            else
            {
                throw new ArgumentException("Пациент не добавлен так как он пустой!");
            }
        }
    }
}
