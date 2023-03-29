# Programming Assignment

This is a simple .NET project demonstrating an exchange matching engine.

## Prerequisites

- .NET SDK 7.0 or later

## Build and Test

To build and test the project, follow these steps:

1. Open a terminal and navigate to the project's root directory.

2. Run the following command to build the project:
   dotnet build Exchange
   dotnet build Exchange.Tests

3. Run the following command to test the project:
   dotnet test Exchange.Tests

4. If the build and tests are successful, you can run the project by executing the following steps:
   Go to Exchange folder

   create orders.txt file with input
   A:GBPUSD:100:1.66
   B:EURUSD:-100:1.11
   F:EURUSD:-50:1.1
   C:GBPUSD:-10:1.5
   C:GBPUSD:-20:1.6
   C:GBPUSD:-20:1.7
   D:EURUSD:100:1.11

   create trades.txt file for output

   run the below command
   dotnet run Exchange < orders.txt > trades.txt

   you can replace the order.txt and trades.txt with your desired file paths
