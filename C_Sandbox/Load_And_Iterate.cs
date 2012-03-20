using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using NUnit.Framework;

namespace C_Sandbox
{
    [TestFixture]
    public class Load_And_Iterate
    {
        [Test]
        public void CPU()
        {
            var files = Directory.EnumerateFiles(@"D:\_junk\PerfLogs\Admin\CPU Performance\", "*.csv", SearchOption.AllDirectories);

            var lines = new List<string>();
            
            Console.WriteLine(files.Count());

            foreach (var file in files)
            {
                string line = null;
                using(var reader = File.OpenText(file))
                while (( line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            Assert.That(lines.Count,Is.GreaterThan(0));
            Console.WriteLine(lines.Count);


            var form = new LineChartForm();

            var a = from line in lines
                        select line.Split(",".ToCharArray());

            var b = from x in a
                    select new {x = x[0].Replace("\"",""), y = x[1].Replace("\"","")};

            var c = from x in b
                    where Regex.IsMatch(x.y, @"\d+\.\d+")
                    select x;

            var d = (from x in c
                    select new
                               {
                                   x = DateTime.ParseExact(x.x, "MM/dd/yyyy HH:mm:ss.FFFF", new CultureInfo("en-GB")),
                                   y = x.y
                               }).OrderByDescending(o => o.x);
                    

            foreach (var x in d)
            {
                form.Add(x.x,x.y);
            }
            
            form.Build();
        }
    
    
        [Test]
        public void ParseDateTime()
        {
            string time = "03/18/2012 22:51:08.491";

            var dateTime = DateTime.ParseExact(time,"MM/dd/yyyy HH:mm:ss.FFFF",new CultureInfo("en-GB"));

            Console.WriteLine(dateTime.ToString());
        }
    }

    public class LineChartForm
    {
        private Chart chart;
        private ChartArea area;
        private Series series;

        public LineChartForm()
        {
            this.chart = new Chart(){Dock = DockStyle.Fill};
            this.area = new ChartArea() {Name = "Area1"};
            this.series = new Series();
            series.ChartType = SeriesChartType.Line;
            series.ChartArea = "Area1";
        }

        public void Add(object x,object y)
        {
            this.series.Points.AddXY(x, y);
        }

        public void Build()
        {
            chart.Series.Add(series);
            chart.ChartAreas.Add(area);
            chart.Width = 2000;
            chart.Height = 1000;
            chart.SaveImage("test.png",ChartImageFormat.Png);
        }
    }
}
