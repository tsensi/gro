# Simulation

This document describes the game simulation layer — how time progresses, what systems run each tick, and how they interact.

## Ticker

The simulation runs on a **tick-based loop** driven by the SDL frame clock. Each frame, real elapsed time is converted to game-days and passed to `SimLoop.Tick(deltaDays)`.

### Time model

- **Unit**: days (fractional). One real-time second at 60fps produces ~60 ticks of ~0.0000116 days each.
- **TimeScale**: multiplier on delta. Default 1.0. Increase to fast-forward the simulation.
- **Paused**: when true, `Tick` is a no-op — no systems execute, elapsed time does not advance.
- **ElapsedDays**: total game time since simulation start.

### Systems

Systems implement `ISystem.Tick(SimState)` and are executed in registration order every tick. A system receives:

- `state.World` — the ECS world (spawn/despawn entities, get/set components)
- `state.DeltaDays` — time elapsed this tick (after TimeScale)
- `state.ElapsedDays` — total elapsed game time

Register systems via `SimLoop.AddSystem(system)`. Order matters — earlier systems see the world before later systems modify it.

## Current systems

_(None yet — systems will be added as game mechanics are implemented.)_

## Planned mechanics

| Mechanic | System responsibility |
|----------|----------------------|
| Xeno growth | Exponential growth per infected zone each tick |
| Spreading | Transfer Xeno value to adjacent zones on player action |
| Biomass | Accumulate global resource based on infected zone count |
| Country response | AI counter-attacks that reduce Xeno value |
