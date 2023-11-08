// Copyright (C) 2023  John Russell C. Camo (@russkyc)
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY

namespace Nitefox.App.Media.Entities;

#pragma warning disable CS8618

public class TrackDownload
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string Author { get; init; }
    public string Collection { get; set; }
    public string SavePath { get; set; }
    public bool IsDownloaded { get; set; }
}