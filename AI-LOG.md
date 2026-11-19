# AI-LOG

Write down, in order, what you asked the AI. It is not a punishment: a missing or false log lowers the level D grade.

| Time (approx.) | What you asked (briefly) | What you kept | What you changed or rejected |
|---|---|---|---|
| ~09:08 | Citeste tot proiectul si cursurile, fara modificari | Lista de bug-uri identificate (R5, R6, R7, R8) | - |
| ~09:10 | Implementeaza pasul A (Features cu 5 vertical slices) | Structura pe vertical slices si utilizarea TypedResults | Am respins versionarea API-ului propusa (schimba rutele din contract si pica testele automate) |
| ~09:13 | Continua pasul A fara versionare | Stergerea folderelor vechi si testarea HTTP a endpoint-urilor | Am stabilit manual mesajul de commit conform conventiei feat: |
| ~09:15 | Verifica daca pasul A este gata complet | Verificarea cazurilor limita pentru regulile R1-R4 si R7 | Am notat lipsa celor 3 commit-uri necesare si a push-ului pe remote |
| ~09:16 | Verifica propunerea de tabel BUGS.md | Cele 5 bug-uri reale confirmate din starter | Am respins scriptul check.ps1 inventat, codul inexistent din "What you changed" si randurile false (Persistence, UTC) |
| ~09:17 | Furnizeaza faptele pentru completarea BUGS.md | Faptele concrete (fisiere, linii, discrepante gasite) | Am formulat manual descrierile in fisier |
| ~09:18 | Propune un mesaj de commit cu feat: | Mesajul propus: feat: restructure API into vertical slices under Features/ | - |
| ~09:20 | Verifica textul din BUGS.md dupa push | Sugestiile de formatare | Am ajustat underscore-urile si formularile la regulile R5 si R7 |
| ~09:22 | Implementeaza cerintele noi (R9, R10, R11) | Plasarea regulilor in domeniu si interpretarea (Trim la R9, >=60 min la R11) | - |
| ~09:25 | Implementeaza pasul B (Rich domain, Result, CQRS) | Entitatile fara setteri publici, Result/Error, ICommandHandler, DateRange | Am efectuat manual commit-ul in git in loc sa las AI-ul |
| ~09:36 | Implementeaza pasul C (EF Core + SQLite) | RoomHubDbContext ca IUnitOfWork, repository-urile si testul de persistenta la restart | Am corectat versiunea pachetului EF Core (fixat de la 11.0.0 incompatibil la 10.0.15 pe net10) |
| ~09:40 | Verificare finala a proiectului | Reformularea comentariilor care contineau DateTime.UtcNow | Am identificat fisierele ramase de completat (DECISIONS.md, AI-LOG.md, BUGS.md) |
