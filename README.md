# Nøsted
Gruppe 2 sin web applikasjon for Nøsted & 2023 - digitalisering av serviceordre.

Applikasjonen har følgende funksjonalitet:
* Registrering og innlogging
* Tilgang på visninger basert på brukerens rolle
* Oppretting av serviceordre og tilhørende sjekkliste
* Serviceordreoversikt
* Kundeoversikt med mulighet for oppretting, endring og sletting av kunder

### Forutsetninger
* Installert Docker Desktop
* Installert .Net 7.0 SDK

### Drift av systemet
* Applikasjonen benytter roller som er definert i program.cs
* Applikasjonen legger automatisk til en administrator som fjernes når permanent administrator er opprettet
* For brukere av systemet oppretter man bruker ved å trykke "Registrer deg" øverst i høyre hjørne
* Administrator tilegner deretter passende rolle til brukeren

### Bruk av systemet med Docker
1. Kjør database-containeren i Docker med kommandoen
```c
docker run --rm --env "TZ=Europe/Oslo" --name noested_mdb -p 3308:3306/tcp -e MYSQL_ROOT_PASSWORD=1234 -d mariadb:10.5.11
```

2.	Navigere deg inn i noested/noestedmvc via CLI og skrive inn kommandoene
```c
dotnet build
dotnet run
```

3.	Teste applikasjonen på localhost-port angitt i CLI
  
## OBS!
For å kjøre noen av testene må Moq library være installert. Dette installerer du i Package Manager.
