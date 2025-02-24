# TreasureMap

A console application that simulates treasure hunting adventures on a map.

## Overview

TreasureMap is a simulation program that tracks adventurers as they navigate a grid-based map, avoid mountains, and collect treasures. The application reads an input file defining the map layout and adventurer information, runs the simulation, and outputs the final state.

## Features

- Grid-based map with mountains as obstacles
- Multiple treasure locations with varying quantities
- Adventurers with movement sequences (forward, turn left, turn right)
- Turn-based movement system with collision detection
- Treasure collection mechanics
- Detailed logging

## Project Structure

```
TreasureMap/
├── src/
│   ├── TreasureMap.Core/       # Core business logic
│   │   ├── Models/             # Data models
│   │   ├── Enums/              # Enumerations
│   │   ├── Interfaces/         # Abstraction layer
│   │   └── Services/           # Business services
│   └── TreasureMap.Console/    # Console application
│       ├── Application/        # Application logic
│       └── Data/               # Input/output files
└── tests/
    └── TreasureMap.Tests/      # Unit tests
```

## How It Works

1. The application reads a map definition from `input.txt`
2. It creates a virtual map with mountains and treasures
3. Adventurers are placed on the map with predefined movement sequences
4. The simulation executes turn by turn, with each adventurer making one move per turn
5. Adventurers collect treasures as they pass over treasure locations
6. The final state is written to `output.txt`

## Input File Format

```
C - width - height        # Map dimensions
M - x - y                 # Mountain position
T - x - y - count         # Treasure position and count
A - name - x - y - dir - moves  # Adventurer with start position, orientation and movements
```

## Running the Application

The application runs with the predefined input file in the Data directory:

```bash
dotnet run
```

## Technologies

- .NET 8.0
- NLog for logging
- xUnit for unit testing
- Dependency Injection
