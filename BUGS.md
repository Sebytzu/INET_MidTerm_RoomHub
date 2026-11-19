# BUGS

The starter contains behaviors that contradict `ASSIGNMENT.md`. Write here every discrepancy you found.
Fill it in yourself, in your own words. There is no required number: what counts is that they are real.

| Which rule from ASSIGNMENT.md is violated (R1–R8) | Where the problem was in the starter (file, approximate line) | How you discovered it (test, reading, AI) | What you changed |
|---|---|---|---|
| _R5_ | _Services/BookingService.cs, metoda Create, liniile 8-45, nu exista verificare ca nu s-a trecut de momentul startUtc, si nu se foloseste TimeProvider_ | _AI_ | _au fost mutate in Booking.Create_ |
| _R6_ | _Services/BookingService.cs, metoda Create, liniile 8-45, nu se verifica durata, o rezervare poate fi si de 10 ore_ | _AI_ | _au fost mutate in Booking.Create_ |
| _R7_ | _Services/BookingService.cs, linia 28: existing.StartUtc <= endUtc && startUtc <= existing.EndUtc, o rezervare 10-12 blocheaza una 12-14 din cauza <=, trebuie comparatie stricta, pentru Overlaps_ | _AI_ | _au fost mutate in Booking.Create + < pentru DateRange,Overlaps_ |
| _R7/R8_ | _Services/BookingService.cs, linia 26: foreach (var existing in bookings.ListForRoom(roomId)), nu este filtru pentru Status==Active_ | _AI_ | _Booking.Create ia in calcul doar rezervarile care au IsActive, iar repository-ul are ListActiveForRoomAsync_ |
| _R8_ | _Services/BookingService.cs, metoda Cancel, liniile 56-60, seteaza Status=Cancelled fara sa verifice starea curenta. A doua anulare da 204, desi trebuie sa dea 409_ | _AI_ | _in Booking.cancel_ |
