# Mixed Reality 2024
## SBBHologuide
- Authors: Riccardo Bollati, Federica Bruni, Luca Tagliavini, Christopher Zanoli

## 1. Project Description
Dynamic railway maintenance demands adaptability and expertise, yet current systems lack clear guidance. SBB HoloGuide bridges this gap with AR-powered train door maintenance. 

You can find the project report [here](//TODO)

## 2. Project Organization
```
├───Editor
├───Materials <- Materials applied to the objects objects
├───MRTK.Generated
├───Prefabs <- Prefabs we created
│   ├───AnchorPoints <- Prefab for the Door placeholder used by the user to place the door
│   ├───Images <- Images used in the variuos interfaces
│   ├───Menu <- Main menu (task selection menu) prefabs
│   ├───miniature <- Dynamic miniature prefabs
│   ├───PlaceDoor <- StepsMenu prefab
│   ├───PopUp <- Tutorial popup prefabs
│   └───Textures <- Menus Textures
├───Samples
│   └───XR Interaction Toolkit
│       └───3.0.3
│           └───XR Device Simulator
│               ├───Hand Expression Captures
│               ├───Scripts
│               └───UI
│                   ├───ControllerDevice
│                   ├───General
│                   ├───Hands
│                   ├───HeadDevice
│                   └───Mouse
├───Scenes <- Scenes folder, it contains the main scene of the application
├───Scripts
│   ├───DoorScene <- Scripts relative to object menagement
│   ├───LoginScene <- Scripts to handle user login at application startup
│   ├───MenuScene <- Main menu scripts
│   ├───PlacementScene <- Scripts used during the door placement
│   └───Utils <- Utilities
├───Settings
├───TextMesh Pro
│   ├───Documentation
│   ├───Examples & Extras
│   │   ├───Fonts
│   │   ├───Materials
│   │   ├───Prefabs
│   │   ├───Resources
│   │   │   ├───Color Gradient Presets
│   │   │   ├───Fonts & Materials
│   │   │   └───Sprite Assets
│   │   ├───Scenes
│   │   ├───Scripts
│   │   ├───Sprites
│   │   └───Textures
│   ├───Fonts
│   ├───Resources
│   │   ├───Fonts & Materials
│   │   ├───Sprite Assets
│   │   └───Style Sheets
│   ├───Shaders
│   └───Sprites
├───TutorialInfo <- Tutorial related Scripts
│   ├───Icons
│   └───Scripts
│       └───Editor
├───UI Toolkit
│   └───UnityThemes
├───XR
│   ├───Loaders
│   ├───Resources
│   ├───Settings
│   └───UserSimulationSettings
│       └───Resources
└───XRI
    └───Settings
        └───Resources
```
## 3. Project Setup
This software requires Unity, version starting from 2022.3 on. This software also require all the following dependencies to integrate the development environment with MRTK 3 Features (these features can be intalled using "MixedRealityFeatureTool"):
- MRTK3
- Platform Support

## 4. Project pipeline

## 5. Preliminary results

## 6. Conclusions

We developed an AR app enabling novice SBB operators to efficiently maintain train doors,achieving tasks 3x faster. Future plans include automating door placement and integrating an ETL data layer.

