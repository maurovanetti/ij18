INCLUDE include/redteam_data.inkl

INCLUDE include/west_gate_it.inkl
INCLUDE include/mine_field_it.inkl
INCLUDE include/window_it.inkl
INCLUDE include/downhill_it.inkl
INCLUDE include/river_it.inkl
INCLUDE include/lunch_time_it.inkl
INCLUDE include/drawbridge_it.inkl
INCLUDE include/lost_it.inkl
INCLUDE include/fire_it.inkl
INCLUDE include/self_confidence_it.inkl
INCLUDE include/factory_west_it.inkl

-> plot

=== plot ==
{ -> west_gate | -> mine_field | -> window | -> downhill | -> river | -> lunch_time | -> drawbrige | -> lost | -> fire | -> self_confidence | -> factory_west | -> END }

/*
west_gate
mine_field --> -1
window → sphere? 0, -1
downhill (A)
river (I) --> 0, -1
lunch_time (G)
drawbridge → sphere? code!
lost (D)
fire → 0 (sphere), -1, -2
self_confidence (H)
factory_west
*/


