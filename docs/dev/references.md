# References - analysis

## Full auto - Unity / Godot 4
### Pros
- easy to use
- readable
- rename/move doesn't affect references
- no GUID database in game
- should work on "foreign" types too
- automatic load in game
- class mirrors can be migratable, migration doesn't affect game objects

### Cons
- slow
- memory overhead
- complex to create
- **game also needs class mirrors or customized JSON parser**
- **not complete control about loading**
- **hard to create migratable class mirrors automatically - different approach?**
- any studio/inspector specific attribute must be also known in the engine code

## Hybrid - Studio reference + Engine reference
### Pros
- completely decoupled studio and game references
- rename/move doesn't affect references
- no GUID database in game
- direct deserialization to classes
- custom migration, easy to maintain, only known to studio - engine is clean
### Cons
- manual load in game
- every reference needs to be created as studio reference and engine reference
- **classes are duplicated - for studio and engine**
- complex, hard to understand
- **studio needs to know about every type, hard to implement for "foreign" types**

## Paths with attributes
### Pros
- easy to use: just attribute and string
- readable in debugger
- no GUID database needed
### Cons
- all assets needs to be loaded and fixed with every change in the project
- **changes outside studio can easily break references! (for example git pull)**
- manual load in game
- **in-game classes used in studio don't have migration or migration integrated into game, where it's useless**
- any studio/inspector specific attribute must be also known in the engine code
- where to load referenced asset? - needs additional storage/management

## GUIDs with attributes

### Pros
- easy to use: just attribute and string
- rename/move doesn't affect references
### Cons
 - GUID database in game
 - manual load in game
 - **in-game classes used in studio don't have migration or migration integrated into game, where it's useless**
 - any studio/inspector specific attribute must be also known in the engine code
 - where to load referenced asset? - needs additional storage/management

## Generic references with GUIDs
### Pros
- easy to use: `Reference<Type>`
- rename/move doesn't affect references
- simple load and storage for loaded asset with easy access
### Cons
- GUID database in game
- manual load in game
- **in-game classes used in studio don't have migration or migration integrated into game, where it's useless**
- any studio/inspector specific attribute must be also known in the engine code

