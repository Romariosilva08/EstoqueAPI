using System.Text.Json;
using MinhaAPIEstoque.Models;

namespace MinhaAPIEstoque.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<T>> GetEstoqueFromApi<T>(string apiUrl)
        {
            var response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<T>>(responseStream);
        }

        public async Task<ApiResponse<T>> PostEstoqueToApi<T>(string apiUrl, Produtos produto)
        {
            var jsonContent = JsonSerializer.Serialize(produto);
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(apiUrl, content);
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<T>>(responseStream);
        }
    }
}
