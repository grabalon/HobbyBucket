using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Table givers = new Table();
        Table1.Controls.Add(givers);

        var headerRow = new TableHeaderRow();
        var giverCell = new TableHeaderCell();
        var recCell = new TableHeaderCell();
        giverCell.Text = "Giver";
        recCell.Text = "Gives To";
        headerRow.Cells.Add(giverCell);
        headerRow.Cells.Add(recCell);
        givers.Rows.Add(headerRow);

        givers.Rows.AddRange(GetGivers());
    }

    private class Participants
    {
        public Matchup[] matchups;

        public class Matchup
        {
            public string giver;
            public string recipient;
        }
    }
    private IEnumerable<KeyValuePair<string, string>> GetGiverPairs()
    {
        var listName = Request.QueryString.AllKeys.Contains("list") ? Request.QueryString["list"] : "Current";
        var partPath = Request.MapPath(Path.Combine("~", "Lists", listName + ".json"));

        using (var reader = File.OpenText(partPath))
        {
            var ser = new JsonSerializer();
            var givers = (Participants)ser.Deserialize(reader, typeof(Participants));
            return givers.matchups.Select(m => new KeyValuePair<string, string>(m.giver, m.recipient));
        }
    }

    private TableRow[] GetGivers()
    {
        return GetGiverPairs().Select(pair =>
        {
            var row = new TableRow();
            var giverCell = new TableCell();
            giverCell.Text = pair.Key;
            var recCell = new TableCell();
            recCell.Text = pair.Value;
            row.Cells.Add(giverCell);
            row.Cells.Add(recCell);
            return row;
        }).ToArray();
    }
}