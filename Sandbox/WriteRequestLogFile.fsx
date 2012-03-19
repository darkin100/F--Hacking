open System.Net 
open System 
open System.IO 



let FileEnumerator filename = seq { 
        use sr = System.IO.File.OpenText(filename)
        while not sr.EndOfStream do 
            let line = sr.ReadLine()
            let words = line.Split [|' '|]
            yield words 
}

let test = FileEnumerator(@"D:\Projects\DgraphScripts\logs\2012_03_11.reqlog")  

let sw = System.Diagnostics.Stopwatch()
sw.Start()

let lines = 
    test 
    |> Seq.filter (fun line -> line.Length > 5)
    |> Seq.map (fun line -> line.[11]) 
    |> Seq.filter (fun line -> (line.Contains(@"/graph?")))
    |> Seq.map (fun line -> line.Replace("&filter=AND(Type%3aProduct%2cIsPublished%3a1)","&filter=IsPublished%3a1"))

File.WriteAllLines(@"D:\Projects\DgraphScripts\logs\clean2.reqlog", lines) 

sw.Stop() 
printfn "Time elapsed: %dms" sw.ElapsedMilliseconds 



  