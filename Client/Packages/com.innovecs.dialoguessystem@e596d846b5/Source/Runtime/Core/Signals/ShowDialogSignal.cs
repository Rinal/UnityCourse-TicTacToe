﻿using System;

namespace Innovecs.DialoguesSystem
{
    /// <summary>
    /// Target of class subscribe for show dialog signal instead of event of base dialog class.
    /// This allow subscribe to signal and not get or instantiate dialog that need to track events
    /// </summary>
    public sealed class ShowDialogSignal
    {
        public Type Type { get; set; }
    }
}