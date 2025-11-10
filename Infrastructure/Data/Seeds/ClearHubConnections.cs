using Infrastructure.Data.DbContext;
using System.Linq;

namespace Infrastructure.Data.Seeds
{
    public static class ClearHubConnections
    {

        //---------------------------------------------------------------------------------------------------


        public static void ClearConnections(LVLgroupDbContext db)
        {
            //Seed Default Fototags

            if (db.HubConnections.Any())
            {
                foreach (var entity in db.HubConnections)
                {
                    db.HubConnections.Remove(entity);
                }
                db.SaveChanges();
            }    
            return;
        }


        //---------------------------------------------------------------------------------------------------

    }
}
