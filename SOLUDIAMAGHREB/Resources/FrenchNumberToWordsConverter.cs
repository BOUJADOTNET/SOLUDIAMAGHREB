namespace SOLUDIAMAGHREB.Resources
{

    //////////////Must Be etude ////////////
    public static class FrenchNumberToWordsConverter
    {
        private static readonly string[] unitsMap = { "zéro", "un", "deux", "trois", "quatre", "cinq", "six", "sept", "huit", "neuf", "dix", "onze", "douze", "treize", "quatorze", "quinze", "seize", "dix-sept", "dix-huit", "dix-neuf" };
        private static readonly string[] tensMap = { "vingt", "trente", "quarante", "cinquante", "soixante", "soixante", "quatre-vingt", "quatre-vingt" };

        public static string ConvertToWords(decimal number)
        {
            if (number == 0) return "zéro dirham";

            // Split the number into integer and decimal parts
            int integerPart = (int)Math.Floor(number);
            int decimalPart = (int)Math.Round((number - integerPart) * 100);

            string result = "";

            // Convert integer part
            if (integerPart > 0)
            {
                result = ConvertNumberToWords(integerPart) + " dirham" + (integerPart > 1 ? "s" : "");
            }

            // Add decimal part if exists
            if (decimalPart > 0)
            {
                if (result.Length > 0) result += " et ";
                result += ConvertNumberToWords(decimalPart) + " centime" + (decimalPart > 1 ? "s" : "");
            }

            return result;
        }

        private static string ConvertNumberToWords(int number)
        {
            if (number < 20) return unitsMap[number];

            if (number < 100)
            {
                int tens = number / 10 - 2;
                int units = number % 10;

                string result = tensMap[tens];

                if (tens == 5 || tens == 6) // Special case for 70-79 and 90-99
                {
                    if (units == 1) return result + " et " + "onze";
                    if (units > 0) return result + "-" + unitsMap[units + 10];
                    return result + "-dix";
                }

                if (tens == 7 || tens == 8) // Special case for 80-89 and 90-99
                {
                    if (units == 0 && tens == 7) return result + "s";
                    if (units > 0) return result + "-" + unitsMap[units];
                    return result;
                }

                if (units == 1 && tens != 7) return result + " et " + unitsMap[units];
                if (units > 0) return result + "-" + unitsMap[units];
                return result;
            }

            if (number < 1000)
            {
                int hundreds = number / 100;
                int remainder = number % 100;

                string result = hundreds == 1 ? "cent" : unitsMap[hundreds] + " cent";
                if (remainder > 0) result += " " + ConvertNumberToWords(remainder);
                return result;
            }

            if (number < 1000000)
            {
                int thousands = number / 1000;
                int remainder = number % 1000;

                string result = thousands == 1 ? "mille" : ConvertNumberToWords(thousands) + " mille";
                if (remainder > 0) result += " " + ConvertNumberToWords(remainder);
                return result;
            }

            if (number < 1000000000)
            {
                int millions = number / 1000000;
                int remainder = number % 1000000;

                string result = millions == 1 ? "un million" : ConvertNumberToWords(millions) + " millions";
                if (remainder > 0) result += " " + ConvertNumberToWords(remainder);
                return result;
            }

            int billions = number / 1000000000;
            int remainderBillions = number % 1000000000;

            string resultBillions = billions == 1 ? "un milliard" : ConvertNumberToWords(billions) + " milliards";
            if (remainderBillions > 0) resultBillions += " " + ConvertNumberToWords(remainderBillions);
            return resultBillions;
        }
    }
}
