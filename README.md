# DevCacheCleaner

DevCacheCleaner is a simple tool to clean the cache of your .net development environment.

## Installation

You can install the package via nuget:

```bash
dotnet tool install -g DevCacheCleaner
```

## Usage

```bash
devcachecleaner -d [threshold days to delete nuget caches]
```

### Options

- `-d` or `--days` (optional): Threshold days for last accessed to nuget cached folders. Nuget cache folders which have accessed older than this threshold will be deleted. Default is 30 days.
