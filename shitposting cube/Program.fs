open System
open System.Threading
open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.Utils.Collections

module Option =

    let ofChoice = function
        | Choice1Of2 x -> Some x
        | _ -> None

let cubeShitpost (s:string) =

    let str = s.ToUpper()

    let len = str.Length

    let space n = [for _ in 1..n do yield " "]

    let explode s = [for c in s -> c.ToString()]

    let rev (s:string) = new string(s.ToCharArray() |> Array.rev)

    sprintf "%s" <| "    " + (String.concat "\n    " ([String.concat " " (explode str)] @
                                [for i in 1..len-2 do yield String.concat " " <| [str.[i].ToString()] @ space(len-2) @ [(rev str).[i].ToString()] @ space(i-1) @ [(rev str).[i].ToString()]] @
                                [String.concat " " <| (str |> rev |> explode) @ space(len-2) @ [str.[0].ToString()]] @
                                [for i in 1..len-2 do yield String.concat " " <| space(i) @ [(rev str).[i].ToString()] @ space(len-2) @ [str.[i].ToString()] @ space(len-2-i) @ [str.[i].ToString()]] @
                                [String.concat " " <| space(len-1) @ explode str]))

[<EntryPoint>]
let main _ =
    let cts = new CancellationTokenSource()
    let conf = { defaultConfig with cancellationToken = cts.Token }

    let shitpost q =
        defaultArg (Option.ofChoice (q ^^ "cube")) "LOL NO CUBE PARAM" |> cubeShitpost

    let sample : WebPart =
        choose
            [path "/cubepost" >=> choose [
                GET  >=> request (fun r -> Writers.setMimeType "text/plain" >=> OK (shitpost r.query))
                request (fun _ -> Writers.setMimeType "text/plain" >=> RequestErrors.NOT_FOUND (cubeShitpost "LOL NO GET")) ]
             request (fun _ -> Writers.setMimeType "text/plain" >=> RequestErrors.NOT_FOUND (cubeShitpost "LOL NO CUBEPOST PATH"))]

    let _, server = startWebServerAsync conf sample

    Async.Start(server, cts.Token)
    printfn "Make requests now"
    Console.ReadKey true |> ignore

    cts.Cancel()

    0