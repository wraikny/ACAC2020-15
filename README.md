# ACAC2020-15

## Build

### Setup

```shell
git submodule update
```

[Altseed2 05f5a06467a944aef3307dbb2e7c05c754f718ab](https://github.com/altseed/Altseed2-csharp/runs/1502649908)

を `lib/Altseed2` の下に配置して、

```
fake build -t copylib
```

### Build

```shell
dotnet build
```

### Publish Server

```shell
fake build -t publishserver
```
