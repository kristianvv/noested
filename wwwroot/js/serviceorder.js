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

    /* For each category within the categories array */
    categories.forEach((category, categoryIndex) => {
        const tbody = document.createElement("tbody"); // Creates a tbody element
        table.appendChild(tbody); // Appends it to the table #checkListTable

        /* First Task = For Each Item Within Each Category */
        let categoryCell; 
        category.items.forEach((item, index) => {
            const row = document.createElement("tr"); // Creates a row element

            /* 1.1) Sub-task (Add Category TD) */ 
            if (index === 0) {
                categoryCell = document.createElement("td"); // extra <td> first to display the category cell in view
                categoryCell.rowSpan = category.items.length; // 'rowSpan' ensures cell covers all associated items in view
                categoryCell.textContent = category.name; // 'textContent' displays category name in view
                categoryCell.classList.add("category-header"); // needed to separate from rows in update functions
                row.appendChild(categoryCell); // Add to <tr>
            }

            /* 1.2) Sub-task (Add Item TD) */
            const itemCell = document.createElement("td");
            itemCell.textContent = item; // Displays to user name of item (categories.category.items.item)
            row.appendChild(itemCell); // Add to row

            /* 1.3) Sub-task (Add Three Radio Options) */
            ['OK', 'Bør Skiftes', 'Defekt'].forEach(status => { // define status values, for each option...
                const statusCell = document.createElement("td"); // ... creating a td element to contain...
                const radio = document.createElement("input"); // ... the input ...
                radio.type = "radio";
                radio.value = status; // Payload value
                radio.name = `item_${sanitize(item)}_category_${sanitize(category.name)}`; // Payload key
                
                radio.addEventListener('change', () => { // if checked
                    updateRowColor(row, status); // adds class to all <td> in <tr> besides the category
                    updateCategoryColor(tbody, categoryCell); // changes category <td> if all rows in tbody checked
                });

                statusCell.appendChild(radio); // Loop appends each radio-input to their respective statusCell <td>...
                row.appendChild(statusCell); // ... and each statusCell to the row.
            });

        tbody.appendChild(row); // Finished row appended to the tbody (for category).
        });
    });
}

// Utility function to sanitize names for form fields
function sanitize(name) {
    return name.replace(/\s+/g, '_').toLowerCase();
}

// Pageload
document.addEventListener('DOMContentLoaded', function () {
    generateChecklist(); // Generate the checklist

    const serviceOrderForm = document.getElementById('serviceOrderForm');

    if (serviceOrderForm !== null) { // if not null
        serviceOrderForm.addEventListener('submit', function (event) { // Listen for Submit
            event.preventDefault(); // pause submit process

            // Update OrderCompleted and ServiceOrderStatus first
            const now = new Date();
            document.getElementById('order_completed').value = now.toISOString();
            document.getElementById('service_order_status').value = "Fullført";

            // Continue form submission
            serviceOrderForm.submit();
        });
    } else {
        console.error("Element with ID 'serviceOrderForm' not found");
    }
});
