/*
Template Name: Skote - Admin & Dashboard Template
Author: Themesbrand
Version: 4.0.0.
Website: https://themesbrand.com/
Contact: themesbrand@gmail.com
File: Main Js File
*/

// Add this JavaScript code to your existing script section in GenerateActEngage.cshtml
document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('actEngageForm');
    const bordereauSelect = document.getElementById('selectedBordereau');
    const appelOffresInput = document.getElementById('appelOffres');
    const nLotContainer = document.getElementById('nLotContainer');

    // Handle bordereau selection change
    bordereauSelect.addEventListener('change', function () {
        const selectedBordereau = this.value;
        if (!selectedBordereau) {
            resetForm();
            return;
        }

        // Add loading indicators
        appelOffresInput.value = 'Loading...';
        nLotContainer.innerHTML = '<div class="text-center">Loading...</div>';

        fetch(`/Filemanager/GetBordereauDetails?bordereauNumber=${selectedBordereau}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                // Fill Appel Offres
                appelOffresInput.value = data.appelOffres || '';

                // Create NLot checkboxes
                if (data.nLots && Array.isArray(data.nLots)) {
                    const nLotHtml = data.nLots.map(lot => `
                            <div class="form-check">
                                <input class="form-check-input" type="checkbox"
                                       id="lot${lot}"
                                       value="${lot}"
                                       name="selectedLots">
                                <label class="form-check-label" for="lot${lot}">
                                    Lot ${lot}
                                </label>
                            </div>
                        `).join('');
                    nLotContainer.innerHTML = nLotHtml || 'No lots available';
                } else {
                    nLotContainer.innerHTML = 'No lots available';
                }
            })
            .catch(error => {
                console.error('Error:', error);
                appelOffresInput.value = '';
                nLotContainer.innerHTML = 'Error loading data';
                alert('Error loading bordereau details: ' + error.message);
            });
    });

    // Handle form submission
    form.addEventListener('submit', function (e) {
        e.preventDefault();

        // Get selected lots
        const selectedLots = Array.from(document.querySelectorAll('input[name="selectedLots"]:checked'))
            .map(cb => cb.value)
            .join(',');

        // Create form data object
        const formData = {
            IdactEng: `ACT${new Date().getTime()}`,
            NomberBordereau: bordereauSelect.value,
            Appel_Offres: appelOffresInput.value,
            Objet_du_Marche: document.getElementById('objetMarche').value,
            Marche_passe: document.getElementById('marchePasse').value,
            Capital: document.getElementById('capital').value,
            NLot: selectedLots,
            TauxTva: parseInt(document.getElementById('tauxTva').value) || 0,
            MontantHtTva: parseInt(document.getElementById('montantHtTva').value) || 0,
            MontantTva: parseInt(document.getElementById('montantTva').value) || 0,
            MontantDh: document.getElementById('montantDh').value,
            MontantTvaComprise: parseInt(document.getElementById('montantTvaComprise').value) || 0,
            DateCreation: document.getElementById('dateCreation').value
        };

        // Send data to server
        fetch('/Filemanager/SaveActEngage', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(formData)
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    alert('Act Engage saved successfully!');
                    window.location.href = '/Filemanager/ListeActEngage'; // Redirect to list page
                } else {
                    alert('Error saving Act Engage: ' + data.message);
                }
            })
            .catch(error => {
                console.error('Error:', error);
                alert('Error saving Act Engage');
            });
    });

    // Reset form helper function
    function resetForm() {
        appelOffresInput.value = '';
        nLotContainer.innerHTML = '';
        form.reset();
    }
});