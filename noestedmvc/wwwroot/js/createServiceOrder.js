document.addEventListener("DOMContentLoaded", function () {

    const form = document.getElementById('create_order_form');
    if (form) {
        form.addEventListener('submit', handleFormSubmit);
    }

    const dropdown = document.getElementById('CustomerId');
    if (dropdown) {
        dropdown.addEventListener('change', toggleNewCustomerSection);
    }

    document.querySelectorAll('.form-group input').forEach(inputField => {
        inputField.addEventListener('input', handleInputEvent);
        inputField.addEventListener('focus', handleFocusEvent);
        inputField.addEventListener('blur', handleBlurEvent);
    });

    document.querySelectorAll('.form-group select').forEach(selectElement => {
        selectElement.addEventListener('change', handleSelectChange);
    });

    document.querySelectorAll('.form-group').forEach(formGroup => {
        formGroup.addEventListener('mouseover', handleHoverEvent);
    });
});

function handleFormSubmit(event) {
    event.preventDefault(); // Pause sub

    this.submit(); // Exe real sub
}

function handleHoverEvent() {
    const inputs = this.querySelectorAll('input');
    const isAnyInputDisabled = Array.from(inputs).some(input => input.disabled);
    if (!isAnyInputDisabled) {
        this.classList.add('form-group-grayed');
    }
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
        clearAllInputs();
        inputElements.forEach((input) => {
            input.classList.add("greyed-out");
            input.required = false;
            input.disabled = true;
        });
    }
}

function clearAllInputs() {
    const formGroups = document.querySelectorAll('.form-group');
    formGroups.forEach(formGroup => {
        formGroup.classList.remove('form-group-filled', 'form-group-grayed');

        const inputs = formGroup.querySelectorAll('input');
        inputs.forEach(input => {
            input.value = '';
        });
    });
}

function handleInputEvent() {
    const formGroup = this.closest('.form-group');
    if (this.value) {
        formGroup.classList.add('form-group-filled');
    } else {
        formGroup.classList.remove('form-group-filled');
    }
}

function handleSelectChange() {
    console.log("select changed");
    const formGroup = this.closest('.form-group');
    if (this.value) {
        formGroup.classList.add('form-group-selected');
    } else {
        formGroup.classList.remove('form-group-selected');
    }
}

function handleFocusEvent() {
    const formGroup = this.closest('.form-group');
    if (!this.disabled) {
        formGroup.classList.add('form-group-grayed');
    }
}

function handleBlurEvent() {
    const formGroup = this.closest('.form-group');
    if (!formGroup.querySelector('input:focus')) {
        formGroup.classList.remove('form-group-grayed');
    }
}