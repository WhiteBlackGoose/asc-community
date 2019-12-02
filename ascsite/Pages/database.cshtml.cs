using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ascsite.Core;
using AscSite.Core.Interface.DbInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ascsite.Pages
{
    public class databaseModel : PageModel
    {
        public string StatusColor { get; set; }
        public string Status { get; set; }
        [BindProperty] public string Password { get; set; }
        public string DbOutput { get; set; }
        [BindProperty] public string SqlQuery { get; set; } = "use [asc]\nselect* from[dbo].[Posts]";
        public void OnPost()
        {
            try
            {
                if (Functions.GetHashString(Password) != Const.ADMIN_PASSWORD_HASH)
                {
                    Status = "Incorrect password";
                    StatusColor = "red";
                    return;
                }

                if (string.IsNullOrEmpty(SqlQuery)) return;

                var result = DbInterface.ExecuteRawSqlQuery(SqlQuery);
                if (result.Count > 0)
                {
                    string format = CreateDbFormatString(result[0].Count);
                    DbOutput = string.Join('\n', result.Select(row => string.Format(format, row.ToArray<string>())));
                }

                Status = "Success";
                StatusColor = "Green";
            }
            catch (Exception e)
            {
                Status = "Error: " + e.Message;
                StatusColor = "red";
            }
        }

        public string CreateDbFormatString(int columns)
        {
            var builder = new System.Text.StringBuilder();
            for (int i = 0; i < columns; i++)
                builder
                    .Append('{')
                    .Append(i)
                    .Append(",-20}|");
            return builder.ToString();
        }
    }
}