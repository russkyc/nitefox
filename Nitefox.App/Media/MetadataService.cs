// Copyright (C) 2023  John Russell C. Camo (@russkyc)
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY

using SpotifyExplode;
using SpotifyExplode.Tracks;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace Nitefox.App.Media;

public class MetadataService
{
    private readonly SpotifyClient _spotifyClient;
    private readonly YoutubeClient _youtubeClient;

    public MetadataService(SpotifyClient spotifyClient, YoutubeClient youtubeClient)
    {
        _spotifyClient = spotifyClient;
        _youtubeClient = youtubeClient;
    }

    public async Task<int> GetAlbumTrackCount(string id)
    {
        var album = await _spotifyClient.Albums.GetAsync(id);
        return album.Tracks.Count;
    }
    
    public async Task<int> GetPlaylistTrackCount(string id)
    {
        var playlists = await _spotifyClient.Playlists.GetAsync(id);
        return playlists.Tracks.Count;
    }

    public async Task<string> GetAlbumImageUrl(string id)
    {
        try
        {
            var album = await _spotifyClient.Albums.GetAsync(id);
            var image = album.Images.FirstOrDefault(image => !string.IsNullOrWhiteSpace(image.Url));
            if (image is null) return string.Empty;
            return image.Url;
        }
        catch (NullReferenceException)
        {
            return string.Empty;
        }
        catch (InvalidOperationException)
        {
            return string.Empty;
        }
    }
    
    public async Task<string> GetPlaylistImageUrl(string id)
    {
        try
        {
            var tracks = await _spotifyClient.Playlists.GetTracksAsync(id, limit: 5);
            var playlistImages = tracks.SelectMany(track => track.Album.Images)
                .Where(image => !string.IsNullOrWhiteSpace(image.Url));
            var image = playlistImages.FirstOrDefault(image => !string.IsNullOrWhiteSpace(image.Url));
            if (image is null) return string.Empty;
            return image.Url;
        }
        catch (NullReferenceException)
        {
            return string.Empty;
        }
        catch (InvalidOperationException)
        {
            return string.Empty;
        }
    }

    public async Task<string?> GetYoutubeSongStream(string url)
    {
        var stream = await _spotifyClient.Tracks.GetDownloadUrlAsync(url);
        return stream;
    }
    
    public async Task<string> GetPreviewStream(string url)
    {
        var youtubeId = await _spotifyClient.Tracks.GetYoutubeIdAsync(url);
        var streamManifest = await _youtubeClient.Videos.Streams.GetManifestAsync($"https://youtube.com/watch?v={youtubeId}");
        return streamManifest.GetAudioOnlyStreams()
            .OrderByDescending(stream => stream.Size)
            .First().Url;
    }

    public async Task<IEnumerable<Track>> GetAlbumTracksMetadata(string id, string title)
    {
        var tracks = await _spotifyClient.Albums.GetAllTracksAsync(id);
        return tracks.AsEnumerable();
    }
    
    
    public async Task<IEnumerable<Track>> GetPlaylistTracksMetadata(string id, string title)
    {
        var tracks = await _spotifyClient.Playlists.GetAllTracksAsync(id);
        return tracks.AsEnumerable();
    }
    
}