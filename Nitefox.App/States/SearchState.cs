// Copyright (C) 2023  John Russell C. Camo (@russkyc)
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY

using System.Collections.ObjectModel;
using System.ComponentModel;
using PropertyChanged.SourceGenerator;
using SpotifyExplode.Search;

namespace Nitefox.App.States;

public partial class SearchState
{
    [Notify] private bool _isLoadingResults;
    [Notify] private string _searchQuery = string.Empty;
    [Notify] private ObservableCollection<ISearchResult> _searchResults = new();

    public void AddSearchResult(ISearchResult result)
    {
        SearchResults.Add(result);
        PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(SearchResults)));
    }

    public void ClearSearchResults()
    {
        SearchResults = new();
    }
}