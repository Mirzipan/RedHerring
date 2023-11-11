# Decisions Changelog

## Purpose

This document tracks more or less important decisions about the engine, in order to allow easy referencing in the future.

## [2023-11-09]

### Math Library
*Decided by: Assemblerbot, Mirzipan*

Due to lack of complete maths library available, we will be implementing our own.
Libraries considered:
* `Silk.NET` - lacks `Color`
* `System.Numerics` - lacks integer vectors, and `Color`
* `Vortice` - lacks float vectors

This decision may be revisited in the future.

## [2023-11-08]

### Importers
*Decided by: Assemblerbot, Mirzipan*

Importers should be kept simple for now.
If we find a need for more configurable and extensively modular importers in the future, then that is when they will be implemented.

## [2023-11-06]

### Engine Renderer
*Decided by: Mirzipan*

Engine should not care about a specific system, such as the `GraphicsSystem`. 
Systems provide an abstraction, and APIs, and logic access grouping for code outside of the engine itself.
As such, engine should care about Renderer interface, rather then the whole `GraphicsSystem`.

## [2023-11-03]

### Input De-abstraction
*Decided by: Mirzipan*

Input was using too many abstractions, either via interfaces or abstract classes.
This was entirely unnecessary, because we can already create a custom input interface implementation.
This means no one will feasibly need to implement custom keyboard state, or any other states.
Now there is no more abstract shortcut class, interfaces for device states.

### Interface Renames
*Decided by: Mirzipan*

Interfaces in input no longer use the `I` prefix.
There is no reason why any method that uses abstracted types should know whether it's an abstract class or an interface.
This also forces better naming on derived types.
For example, there is no longer `IInput interface` and `Input class`, instead there is `Input interface` and `SilkInput class`.
This way, it clearly communicates what it represent, because just `Input` isn't specific enough.
So far it seems that readability was not hurt by these changes.

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
