# Engine Architecture

## Purpose
This document shows a high-level outline of how the engine works and/or is intended to be used.

## Overview

### Engine
The class named `Engine` is the main entry-point for the engine. It does require an instance of `AnEngineContext` to run.

### Session
A game `Session` may be run by supplying the `Engine` with `ASessionComponent`.
