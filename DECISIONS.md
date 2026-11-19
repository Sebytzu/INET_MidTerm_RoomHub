# DECISIONS

Answer yourself, in your own words, in 3-5 sentences per question.
Refer to concrete files and classes from your own repo (for example Features/Bookings/CreateBooking/...).
An unanswered question gives 0 points.

## 1. A rule you moved
Pick one business rule (R1-R8). In which file did it live in the starter and in which file does it live now? Why there?

Am mutat regula R7 (verificarea suprapunerii rezervarilor active). In starter, aceasta regula traia in Services/BookingService.cs in metoda Create, unde serviciul itera manual peste rezervari cu o conditie eronata de comparare (<=). Acum, regula traieste in modelul de domeniu bogat din Domain/Bookings/Booking.cs (in metoda statica de fabrica Booking.Create), ajutata de Domain/Bookings/DateRange.cs prin metoda Overlaps. Am mutat-o acolo pentru a respecta principiul de Rich Domain: entitatea de domeniu este responsabila sa-si protejeze invarianti si integritatea starii, impiedicand crearea unui Booking invalid indiferent de unde ar fi apelat.

## 2. Result instead of an exception
Why is Result a better choice than an exception for "the room is occupied"? What happens in your code when an Error is returned?

Result este o alegere mult mai buna decat o exceptie deoarece faptul ca o camera este ocupata reprezinta un scenariu normal, previzibil de business, nu o defectiune catastrofala a sistemului. Exceptiile ascund fluxul de control, ingreuneaza urmarirea codului si aduc un cost semnificativ de performanta prin generarea de stack trace. In codul nostru, cand o regula este incalcata, se returneaza Result.Failure(BookingErrors.RoomOccupied). In Features/Bookings/CreateBooking/CreateBookingEndpoint.cs, acest esec este interceptat si mapat prin metoda de extensie ToProblem() din Common/ErrorHttpExtensions.cs direct intr-un raspuns HTTP 409 Conflict de tip ProblemDetails.

## 3. Who knows about EF Core?
Which classes in your project know about EF Core and which do not? What do you gain and what do you pay with Repository + Unit of Work?

Doar clasele din layer-ul de infrastructura cunosc EF Core: contextul Infrastructure/Persistence/RoomHubDbContext.cs, configuratiile din Infrastructure/Persistence/Configurations/ si implementarile RoomRepository.cs si BookingRepository.cs, alaturi de inregistrarea din Program.cs. Entitatile din Domain/ si slice-urile din Features/ nu stiu nimic despre EF Core, ele depinzand doar de interfetele abstracte IRoomRepository, IBookingRepository si IUnitOfWork. Prin Repository + Unit of Work castigam o decuplare clara si o testabilitate usoara a logicii de business fara a depinde de o baza de date reala. Pretul platit consta in clase suplimentare de abstractizare (boilerplate) si restrictia de a nu putea folosi direct optimizari sau interogari LINQ avansate specifice EF Core in handlere.

## 4. The flow of a request
Draw in text (ASCII arrows) the path of a POST /api/rooms/{roomId}/bookings request through your project, with the real names of files and classes, from the endpoint to the database.

POST /api/rooms/{roomId}/bookings
-> BookingsEndpoints.cs
-> CreateBookingEndpoint.cs (construieste CreateBookingCommand)
-> CreateBookingHandler.cs
-> RoomRepository.cs (GetByIdAsync) & BookingRepository.cs (ListActiveForRoomAsync)
-> Booking.Create (Domain/Bookings/Booking.cs)
-> BookingRepository.cs (Add)
-> RoomHubDbContext.cs / IUnitOfWork (SaveChangesAsync) -> SQLite
-> CreateBookingEndpoint.cs -> 201 Created

## 5. What you rejected from what the AI gave you
Give a concrete example of code or an idea proposed by the AI that you corrected or rejected, and say why.

La pasul A, AI-ul a propus introducerea versionarii pentru rutele API-ului. Am respins aceasta propunere deoarece ar fi schimbat contractul strict definit in ASSIGNMENT.md (/api/rooms, /api/bookings), astfel ajungeam la picarea automata a testelor de evaluare. De asemenea, la verificarea propunerilor pentru BUGS.md, am respins elementele halucinate (un script check.ps1 inventat si presupuse bug-uri de persistenta) si am pastrat doar problemele reale identificate in codul starter-ului.

## 6. If you had 30 more minutes
What would you improve and what risk did you leave in the project?

Daca mai aveam 30 de minute, as fi adaugat versionare pentru API, pentru ca orice schimbare pe viitor poate strica clientii existenti. De asemenea, as fi scris mai multe teste automate simple pentru a acoperi mai bine cazurile limita la rezervari si anulari. Riscul ramas in proiect este daca doi utilizatori trimit cererea in aceeasi secunda pentru aceeasi camera. Amandoi ar putea trece de verificare inainte de salvare si s-ar crea rezervari duble, pentru ca nu avem o blocare pusa direct in baza de date SQLite.
