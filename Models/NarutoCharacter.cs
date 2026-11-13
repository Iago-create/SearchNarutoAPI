using System.Text.Json;
using System.Text.Json.Serialization;

namespace SearchNarutoAPI.Models;

public class CharactersResponse
{
    [JsonPropertyName("characters")]
    public List<Character> Characters { get; set; } = new();

    [JsonPropertyName("totalCharacters")]
    public int TotalCharacters { get; set; }
}

public class Character
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("images")]
    public List<string>? Images { get; set; }

    [JsonPropertyName("personal")]
    public Personal? Personal { get; set; }

    [JsonPropertyName("jutsu")]
    public List<string>? Jutsu { get; set; }

    [JsonPropertyName("rank")]
    public Rank? Rank { get; set; }

    public string? PrimeiraImagem => Images?.FirstOrDefault(i => !string.IsNullOrWhiteSpace(i));

    public string ClaOuAfiliacao
    {
        get
        {
            var cla = JsonHelper.ToText(Personal?.Clan);
            if (!string.IsNullOrWhiteSpace(cla)) return cla;
            var afiliacao = JsonHelper.ToText(Personal?.Affiliation);
            return string.IsNullOrWhiteSpace(afiliacao) ? "Não informado" : afiliacao;
        }
    }

    public string PatenteNinja
    {
        get
        {
            var patente = JsonHelper.ToText(Rank?.NinjaRank);
            return string.IsNullOrWhiteSpace(patente) ? "Não informada" : patente;
        }
    }
}

public class Personal
{
    [JsonPropertyName("clan")]
    public JsonElement? Clan { get; set; }

    [JsonPropertyName("affiliation")]
    public JsonElement? Affiliation { get; set; }
}

public class Rank
{
    [JsonPropertyName("ninjaRank")]
    public JsonElement? NinjaRank { get; set; }
}

public static class JsonHelper
{
    public static string ToText(JsonElement? element)
    {
        if (element is null) return string.Empty;
        var e = element.Value;

        switch (e.ValueKind)
        {
            case JsonValueKind.String:
                return e.GetString() ?? string.Empty;
            case JsonValueKind.Number:
            case JsonValueKind.True:
            case JsonValueKind.False:
                return e.ToString();
            case JsonValueKind.Array:
                return string.Join(", ", e.EnumerateArray()
                    .Select(i => ToText(i))
                    .Where(s => !string.IsNullOrWhiteSpace(s)));
            case JsonValueKind.Object:
                return string.Join(" | ", e.EnumerateObject()
                    .Select(p => $"{p.Name}: {ToText(p.Value)}")
                    .Where(s => !s.EndsWith(": ")));
            default:
                return string.Empty;
        }
    }
}