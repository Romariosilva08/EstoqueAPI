using System.Globalization;

namespace MinhaAPIEstoque.Utils
{
    public class ConversorData
    {
        public static DateTime ConverterData(string data)
        {
            // Defina o formato da data esperada
            string formatoData = "dd/MM/yyyy";

            // Tente converter a string de data para DateTime usando o formato especificado
            if (DateTime.TryParseExact(data, formatoData, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataConvertida))
            {
                return dataConvertida;
            }
            else
            {
                throw new ArgumentException("Formato de data inválido.");
            }
        }
    }
}
