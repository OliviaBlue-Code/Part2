using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace demo
{
    public class NLPProcessor
    {
        // MainWindow calls ExtractDate() - so we need this exact name
        public DateTime? ExtractDate(string text)
        {
            text = text.ToLower();
            DateTime now = DateTime.Now;

            if (text.Contains("tomorrow"))
            {
                DateTime date = now.AddDays(1).Date.AddHours(9);
                Match timeMatch = Regex.Match(text, @"(\d{1,2})(am|pm)");
                if (timeMatch.Success)
                {
                    int hour = int.Parse(timeMatch.Groups[1].Value);
                    if (timeMatch.Groups[2].Value == "pm" && hour != 12) hour += 12;
                    if (timeMatch.Groups[2].Value == "am" && hour == 12) hour = 0;
                    date = date.AddHours(hour - 9);
                }
                return date;
            }

            if (text.Contains("next week"))
                return now.AddDays(7).Date.AddHours(9);

            Match dateMatch = Regex.Match(text, @"(\d{1,2})/(\d{1,2})/(\d{4})");
            if (dateMatch.Success)
            {
                if (DateTime.TryParseExact(dateMatch.Value, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                    return parsedDate.Date.AddHours(9);
            }
            return null;
        }

        // MainWindow calls ExtractTitle() - so we need this exact name
        public string ExtractTitle(string text)
        {
            text = text.ToLower();
            text = text.Replace("remind me to ", "");
            text = Regex.Replace(text, @"(tomorrow|next week|\d{1,2}/\d{1,2}/\d{4}|\d{1,2}(am|pm))", "").Trim();
            text = Regex.Replace(text, @"\s+", " ").Trim();
            if (text.Length > 0)
                return char.ToUpper(text[0]) + text.Substring(1);
            return "Untitled Task";
        }
    }
}