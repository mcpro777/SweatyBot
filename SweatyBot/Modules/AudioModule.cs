using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using SweatyBot.Services;

namespace SweatyBot.Modules
{
    [Name("Audio")]
    [Summary("Audio module to interact with voice chat. Currently, used to playback audio in a stream.")]
    public class AudioModule : CustomModule
    {
        private readonly AudioService _service;
        private const String downloadPath = @"E:\temp\SweatyBot\temp";

        public AudioModule(AudioService service)
        {
            _service = service;
            _service.SetParentModule(this);
            _service.SetDownloadPath(downloadPath);
        }

        [Command("join", RunMode = RunMode.Async)]
        [Remarks("join")]
        [Summary("Joins the user's voice channel.")]
        public async Task JoinVoiceChannel()
        {
            if (_service.GetDelayAction()) return; // Stop multiple attempts to join too quickly.
            await _service.JoinAudioAsync(Context.Guild, (Context.User as IVoiceState).VoiceChannel);

            // Start the autoplay service if enabled, but not yet started.
            await _service.CheckAutoPlayAsync(Context.Guild, Context.Channel);
        }

        [Command("leave", RunMode = RunMode.Async)]
        [Remarks("leave")]
        [Summary("Leaves the current voice channel.")]
        public async Task LeaveVoiceChannel()
        {
            await _service.LeaveAudioAsync(Context.Guild);
        }

        [Command("play", RunMode = RunMode.Async)]
        [Remarks("play [url/index]")]
        [Summary("Plays a song by url or local path.")]
        public async Task PlayVoiceChannel([Remainder] string song)
        {
            if (_service.GetDelayAction()) return; // Stop multiple attempts to join too quickly.
            await _service.JoinAudioAsync(Context.Guild, (Context.User as IVoiceState).VoiceChannel);

            // Play the audio. We check if audio is null when we attempt to play. This function is BLOCKING.
            await _service.ForcePlayAudioAsync(Context.Guild, Context.Channel, song);

            // Start the autoplay service if enabled, but not yet started.
            // Once force play is done, if auto play is enabled, we can resume the autoplay here.
            // We also write a counter to make sure this is the last play called, to avoid cascading auto plays.
            if (_service.GetNumPlaysCalled() == 0) await _service.CheckAutoPlayAsync(Context.Guild, Context.Channel);
        }

        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayVoiceChannelByIndex(int index)
        {
            // Play a song by it's local index in the download folder.
            await PlayVoiceChannel(_service.GetLocalSong(index));
        }

        [Command("pause", RunMode = RunMode.Async)]
        [Remarks("pause")]
        [Summary("Pauses the current song, if playing.")]
        public async Task PauseVoiceChannel()
        {
            _service.PauseAudio();
            await Task.Delay(0); // Suppress async warrnings.
        }

        [Command("resume", RunMode = RunMode.Async)]
        [Remarks("resume")]
        [Summary("Pauses the current song, if paused.")]
        public async Task ResumeVoiceChannel()
        {
            _service.ResumeAudio();
            await Task.Delay(0); // Suppress async warrnings.
        }

        [Command("stop", RunMode = RunMode.Async)]
        [Remarks("stop")]
        [Summary("Stops the current song, if playing or paused.")]
        public async Task StopVoiceChannel()
        {
            _service.StopAudio();
            await Task.Delay(0); // Suppress async warrnings.
        }

        [Command("fuckoff", RunMode = RunMode.Async)]
        [Remarks("fuckoff")]
        [Summary("Stops the current song, and forces the bot to leave.")]
        public async Task FuckOff()
        {
            await _service.FuckOffAsync(Context.Guild);
        }

        [Command("volume")]
        [Remarks("volume [num]")]
        [Summary("Changes the volume to [0 - 100].")]
        public async Task VolumeVoiceChannel(int volume)
        {
            _service.AdjustVolume((float)volume / 100.0f);
            await Task.Delay(0); // Suppress async warrnings.
        }

        [Command("add", RunMode = RunMode.Async)]
        [Remarks("add [url/index]")]
        [Summary("Adds a song by url or local path to the playlist.")]
        public async Task AddVoiceChannel([Remainder] string song)
        {
            // Add it to the playlist.
            await _service.PlaylistAddAsync(song);

            // Start the autoplay service if enabled, but not yet started.
            await _service.CheckAutoPlayAsync(Context.Guild, Context.Channel);
        }

        [Command("add", RunMode = RunMode.Async)]
        public async Task AddVoiceChannelByIndex(int index)
        {
            // Add a song by it's local index in the download folder.
            await AddVoiceChannel(_service.GetLocalSong(index));
        }

        [Command("skip", RunMode = RunMode.Async)]
        [Alias("skip", "next")]
        [Remarks("skip")]
        [Summary("Skips the current song, if playing from the playlist.")]
        public async Task SkipVoiceChannel()
        {
            _service.PlaylistSkip();
            await Task.Delay(0);
        }

        [Command("playlist", RunMode = RunMode.Async)]
        [Remarks("playlist")]
        [Summary("Shows what's currently in the playlist.")]
        public async Task PrintPlaylistVoiceChannel()
        {
            _service.PrintPlaylist();
            await Task.Delay(0);
        }

        [Command("autoplay", RunMode = RunMode.Async)]
        [Remarks("autoplay [enable]")]
        [Summary("Starts the autoplay service on the current playlist.")]
        public async Task AutoPlayVoiceChannel(bool enable)
        {
            _service.SetAutoPlay(enable);

            // Start the autoplay service if already on, but not started.
            await _service.CheckAutoPlayAsync(Context.Guild, Context.Channel);
        }

        //[Command("download", RunMode = RunMode.Async)]
        //[Remarks("download [http]")]
        //[Summary("Download songs into our local folder.")]
        //public async Task DownloadSong([Remainder] string path)
        //{
        //    await _service.DownloadSongAsync(path);
        //}

        [Command("songs", RunMode = RunMode.Async)]
        [Remarks("songs [page]")]
        [Summary("Shows songs in our local folder in pages.")]
        public async Task PrintSongDirectory(int page = 0)
        {
            _service.PrintLocalSongs(page);
            await Task.Delay(0);
        }

        [Command("cleanupsongs", RunMode = RunMode.Async)]
        [Remarks("cleanupsongs")]
        [Summary("Cleans the local folder of duplicate files created by our downloader.")]
        public async Task CleanSongDirectory()
        {
            await _service.RemoveDuplicateSongsAsync();
        }
    }
}
