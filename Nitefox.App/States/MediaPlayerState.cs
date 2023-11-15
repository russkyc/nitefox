// Copyright (C) 2023  John Russell C. Camo (@russkyc)
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY

using PropertyChanged.SourceGenerator;

namespace Nitefox.App.States;

public partial class MediaPlayerState
{
    [Notify] private bool _loaded;
    [Notify] private string? _track = string.Empty;
    [Notify] private string? _streamUrl = string.Empty;
    
    public void Play(string track, string url)
    {
        Track = track;
        StreamUrl = url;
    }
    
    public void Stop()
    {
        Track = string.Empty;
        StreamUrl = string.Empty;
        Loaded = false;
    }

    public bool IsPlaying(string track)
    {
        return track == Track && Loaded;
    }
}