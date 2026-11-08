namespace WebSharper.Geolocation.Sample

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Notation
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper.Geolocation

[<JavaScript>]
module Client =
    // The template is loaded from the DOM, so editing index.html and refreshing
    // the browser is enough; a rebuild is only needed when holes change.
    type IndexTemplate = Template<"wwwroot/index.html", ClientLoad.FromDocument>

    let status = Var.Create "Ready to request your location."
    let latitude = Var.Create "—"
    let longitude = Var.Create "—"
    let accuracy = Var.Create "—"
    let altitude = Var.Create "—"
    let timestamp = Var.Create "—"

    let geolocation = As<Navigator>(JS.Window.Navigator).Geolocation

    let format (value: float) =
        if value <> value then "—" else value.ToString("0.00000")

    let getLocation () =
        try
            status := "Requesting location…"
            geolocation.GetCurrentPosition(
                (fun position ->
                    let coords = position.Coords
                    latitude := format coords.Latitude
                    longitude := format coords.Longitude
                    accuracy := format coords.Accuracy
                    altitude := if coords.Altitude <> coords.Altitude then "—" else format coords.Altitude
                    timestamp := Date(position.Timestamp).ToISOString()
                    status := "Location found."
                ),
                (fun error ->
                    status := $"Error: {error.Message}"
                )
            )
        with _ ->
            status := "Geolocation is not supported in this browser."

    [<SPAEntryPoint>]
    let Main () =
        IndexTemplate()
            .getLocation(fun _ -> getLocation ())
            .status(status.V)
            .latitude(latitude.V)
            .longitude(longitude.V)
            .accuracy(accuracy.V)
            .altitude(altitude.V)
            .timestamp(timestamp.V)
            .Bind()
