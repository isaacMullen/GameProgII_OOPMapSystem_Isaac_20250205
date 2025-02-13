# Class Structure Justification

## Overview

The current state of my project is still very small in scope, the structure of my classes reflect this. I have a `GameEntity` class that all players, NPCs, and enemies will inherit from. This is efficient because:
- There are typically a lot of game entities in a game, and this class defines a structure for them to follow.
- It also has common virtual methods so I retain individuality over each derived entity.

## Map System

I do have some concerns about my map system, particularly the `TileManager` class. Here's what I have so far:

- **Tile Class**: There is a `Tile` class, which could use some review at this point.
- **TileManager Class**: This is responsible for:
  - Loading maps from a text file.
  - Generating random maps if desired.

### Refactoring Considerations

- **GameManager or MapBuilder**: I'm thinking of whether some things should be moved out of the `TileManager` class and into something like a `GameManager` or `MapBuilder`. I'll revisit this in the next sprint and look for opportunities to decouple where possible.

## Game1 Class

Currently, my `Game1` class isn't as lean as I'd like. There are a few ideas I have to abstract some code into `TileManager` or maybe a `GameManager`:
- **Map File Management**: One of the things is the locating and compiling of map files into a list. I feel like this could be done outside of `Game1` quite easily.

## Movement and Collision

I have very barebones movement as well as collision, and I'm happy with how it works. I just worry about future sprints and whether the structure will impede me from fulfilling specific requirements.

## Future Concerns

One thing I'm anticipating is the dynamic switching between predetermined maps and generated maps. I still am not 100% sure how I'd do this in a succinct way yet.
