using ascsite.Core;
using AscSite.Core.Interface.Database;
using AscSite.Core.Interface.DbInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscSite.Core.Render.Renderers
{
    using RelationType = AscSite.Core.Interface.Database.UserPostContribution.TYPE;
    public class ProblemPageRenderer : PageRenderer
    {

        public ProblemPageRenderer(int postId) : base(postId)
        {
        }

        public override string Render()
        {
            StringBuilder builder = new StringBuilder(base.Render());

            // only one request to the database is performed here
            var entries = DbInterface.GetUsersRelatedToPostById(postId);

            var authors = entries.Where(e => e.postRelation == RelationType.AUTHOR);
            builder.Append(MakeRelationString("Author", authors));

            var investigators = entries.Where(e => e.postRelation == RelationType.INVESTIGATOR);
            builder.Append(MakeRelationString("Investigator", investigators));

            var solvers = entries.Where(e => e.postRelation == RelationType.SOLVER);
            builder.Append(MakeRelationString("Solver", solvers));

            return builder.ToString();
        }

        private string MakeRelationString(string relationName, IEnumerable<DbInterface.PostUserEntry> entries)
        {
            if (entries.Count() == 0) return string.Empty;

            relationName += (entries.Count() < 2 ? ": " : "s: ");
            return "<br>" + relationName + string.Join(", ", entries.Select(e => e.userData.Name));
        }
    }
}
