// Copyright (C) 2023  John Russell C. Camo (@russkyc)
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY

using Russkyc.Configuration;

namespace Nitefox.App.Configuration;

public class NitefoxConfig : ConfigProvider
{
    public NitefoxConfig() : base($"{AppContext.BaseDirectory}nitefox.json")
    {
        try
        {
            if (string.IsNullOrWhiteSpace(FfmpegLocation))
            {
                FfmpegLocation = $"{AppContext.BaseDirectory}ffmpeg\\";
            }
        }
        catch (Exception)
        {
            FfmpegLocation = $"{AppContext.BaseDirectory}ffmpeg\\";
            throw;
        }

        try
        {
            if (string.IsNullOrWhiteSpace(TempFilesLocation))
            {
                TempFilesLocation = $"{AppContext.BaseDirectory}temp\\";
            }
        }
        catch (Exception)
        {
            TempFilesLocation = $"{AppContext.BaseDirectory}temp\\";
            throw;
        }

        try
        {
            if (string.IsNullOrWhiteSpace(DownloadLocation))
            {
                DownloadLocation = $"{AppContext.BaseDirectory}songs\\";
            }
        }
        catch (Exception)
        {
            DownloadLocation = $"{AppContext.BaseDirectory}songs\\";
            throw;
        }
    }

    public string FfmpegLocation
    {
        get => GetValue<string>(nameof(FfmpegLocation));
        set => SetValue(nameof(FfmpegLocation), value);
    }
    
    public string TempFilesLocation
    {
        get => GetValue<string>(nameof(TempFilesLocation));
        set => SetValue(nameof(TempFilesLocation), value);
    }
    
    public string DownloadLocation
    {
        get => GetValue<string>(nameof(DownloadLocation));
        set => SetValue(nameof(DownloadLocation), value);
    }
}