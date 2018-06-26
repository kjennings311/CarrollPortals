using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carroll.Data.Entities;
namespace Carroll.Data.Entities.Helpers
{
    public class EFConnectionAccessor : IDisposable
    {
        private readonly SqlConnection sqlConnection;
        private readonly CarrollFormsEntities  entities;
        //usage
        //using (SqlConnection sqlConnection = new EFConnectionAccessor().connection)
        public EFConnectionAccessor()
        {

            entities = new CarrollFormsEntities();
            var entityConnection = entities.Database.Connection as EntityConnection;

            if (entityConnection != null)
            {
                sqlConnection = entityConnection.StoreConnection as SqlConnection;
            }
        }

        public SqlConnection connection
        {
            get
            {
                sqlConnection.Open();
                return sqlConnection;
            }
        }

        public CarrollFormsEntities Entities
        {
            get
            {
                return entities;
            }
        }

        public void Dispose()
        {
            sqlConnection.Close();
            sqlConnection.Dispose();
            entities.Dispose();
        }
    }
}
