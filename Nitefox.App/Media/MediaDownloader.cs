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

public class MediaDownloader(
    IFileService fileService,
    FfmpegService ffmpegService,
    NitefoxConfig nitefoxConfig,
    MetadataService metadataService)
{
    
    private readonly DataStore _mediaDataStore = new ($"{nitefoxConfig.DownloadLocation}downloads.json",
        reloadBeforeGetCollection: true);

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
            await ffmpegService.DownloadFfmpeg();
            
            ffmpegService.ConfigureFfmpeg();
            fileService.SetupDirectories();
            
            var streamUrl = await metadataService.GetYoutubeSongStream(url);
            
            if (string.IsNullOrWhiteSpace(streamUrl))
            {
                return trackDownload;
            }
            
            if (collection is not null)
            {
                var songAlbumPath = $"{nitefoxConfig.DownloadLocation}{collection}\\";

                trackDownload.Collection = collection;
                trackDownload.SavePath = $"{songAlbumPath}{songSaveName}";
                trackDownload.IsDownloaded =
                    await ffmpegService.StreamConvert(streamUrl, $"{songAlbumPath}{songSaveName}");
                
                if (trackDownload.IsDownloaded)
                {
                    await _mediaDataStore.GetCollection<TrackDownload>()
                        .InsertOneAsync(trackDownload);
                }
                
                return trackDownload;
            }

            trackDownload.SavePath = $"{nitefoxConfig.DownloadLocation}{songSaveName}";
            trackDownload.IsDownloaded =
                await ffmpegService.StreamConvert(streamUrl, $"{nitefoxConfig.DownloadLocation}{songSaveName}");
            
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
        fileService.CreateMediaDirectory(nitefoxConfig.DownloadLocation, title.ToPathSafeString());

        foreach (var track in await metadataService.GetAlbumTracksMetadata(id, title))
        {
            var authors = track.Artists.Select(artist => artist.Name).Take(3).ToDelimitedString();
            yield return await Download(track.Title, track.Url, authors, title.ToPathSafeString());
        }
    }
    
    public async IAsyncEnumerable<TrackDownload> DownloadPlaylist(string title, string id)
    {
        fileService.CreateMediaDirectory(nitefoxConfig.DownloadLocation, title.ToPathSafeString());

        foreach (var track in await metadataService.GetPlaylistTracksMetadata(id, title))
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