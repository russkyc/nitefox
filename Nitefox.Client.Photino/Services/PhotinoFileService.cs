// Copyright (C) 2023  John Russell C. Camo (@russkyc)
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY

using System;
using System.IO;
using System.Threading.Tasks;
using Nitefox.App.Configuration;
using Nitefox.App.Services.Interfaces;

namespace Nitefox.Client.Photino.Services;

public class PhotinoFileService : IFileService
{
    private readonly NitefoxConfig _nitefoxConfig;

    public PhotinoFileService(NitefoxConfig nitefoxConfig)
    {
        _nitefoxConfig = nitefoxConfig;
        SetupDirectories();
    }
    
    public async Task<string> OpenFolder()
    {
        try
        {
            var path = await Program.App.MainWindow.ShowOpenFolderAsync();
            return path[0];
        }
        catch (Exception)
        {
            return "";
        }
    }
    
    public bool CreateMediaDirectory(string basePath, string directoryName)
    {
        var mediaDirectory = $"{basePath}{directoryName}\\";
        
        if (Directory.Exists(mediaDirectory))
        {
            return true;
        }
        
        Directory.CreateDirectory(mediaDirectory);
        return Directory.Exists(mediaDirectory);
    }

    public void SetupDirectories()
    {
        if (string.IsNullOrWhiteSpace(_nitefoxConfig.DownloadLocation))
        {
            _nitefoxConfig.DownloadLocation = Environment.CurrentDirectory + "\\songs\\";
        }
        
        if (!Directory.Exists(_nitefoxConfig.DownloadLocation))
        {
            Directory.CreateDirectory(_nitefoxConfig.DownloadLocation);
        }

        if (string.IsNullOrWhiteSpace(_nitefoxConfig.TempFilesLocation))
        {
            _nitefoxConfig.TempFilesLocation = Environment.CurrentDirectory + "\\temp\\";
        }
        
        if (!Directory.Exists(_nitefoxConfig.TempFilesLocation))
        {
            Directory.CreateDirectory(_nitefoxConfig.TempFilesLocation);
        }

        if (string.IsNullOrWhiteSpace(_nitefoxConfig.FfmpegLocation))
        {
            _nitefoxConfig.FfmpegLocation = Environment.CurrentDirectory + "\\ffmpeg\\";
        }
        
        if (!Directory.Exists(_nitefoxConfig.FfmpegLocation))
        {
            Directory.CreateDirectory(_nitefoxConfig.FfmpegLocation);
        }
    }
}