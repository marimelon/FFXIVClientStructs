using FFXIVClientStructs.FFXIV.Component.GUI;

namespace FFXIVClientStructs.FFXIV.Client.UI;

// Client::UI::AddonTripleTriadRequest
//   Component::GUI::AtkUnitBase
//     Component::GUI::AtkEventListener
[StructLayout(LayoutKind.Explicit, Size = 0x2B0)]
public unsafe struct AddonTripleTriadRequest
{
    [FieldOffset(0x0)] public AtkUnitBase AtkUnitBase;

    [FieldOffset(0x220)] public AtkTextNode* AtkTextNode220;
    [FieldOffset(0x228)] public AtkTextNode* AtkTextNode228;
    [FieldOffset(0x230)] public AtkTextNode* AtkTextNode230;
    [FieldOffset(0x238)] public AtkTextNode* AtkTextNode238;

    [FieldOffset(0x240)] public AtkComponentDropDownList* AtkComponentDropDownList240;
    [FieldOffset(0x248)] public AtkComponentDropDownList* AtkComponentDropDownList248;

    [FieldOffset(0x250)] public AtkComponentBase* AtkComponentBase250;  // Regional Rule 1
    [FieldOffset(0x258)] public AtkComponentBase* AtkComponentBase258;  // Regional Rule 2
    [FieldOffset(0x260)] public AtkComponentBase* AtkComponentBase260;  // Match Rule 1
    [FieldOffset(0x268)] public AtkComponentBase* AtkComponentBase268;  // Match Rule 2 

    [FieldOffset(0x270)] public AtkResNode* MatchRulesResNode;

    [FieldOffset(0x278)] public AtkComponentButton* ChallengeButton;
    [FieldOffset(0x280)] public AtkComponentButton* QuitButton;
    [FieldOffset(0x288)] public AtkComponentButton* RuleChangeButton;

    [FieldOffset(0x2A8)] public AtkTextNode* AtkTextNode2A8;
}