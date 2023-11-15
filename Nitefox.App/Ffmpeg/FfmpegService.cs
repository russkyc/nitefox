// Copyright (C) 2023  John Russell C. Camo (@russkyc)
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY

using FFMpegCore;
using GithubReleaseDownloader;
using GithubReleaseDownloader.Entities;
using Nitefox.App.Configuration;
using SharpCompress.Common;
using SharpCompress.Readers;
namespace Nitefox.App.Ffmpeg;

public class FfmpegService
{
    private readonly NitefoxConfig _nitefoxConfig;

    public FfmpegService(NitefoxConfig nitefoxConfig)
    {
        _nitefoxConfig = nitefoxConfig;
        
    }

    public async Task<bool> StreamConvert(string url, string fileName)
    {
        return await FFMpegArguments
            .FromUrlInput(new Uri(url))
            .OutputToFile(fileName)
            .ProcessAsynchronously();
    }

    public void ConfigureFfmpeg()
    {
        GlobalFFOptions.Configure(new FFOptions
        {
            BinaryFolder = _nitefoxConfig.FfmpegLocation,
            TemporaryFilesFolder = _nitefoxConfig.TempFilesLocation
        });
    }
    
    public async Task<bool> DownloadFfmpeg()
    {
        if (new DirectoryInfo(_nitefoxConfig.FfmpegLocation).EnumerateFiles()
                .Count() >= 3)
        {
            return true;
        }
        
        var release = await ReleaseManager.Instance.GetLatestAsync("Tyrrrz","FFmpegBin");
        if (release is null || !release.Assets.Any()) return false;

        var releaseAsset = new ReleaseAsset();

        if (OperatingSystem.IsWindows())
        {
            releaseAsset = release.Assets.First(asset => asset.Name.EndsWith("windows-x64.zip"));
        }

        if (OperatingSystem.IsLinux())
        {
            releaseAsset = release.Assets.First(asset => asset.Name.EndsWith("linux-x64.zip"));
        }

        if (OperatingSystem.IsMacOS())
        {
            releaseAsset = release.Assets.First(asset => asset.Name.EndsWith("osx-x64.zip"));
        }
        
        if (OperatingSystem.IsMacCatalyst())
        {
            releaseAsset = release.Assets.First(asset => asset.Name.EndsWith("osx-arm64.zip"));
        }
        
        var downloadInfo = await AssetDownloader.Instance.DownloadAssetAsync(releaseAsset, _nitefoxConfig.FfmpegLocation);

        await Task.Run(async () =>
        {
            await using var stream = File.OpenRead(downloadInfo.Path);
            using var reader = ReaderFactory.Open(stream);

            while (reader.MoveToNextEntry())
            {
                if (!reader.Entry.IsDirectory)
                {
                    reader.WriteEntryToDirectory(_nitefoxConfig.FfmpegLocation, new ExtractionOptions()
                    {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
                }
            }

        }).ContinueWith(_ =>
        {
            File.Delete(downloadInfo.Path);
        });

        return true;
    }
}