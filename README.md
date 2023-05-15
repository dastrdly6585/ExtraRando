# ExtraRando
A Hollow Knight Randomizer 4 connection with miscellaneous settings.

## Split Shade Cloak
Splits the shade cloak ability into two direction items (like "Split Mothwing Cloak"). **This setting only has an effect if "Skills" are randomized. Additionally it will split the normal mothwing cloak regardless of the base rando setting.**

## Scarce Item Pool
Remove or packs certain items to lower the overall item amount.
- Merges spells with the respective upgrade.
- Merges all small keys into one "Key ring" which grants you 4 keys on pickup.
- Merges mothwing and shade cloak into one item (If "Split Mothwing Cloak" or "Split Shade Cloak" is used, one item for the left and one the right side will be created instead).
- Removes 2 full masks, 1 vessel and 1 charm notch.
- If Rando Plus with the randomize nail upgrade is used, one upgrade will be removed as well.
- If Curse Randomizer with Cursed Wallet is used, one wallet will be removed as well.

## Randomize Hot Springs
Adds 6 "Hot Spring Water" items to the rando pool as well as their respective location.
This includes:
- Crossroads
- Deepnest
- Colosseum
- Bathhouse
- Lower Godhome (The sea at the bottom right)
- Upper Godhome (Requires beating Pantheon 4 (or a bench I guess))

**The Hot Spring within the Pantheons is NOT randomized!**

Picking up Hot Spring water will quickly replenish your soul (20 per second) and health (1 health each 1.5 seconds). This effect last 30 seconds.
Hot Spring Water is in the same item group as soul totems and behaves like them (Refreshes after sitting on a bench).

**Since the state calculation wasn't changed, there is a rare chance that rando expects you to get soul via a Hot Spring. If this is your only source of soul/health this might result in a softlock without debug. Caution is advised.**

## Randomize Markers
Adds enhanced version of the 4 types of marker (which you can be from Iselda) to the item pool.
This will give you hints upon picking them up.

- Scarab Marker: Tells you the location of a "junk" item. This mostly includes geo, maps and soul.
- Gleaming Marker: Tells you the location of a potential useful item. This mostly includes relics, (most) charms, mask shard, vessel fragments and keys. (Also nail upgrades, if the rando plus option is used)
- Token Marker: Tells you the location of useful item. This includes all skills (besides nail arts), nail direction slashes, Dreamers, Kingsoul/Voidheart and Compass (obviously) or special custom items like the reading and listening ability (from Lore Rando), the "Key Ring" item or the Wallet and Dream Nail Fragment items (from Curse Rando)
- Shell Marker: Tells you the **ITEM** of a random location.

**These will only pick items, which have not been obtained yet!**
**This setting will neither remove the ones from the original shop (and neither act as these) nor adding Iseldas shop as a viable rando location on its own!**
The markers will be added to the "Map" pool.
One of each marker is added, but for each 100 randomized items one of each type is additionally added.

## No Logic
Removes all logic from the randomization process. This will greatly decrease the setup time, but obviously could render some seeds **impossible**. It is recommended that some start items and duplicates are used.

Caution is especially advised if you choose to use "road blocking" connections like Lever Rando, More Doors and/or Breakable Wall Rando.

# Mod support
- Works with RSM (RandoSettingsManager)
- Hot Spring Pins appear on RMM (RandoMapMod)

# To do
- Adjust RMM Pins + sprites
- Readd Bardoon Butt setting.
