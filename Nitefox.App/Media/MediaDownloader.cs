// Copyright (C) 2023  John Russell C. Camo (@russkyc)
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY

using JsonFlatFileDataStore;
using Nitefox.App.Configuration;
using Nitefox.App.Ffmpeg;
using Nitefox.App.Media.Entities;
using Nitefox.App.Services.Interfaces;
using Nitefox.App.Utilities;

namespace Nitefox.App.Media;

public class MediaDownloader
{
    private readonly IFileService _fileService;
    private readonly DataStore _mediaDataStore;
    private readonly FfmpegService _ffmpegService;
    private readonly NitefoxConfig _nitefoxConfig;
    private readonly MetadataService _metadataService;

    public MediaDownloader(
        IFileService fileService,
        FfmpegService ffmpegService,
        NitefoxConfig nitefoxConfig,
        MetadataService metadataService)
    {
        _fileService = fileService;
        _ffmpegService = ffmpegService;
        _nitefoxConfig = nitefoxConfig;
        _metadataService = metadataService;
        _mediaDataStore = new DataStore($"{_nitefoxConfig.DownloadLocation}downloads.json",
            reloadBeforeGetCollection: true);
    }
    
    public async Task<TrackDownload> Download(string title, string url, string author, string? collection = null)
    {
        var trackDownload = new TrackDownload
        {
            Name = title,
            Author = author
        };
        
        var songSaveName = $"{title.ToPathSafeString()} - {author.ToPathSafeString()}.mp3";
        
        try
        {
            await _ffmpegService.DownloadFfmpeg();
            
            _ffmpegService.ConfigureFfmpeg();
            _fileService.SetupDirectories();
            
            var streamUrl = await _metadataService.GetYoutubeSongStream(url);
            
            if (string.IsNullOrWhiteSpace(streamUrl))
            {
                return trackDownload;
            }
            
            if (collection is not null)
            {
                var songAlbumPath = $"{_nitefoxConfig.DownloadLocation}{collection}\\";

                trackDownload.Collection = collection;
                trackDownload.SavePath = $"{songAlbumPath}{songSaveName}";
                trackDownload.IsDownloaded =
                    await _ffmpegService.StreamConvert(streamUrl, $"{songAlbumPath}{songSaveName}");
                
                if (trackDownload.IsDownloaded)
                {
                    await _mediaDataStore.GetCollection<TrackDownload>()
                        .InsertOneAsync(trackDownload);
                }
                
                return trackDownload;
            }

            trackDownload.SavePath = $"{_nitefoxConfig.DownloadLocation}{songSaveName}";
            trackDownload.IsDownloaded =
                await _ffmpegService.StreamConvert(streamUrl, $"{_nitefoxConfig.DownloadLocation}{songSaveName}");
            
            if (trackDownload.IsDownloaded)
            {
                await _mediaDataStore.GetCollection<TrackDownload>()
                    .InsertOneAsync(trackDownload);
            }
            
            return trackDownload;
        }
        catch (Exception)
        {
            File.Delete(trackDownload.SavePath);
            return trackDownload;
        }
    }
    
    public async IAsyncEnumerable<TrackDownload> DownloadAlbum(string title, string id)
    {
        _fileService.CreateMediaDirectory(_nitefoxConfig.DownloadLocation, title.ToPathSafeString());

        foreach (var track in await _metadataService.GetAlbumTracksMetadata(id, title))
        {
            var authors = track.Artists.Select(artist => artist.Name).Take(3).ToDelimitedString();
            yield return await Download(track.Title, track.Url, authors, title.ToPathSafeString());
        }
    }
    
    public async IAsyncEnumerable<TrackDownload> DownloadPlaylist(string title, string id)
    {
        _fileService.CreateMediaDirectory(_nitefoxConfig.DownloadLocation, title.ToPathSafeString());

        foreach (var track in await _metadataService.GetPlaylistTracksMetadata(id, title))
        {
            var authors = track.Artists.Select(artist => artist.Name).Take(3).ToDelimitedString();
            yield return await Download(track.Title, track.Url, authors, title.ToPathSafeString());
        }
    }

    public bool GetIfDownloadExists(string title, string author)
    {
        return _mediaDataStore.GetCollection<TrackDownload>()
            .AsQueryable()
            .Any(track => track.Name.Equals(title) && track.Author.Equals(author));
    }
    
    public bool GetIfDownloadCollectionExists(string collection, int trackCount)
    {
        return _mediaDataStore.GetCollection<TrackDownload>()
            .AsQueryable()
            .Any(track => track.Collection == collection);
    }
    
}