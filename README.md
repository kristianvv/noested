# Nøsted
Gruppe 2 sin web applikasjon for Nøsted 2023. Denne applikasjonen digitaliserer sjekklisten hos Nøsted. I tillegg har det blitt laget et service ordre system som kan redigeres og lagres i database slik at Nøsted kan sotere, redigere og ha bedre oversikt over kunder, arbeid og relevante yrkesrelaterte elementer. 

###  Lage en mariadb container som inneholder sti til hvor du ønsker databasen skal lagres lokalt
```c
docker run --name noested_mdb -e MYSQL_ROOT_PASSWORD=1234 -p 3306:3306 -d mariadb:10.5.11
```

### Lage en database i mariadb med samme navn som i appsettings.json
Logge inn i container via CLI;
```c
docker exec -it noested_mdb mysql -u root -p
```
## VIKTIG!
* Start mariadb-containeren før du kjører programmet
* Gjøres det endringer i modellene, må det **alltid** lages ny EF-migrasjon. 

Dette gjøres i Package Manager med følgende kommandoer:
```c
Add-Migration "Beskrivende navn på migrasjon"
```
Legger til ny migrasjon:
```c
Update-Database
```
## OBS!
``/Areas/Identity/`` er direktivet hvor AspNet autentisering og autorisering er 
lokalisert
