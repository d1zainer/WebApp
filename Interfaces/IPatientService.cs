using WebApp.Model;

namespace WebApp.Interfaces
{
    public interface IPatientService
    {
        Patient GetPatient(string fullname);       
        Patient GetPatient(Guid guid);       
        IEnumerable <Patient> GetPatients();
        void UpdatePatient(Patient patient);
        void DeletePatient(Guid guid);
        void DeletePatient(string name);

        void AddPatient(Patient patient);

    }
}
