INCLUDE include/redteam_data.inkl

INCLUDE include/west_gate_en.inkl
INCLUDE include/mine_field_en.inkl
INCLUDE include/window_en.inkl
INCLUDE include/downhill_en.inkl
INCLUDE include/river_en.inkl
INCLUDE include/lunch_time_en.inkl
INCLUDE include/drawbridge_en.inkl
INCLUDE include/lost_en.inkl
INCLUDE include/fire_en.inkl
INCLUDE include/self_confidence_en.inkl
INCLUDE include/factory_west_en.inkl

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


