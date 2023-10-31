document.addEventListener("DOMContentLoaded", function () {
    toggleNewCustomerSection();
});

// If existing customer isn't chosen from dropdown, then new customer fields show in form and are required.
function toggleNewCustomerSection() {
    const dropdown = document.getElementById('CustomerId');
    const newCustomerSection = document.getElementById('newCustomerSection');
    const inputElements = newCustomerSection.querySelectorAll('input');

    if (dropdown.value == "0") {
        inputElements.forEach((input) => {
            input.classList.remove("greyed-out");
            input.required = true;
            input.disabled = false;
        });
    } else {
        inputElements.forEach((input) => {
            input.classList.add("greyed-out");
            input.required = false;
            input.disabled = true;
        });
    }
}