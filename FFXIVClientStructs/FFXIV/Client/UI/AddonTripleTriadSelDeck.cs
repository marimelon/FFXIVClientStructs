using FFXIVClientStructs.FFXIV.Component.GUI;

namespace FFXIVClientStructs.FFXIV.Client.UI;

// Client::UI::TripleTriadSelDeck
//   Component::GUI::AtkUnitBase
//     Component::GUI::AtkEventListener
[StructLayout(LayoutKind.Explicit, Size = 0x230)]
public unsafe struct AddonTripleTriadSelDeck
{
    [FieldOffset(0x0)] public AtkUnitBase AtkUnitBase;

    [FieldOffset(0x220)] public AtkComponentList* AtkComponentList220;
    [FieldOffset(0x228)] public AtkTextNode* AtkTextNode228;
}