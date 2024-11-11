namespace ApiTalking.Helpers;

public class Converter
{
    public static DateOnly convertStringToDateOnly(string dateString)
    {
        if (DateOnly.TryParse(dateString, out DateOnly dateOnly))
        {
            return dateOnly;
        }
        throw new FormatException("El formato de fecha no es válido.");
    }

    public static string convertDateOnlyToString(DateOnly dateOnly)
    {
        return dateOnly.ToString("dd-MM-yyyy"); // Cambia el formato según sea necesario
    }


    public static DateTime convertStringToDateTime(string dateTimeString)
    {
        return DateTime.ParseExact(dateTimeString, "yyyy-MM-dd HH:mm:ss", null); // Asegúrate de que el formato coincida
    }


    public static string convertDateTimeToString(DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss"); // Formato de ejemplo
    }


}