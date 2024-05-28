// References: 
// https://stackoverflow.com/questions/2954962/convert-integer-to-binary-in-c-sharp
// The RomanNumeral.cs file in this repo 

using DatesService.Utility; // Import necessary utility classes
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DatesService
{
    public static class Binary
    {
        // Function to convert date to binary format
        [FunctionName("ToBinary")]
        public static async Task<IActionResult> ToBinary(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "binary")] HttpRequest req,
            ILogger log)
        {
            // Log function invocation
            log.LogInformation("HTTP trigger function 'ToBinary' processed a request.");

            // Extract date input from the HTTP request
            string input = await HTTPInputHelper.getDateStringOrCurrentDateString(req);

            // Validate the input date
            if (!DateTime.TryParse(input, out DateTime date))
            {
                return new BadRequestObjectResult("Invalid date format. Please provide a valid date in the format 'mm/dd/yyyy'");
            }

            // Check if the input date is within the allowed year range 2000-2100
            const int maxYear = 2100;
            const int minYear = 2000;
            if (date.Year > maxYear || date.Year < minYear)
            {
                return new BadRequestObjectResult($"Date out of range. Please provide a date within the range '01/01/{minYear}' to '12/31/{maxYear}'.");
            }

            // Check if the input date is within the allowed month range 01-12
            const int maxMonth = 12;
            const int minMonth = 1;
            if (date.Month > maxMonth || date.Month < minMonth)
            {
                return new BadRequestObjectResult($"Date out of range. Please provide a month within the range 01 to 12.");
            }

            // Check if the input date is within the allowed day range 01-31
            const int maxDay = 31;
            const int minDay = 1;
            if (date.Day > maxDay || date.Day < minDay)
            {
                return new BadRequestObjectResult($"Date out of range. Please provide a day within the range 01 to 31.");
            }

            // Generate a string representing the current date in 'mm/dd/yyyy' format
            string result = $"Current date: {date.Month:D2}/{date.Day:D2}/{date.Year}";

            // Append the binary representation of date components to the result string
            result += $"\nCurrent date in binary format: {ToBinary(date.Month)}/{ToBinary(date.Day)}/{ToBinary(date.Year)}";

            // Return the result as an HTTP response
            return new OkObjectResult(result);
        }

        // Helper function to convert an integer to a binary string
        private static string ToBinary(int num)
        {
            string binary = Convert.ToString(num, 2);
            return binary; 
        }
    }
}

