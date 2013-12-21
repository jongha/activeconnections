# Active Connections
<!---
[![Build Status](https://travis-ci.org/jongha/dotnet-activeconnections.png?branch=master)](https://travis-ci.org/jongha/dotnet-activeconnections)
-->
Print active/established network connections. It's console application using .net framework on Windows. It is useful to check bottlenecks or suffering of a specific IP or the status of the DDoS attack.

## Usage

    Usage: ActiveConnections.exe [-n|numeric] [-h|help]
    Print active/established network connections.

    Options:
      -n, --numeric=VALUE        This option sets the minimum count of active
                                   network connections.
      -h, --help                 Show help.

### Output

    Total Connection Count: 121

    Address Count
    0.0.0.0 18
    127.0.0.1       9
    192.168.1.2     65
    [::]    14
    [fe80::c00:b282:1308:c851%10]   7

    Port    Count
    1900    6

## License

dotnet-activeconnections is available under the terms of the MIT License.
