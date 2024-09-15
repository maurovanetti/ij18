VAR inter__conveyor_belt = false
VAR inter__fridge_locked = false
VAR inter__electrification = true
VAR x = -1
VAR y = -1

~ x = 0.50
~ y = 0.00

-> ready

=== ready ===

SUPPLY FACTORY
AUTOMATED CONTROL SYSTEM
AUTHENTICATE, PLEASE
+   /admin14, tincan14.
    -> unauthorized
+   /admin14, tincan15.
    -> authorized
+   /admin15, tincan14.
    -> unauthorized
+   /admin15, tincan15.
    -> unauthorized
-> END

= unauthorized
UNRECOGNISED CREDENTIALS
+   /I don't remember well!
-> ready

= authorized
VOICE RECOGNITION OK: HELLO, admin14
AVAILABLE OPERATIONS:
1. PICKLE ORDERS
{
    - inter__conveyor_belt:
        2. SHUT DOWN CONVEYOR BELT
    - else:
        2. START UP CONVEYOR BELT
}
{
    - inter__fridge_locked:
        3. OPEN FRIDGE ROOM
    - else:
        3. CLOSE FRIDGE ROOM
}
{
    - inter__electrification:
        4. ELECTRIFIED FENCE OFF
    - else:
        4. ELECTRIFIED FENCE ON
}
-> select

= select
+   /One
    ORDER DISPATCHED
+   /Two
    ~ inter__conveyor_belt = not inter__conveyor_belt
{
    - inter__conveyor_belt:
        STARTUP COMPLETED
    - else:
        SHUTDOWN COMPLETED
}
+   /Three
    ~ inter__fridge_locked = not inter__fridge_locked
{
    - inter__fridge_locked:
        FRIDGE ROOM CLOSED
    - else:
        FRIDGE ROOM OPENED
}
+   /Four
    ~ inter__electrification = not inter__electrification
{
    - inter__electrification:
        ELECTRIFIED FENCE IN INTERMITTENT MODE
    - else:
        ELECTRIFIED FENCE PAUSED
}
- GOODBYE, admin14. # shortbreak
-> ready

    
