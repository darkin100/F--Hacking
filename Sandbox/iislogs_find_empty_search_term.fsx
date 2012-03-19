
open System 
open System.IO 

//fethc the contents of the file
let FileEnumerator filename = seq { 
        use sr = System.IO.File.OpenText(filename)
        while not sr.EndOfStream do 
            let line = sr.ReadLine()
            let words = line.Split [|' '|]
            yield words 
}


//let test = FileEnumerator(@"D:\Projects\HttpScripts\test.log") 
let test = FileEnumerator(@"D:\HTTPLogs\TES10WEB22_W3SVC2_u_ex120313\u_ex120313.log") 

let sw = System.Diagnostics.Stopwatch()
sw.Start()

test |> Seq.filter (fun line -> line.Length > 4)
|> Seq.filter (fun line -> (System.String.Equals(line.[4], @"/Store/w/search.tla", System.StringComparison.CurrentCultureIgnoreCase)))
|> Seq.filter (fun line -> not (line.[5].Contains(@"keyword")))
|> Seq.map(fun line -> (line.[4],line.[5]))
|> Seq.iter(fun (x,y)-> printfn "%s:%s" x y)
//|> Seq.groupBy(fun (x,y) -> y)
//|> Seq.map(fun (x,y)-> (x, Seq.length y))
//|> Seq.sortBy(fun (x,y)-> y)
//|> Seq.iter(fun (x,y)-> printfn "%d:%s" y x)

sw.Stop() 
printfn "Time elapsed: %dms" sw.ElapsedMilliseconds 