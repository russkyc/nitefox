using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using MudExtensions.Services;
using Nitefox.App.Configuration;
using Nitefox.App.Ffmpeg;
using Nitefox.App.Media;
using Nitefox.App.States;
using SpotifyExplode;

namespace Nitefox.Client.Shared
{
    public static class NitefoxShared
    {
        public static IServiceCollection AddNitefoxServices(this IServiceCollection collection)
        {
            return collection.AddSingleton(_ => new SpotifyClient())
                .AddMudServices()
                .AddMudExtensions()
                .AddSingleton<SearchState>()
                .AddSingleton<NitefoxConfig>()
                .AddSingleton<FfmpegService>()
                .AddSingleton<MediaSearch>()
                .AddSingleton<MediaPlayerState>()
                .AddSingleton<MetadataService>()
                .AddSingleton<MediaDownloader>();
        }
    }
}