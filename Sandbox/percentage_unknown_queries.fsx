#light

let FileEnumerator filename = seq { 
        use sr = System.IO.File.OpenText(filename)
        while not sr.EndOfStream do 
            let line = sr.ReadLine()
            let words = line.Split [|' '|]
            yield words 
}

//let test = FileEnumerator(@"D:\Projects\DgraphScripts\logs\test_1.reqlog")  
let test = FileEnumerator(@"D:\Projects\DgraphScripts\logs\2012_03_13.reqlog")  

let sw = System.Diagnostics.Stopwatch()
sw.Start()

let k = 
    test 
        |> Seq.filter (fun line -> line.Length > 5)
        |> Seq.filter (fun line -> (line.[11].Contains(@"/graph?")))
        |> Seq.filter (fun line -> (line.[11].Contains(@"&attrs=")))
        //|> Seq.map(fun line -> System.Decimal.Parse line.[5])
        //|> Seq.average
        |> Seq.length

printfn "k : %d" k

let u =
    test |> Seq.filter (fun line -> line.Length > 5)
        |> Seq.filter (fun line -> (line.[11].Contains(@"/graph?")))
        |> Seq.filter (fun line -> not (line.[11].Contains(@"&attrs=")))
        //|> Seq.map(fun line -> System.Decimal.Parse line.[5])
        //|> Seq.average
        |> Seq.length

printfn "u : %d" u

let known = decimal k
let unknown = decimal u

let percentage = (unknown/(unknown + known))*100M

printfn "%g" percentage

sw.Stop() 
printfn "Time elapsed: %dms" sw.ElapsedMilliseconds 