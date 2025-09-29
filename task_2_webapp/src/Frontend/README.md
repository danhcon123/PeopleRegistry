# Frontend Project

This project uses [LibMan](https://docs.microsoft.com/en-us/aspnet/core/client-side/libman/cli) to manage client-side libraries.

## Restoring Packages

The required libraries (like Bootstrap) are defined in `libman.json`. They are not checked into source control.

To restore the libraries into the `wwwroot/lib` folder, run the following command in this directory:

```sh
libman restore
```

If you do not have the `libman` CLI installed, you can install it globally via .NET with this command:

```sh
dotnet tool install -g Microsoft.Web.LibraryManager.Cli
```
