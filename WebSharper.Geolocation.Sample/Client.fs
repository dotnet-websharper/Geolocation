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

    let geolocation = As<Navigator>(JS.Window.Navigator).Geolocation

    let getLocation() = 
        try
            status := "Getting location..."

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

        IndexTemplate.Main()
            .location(location.View)
            .status(status.View)
            .getLocation(fun _ -> getLocation())
            .Doc()
        |> Doc.RunById "main"
