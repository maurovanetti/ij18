VAR inter__conveyor_belt = false
VAR inter__fridge_locked = false
VAR inter__electrification = true
VAR x = -1
VAR y = -1

~ x = 0.50
~ y = 0.00

-> ready

=== ready ===

FABBRICA APPROVIGIONAMENTI
SISTEMA AUTOMATIZZATO DI CONTROLLO
IDENTIFICARSI, PREGO
+   /admin14, barattolo14.
    -> unauthorized
+   /admin14, barattolo15.
    -> authorized
+   /admin15, barattolo14.
    -> unauthorized
+   /admin15, barattolo15.
    -> unauthorized
-> END

= unauthorized
CREDENZIALI NON RICONOSCIUTE
+   /Non me le ricordo bene!
-> ready

= authorized
RICONOSCIMENTO VOCALE VALIDO: BENVENUTO, admin14
OPERAZIONI DISPONIBILI:
1. ORDINAZIONI CONSERVE
{
    - inter__conveyor_belt:
        2. SPEGNIMENTO CATENA DI MONTAGGIO
    - else:
        2. ACCENSIONE CATENA DI MONTAGGIO
}
{
    - inter__fridge_locked:
        3. APERTURA LOCALE FRIGORIFERO
    - else:
        3. CHIUSURA LOCALE FRIGORIFERO
}
{
    - inter__electrification:
        4. SOSPENSIONE ELETTRIFICAZIONE RECINZIONE
    - else:
        4. ELETTRIFICAZIONE RECINZIONE
}
-> select

= select
+   /Uno
    ORDINAZIONE ESEGUITA
+   /Due
    ~ inter__conveyor_belt = not inter__conveyor_belt
{
    - inter__conveyor_belt:
        ACCENSIONE ESEGUITA
    - else:
        SPEGNIMENTO ESEGUITO
}
+   /Tre
    ~ inter__fridge_locked = not inter__fridge_locked
{
    - inter__fridge_locked:
        LOCALE FRIGORIFERO CHIUSO
    - else:
        LOCALE FRIGORIFERO APERTO
}
+   /Quattro
    ~ inter__electrification = not inter__electrification
{
    - inter__electrification:
        RECINZIONE ELETTRIFICATA IN MODALITÀ INTERMITTENTE
    - else:
        ELETTRIFICAZIONE RECINZIONE SOSPESA
}
- ARRIVEDERCI, admin14. # shortbreak
-> ready

    
