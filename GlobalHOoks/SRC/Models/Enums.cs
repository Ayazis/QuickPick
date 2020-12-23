using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QuickPick.Enums
{ 
    public enum ClickAction
    {
        None,
        [Display(Name="SQL query")]
        RunQuery,     
        [Display(Name = "Launch Process")]
        RunProcess,
        [Display(Name = "Screen Snippet")]
        TakeSnippet,
        [Display(Name = "Close QuickPick")]
        ExitQuickPick,
        [Display(Name = "Paste text")]
        PasteText,
    }

    public enum HotKey
    {
        KeyCombination,
        XMouse1,
        XMouse2
    }



}
