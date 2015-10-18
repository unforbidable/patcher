using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Data.Plugins.Content.Constants.Skyrim
{
    public enum MiscStat
    {
        AnimalsKilled = unchecked((int)0xFCDD5011),
        ArmorImproved = 0x366D84CF,
        ArmorMade = 0x023497E6,

        // Misc stat values appear to be some kind of hash produced from the stat name
    }
}
