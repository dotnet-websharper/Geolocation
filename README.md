# WebSharper Geolocation API Binding

This repository provides an F# [WebSharper](https://websharper.com/) binding for the [Geolocation API](https://developer.mozilla.org/en-US/docs/Web/API/Geolocation_API), enabling seamless access to location-based services in WebSharper applications.

## Repository Structure

The repository consists of two main projects:

1. **Binding Project**:

   - Contains the F# WebSharper binding for the Geolocation API.

2. **Sample Project**:
   - Demonstrates how to use the Geolocation API with WebSharper syntax.
   - Includes a GitHub Pages demo: [View Demo](https://dotnet-websharper.github.io/Geolocation/).

## Features

- WebSharper bindings for the Geolocation API.
- Easy access to real-time location data.
- Example usage for building location-aware applications.
- Hosted demo to explore API functionality.

## Installation and Building

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) installed on your machine.
- Node.js and npm (for building web assets).
- WebSharper tools.

### Steps

1. Clone the repository:

   ```bash
   git clone https://github.com/dotnet-websharper/Geolocation.git
   cd Geolocation
   ```

2. Build the Binding Project:

   ```bash
   dotnet build WebSharper.Geolocation/WebSharper.Geolocation.fsproj
   ```

3. Build and Run the Sample Project:

   ```bash
   cd WebSharper.Geolocation.Sample
   dotnet build
   dotnet run
   ```

4. Open the hosted demo to see the Sample project in action:
   [https://dotnet-websharper.github.io/Geolocation/](https://dotnet-websharper.github.io/Geolocation/)

## Why Use the Geolocation API

The Geolocation API provides a simple way to retrieve the user's geographic location. Key benefits include:

1. **Real-Time Location Data**: Retrieve latitude, longitude, altitude, and speed.
2. **Enhanced User Experience**: Enable location-based features such as maps and directions.
3. **Support for GPS and Network Positioning**: Works across multiple devices and connection types.
4. **Standardized API**: A browser-native way to access location data.

Integrating the Geolocation API with WebSharper allows developers to build powerful location-aware web applications in F#.

## How to Use the Geolocation API

### Example Usage

Below is an example of how to use the Geolocation API in a WebSharper project:

```fsharp
namespace WebSharper.Geolocation.Sample

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper.UI.Notation
open WebSharper.Geolocation

[<JavaScript>]
module Client =
    // The templates are loaded from the DOM, so you just can edit index.html
    // and refresh your browser, no need to recompile unless you add or remove holes.
    type IndexTemplate = Template<"wwwroot/index.html", ClientLoad.FromDocument>

    let location = Var.Create ""
    let status = Var.Create ""

    // Access the browser's Geolocation API
    let geolocation = As<Navigator>(JS.Window.Navigator).Geolocation

    // Function to retrieve the user's current location
    let getLocation() =
        try
            status := "Getting location..."

            // Request the current position from the Geolocation API
            geolocation.GetCurrentPosition((fun position ->
                let latitude = position.Coords.Latitude
                let longitude = position.Coords.Longitude

                location := $"Latitude: {latitude}, Longitude: {longitude}"
                status := "Location found!"
            ), (fun error ->
                status := $"Error: {error.Message}"
            ))

        with _ ->
            status := "Geolocation is not supported in this browser."

    [<SPAEntryPoint>]
    let Main () =

        // Initialize the UI template and bind variables to UI elements
        IndexTemplate.Main()
            .location(location.View)
            .status(status.View)
            .getLocation(fun _ -> getLocation())
            .Doc()
        |> Doc.RunById "main"

```

This example demonstrates how to retrieve the user's location and display it dynamically in the UI.

For a complete implementation, refer to the [Sample Project](https://dotnet-websharper.github.io/Geolocation/).
