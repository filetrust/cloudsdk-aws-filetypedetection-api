﻿namespace Glasswall.Core.Engine.Common.FileProcessing
{
    public interface IFileProtectResponse
    {
        byte[] ProtectedFile { get; set; }
        EngineOutcome Outcome { get; set; }
    }
}
