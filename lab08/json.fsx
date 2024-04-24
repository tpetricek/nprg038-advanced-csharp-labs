#r "nuget: FSharp.Data"
open FSharp.Data

type Post = JsonProvider<"https://b2c.cpost.cz/services/PostCode/getDataAsJson?cityOrPart=Praha&nameStreet=Malostranske namesti">

let res = Post.GetSamples()
for no in res do
  printfn "%s" no.Number.String.Value




