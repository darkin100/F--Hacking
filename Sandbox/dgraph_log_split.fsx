#light

open System
open System.IO 



let procLine(line:string) = 
    
    if line.Contains("attrs=Search") then
        (line,"search")
    elif line.Contains("attrs=NamedListSearch") then
        (line, "namedlistsearch")
    elif line.Contains("attrs=ProductIDSearch") then
        (line, "productidsearch")
    elif line.Contains("attrs=TypeAhead") then
        (line, "typeahead")
    else
        (line, "other")


let writeout lines name =
    let res = 
        lines
            |> Seq.filter(fun (x,y) -> y = name)
            |> Seq.map(fun (x,y) -> x)

    File.WriteAllLines(@"D:\Projects\DgraphScripts\output\f\" + name + ".txt",res)




let files = 
    seq{
     for f in Directory.EnumerateFiles(@"D:\Projects\DgraphScripts\logs\","*.reqlog",SearchOption.AllDirectories) do
              yield! File.ReadLines f
    }



let sw = System.Diagnostics.Stopwatch()
sw.Start()

let clean =
    files |> Seq.filter (fun line -> (line.Contains(@"/graph?")))
    |> Seq.map(fun line ->  procLine(line))
    
writeout clean "other"

writeout clean "search"

writeout clean "namedlistsearch"

writeout clean "typeahead"

writeout clean "productidsearch"

sw.Stop() 
printfn "Time elapsed: %dms" sw.ElapsedMilliseconds 