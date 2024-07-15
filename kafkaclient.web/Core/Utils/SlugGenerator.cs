using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace kafkaclient.web.Core.Utils;

public class SlugGenerator
{
    public static string GenerateSlug(string phrase)
    {
        string str = RemoveAccents(phrase).ToLower();
        str = Regex.Replace(str, @"[^a-z0-9\s-]", ""); // Eliminar caracteres no deseados
        str = Regex.Replace(str, @"\s+", " ").Trim(); // Eliminar espacios adicionales
        str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim(); // Limitar longitud a 45 caracteres
        str = Regex.Replace(str, @"\s", "-"); // Reemplazar espacios por guiones
        return str;
    }

    private static string RemoveAccents(string text)
    {
        StringBuilder sb = new StringBuilder();
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}