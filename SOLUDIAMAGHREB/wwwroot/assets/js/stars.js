/*
Template Name: Skote - Admin & Dashboard Template
Author: Themesbrand
Version: 4.0.0.
Website: https://themesbrand.com/
Contact: themesbrand@gmail.com
File: Main Js File
*/


document.addEventListener("DOMContentLoaded", function () {
    flatpickr("#dateCreation", {
        //dateFormat: "d-m-Y"  // Specify your desired date format here
        dateFormat: "Y-m-d",  // Specify your desired date format here
        defaultDate: new Date()
    });


    // Convert number to words in French
    function numberToFrenchWords(number) {
        const ones = ["", "un", "deux", "trois", "quatre", "cinq", "six", "sept", "huit", "neuf"];
        const teens = ["dix", "onze", "douze", "treize", "quatorze", "quinze", "seize", "dix-sept", "dix-huit", "dix-neuf"];
        const tens = ["", "", "vingt", "trente", "quarante", "cinquante", "soixante", "soixante-dix", "quatre-vingt", "quatre-vingt-dix"];


        if (number === 0) return "zero"; // Fix for zero

        if (number < 10) return ones[number];

        if (number < 20) return teens[number - 10];

        if (number < 100) {
            let word = tens[Math.floor(number / 10)];
            if (number % 10 === 1 && Math.floor(number / 10) !== 8) {
                word += " et un";
            } else if (number % 10 > 0) {
                word += "-" + ones[number % 10];
            }
            return word;
        }

        if (number < 1000) {
            let word = (number >= 200 ? ones[Math.floor(number / 100)] + " cent" : "cent");
            if (number % 100 > 0) {
                word += " " + numberToFrenchWords(number % 100);
            }
            return word;
        }

        if (number < 1000000) {
            let word = (Math.floor(number / 1000) > 1 ? numberToFrenchWords(Math.floor(number / 1000)) + " mille" : "mille");
            if (number % 1000 > 0) {
                word += " " + numberToFrenchWords(number % 1000);
            }
            return word;
        }

        if (number < 1000000000) {
            let word = (Math.floor(number / 1000000) > 1 ? numberToFrenchWords(Math.floor(number / 1000000)) + " millions" : "un million");
            if (number % 1000000 > 0) {
                word += " " + numberToFrenchWords(number % 1000000);
            }
            return word;
        }

        if (number < 1000000000000) {
            let word = (Math.floor(number / 1000000000) > 1 ? numberToFrenchWords(Math.floor(number / 1000000000)) + " milliards" : "un milliard");
            if (number % 1000000000 > 0) {
                word += " " + numberToFrenchWords(number % 1000000000);
            }
            return word;
        }

        return number.toString();
    }

    // Event listener to update Montant dirham in words based on Montant de la T.V.A
    document.getElementById('montantTva').addEventListener('input', function () {
        let montantTva = parseFloat(this.value);
        if (!isNaN(montantTva)) {
            let montantDhText = numberToFrenchWords(montantTva);
            document.getElementById('montantDh').value = montantDhText + " dirhams";
        } else {
            document.getElementById('montantDh').value = '';
        }
    });

   

});



