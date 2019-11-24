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

            var entries = DbInterface.GetUsersRelatedToPostById(postId);

            var authors = entries.Where(e => e.postRelation == RelationType.AUTHOR);
            builder.Append(Functions.MakeRelationString("Author", authors));

            var investigators = entries.Where(e => e.postRelation == RelationType.INVESTIGATOR);
            builder.Append(Functions.MakeRelationString("Investigator", investigators));

            var solvers = entries.Where(e => e.postRelation == RelationType.SOLVER);
            builder.Append(Functions.MakeRelationString("Solver", solvers));

            var contributors = entries.Where(e => e.postRelation == RelationType.CONTRIBUTOR);
            builder.Append(Functions.MakeRelationString("Contributor", contributors));

            return builder.ToString();
        }
    }
}
