/// Copyright(C) 2015 Unforbidable Works
///
/// This program is free software; you can redistribute it and/or
/// modify it under the terms of the GNU General Public License
/// as published by the Free Software Foundation; either version 2
/// of the License, or(at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
/// GNU General Public License for more details.
///
/// You should have received a copy of the GNU General Public License
/// along with this program; if not, write to the Free Software
/// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patcher.Data.Plugins.Content
{
    /// <summary>
    /// Provides 4-letter singatures for records and fields.
    /// </summary>
    public static class Names
    {
        public const string ACHR = "ACHR";
        public const string ACTI = "ACTI";
        public const string ALCH = "ALCH";
        public const string AMMO = "AMMO";
        public const string AORU = "AORU";
        public const string ARMA = "ARMA";
        public const string ARMO = "ARMO";
        public const string ASPC = "ASPC";
        public const string ASTP = "ASTP";
        public const string BOOK = "BOOK";
        public const string CELL = "CELL";
        public const string CLAS = "CLAS";
        public const string COBJ = "COBJ";
        public const string COLL = "COLL";
        public const string CONT = "CONT";
        public const string DOOR = "DOOR";
        public const string ECZN = "ECZN";
        public const string ENCH = "ENCH";
        public const string EQUP = "EQUP";
        public const string EXPL = "EXPL";
        public const string FACT = "FACT";
        public const string FLST = "FLST";
        public const string FURN = "FURN";
        public const string GLOB = "GLOB";
        public const string GMST = "GMST";
        public const string GRAS = "GRAS";
        public const string IDLE = "IDLE";
        public const string IDLM = "IDLM";
        public const string IMGS = "IMGS";
        public const string INGR = "INGR";
        public const string IPDS = "IPDS";
        public const string KEYM = "KEYM";
        public const string KYWD = "KYWD";
        public const string LCRT = "LCRT";
        public const string LCTN = "LCTN";
        public const string LIGH = "LIGH";
        public const string LVLI = "LVLI";
        public const string LVSP = "LVSP";
        public const string MATT = "MATT";
        public const string MISC = "MISC";
        public const string MGEF = "MGEF";
        public const string MSTT = "MSTT";
        public const string NPC_ = "NPC_";
        public const string PACK = "PACK";
        public const string PARW = "PARW";
        public const string PBEA = "PBEA";
        public const string PCON = "PCON";
        public const string PERK = "PERK";
        public const string PFLA = "PFLA";
        public const string PGRE = "PGRE";
        public const string PHZD = "PHZD";
        public const string PLYR = "PLYR";
        public const string PMIS = "PMIS";
        public const string PROJ = "PROJ";
        public const string QUST = "QUST";
        public const string RACE = "RACE";
        public const string REFR = "REFR";
        public const string REGN = "REGN";
        public const string RFCT = "RFCT";
        public const string SCEN = "SCEN";
        public const string SCRL = "SCRL";
        public const string SHOU = "SHOU";
        public const string SLGM = "SLGM";
        public const string SNDR = "SNDR";
        public const string SOUN = "SOUN";
        public const string SPEL = "SPEL";
        public const string SPGD = "SPGD";
        public const string STAT = "STAT";
        public const string TACT = "TACT";
        public const string TES4 = "TES4";
        public const string TREE = "TREE";
        public const string TXST = "TXST";
        public const string VTYP = "VTYP";
        public const string WEAP = "WEAP";
        public const string WRLD = "WRLD";
        public const string WTHR = "WTHR";

        public const string ANAM = "ANAM";
        public const string BAMT = "BAMT";
        public const string BIDS = "BIDS";
        public const string BNAM = "BNAM";
        public const string BODT = "BODT";
        public const string BOD2 = "BOD2";
        public const string CIS1 = "CIS1";
        public const string CIS2 = "CIS2";
        public const string COCT = "COCT";
        public const string CNAM = "CNAM";
        public const string CNTO = "CNTO";
        public const string COED = "COED";
        public const string CRDT = "CRDT";
        public const string CTDA = "CTDA";
        public const string DATA = "DATA";
        public const string DALC = "DALC";
        public const string DELE = "DELE";
        public const string DESC = "DESC";
        public const string DEST = "DEST";
        public const string DNAM = "DNAM";
        public const string DSTD = "DSTD";
        public const string DSTF = "DSTF";
        public const string EAMT = "EAMT";
        public const string EDID = "EDID";
        public const string EFID = "EFID";
        public const string EFIT = "EFIT";
        public const string EITM = "EITM";
        public const string ENAM = "ENAM";
        public const string ENIT = "ENIT";
        public const string ETYP = "ETYP";
        public const string FLTV = "FLTV";
        public const string FNAM = "FNAM";
        public const string FULL = "FULL";
        public const string GNAM = "GNAM";
        public const string HEDR = "HEDR";
        public const string HNAM = "HNAM";
        public const string ICON = "ICON";
        public const string IMSP = "IMSP";
        public const string INAM = "INAM";
        public const string INCC = "INCC";
        public const string INTV = "INTV";
        public const string JNAM = "JNAM";
        public const string KSIZ = "KSIZ";
        public const string KWDA = "KWDA";
        public const string LNAM = "LNAM";
        public const string MAST = "MAST";
        public const string MICO = "MICO";
        public const string MNAM = "MNAM";
        public const string MODL = "MODL";
        public const string MODS = "MODS";
        public const string MODT = "MODT";
        public const string MOD2 = "MOD2";
        public const string MO2S = "MO2S";
        public const string MO2T = "MO2T";
        public const string MOD4 = "MOD4";
        public const string MO4S = "MO4S";
        public const string MO4T = "MO4T";
        public const string NAM0 = "NAM0";
        public const string NAM1 = "NAM1";
        public const string NAM2 = "NAM2";
        public const string NAM3 = "NAM3";
        public const string NAM4 = "NAM4";
        public const string NAM5 = "NAM5";
        public const string NAM6 = "NAM6";
        public const string NAM7 = "NAM7";
        public const string NAM8 = "NAM8";
        public const string NAM9 = "NAM9";
        public const string NNAM = "NNAM";
        public const string OBND = "OBND";
        public const string OFST = "OFST";
        public const string ONAM = "ONAM";
        public const string PNAM = "PNAM";
        public const string QNAM = "QNAM";
        public const string RNAM = "RNAM";
        public const string SCRN = "SCRN";
        public const string SNAM = "SNAM";
        public const string TNAM = "TNAM";
        public const string UNAM = "UNAM";
        public const string VMAD = "VMAD";
        public const string VNAM = "VNAM";
        public const string WGDR = "WGDR";
        public const string WNAM = "WNAM";
        public const string XNAM = "XNAM";
        public const string YNAM = "YNAM";
        public const string ZNAM = "ZNAM";
    }
}
