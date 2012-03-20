open System.IO 

let FileEnumerator filename = seq { 
        use sr = System.IO.File.OpenText(filename)
        while not sr.EndOfStream do 
            let line = sr.ReadLine()
            yield line 
}

let files = Directory.EnumerateFiles(@"D:\_junk\PerfLogs\Admin\CPU Performance\","*.csv",SearchOption.AllDirectories)

let res =
    files 
        |> Seq.fold(fun x item -> 
            let lines =  FileEnumerator(item)
            let sq = Seq.append x lines
            sq
    ) Seq.empty

printfn "%A" res
