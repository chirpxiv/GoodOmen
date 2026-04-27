# GoodOmen

Accessibility plugin for FFXIV that allows users to recolor AoE indicators.<br/>
It allows you to apply a tint to ground AoE markers, enabling better visibility for colorblind players.

This is an early implementation that will be refined in later versions.

## Bug Reports

Bugs should be reported as [GitHub issues](https://github.com/chirpxiv/GoodOmen/issues), which helps me to easily keep track of them. I am unlikely to see direct messages.

## Installation

This is a plugin for Dalamud, which is available using [XIVLauncher](https://github.com/goatcorp/FFXIVQuickLauncher).

Add the following repo URL under **Custom Plugin Repositories** in your Dalamud settings:

`https://raw.githubusercontent.com/chirpxiv/GoodOmen/refs/heads/main/repo.json`

After installing, you can type the `/goodomen` command to access the plugin's settings.

## Development 

### Maintaining Releases For Dummies

1. Update DalamudPackager to latest version via nuget
2. Update GLib if major version bump
3. Update any actual code that needs to change
4. Update API level in .csproj (and repo.json if major version bump)
5. Update version number in .csproj
6. Build + Test (in game)
7. Push + PR
8. Merge to `main`
9. Release