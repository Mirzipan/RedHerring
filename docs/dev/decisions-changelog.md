# Decisions Changelog

## Purpose

This document tracks more or less important decisions about the engine, in order to allow easy referencing in the future.

## [2023-10-21]

### Game / Session
Game was renamed to Session in order to better communicate its intention.
It's original name would make it seem like it needed to contain the whole game logic.
The intent of the Session is to be used for actual game session, such as after the player clicks `new game` or `load game` in the main menu.
This means you can have different Session per game mode, which can reuse or even completely eliminate game mode checks within the game logic, since only the component relevant for the specific game mode can be loaded, instead of all of them.
