namespace API.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateTime dateTimeInput)
        {
            var today = DateTime.Today;
            var age = today.Year - dateTimeInput.Year;
            if (dateTimeInput.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
