
open System.Net 
open System 
open System.IO 
 



 /// Fetch the contents of a web page 
let http(url: string) =  
    let req = System.Net.WebRequest.Create(url)  
    let resp = req.GetResponse() 
    let stream = resp.GetResponseStream()  
    let reader = new IO.StreamReader(stream)  
    let html = reader.ReadToEnd() 
    resp.Close()     


// ok, now fetch a page.  Create the web request, 
// wait for the response, read off the response. 
let req = System.Net.WebRequest.Create("http://localhost:15001/graph?node=0&group=0&offset=0&nbins=10&attrs=ProductIDSearch+dvdnewandbestsellers10wk39|mode%2bmatchallpartial&pred=AvailableForSaleFrom%7cLTEQ+20120302&pred=AvailableForSaleTo%7cGTEQ+20120302&filter=IsPublished%3a1&irversion=601") 
let stream = req.GetResponse().GetResponseStream() 
let reader = new IO.StreamReader(stream) 
let html = reader.ReadToEnd() 


// Ok, let's look at the HTML 
html 