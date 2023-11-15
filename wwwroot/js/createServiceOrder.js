document.addEventListener("DOMContentLoaded", function () {
    
    const form = document.getElementById('create_order_form');
    if (form) {
        form.addEventListener('submit', handleFormSubmit);
    }

    const dropdown = document.getElementById('CustomerId');
    if (dropdown) {

        dropdown.addEventListener('change', toggleNewCustomerSection);
    }
});

function handleFormSubmit(event) {
    event.preventDefault(); // Pause sub

    this.submit(); // Exe real sub
}

// If existing customer isn't chosen from dropdown, then new customer fields show in form and are required.

function toggleNewCustomerSection() {
    const dropdown = document.getElementById('CustomerId');
    const newCustomerSection = document.getElementById('newCustomerSection');
    const inputElements = newCustomerSection.querySelectorAll('input');

    if (dropdown.value === "0") {
        
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
            input.value = '';
        });
    }
}
