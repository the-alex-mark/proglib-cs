using System;

namespace ProgLib.Network
{
    /// <summary>
    /// Типы серверов.
    /// </summary>
    [Flags]
    public enum TypeServer : uint
    {
        All = 0xffffffff,
        Workstation = 0x00000001,
        Server = 0x00000002,
        Sqlserver = 0x00000004,
        Domain_Ctrl = 0x00000008,
        Domain_Bakctrl = 0x00000010,
        Time_Source = 0x00000020,
        AFP = 0x00000040,
        Novell = 0x00000080,
        Domain_Member = 0x00000100,
        Printq_Server = 0x00000200,
        Dialin_Server = 0x00000400,
        Xenix_Server = 0x00000800,
        Server_Unix = Xenix_Server,
        NT = 0x00001000,
        WFW = 0x00002000,
        Server_MFPN = 0x00004000,
        Server_NT = 0x00008000,
        Potential_Browser = 0x00010000,
        Backup_Browser = 0x00020000,
        Master_Browser = 0x00040000,
        Domain_Master = 0x00080000,
        Server_OSF = 0x00100000,
        Server_VMS = 0x00200000,
        Windows = 0x00400000,
        DFS = 0x00800000,
        Cluster_NT = 0x01000000,
        TerminalServer = 0x02000000,
        Cluster_VS_NT = 0x04000000,
        DCE = 0x10000000,
        Alternate_Xport = 0x20000000,
        Local_List_Only = 0x40000000,
        Domain_Enum = 0x80000000
    }
}
