# RadMapCopySharp Manual QA Matrix

## Scope

This matrix covers Phase 6 verification for map/statics copy flows across MUL and UOP workflows.

## Test Data

- Source MUL maps: Felucca (6144x4096), ML (7168x4096), Ilshenar (2304x1600), Tokuno (1448x1448)
- Destination maps: MUL and UOP copies
- Source statics: matching staidx/statics for selected map index
- Definitions presets: definitions.ini entries with Map= tile IDs

## Matrix

1. Felucca MUL -> Felucca MUL, map only
- Copy map checked, statics unchecked
- Altitude mode: Keep Z
- Verify tile IDs and Z unchanged at sample coordinates

2. Felucca MUL -> Felucca MUL, map only, random fixed Z
- Altitude mode: Random Z, Z1/Z2 set
- Verify all copied cells land within [Z1, Z2]

3. Felucca MUL -> Felucca MUL, map only, add random offset
- Altitude mode: Add Random, Z1/Z2 set
- Verify copied Z = source Z + random(Z1..Z2), clamped to sbyte range

4. Felucca MUL -> Felucca MUL, map only, add fixed offset
- Altitude mode: Add Fixed, Offset +28 (leave Z2 at 0/disabled — this is the default UI state)
- Verify copied land Z = source Z + 28 at sample coordinates (e.g. source Z -30 becomes -2)
- With statics enabled, verify static item Z values match source Z + 28
- With spawners enabled, verify CentreZ values match source CentreZ + 28
- Repeat with negative offset (e.g. -10) and confirm Z2=0 still applies correctly

5. Felucca MUL -> Felucca MUL, skip preset
- Choose preset with known tile IDs
- Verify matching map tile IDs are not overwritten in destination

6. Felucca MUL -> Felucca MUL, statics only
- Copy map unchecked, statics checked
- Block-aligned source and destination coords
- Verify destination block index entries are rewritten and statics appear in-game

7. Felucca MUL -> Felucca MUL, map + statics
- Both toggles checked
- Verify Add Random mode radio is hidden
- Verify Add Fixed mode remains available
- Verify both land and statics copy complete successfully

8. Misaligned statics coordinates
- Statics enabled with non-block-aligned coordinates
- Verify operation rejects with alignment error

9. Create Empty Map (all presets)
- Run Create Empty Map for each preset
- Verify output file size matches profile dimensions
- Optional statics creation produces initialized staidx and empty statics

10. Extend to ML
- Source: non-ML profile map
- Verify output is 7168x4096 and source area is copied in top-left
- Verify ML source is rejected

11. MUL -> UOP land patch
- Destination map path is .uop
- Confirm warning prompt appears and operation proceeds on user confirmation
- Verify cell updates are present in destination UOP copy

12. MUL -> UOP with statics enabled
- Verify warning includes note that statics remain MUL files
- Verify map writes to UOP and statics writes to destination MUL files

13. Spawner copy by centre in source rect
- Source spawner xml with entries where CentreX/CentreY is inside the copy rect but anchor X/Y is outside
- Verify all centre-in-rect spawners are copied (not only those with anchor inside)
- Example: Ilshenar rect 1752,944-1872,1008 should copy 4 spawners, not 2
- Preview with copy overlay: source pane "In copy area" shows only spawners whose centre is in the source rect
- Preview "All spawners" shows the full facet overlay; both checkboxes can be toggled independently (both checked shows all)

14. Spawner tooltip lists all Objects2 creatures
- Open preview with Felucca spawns; hover a spawner with chained Objects2 (e.g. WildLife#22)
- Tooltip Spawns line lists all creature types (Boar, Cougar, Goat, etc.), not only the first entry

15. Point spawners (Width/Height zero) in preview
- Load Felucca `Spawns/Felucca/Towns/vendors.xml` with **All spawners** checked
- Expect ~288 spawner markers on Felucca (not 12)
- Hover a point spawner (`Width=0`, `Height=0`) — tooltip shows vendor name, range, and creature list
- Range circle is clickable for tooltip (not only the 1x1 centre tile)

## Delphi Parity Notes

- Replicated behavior
- Statics copy replaces destination 8x8 block by appending new static data and updating staidx entry.
- Skip ID presets apply to map tiles only.
- Coordinate validator rejects all-zero source rectangle.

- Intentional improvements
- Added map profile auto-detection by size and filename hints.
- Added UOP destination write support for land-map cells.
- Added explicit warning dialog for UOP destinations and backup recommendation.
- Added settings persistence for map and statics paths.
- Added menu shell with Help/About and versioned title.

