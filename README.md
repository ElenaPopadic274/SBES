# SBES
**Projekat iz predmeta Sigurnost i Bezbednost Elektroenergetskih Sistema, Fakultet Tehničkih Nauka.**


**Potrebno je realizovati sistem za sigurno korišćenje timer-a.**

  -Korisnici se autentifikuju pomoću Windows autentifikacionog protokola.
  
  -Za autorizaciju sistema implementirati RBAC mehanizam. RBAC permisije koje sistem treba da ima su: </br>
   * See </br>
   * Change </br>
   * StartStop </br>
  
  -Mapiranje permisija na grupe treba učiniti konfigurabilnim.
  
  -Servis treba da omogući: </br>
   * PokreniTimer (zahteva StartStop permisiju) </br>
   * ZaustaviTimer (zahteva StartStop permisiju) </br>
   * PonistiTimer (zahteva Change permisiju) </br>
   * PostaviTimer (zahteva Change permisiju) - vreme koje korisnik šalje serveru treba poslati šifrovano DES kriptografskim algoritmom u CBC modu </br>
   * OcitajTimer (zahteva See permisiju) </br>

  -Timer se zaustavlja ili kad ga neko uspešno zaustavi metodom ZaustaviTimer, ili kada istekne postavljeno vreme timer-a koji je neko od korisnika pokrenuo.
  -Poništavanje timer-a podrazumeva njegovo postavljanje na nulu.
  -Neophodno je logovati sve akcije koje korisnik vrši u sistemu, osim čitanja vremena štoperice. Logovati pomoću Windows Event Log-a.
  
  --------------------------------------------------------------------------------------------------------------------------------------------------------------------
**Project in the subject of Security and Safety of Electric Power Systems, Faculty of Technical Sciences.** 

**It is necessary to implement a system for safe use of the timer.** 

  -Users are authenticated using the Windows authentication protocol. 
  
  -Implement the RBAC mechanism to authorize the system. The RBAC permissions that the system should have are: </br>
   * See </br>
   * Change </br>
   * StartStop</br>
      
   -Mapping permissions to groups should be made configurable. 
   
   -The service should enable: </br>
   * StartTimer (requires StartStop permission) </br>
   * StopTimer (requires StartStop permission) </br>
   * ResetTimer (requires Change permission) </br>
   * SetTimer (requires Change permission) - the time the user sends to the server should be sent encrypted with DES cryptographic algorithm in CBC mode </br>
   * Read Timer (requires See permission) </br>
      
    -The timer stops either when someone successfully stops it with the Stop Timer method, or when the set timer time that one of the users has started expires.
    -Resetting the timer means setting it to zero. 
    -It is necessary to log all actions performed by the user in the system, except for reading the stopwatch time. Log in using Windows Event Log.
    
