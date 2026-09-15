# Auto Pin Ores

Auto Pin Ores is a lightweight client mod for **Valheim** that takes the hassle out of marking mining veins. Simply aim your crosshair at any resource deposit or ore node to automatically drop an accurate, labeled pin directly onto your minimap.

Never miss a hidden vein or spend time manually stopping to type pin markers while exploring the wilderness.

---

## Features

* **Instant Hover Pinning** Just look at any valid deposit to place a minimap pin right where your crosshair aims.
* **Smart Node Detection** Uses game piece deposit metadata to accurately differentiate real ore veins from ordinary scenery rocks and landscape boulders.
* **Multiple Ore Support** Automatically pins and labels:
  * Copper
  * Tin
  * Silver
  * Obsidian
  * Iron (Muddy Scrap Piles and Bog Iron)
  * Flametal (Meteorites and Lava nodes)
* **No Clutter Duplicates** Checks your active map before dropping a marker so you never get stacked duplicate pins on the same deposit.
* **Native Deletion Support** You can right click delete pins on your minimap at any time. Aiming at the vein again will safely recreate the pin if needed.

---

## Installation

1. Install **[BepInExPack Valheim](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/)** if you have not already.
2. Download the latest `AutoPinOres.dll` from the **Releases** tab.
3. Place `AutoPinOres.dll` into your game directory under:
   ```text
   Valheim/BepInEx/plugins/

---
## Requirements

* **BepInExPack Valheim**

---

## Building from Source

1. Clone this repository to your machine.
2. Open the solution file in **Visual Studio**.
3. Ensure your project references `assembly_valheim.dll`, `UnityEngine.dll`, `UnityEngine.CoreModule.dll`, `Splatform.dll`, and `0Harmony20.dll` from your local game directory.
4. Target `.NET Framework 4.8` with language version `8.0`.
5. Build using the `Release` configuration to generate `AutoPinOres.dll`.
