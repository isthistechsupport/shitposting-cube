let str = "LOL NOT CUBE SHITPOSTING"

let len = str.Length

let space n = [for _ in 1..n do yield " "]

let explode s = [for c in s -> c.ToString()]

let rev (s:string) = new string(s.ToCharArray() |> Array.rev)

[<EntryPoint>]
let main _ =
    printfn "%s" <| "    " + (String.concat "\n    " ([String.concat " " (explode str)] @
                                          [for i in 1..len-2 do yield String.concat " " <| [str.[i].ToString()] @ space(len-2) @ [(rev str).[i].ToString()] @ space(i-1) @ [(rev str).[i].ToString()]] @
                                          [String.concat " " <| (str |> rev |> explode) @ space(len-2) @ [str.[0].ToString()]] @
                                          [for i in 1..len-2 do yield String.concat " " <| space(i) @ [(rev str).[i].ToString()] @ space(len-2) @ [str.[i].ToString()] @ space(len-2-i) @ [str.[i].ToString()]] @
                                          [String.concat " " <| space(len-1) @ explode str]))
    System.Console.ReadKey true |> ignore
    0
