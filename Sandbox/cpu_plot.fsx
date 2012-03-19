#r "System.Windows.Forms.DataVisualization"

open System.IO 
open System.Windows.Forms
open System.Windows.Forms.DataVisualization.Charting
open System.Text.RegularExpressions

type LineChartForm( title, xs : float seq ) =
    inherit Form( Text=title )

    let chart = new Chart(Dock=DockStyle.Fill)
    let area = new ChartArea(Name="Area1")
    let series = new Series()
    do series.ChartType <- SeriesChartType.Line
    do xs |> Seq.iter (series.Points.Add >> ignore)
    do series.ChartArea <- "Area1"
    do chart.Series.Add( series )
    do chart.ChartAreas.Add(area)
    do base.Controls.Add( chart )





let FileEnumerator filename = seq { 
        use sr = System.IO.File.OpenText(filename)
        while not sr.EndOfStream do 
            let line = sr.ReadLine()
            yield line 
}

let file = FileEnumerator(@"D:\Projects\DgraphScripts\logs\System_Overview.csv")  

let data = 
    file
    |> Seq.map(fun line -> line.Split [|','|])
    |> Seq.map(fun line -> line.[1])
    |> Seq.map(fun line -> line.Replace("\"",""))    
    |> Seq.filter(fun line -> Regex.IsMatch(line,"\d+\.\d+"))
    |> Seq.map(fun line -> float line)

    
let f = new LineChartForm("CPU Plot", data)
f.Show()