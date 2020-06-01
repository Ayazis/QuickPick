﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GlobalHOoks.Enums
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
        ExitQuickPick
    }
}
