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

public class MediaSearch(SpotifyClient spotifyClient)
{
    public async Task<IEnumerable<ISearchResult>> SearchAsync(string query, int limit = Int32.MaxValue, int skip = 0)
    {
        try
        {
            var cleanQuery = query.ToCleanQueryString();

            if (query.Contains("album", StringComparison.InvariantCultureIgnoreCase))
            {
                return await spotifyClient.Search.GetAlbumsAsync(cleanQuery, skip, limit);
            }

            if (query.Contains("playlist", StringComparison.InvariantCultureIgnoreCase))
            {
                return await spotifyClient.Search.GetPlaylistsAsync(cleanQuery, skip, limit);
            }

            return await spotifyClient.Search.GetTracksAsync(cleanQuery, skip, limit);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Enumerable.Empty<ISearchResult>();
        }
    }

}