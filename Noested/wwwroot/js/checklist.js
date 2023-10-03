let categories = [
    {
        name: "Mekanisk",
        items: [
            "Sjekk clutch lameller for slitasje",
            "Sjekk bremser. Bånd/Pal",
            "Sjekk lager for trommel",
            "Sjekk PTO og opplagring",
            "Sjekk kjedestrammer",
            "Sjekk wire",
            "Sjekk pinion lager",
            "Sjekk kile på kjedehjul",
        ]
    },
    {
        name: "Hydraulikk",
        items: [
            "Sjekk hydraulikk sylinder for lekkasje.",
            "Sjekk slanger for skader og lekkasje",
            "Test hydraulikkblokk i testbenk",
            "Skift olje i tank",
            "Skift olje på girboks",
            "Sjekk Ringsylinder åpne og skift tetninger",
            "Sjekk bremsesylinder åpne og skift tetninger",
        ]
    },
    {
        name: "Elektro",
        items: [
            "Sjekk ledningsnett på vinsj",
            "Sjekk og test radio",
            "Sjekk og test knappekasse",
        ]
    },
    {
        name: "Trykksettinger",
        items: [
            "xx-Bar",
            "???",
            "???+++",
        ]
    },
    {
        name: "Funksjonstest",
        items: [
            "Test vinsj og kjør alle funksjoner",
            "Trekkraft KN",
            "Bremsekraft KN",
        ]
    },
    // NB: Må være unike items for at radio-options skal fungere riktig.
];

// Oppdaterer fargen på individuelle rader
function updateRowColor(row, status) {
    const tds = row.querySelectorAll('td:not(.category-header)'); // Markerer alle td'er utenom kategori-headeren

    tds.forEach(td => {
        td.classList.remove("ok", "needs-replacement", "defective");
        switch (status) {
            case 'OK':
                td.classList.add("ok");
                break;
            case 'Bør Skiftes':
                td.classList.add("needs-replacement");
                break;
            case 'Defekt':
                td.classList.add("defective");
                break;
        }
    });
}

// Oppdaterer fargen på kategori-headeren
function updateCategoryColor(tbody, row) {
    const allRows = tbody.querySelectorAll('tr');
    const checkedRows = Array.from(allRows).filter(row => row.querySelector('input[type="radio"]:checked'));
    const categoryHeaderCells = tbody.querySelectorAll('.category-header');

    if (checkedRows.length === allRows.length) {
        categoryHeaderCells.forEach(cell => cell.classList.add("all-checked"));
    } else {
        categoryHeaderCells.forEach(cell => cell.classList.remove("all-checked"));
    }
}

// Genererer sjekklisten
function generateChecklist() {
    const table = document.querySelector("#checklistTable");

    categories.forEach((category) => {
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

            const itemCell = document.createElement("td");
            itemCell.textContent = item;
            row.appendChild(itemCell);

            ['OK', 'Bør Skiftes', 'Defekt'].forEach((status) => {
                const statusCell = document.createElement("td");
                const radio = document.createElement("input");
                radio.type = "radio";
                radio.name = item.replace(/\s+/g, '_').toLowerCase();
                radio.value = status;
                radio.addEventListener('change', function () {
                    updateRowColor(row, status);
                    updateCategoryColor(tbody, categoryCell);
                });
                statusCell.appendChild(radio);
                row.appendChild(statusCell);
            });

            tbody.appendChild(row);
        });
    });
}

// Onload generere sjekklisten og eventuelt flere ting
document.addEventListener('DOMContentLoaded', function () {
    generateChecklist();
    attachEventListeners();
});

function attachEventListeners() {
  // flere ting?
}