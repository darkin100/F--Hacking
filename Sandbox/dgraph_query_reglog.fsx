open System.Net 
open System 
open System.IO 

let http(url: string) =  
    
        printfn "%s" url
        let req = System.Net.WebRequest.Create(url)  
        let resp = req.GetResponse() :?>  HttpWebResponse
        let status = resp.StatusCode.ToString()

        printfn "%s" status
    
        let stream = resp.GetResponseStream()  
        let reader = new IO.StreamReader(stream)  
        let html = reader.ReadToEnd() 
        resp.Close() 
    
    
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

test |> Seq.filter (fun line -> line.Length > 5)
|> Seq.map (fun line -> line.[11]) 
|> Seq.filter (fun line -> (line.Contains(@"/graph?")))
|> Seq.map (fun line -> "http://sd-swk-end001:15001" + line) 
|> Seq.iter(fun (url) -> http url)

sw.Stop() 
printfn "Time elapsed: %dms" sw.ElapsedMilliseconds 



  