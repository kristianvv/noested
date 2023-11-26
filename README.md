# Nøsted
Gruppe 2 sitt prosjekt for høsten 2023

###  Lage en mariadb container som inneholder sti til hvor du ønsker databasen skal lagres lokalt
```c
docker run --name [databasenavn] -e MYSQL_ROOT_PASSWORD=1234 -p 3306:3306 -d mariadb:10.5.11
```

### Lage en database i mariadb med samme navn som i appsettings.json
1. Logge inn i container via CLI
```c
docker exec -it noested_mdb mysql -u root -p
```
2. Sjekke hva slags databaser som faktisk eksisterer allerede
```sql
show databases;
```
3. Opprette en ny database for noested
```sql
create database noested_mdb;
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
