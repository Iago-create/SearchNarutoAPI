using System.Net.Http.Json;
using SearchNarutoAPI.Models;

namespace SearchNarutoAPI.Services;

public class NarutoApiService
{
    private readonly HttpClient _http;

    public NarutoApiService(HttpClient http) => _http = http;

    public Task<CharactersResponse?> ListarAsync(int page = 1, int limit = 12) =>
        _http.GetFromJsonAsync<CharactersResponse>($"characters?page={page}&limit={limit}");

    public Task<CharactersResponse?> BuscarPorNomeAsync(string nome, int page = 1, int limit = 12) =>
        _http.GetFromJsonAsync<CharactersResponse>(
            $"characters?name={Uri.EscapeDataString(nome)}&page={page}&limit={limit}");

    public Task<Character?> ObterPorIdAsync(int id) =>
        _http.GetFromJsonAsync<Character>($"characters/{id}");
}