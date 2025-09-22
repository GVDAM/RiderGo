using System.Text.RegularExpressions;

namespace RiderGo.Domain.Extension
{
    public static class StringExtensions
    {
        public static string CleanUpCnpj(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            return Regex.Replace(str, @"[^\d]", "");
        }

        public static bool IsValidCnpj(this string str)
        {
            var cleanedCnpj = str.CleanUpCnpj();

            if (cleanedCnpj.Length != 14 || cleanedCnpj.Distinct().Count() == 1)
                return false;

            switch (cleanedCnpj)
            {
                case "00000000000000":
                case "11111111111111":
                case "22222222222222":
                case "33333333333333":
                case "44444444444444":
                case "55555555555555":
                case "66666666666666":
                case "77777777777777":
                case "88888888888888":
                case "99999999999999":
                    return false;
            }


            int[] multiplicators1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int sum = 0;

            string tempCnpj = cleanedCnpj.Substring(0, 12);
            for (int i = 0; i < 12; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * multiplicators1[i];

            int remainder = (sum % 11);
            if (remainder < 2)
                remainder = 0;
            else
                remainder = 11 - remainder;

            if (int.Parse(cleanedCnpj[12].ToString()) != remainder)
                return false;


            int[] multiplicators2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            
            string digit = remainder.ToString();
            tempCnpj += digit;
            sum = 0;

            for (int i = 0; i < 13; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * multiplicators2[i];

            remainder = (sum % 11);
            if (remainder < 2)
                remainder = 0;
            else
                remainder = 11 - remainder;

            digit += remainder.ToString();
            return cleanedCnpj.EndsWith(digit);
        }
    }
}
