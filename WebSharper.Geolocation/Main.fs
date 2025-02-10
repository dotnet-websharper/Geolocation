namespace WebSharper.Geolocation

open WebSharper
open WebSharper.JavaScript
open WebSharper.InterfaceGenerator

module Definition =

    let PositionOptions =
        Pattern.Config "PositionOptions" {
            Required = []
            Optional = [
                "enableHighAccuracy", T<bool>
                "timeout", T<int>
                "maximumAge", T<int>
            ]
        }

    let GeolocationCoordinates = 
        Class "GeolocationCoordinates"
        |+> Instance [
            "accuracy" =? T<float>
            "altitude" =? T<float>
            "altitudeAccuracy" =? T<float>
            "heading" =? T<float>
            "latitude" =? T<float>
            "longitude" =? T<float>
            "speed" =? T<float>

            "toJSON" => T<unit> ^-> T<obj>
        ]

    let GeolocationPosition = 
        Class "GeolocationPosition"
        |+> Instance [
            "coords" =? GeolocationCoordinates
            "timestamp" =? T<int>

            "toJSON" => T<unit> ^-> T<obj>
        ]

    let GeolocationPositionError = 
        Class "GeolocationPositionError"
        |+> Instance [
            "code" =? T<int>
            "message" =? T<string>
        ]

    let Geolocation =
        Class "Geolocation"
        |+> Instance [
            "getCurrentPosition" =>
                (GeolocationPosition ^-> T<unit>)?success * !?(GeolocationPositionError ^-> T<unit>)?error * !?PositionOptions?options
                ^-> T<unit>

            "watchPosition" =>
                (GeolocationPosition ^-> T<unit>)?success * !?(GeolocationPositionError ^-> T<unit>)?error * !?PositionOptions?options
                ^-> T<int>

            "clearWatch" => T<int>?watchId ^-> T<unit>
        ]

    let Navigator =
        Class "Navigator"
        |+> Instance [
            "geolocation" =? Geolocation.Type
        ]

    let Assembly =
        Assembly [
            Namespace "WebSharper.Geolocation" [
                GeolocationCoordinates
                GeolocationPosition
                GeolocationPositionError
                PositionOptions
                Geolocation
                Navigator
            ]
        ]

[<Sealed>]
type Extension() =
    interface IExtension with
        member ext.Assembly =
            Definition.Assembly

[<assembly: Extension(typeof<Extension>)>]
do ()
