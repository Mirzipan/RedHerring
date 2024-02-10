# Decisions Changelog

## Purpose

This document tracks more or less important decisions about the engine, in order to allow easy referencing in the future.

## [2024-02-06]

### Assets
*Decided by: Assemblerbot, Mirzipan*

After many trails and tribulations, we have arrived to a new way of handling assets.
It embraces the fact that the engine is meant to be used predominantly with code-friendly people.
Assets will all exist as direct references to instances whenever possible in a class that will be generated from resources.
This will allow us to completely avoid a certain class of issues and bug that could arise from more traditional asset handling pipelines.

### Definitions
*Decided by: Assemblerbot, Mirzipan*

In order to avoid having to serialize definitions at all, they will all exists as their own types instead.

### Statics
*Decided by: Assemblerbot, Mirzipan*

Many parts of the engine need to be accessible pretty much everywhere (assets, input, definitions, etc...).
This means we need to add boilerplate `[Infuse]` to many types, which is fine for engine types, but gets very annoying to do for many studio utilities.
We know for sure that some registries, managers and other systems will only ever live once within the runtime, so as long as they are properly taken care of during startup and shutdown, the fact that they are static should not pose any issue.
Some specific classes, such as `Input` and `Render` will serve as static, implementation-agnostic wrappers.

## [2024-01-03]

### Definitions
*Decided by: Mirzipan*

Definitions will use `Guid` as their identifier, to be more in line with the rest of the assets.
In addition to that, it is not necessary to have `Definitions` as an engine system, as it does not come with any inherent benefits (they are neither `Drawable` not `Updatable`).
The whole definition system is likely to receive future overhauls, such as automagic generation of serialized versions of definitions via Roslyn.

### Rendering
*Decided by: Assemblerbot, Mirzipan*

Given that `Veldrid` is no longer being maintained, we are looking into other suitable replacements.
So far, the best candidate seems to be `bgfx`, which covers all necessary platforms and graphics backends, and is already battle-tested.
The only downside so far is that it is in C++ and there is no good wrapper, so it will be necessary to use the provided bare bones bindings.
This might not be a big issue though, as we have no plans to expose any of its internals to anything outside of the `Renderer` project.

## [2023-11-19]

### Input Changes
*Decided by: Mirzipan*

Creating input shortcuts was too complex for any shortcuts that wanted to use modifiers.
As an example, if you wanted to create an `Any Shift + F12` shortcut, it would involve creating a top-level `CompositeShortcut` with a `KeyboardShortcut` and an inner `CompositeShortcut`.
The inner shortcut would then need to contain both, `ShiftLeft` and `ShiftRight`.
Now all atomic implementations of the `Shortcut` interface include `Modifiers`.
This is an optional addition, that is not enforced by the interface.

`Input` interface and all shortcuts also lost the `IsKeyUp`, `IsButtonUp`, etc. queries.
These were not serving any purpose and input could already be checked by negating their `IsDown` counterparts.

## [2023-11-12]

### Math Library - Vol.2
*Decided by: Assemblerbot, Mirzipan*

After further discussion and evaluation, we came to a conclusion to just implement complementary types to what `System.Numerics` has.
A lot of libraries are already using `System.Numerics`, and there is a chance that it will be expanded in the future.
While `Vortice` already looks like a complementary library, it also includes some very questionable decisions (likely taken from `SharpDX`), which ultimately lead to weird inconsistencies between types. 

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
