#light

open System 
open System.IO
open System.Net 

let assert_request(url : string) =
    try
        let req = System.Net.WebRequest.Create(url)  
        let resp = req.GetResponse() :?>  HttpWebResponse
        let status = resp.StatusCode.ToString()
        let stream = resp.GetResponseStream()  
        let reader = new IO.StreamReader(stream)  
        let html = reader.ReadToEnd() 
        resp.Close()     
    
    with 
        | :? System.Net.WebException as ex -> 
            
                let status = (ex.Response :?> HttpWebResponse).StatusCode.ToString()

                printfn "%s:%s " status url    

let files = 
    seq{
     for f in Directory.EnumerateFiles(@"D:\work\FHacking\data\WebmasterTools\","*.csv",SearchOption.AllDirectories) do
              yield! File.ReadLines f
    }


files
    |> Seq.filter(fun line -> line.StartsWith("http"))
    |> Seq.map(fun line -> line.Split [|','|])
    |> Seq.map(fun line -> line.[0])
    |> Seq.iter(assert_request >> ignore)