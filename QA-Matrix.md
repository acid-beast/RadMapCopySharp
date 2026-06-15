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

4. Felucca MUL -> Felucca MUL, skip preset
- Choose preset with known tile IDs
- Verify matching map tile IDs are not overwritten in destination

5. Felucca MUL -> Felucca MUL, statics only
- Copy map unchecked, statics checked
- Block-aligned source and destination coords
- Verify destination block index entries are rewritten and statics appear in-game

6. Felucca MUL -> Felucca MUL, map + statics
- Both toggles checked
- Verify Add Random mode radio is hidden
- Verify both land and statics copy complete successfully

7. Misaligned statics coordinates
- Statics enabled with non-block-aligned coordinates
- Verify operation rejects with alignment error

8. Create Empty Map (all presets)
- Run Create Empty Map for each preset
- Verify output file size matches profile dimensions
- Optional statics creation produces initialized staidx and empty statics

9. Extend to ML
- Source: non-ML profile map
- Verify output is 7168x4096 and source area is copied in top-left
- Verify ML source is rejected

10. MUL -> UOP land patch
- Destination map path is .uop
- Confirm warning prompt appears and operation proceeds on user confirmation
- Verify cell updates are present in destination UOP copy

11. MUL -> UOP with statics enabled
- Verify warning includes note that statics remain MUL files
- Verify map writes to UOP and statics writes to destination MUL files

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

