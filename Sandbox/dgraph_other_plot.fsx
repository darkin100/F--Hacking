#light
#r "System.Windows.Forms.DataVisualization"

open System
open System.IO 
open System.Windows.Forms
open System.Windows.Forms.DataVisualization.Charting


type LineChartForm(title) =
    inherit Form( Text=title )

    let chart = new Chart(Dock=DockStyle.Fill)
    let area = new ChartArea(Name="Area1")
    let series = new Series()
    do series.ChartType <- SeriesChartType.Column
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



let BuildDate(line:float)=
    let epoch = new DateTime(1970,1,1,0,0,0,0)
    let ts = TimeSpan.FromMilliseconds(line)
    let newTime = epoch.Add(ts)
    newTime

let datetime(x,y) =
    let value = x.ToString() + "/" + y.ToString()
    value


let lines = File.ReadAllLines(@"D:\Projects\DgraphScripts\output\f\productidsearch.txt")

let xy = 
    lines
    |> Seq.map(fun line -> line.Split [|' '|])
    |> Seq.filter (fun line -> line.Length > 5)
    |> Seq.filter (fun line -> (line.[11].Contains(@"/graph?")))
    |> Seq.map(fun line -> (float line.[0], line.[11]))
    |> Seq.map(fun (x,y) -> (BuildDate x, y))
    |> Seq.groupBy(fun (x,y) -> (x.Day,x.Month))
    |> Seq.map(fun (x,y) -> (datetime x, Seq.length y))
    |> Seq.map(fun (x,y) -> (x :> System.Object, y :> System.Object))
    |> Seq.iter(f.Add >> ignore)


f.Build()
f.Show() 