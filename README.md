# HostBuilderUtils

[![Solution](https://github.com/mbochmann/HostBuilderUtils/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/mbochmann/HostBuilderUtils/actions/workflows/dotnet.yml)
[![codecov](https://codecov.io/gh/mbochmann/HostBuilderUtils/branch/main/graph/badge.svg?token=F7YOC3NUQL)](https://codecov.io/gh/mbochmann/HostBuilderUtils)


Contains the sourcecode for different tools when working with [Microsoft.Extensions.Hosting.IHostBuilder](https://docs.microsoft.com/de-de/dotnet/api/microsoft.extensions.hosting.ihostbuilder?view=dotnet-plat-ext-6.0)

## Table of contents
For now, just one extension is fully ready to use:


| Name                                                                              | Status | Description                                      | Build                                                                                                                                                                                                                                                             |
| --------------------------------------------------------------------------------- | ------ | ------------------------------------------------ | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| [PreBuildServiceProvider](src/HostBuilderUtils.PreBuildServiceProvider/README.md) | Done   | Usage of `IServiceProvider` before host is built | [![.NET PreBuildServiceProvider](https://github.com/mbochmann/HostBuilderUtils/actions/workflows/dotnet-prebuildserviceprovider.yml/badge.svg?branch=main)](https://github.com/mbochmann/HostBuilderUtils/actions/workflows/dotnet-prebuildserviceprovider.yml) |
