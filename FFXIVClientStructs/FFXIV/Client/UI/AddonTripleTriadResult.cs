using FFXIVClientStructs.FFXIV.Component.GUI;

namespace FFXIVClientStructs.FFXIV.Client.UI;

// Client::UI::AddonTripleTriadResult
//   Component::GUI::AtkUnitBase
//     Component::GUI::AtkEventListener
[StructLayout(LayoutKind.Explicit, Size = 0x260)]
public unsafe struct AddonTripleTriadResult
{
    [FieldOffset(0x0)] public AtkUnitBase AtkUnitBase;

    [FieldOffset(0x220)] public AtkTextNode* AtkTextNode220;
    [FieldOffset(0x228)] public AtkTextNode* AtkTextNode228;

    [FieldOffset(0x230)] public AtkComponentButton* QuitButton;
    [FieldOffset(0x238)] public AtkComponentButton* RematchButton;

    [FieldOffset(0x240)] public AtkComponentCheckBox* AtkComponentCheckBox240;
}