# CyberPunch 🥊⚡

A 2D fighting game built with Unity, pitting cybernetically-enhanced brawlers against each other in fast-paced arena combat.

## Features

- **State-driven combat** — a fighter state machine (`FighterStateMachine`) drives movement, attacks, and reactions
- **Hit/Hurtbox system** — precise collision-based combat via `Hitbox` / `Hurtbox` and the `IDamageable` interface
- **Attack data as assets** — attacks (`LightPunch`, `HeavyPunch`) are defined as ScriptableObjects (`AttackData`) for easy tuning without touching code
- **Health & match flow** — `FighterHealth` tracks damage, `MatchManager` and `MatchEndController` handle round/match state
- **UI** — `MainMenuController` for the front end, `HUDController` for in-match health/status display
- **Audio** — centralized playback through `AudioManager`

## Tech Stack

- **Engine:** Unity `6000.3.19f1`
- **Language:** C#
- **Input:** Unity Input System (`InputSystem_Actions`)

## Project Structure

```
Assets/
├── Attacks/          # ScriptableObject attack definitions (LightPunch, HeavyPunch)
├── Scenes/           # MainMenu, Arena, SampleScene
├── Scripts/           # Gameplay, UI, and system code
├── Materials/
└── Settings/          # Render pipeline / input settings
```

## Core Scripts

| Script | Responsibility |
|---|---|
| `FighterStateMachine` | Central controller for a fighter's behavior states |
| `FighterMovement` | Movement and physics |
| `FighterAnimator` | Drives animation from fighter state |
| `FighterHealth` | Damage, health, and death handling |
| `AttackData` | ScriptableObject describing an attack's stats |
| `Hitbox` / `Hurtbox` | Combat collision detection |
| `IDamageable` | Contract for anything that can take damage |
| `MatchManager` | Round/match lifecycle |
| `MatchEndController` | End-of-match flow and results |
| `HUDController` | In-match UI (health bars, etc.) |
| `MainMenuController` | Main menu navigation |
| `AudioManager` | Sound effects and music playback |

## Getting Started

1. Install **Unity 6000.3.19f1** (or later 6000.3.x) via Unity Hub
2. Clone the repo:
   ```bash
   git clone https://github.com/alisonsavi64/CyberPunch.git
   ```
3. Open the project folder in Unity Hub
4. Open the `MainMenu` scene under `Assets/Scenes` and hit Play

## Scenes

- **MainMenu** — entry point and navigation
- **Arena** — the main fighting stage
- **SampleScene** — default Unity template scene
