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
    
    member self.Add(x : System.Object, y: System.Object) = 
            series.Points.AddXY(x,y)

    member self.Build() = 
        do chart.Series.Add( series )
        do chart.ChartAreas.Add(area)
        do base.Controls.Add( chart )


let f = new LineChartForm("CPU Plot")

let files = 
    seq{
     for f in Directory.EnumerateFiles(@"D:\_junk\PerfLogs\Admin\CPU Performance\","*.csv",SearchOption.AllDirectories) do
              yield! File.ReadLines f
    }

files
    |> Seq.map(fun line -> line.Split [|','|])
    |> Seq.map(fun line -> (line.[0], line.[1]))
    |> Seq.map(fun (x,y) -> (x.Replace("\"",""),y.Replace("\"","")))    
    |> Seq.filter(fun (x,y) -> Regex.IsMatch(y,"\d+\.\d+"))
    |> Seq.map(fun (x,y) -> (System.DateTime.ParseExact(x, "MM/dd/yyyy HH:mm:ss.FFFF", new CultureInfo("en-GB")),y))    
    |> Seq.sortBy(fun (x,y) -> x)
    |> Seq.map(fun (x,y) -> (x :> System.Object,y :> System.Object))
    |> Seq.iter(f.Add >> ignore)
    
f.Build()
f.Show()    