// Function to update row color based on status
function updateRowColor(row, status) {
    const tds = row.querySelectorAll('td:not(.category-header)');
    tds.forEach(td => {
        td.className = ""; // Clearing previous status
        td.classList.add(status.toLowerCase().replace(/\s+/g, '-'));
    });
}

// Function to update category row color based on item status
function updateCategoryColor(tbody, categoryCell) {
    const rows = tbody.querySelectorAll('tr');
    const checkedRows = Array.from(rows).filter(row => row.querySelector('input[type="radio"]:checked'));

    if (checkedRows.length === rows.length) {
        categoryCell.classList.add("all-checked");
    } else {
        categoryCell.classList.remove("all-checked");
    }
}

// Function to generate the checklist table
function generateChecklist() {
    const form = document.querySelector("form");
    const table = document.querySelector("#checklistTable");

    categories.forEach((category, categoryIndex) => {
        const tbody = document.createElement("tbody");
        table.appendChild(tbody);

        let categoryCell;
        category.items.forEach((item, index) => {
            const row = document.createElement("tr");

            if (index === 0) {
                categoryCell = document.createElement("td");
                categoryCell.rowSpan = category.items.length;
                categoryCell.textContent = category.name;
                categoryCell.classList.add("category-header");
                row.appendChild(categoryCell);
            }


            // Hidden input for each item's category
            const hiddenInput = document.createElement("input");
            hiddenInput.type = "hidden";
            hiddenInput.name = `category_${sanitize(item)}`;
            hiddenInput.value = category.name;
            form.appendChild(hiddenInput); * /

            // Item cell
            const itemCell = document.createElement("td");
            itemCell.textContent = item;
            row.appendChild(itemCell);

            ['OK', 'Bør Skiftes', 'Defekt'].forEach(status => {
                const statusCell = document.createElement("td");
                const radio = document.createElement("input");
                radio.type = "radio";
                radio.name = `item_${sanitize(item)}`;
                radio.value = status;
                radio.addEventListener('change', () => {
                    updateRowColor(row, status);
                    updateCategoryColor(tbody, categoryCell);
                });

                // Status cell
                statusCell.appendChild(radio);
                row.appendChild(statusCell);
            });
            tbody.appendChild(row);
        });
    });
}

// Utility function to sanitize names for form fields
function sanitize(name) {
    return name.replace(/\s+/g, '_').toLowerCase();
}


document.addEventListener('DOMContentLoaded', function () {
    generateChecklist();

    const serviceOrderForm = document.getElementById('serviceOrderForm');

    serviceOrderForm.addEventListener('submit', function (event) {
        console.log('Submit event triggered');  // Log to ensure this is only being called once
        event.preventDefault();  // Prevent the default form submission

        // Set each select option to its matching status
        const selects = document.querySelectorAll("select");
        selects.forEach((select) => {
            const status = select.getAttribute('data-status');
            select.value = status;
        });

        // Add current DateTime to hidden input field
        const now = new Date();
        document.getElementById('date_time').value = now.toISOString();

        // Log the form data
        const formData = new FormData(serviceOrderForm);
        for (const [key, value] of formData.entries()) {
            console.log(key, value);  // Log each form field and its value
        }

        // Perform the actual form submission
        serviceOrderForm.submit();
    });
});