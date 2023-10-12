// Specific to "Views >ServiceOrder >Index.cshtml" for generating checklist dynamically
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
];