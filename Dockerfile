FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY /src .
RUN dotnet restore "TicTacToe.WebAPI/TicTacToe.WebAPI.csproj"
RUN dotnet publish "TicTacToe.WebAPI/TicTacToe.WebAPI.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["sh", "-c", "dotnet TicTacToe.WebAPI.dll"]