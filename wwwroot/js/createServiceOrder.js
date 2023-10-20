document.addEventListener("DOMContentLoaded", function () {
    toggleNewCustomerSection();
});

// If existing customer isn't chosen from dropdown, then new customer fields show in form and are required.
function toggleNewCustomerSection() {
    const dropdown = document.getElementById('existingCustomer');
    const newCustomerSection = document.getElementById('newCustomerSection');
    const inputs = newCustomerSection.querySelectorAll('input');

    if (dropdown.value === "") {
        newCustomerSection.style.display = 'block';
        inputs.forEach(input => input.required = true);
    } else {
        newCustomerSection.style.display = 'none';
        inputs.forEach(input => input.required = false);
    }
}
