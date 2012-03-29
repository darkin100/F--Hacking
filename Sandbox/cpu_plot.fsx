#light
#r "System.Windows.Forms.DataVisualization"

open System.IO 
open System.Windows.Forms
open System.Windows.Forms.DataVisualization.Charting
open System.Text.RegularExpressions
open System.Diagnostics
open System.Globalization


type LineChartForm(title) =
    inherit Form( Text=title )

    let chart = new Chart(Dock=DockStyle.Fill)
    let area = new ChartArea(Name="Area1")
    let series = new Series()
    do series.ChartType <- SeriesChartType.Line
    do series.ChartArea <- "Area1"
    do base.Height <- 800
    do base.Width <- 1500
    
    member self.Add(x : System.Object, y: System.Object) = 
            series.Points.AddXY(x,y)

    member self.Build() = 
        do chart.Series.Add( series )
        do chart.ChartAreas.Add(area)
        do base.Controls.Add( chart )


let f = new LineChartForm("CPU Plot")

let average(line : string[]) =
    printfn "%A" line
    let total = (float line.[1] + float line.[2] + float line.[3] + float line.[4] + float line.[5] + float line.[6] + float line.[7])
    let ave = total / float 7.0
    ave

let files = 
    seq{
     for f in Directory.EnumerateFiles(@"D:\work\FHacking\data\PerfLogs\Admin\CPU Performance\","*.csv",SearchOption.AllDirectories) do
              yield! File.ReadLines f
    }

files
    |> Seq.map(fun line -> line.Replace("\"",""))    
    |> Seq.map(fun line -> line.Split [|','|])
    |> Seq.filter(fun line -> line.Length > 8)
    |> Seq.filter(fun line -> Regex.IsMatch(line.[1],"\d+\.\d+"))
    |> Seq.map(fun line -> (line.[0],average(line)))
    |> Seq.map(fun (x,y) -> (System.DateTime.ParseExact(x, "MM/dd/yyyy HH:mm:ss.FFFF", new CultureInfo("en-GB")),y))    
    |> Seq.sortBy(fun (x,y) -> x)
    |> Seq.map(fun (x,y) -> (x :> System.Object,y :> System.Object))
    |> Seq.iter(f.Add >> ignore)
    
printfn "%A" files

f.Build()
f.Show()    