// Function to update row color based on status
function updateRowColor(row, status) {
    const tds = row.querySelectorAll('td:not(.category-header)');
    tds.forEach(td => {
        // Clearing previous status classes
        td.className = "";
        // Add the new status class
        td.classList.add(`status-${status.toLowerCase().replace(/\s+/g, '-')}`);
    });
}

// Function to update category row color based on item status
function updateCategoryColor(tbody, categoryCell) {
    const rows = tbody.querySelectorAll('tr:not(:first-child)');
    const checkedRows = Array.from(rows).filter(row => row.querySelector('input[type="radio"]:checked'));

    if (checkedRows.length === rows.length) {
        categoryCell.classList.add("all-checked");
    } else {
        categoryCell.classList.remove("all-checked");
    }
}

// Function to initialize event listeners on radio buttons
function initRadioListeners() {
    const radios = document.querySelectorAll('input[type="radio"]');

    radios.forEach(radio => {
        radio.addEventListener('change', () => {
            const row = radio.closest('tr');
            const status = radio.value;
            const tbody = row.closest('tbody');
            const categoryCell = tbody.querySelector('.category-header');

            updateRowColor(row, status);
            updateCategoryColor(tbody, categoryCell);
        });
    });
}

// Pageload
document.addEventListener('DOMContentLoaded', function () {
    // Initialize radio button event listeners
    initRadioListeners();

    const serviceOrderForm = document.getElementById('serviceOrderForm');

    if (serviceOrderForm) {
        serviceOrderForm.addEventListener('submit', function (event) {
            event.preventDefault(); // Prevent the form from submitting

            // Update OrderCompleted and ServiceOrderStatus
            const now = new Date();
            document.getElementById('order_completed').value = now.toISOString();
            document.getElementById('service_order_status').value = "Fullført";

            // Continue with form submission
            serviceOrderForm.submit();
        });
    } else {
        console.error("Element with ID 'serviceOrderForm' not found");
    }
});
