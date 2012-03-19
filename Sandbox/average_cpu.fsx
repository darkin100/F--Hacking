#light

open System 
open System.IO 
open System.Text.RegularExpressions

let FileEnumerator filename = seq { 
        use sr = System.IO.File.OpenText(filename)
        while not sr.EndOfStream do 
            let line = sr.ReadLine()
            yield line 
}

let file = FileEnumerator(@"D:\Projects\DgraphScripts\logs\System_Overview.csv")  

let count = 
    file
    |> Seq.map(fun line -> line.Split [|','|])
    |> Seq.map(fun line -> line.[1])
    |> Seq.map(fun line -> line.Replace("\"",""))    
    |> Seq.filter(fun line -> Regex.IsMatch(line,"\d+\.\d+"))
    |> Seq.map(fun line -> System.Decimal.Parse(line))
    |> Seq.average

printfn "%g" count