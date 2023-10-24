# Decisions Changelog

## Purpose

This document tracks more or less important decisions about the engine, in order to allow easy referencing in the future.

## [2023-10-24]

### Component Containers and Contexts
*Decided By: Assemblerbot, Mirzipan*

Components for the `Engine` and `Session` will no longer be dynamically created by the engine.
The instances will be supplied within the `Context`, and therefore created somewhere externally.
As a side-effect, we can now merge component collections and contexts, to avoid pointless copying of components between the two.
The `Engine / Session` will only take care of initialization and injection of the components.

## [2023-10-23]

### Avalonia / ImGui
*Decided By: Assemblerbot, Mirzipan*

`Avalonia` shall no longer be used for the editor, as it contains long-standing bugs that we deemed critical for our usage.
It is to be replaced by `ImGui`, as we have already intended to use it for debug.
ImGui looks like a powerful, feature-rich yet simple to use, battle-tested GUI library.
We don't expect many bugs to begin with, and if we encounter some, it is reasonable to expect them to be fixed relatively swiftly.
As a side-effect, this brings down out dependency count.

## [2023-10-22]

### Engine Components / Systems
*Decided By: Mirzipan*

`Engine components` renamed to `engine systems`, as they are conceptually very different from components in the session or on an entity.

### Render Components / Features
*Decided By: Mirzipan*

`Render components` renamed to `features`, because that's closer to their intended usage. 
Expected features are debug, imgui, particles, meshes, etc.

## [2023-10-21]

### Game / Session
*Decided By: Mirzipan*

`Game` was renamed to `Session` in order to better communicate its intention.
It's original name would make it seem like it needed to contain the whole game logic.
The intent of the Session is to be used for actual game session, such as after the player clicks `new game` or `load game` in the main menu.
This means you can have different Session per game mode, which can reuse or even completely eliminate game mode checks within the game logic, since only the component relevant for the specific game mode can be loaded, instead of all of them.
