INCLUDE include/blueteam_data.inkl

INCLUDE include/east_gate_it.inkl
INCLUDE include/cables_it.inkl
INCLUDE include/stack_it.inkl
INCLUDE include/lemons_it.inkl
INCLUDE include/robots_it.inkl
INCLUDE include/tunnel_it.inkl
INCLUDE include/bridge_it.inkl
INCLUDE include/terminal_it.inkl
INCLUDE include/traffic_lights_it.inkl
INCLUDE include/electrified_fence_it.inkl
INCLUDE include/factory_east_it.inkl

-> plot
=== plot ===
{ -> east_gate | -> cables | -> stack | -> lemons | -> robots | -> tunnel | -> bridge | -> terminal | -> traffic_lights | -> electrified_fence | -> factory_east | -> END }

/*
east_gate
cables (E) → rope?
stack → turret?, 0 (rope), -1
lemons (B)
robots → 0, -1
tunnel (C) → 0 (turret), -1
bridge
terminal --> code?
traffic_lights (F)
electrified_fence --> 0, -1, -2
factory_east
*/
