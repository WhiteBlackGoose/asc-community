using ascsite.Core;
using AscSite.Core.Interface.DbInterface;
using System.Linq;
using System.Text;

namespace AscSite.Core.Render.Renderers
{
    using RelationType = AscSite.Core.Interface.Database.UserPostContribution.TYPE;
    public class ProjectPageRenderer : PageRenderer
    {
        public ProjectPageRenderer(int postId) : base(postId)
        {
            
        }

        public override string Render()
        {
            var post = DbInterface.GetPostById(postId);
            var sb = new StringBuilder();
            sb
                .Append("<h1>")
                .Append(post.Name)
                .Append("</h1>")
                .Append("<br>")
                .Append(Functions.Md2Html(post.Announcement))
                .Append("<hr>")
                .Append(Functions.Md2Html(post.Body));

            var entries = DbInterface.GetUsersRelatedToPostById(postId);

            var authors = entries.Where(e => e.postRelation == RelationType.AUTHOR);
            sb.Append(Functions.MakeRelationString("Author", authors));

            var investigators = entries.Where(e => e.postRelation == RelationType.INVESTIGATOR);
            sb.Append(Functions.MakeRelationString("Investigator", investigators));

            var solvers = entries.Where(e => e.postRelation == RelationType.SOLVER);
            sb.Append(Functions.MakeRelationString("Solver", solvers));

            var contributors = entries.Where(e => e.postRelation == RelationType.CONTRIBUTOR);
            sb.Append(Functions.MakeRelationString("Contributor", contributors));

            return sb.ToString();
        }
    }
}
