# Kattis Search
Easily find problems on Kattis with a search query. Can't find that Kattis problem you've been looking for? Well as long as you recall some of its problem statement, you can search for the problem with this tool.

Download it [here](https://github.com/minidomo/KattisSearch/releases/tag/v0.1.0).

## Prequisites
- [.NET Core 2.2](https://dotnet.microsoft.com/download) (or compatible) installed

## How to Use
KattisSearch must be used with flags.
The available flags can be seen in the following table.

| Flag | Description |
| - | - |
| `-s` or `--search` | Finds matching Kattis problems given a search query. Type your search query in a file called `search.txt` before using this flag. |
| `-g` or `--generate` | Generates `data.json` containing, the problem id, name, difficulty and problem body (problem statement, input, and output). This may take a minutes as it is reliant on internet and the number of problems on Kattis. |

Run the program by typing the following in command prompt:
```shell
dotnet KattisSearch <flag>
```

## Building
To build the code, type the following in command prompt:
```shell
git clone https://github.com/minidomo/KattisSearch.git
cd KattisSearch
dotnet restore
```
