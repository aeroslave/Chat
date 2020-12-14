namespace ChatClient.Services
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using ChatClient.Interfaces;
    using ChatClient.Models;
    using ChatClient.Utilites;

    using Newtonsoft.Json;

    /// <summary>
    /// Сервис взаимодействия с API.
    /// </summary>
    public class PersonService : IPersonService
    {
        /// <summary>
        /// Сервис взаимодействия с API.
        /// </summary>
        public PersonService()
        {
            HttpClient = new HttpClient();
            Address = ConnectionUtils.GetAddressConnection().Address;
        }

        /// <summary>
        /// Адрес.
        /// </summary>
        private string Address { get; }

        /// <summary>
        /// Http клиент.
        /// </summary>
        private HttpClient HttpClient { get; }

        /// <summary>
        /// Адрес веб апи.
        /// </summary>
        private string WebApiAddress => Address + "/api/chat";

        /// <summary>
        /// Добавить сообщение.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        public async Task<bool> AddMessage(Message message)
        {
            var jsonInString = JsonConvert.SerializeObject(message);
            var uri = $"{WebApiAddress}/addMessage";

            var response = await HttpClient.PostAsync(uri,
                new StringContent(jsonInString, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Проверить активность пользователя.
        /// </summary>
        /// <param name="person">Пользователь.</param>
        /// <returns>True - если пользователь активен.</returns>
        public async Task<string> CheckPersonActivityAsync(Person person)
        {
            var jsonInString = JsonConvert.SerializeObject(person);
            var responseActivity = await HttpClient.PutAsync(WebApiAddress + "/isActive",
                new StringContent(jsonInString, Encoding.UTF8, "application/json"));

            return await responseActivity.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Проверить существование пользователя.
        /// </summary>
        /// <param name="person">Пользователь.</param>
        /// <returns>True - если пользователь существует.</returns>
        public async Task<bool> CheckPersonExistingAsync(Person person)
        {
            var jsonInString = JsonConvert.SerializeObject(person);
            var responsePersonExist = await HttpClient.PutAsync(WebApiAddress,
                new StringContent(jsonInString, Encoding.UTF8, "application/json"));

            return responsePersonExist.IsSuccessStatusCode;
        }

        /// <summary>
        /// Создать пользователя.
        /// </summary>
        /// <param name="person">Пользователь.</param>
        /// <returns>True - если пользователь создан.</returns>
        public async Task<bool> CreatePersonAsync(Person person)
        {
            var jsonInString = JsonConvert.SerializeObject(person);
            var response = await HttpClient.PostAsync(WebApiAddress,
                new StringContent(jsonInString, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Получить список сообщений.
        /// </summary>
        public async Task<List<Message>> GetMessages()
        {
            var uri = $"{WebApiAddress}/getMessages";

            var response = await HttpClient.GetAsync(uri);
            var responseResult = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Message>>(responseResult);
        }

        /// <summary>
        /// Получить список пользователей асинхронно.
        /// </summary>
        public async Task<List<Person>> GetPersonsAsync()
        {
            var response = await HttpClient.GetAsync(WebApiAddress);
            var responseResult = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Person>>(responseResult);
        }

        /// <summary>
        /// Разлогинить пользователя.
        /// </summary>
        /// <param name="person">Пользователь.</param>
        /// <returns>True - если успешно.</returns>
        public async Task<bool> LogOutAsync(Person person)
        {
            var jsonInString = JsonConvert.SerializeObject(person);
            var uri = $"{WebApiAddress}/setactivityfalse";
            var response = await HttpClient.PutAsync(uri,
                new StringContent(jsonInString, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }
    }
}