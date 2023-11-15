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

public class PhotinoFileService(NitefoxConfig nitefoxConfig) : IFileService
{
    public Task<string> OpenFolder()
    {
        try
        {
            var path = Program.App.MainWindow.ShowOpenFolder();
            return Task.FromResult(path[0]);
        }
        catch (Exception)
        {
            return Task.FromResult("");
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
        if (string.IsNullOrWhiteSpace(nitefoxConfig.DownloadLocation))
        {
            nitefoxConfig.DownloadLocation = Environment.CurrentDirectory + "\\songs\\";
        }
        
        if (!Directory.Exists(nitefoxConfig.DownloadLocation))
        {
            Directory.CreateDirectory(nitefoxConfig.DownloadLocation);
        }

        if (string.IsNullOrWhiteSpace(nitefoxConfig.TempFilesLocation))
        {
            nitefoxConfig.TempFilesLocation = Environment.CurrentDirectory + "\\temp\\";
        }
        
        if (!Directory.Exists(nitefoxConfig.TempFilesLocation))
        {
            Directory.CreateDirectory(nitefoxConfig.TempFilesLocation);
        }

        if (string.IsNullOrWhiteSpace(nitefoxConfig.FfmpegLocation))
        {
            nitefoxConfig.FfmpegLocation = Environment.CurrentDirectory + "\\ffmpeg\\";
        }
        
        if (!Directory.Exists(nitefoxConfig.FfmpegLocation))
        {
            Directory.CreateDirectory(nitefoxConfig.FfmpegLocation);
        }
    }
}