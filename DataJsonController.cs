using Newtonsoft.Json;
using WebApp.Model;

namespace WebApp
{
    public class DataJsonController
    {
        /// <summary>
        /// загружает список пациентов
        /// </summary>
        /// <returns></returns>
        public static List<Patient> LoadPatientsFromJson()
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "list_patient.json");
                Console.WriteLine($"Путь к файлу: {filePath}");

                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Файл не найден");
                    return new List<Patient>();
                }

                var json = File.ReadAllText(filePath);

                return JsonConvert.DeserializeObject<List<Patient>>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке пациентов из JSON файла: {ex.Message}");
                return new List<Patient>();
            }
        }

        /// <summary>
        /// Сохраняет список пациентов
        /// </summary>
        /// <param name="patients"></param>
        public static void SavePatientsToJson(IEnumerable<Patient> patients)
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "list_patient.json");
                Console.WriteLine($"Путь к файлу: {filePath}");

                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Файл не найден");
                }

                var json = JsonConvert.SerializeObject(patients);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке пациентов из JSON файла: {ex.Message}");
            }
        }


    }


}
