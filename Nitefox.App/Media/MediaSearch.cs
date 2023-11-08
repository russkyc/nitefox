// Copyright (C) 2023  John Russell C. Camo (@russkyc)
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY

using Nitefox.App.Utilities;
using SpotifyExplode;
using SpotifyExplode.Search;

namespace Nitefox.App.Media;

public class MediaSearch
{
    private readonly SpotifyClient _spotifyClient;

    public MediaSearch(SpotifyClient spotifyClient)
    {
        _spotifyClient = spotifyClient;
    }

    public async Task<IEnumerable<ISearchResult>> SearchAsync(string query, int limit = Int32.MaxValue, int skip = 0)
    {
        var cleanQuery = query.ToCleanQueryString();
        
        if (query.Contains("album", StringComparison.InvariantCultureIgnoreCase))
        {
            return await _spotifyClient.Search.GetAlbumsAsync(cleanQuery, skip, limit);
        }
        
        if (query.Contains("playlist", StringComparison.InvariantCultureIgnoreCase))
        {
            return await  _spotifyClient.Search.GetPlaylistsAsync(cleanQuery, skip, limit);
        }

        return await _spotifyClient.Search.GetTracksAsync(cleanQuery, skip, limit);
    }

}