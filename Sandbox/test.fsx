open System.IO 

let FileEnumerator filename = seq { 
        use sr = System.IO.File.OpenText(filename)
        while not sr.EndOfStream do 
            let line = sr.ReadLine()
            yield line 
}

let files = Directory.EnumerateFiles(@"D:\_junk\PerfLogs\Admin\CPU Performance\","*.csv",SearchOption.AllDirectories)

//TEST1 using my own iterator and yeilding
let sw = System.Diagnostics.Stopwatch()
sw.Start()

let res2 =
    seq{
        for item in files do
            yield! FileEnumerator(item)
    }

printfn "%A" res2

sw.Stop() 

printfn "TEST1:Time elapsed: %dms" sw.ElapsedMilliseconds 

//TEST2:using my own iterator and folding
let sw2 = System.Diagnostics.Stopwatch()
sw2.Start()

let res =
    files 
        |> Seq.fold(fun x item -> 
            let lines =  FileEnumerator(item)
            let sq = Seq.append x lines
            sq
        ) Seq.empty

printfn "%A" res 

sw2.Stop() 

printfn "TEST2:Time elapsed: %dms" sw2.ElapsedMilliseconds 

//TEST3: Using readlines
let sw3 = System.Diagnostics.Stopwatch()
sw3.Start()


let res3 = 
    seq { for f in Directory.EnumerateFiles(@"D:\_junk\PerfLogs\Admin\CPU Performance\","*.csv",SearchOption.AllDirectories) do
              yield! File.ReadLines f }//Readlines can start enumerating the colelction before the end, therefore its laxy initialised

printfn "%A" res3

sw3.Stop() 
printfn "TEST3:Time elapsed: %dms" sw3.ElapsedMilliseconds 

//TEST4: Using readlines & concaat
let sw4 = System.Diagnostics.Stopwatch()
sw4.Start()


let res4 = Directory.EnumerateFiles(@"D:\_junk\PerfLogs\Admin\CPU Performance\","*.csv",SearchOption.AllDirectories)
          |> Seq.map File.ReadLines 
          |> Seq.concat
   
printfn "%A" res4

sw4.Stop() 
printfn "TEST4: Time elapsed: %dms" sw4.ElapsedMilliseconds 