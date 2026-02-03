using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using SimpleAppWithApi.Models;

namespace SimpleAppWithApi.Services
{
    public class ReportService
    {
    public void GenerateHtml(string path, IEnumerable<User> users)
{
    var sb = new StringBuilder();

    sb.Append("""
    <html>
    <head>
        <meta charset="utf-8"/>
        <style>
            table { border-collapse: collapse; width: 100%; }
            th, td { border: 1px solid #ccc; padding: 6px; text-align: center; }
            th { background: #f0f0f0; }
        </style>
    </head>
    <body>
        <h2>Отчёт по сотрудникам</h2>
        <table>
            <tr>
                <th>ID</th>
                <th>Фамилия</th>
                <th>Имя</th>
                <th>Отчество</th>
                <th>Дата рождения</th>
                <th>Роль</th>
            </tr>
    """);

    foreach (var u in users)
    {
        sb.Append($"""
        <tr>
            <td>{u.Id}</td>
            <td>{u.Surname}</td>
            <td>{u.Name}</td>
            <td>{u.Patronymic ?? "-"}</td>
            <td>{u.Birthday:dd.MM.yyyy}</td>
            <td>{u.RoleName}</td>
        </tr>
        """);
    }

    sb.Append("""
        </table>
    </body>
    </html>
    """);

    File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
}
    public void GenerateExcel(string path, IEnumerable<User> users)
    {
        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("Employees");

        ws.Cell(1, 1).Value = "ID";
        ws.Cell(1, 2).Value = "Фамилия";
        ws.Cell(1, 3).Value = "Имя";
        ws.Cell(1, 4).Value = "Отчество";
        ws.Cell(1, 5).Value = "Дата рождения";
        ws.Cell(1, 6).Value = "Роль";

        ws.Range(1, 1, 1, 6).Style.Font.Bold = true;

        int row = 2;//строка с которой чуваки
        foreach (var u in users)
        {
            ws.Cell(row, 1).Value = u.Id;
            ws.Cell(row, 2).Value = u.Surname;
            ws.Cell(row, 3).Value = u.Name;
            ws.Cell(row, 4).Value = u.Patronymic ?? "-";
            ws.Cell(row, 5).Value = u.Birthday;
            ws.Cell(row, 6).Value = u.RoleName;
            row++;
        }

        var lastRow = ws.LastRowUsed().RowNumber();
        var table = ws.Range(1, 1, lastRow, 6);

        table.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        table.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        ws.Columns().AdjustToContents();

        workbook.SaveAs(path);
    }
}
}