# Town Without Light - Unity Prototype

This folder contains a Unity 2022.3 LTS project scaffold with the baseline gameplay systems requested in the Phase 0 task list:

- Character-based movement and mouse look driven by `PlayerController` and `PlayerLook`.
- Flashlight toggle logic via `Flashlight`.
- Raycast-based interaction using `Interactor`, `IInteractable`, and `InvestigationPoint`.
- Global `GameStateManager`, JSON `SaveSystem`, and a `PlayerPersistence` helper.
- A title screen controller script that wires "New Game" and "Continue" buttons.

To try the prototype inside Unity:

1. Open the project folder with Unity 2022.3 LTS.
2. Create the `TitleScene` and `MainScene` described in `Assets/Scenes/README.md`.
3. Add the `Player` prefab described in `Assets/Prefabs/README.md` to `MainScene` and reference the scripts accordingly.
4. Press Play to validate input, interaction, save/load, and flashlight toggling.
5. Use **Build & Run** (Development Build) to perform the lightweight performance smoke test.
