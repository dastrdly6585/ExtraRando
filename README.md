# ExtraRando
A Hollow Knight Randomizer 4 connection with miscellaneous settings.

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

## Randomize Colo Access
Adds three "ticket" items that grant access to the three trials in the colosseum.

## Randomize Pantheon Access
Adds five items to the item pool that opens their respective pantheon, skipping their vanilla requirements. Also adds 5 locations for items, when you met the vanilla condition for opening the pantheon. If the door/seal is already open by the time it has an item left, it will place a shiny left from the door/seal that holds the item.

## Randomize Butt
Adds Bardoons Butt as an item and location... yeah.

## Randomize Awful Locations
Adds White Defender 5 and GPZ 10 as locations. In case you're unfamiliar with this, after defeating White Defender 5 times or/and Grey Prince Zote 10 times, you'll get a "reward" (a dirt statue of the knight and the statue of Zote turns golden respectivly). Yeah... these are randomized.
Additionally this will lower the requirement for White Defender to be accessable from 3 to 1 Dreamer (so there is a chance that a dreamer is locked behind White Defender 5 :))

## Split Fireball
Splits the ability to fire Vengeful Spirit/Shade Soul into specific direction items (4 items in total). Also adds two locations at their vanilla location (like with Split Mothwing Cloak).
Note, that if Vengeful Spirit is a starting item, it will be replaced by only one directional item!
**This setting does nothing if "Skills" are not randomized!**

## Split Shade Cloak
Splits the shade cloak ability into two direction items (like "Split Mothwing Cloak"). 
**This setting only has an effect if "Skills" are randomized. Additionally it will split the normal mothwing cloak regardless of the base rando setting.**

## Scarce Item Pool
Remove or packs certain items from the pool to lower the overall item amount.
- Merges spells with the respective upgrade. (If "Split Fireball" is used, it will create a left and right shade soul)
- Merges mothwing and shade cloak into one item (If "Split Mothwing Cloak" or "Split Shade Cloak" is used, one item for the left and one the right side will be created instead).
- Removes 2 full masks, 1 vessel and 1 charm notch.
- If Rando Plus with the randomize nail upgrade is used, one upgrade will be removed as well.
- If Curse Randomizer with Cursed Wallet is used, one wallet will be removed as well.

## Keyring
Merges all 4 simple keys into one item. Does nothing if keys are not randomized. If simple key duplicates are enabled, an extra keyring is added as well (Note that currently the duplicate will still grant the keys instead of geo)

## Block Early Game Stags
This setting will create an item at the stag station door in Dirtmouth near the bench with is required (in addition to the normal stag item) to open the Stag station door.
Additionally it will prevent travelling to Crossroads Stag even if it was unlocked. 
Logic wise this completely removes the possiblity to gain Dirtmouth/Crossroads access via stag item (unless you start in this area obviously), to potentially make the early game more interesting.

## Add Hint Markers
Adds "enhanced" version of the 4 types of marker (which you can be from Iselda) to the item pool.
This will give you hints upon picking them up.

- Scarab Marker: Tells you the location of a "junk" item. This mostly includes geo, maps and soul.
- Gleaming Marker: Tells you the location of a potential useful item. This mostly includes relics, (most) charms, mask shard, vessel fragments and keys. (Also nail upgrades, if the rando plus option is used)
- Token Marker: Tells you the location of useful item. This includes all skills (besides nail arts), nail direction slashes, Dreamers, Kingsoul/Voidheart and Compass (obviously) or special custom items like the reading and listening ability (from Lore Rando), the "Key Ring" item or the Wallet and Dream Nail Fragment items (from Curse Rando)
- Shell Marker: Tells you the **ITEM** of a random location.

**These will only pick items, which have not been obtained yet!**
**This setting will neither remove the ones from the original shop (and neither act as these) nor adding Iseldas shop as a viable rando location on its own!**
The markers will be added to the "Map" pool.
The markers will respawn after sitting on the bench but don't change the hint they gave initially.

## No Logic
Removes all logic from the randomization process. This will greatly decrease the setup time, but obviously could render some seeds **impossible**. It is recommended that some start items and duplicates are used.

**Caution is especially advised if you choose to use "road blocking" connections like Lever Rando, More Doors and/or Breakable Wall Rando.**

# Mod support
- Works with RSM (RandoSettingsManager)
- Location Pins appear in RandoMapMod
