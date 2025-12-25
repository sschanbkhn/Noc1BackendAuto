using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network.API.Infrastructure;
//https://xunit.net/docs/shared-context
namespace Network.UnitTest
{
    public class DatabaseFixture
    {
        public DatabaseFixture()
        {
            var _contextOptions = new DbContextOptionsBuilder<DomainDbContext>()
                      .UseInMemoryDatabase("DatabaseFixture")
                      .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                      .Options;
            Db = new DomainDbContext(_contextOptions);
            InitializeDbForTests(Db);
        }
        public void Dispose()
        {            
            Db.Dispose();            
        }
        private void InitializeDbForTests(DomainDbContext domainDbContext)
        {
            domainDbContext.Sys_Organizations.Add(new API.Model.Sys_Organization()
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111000"),
                Code = "Org0",
                Name = "Org0",
                Type = Network.Core.Enums.OrganizationType.Organization,
                ParentId = Guid.Empty
            });

            domainDbContext.Sys_Organizations.Add(new API.Model.Sys_Organization()
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111001"),
                Code = "Org0.1",
                Name = "Org0.1",
                Type = Network.Core.Enums.OrganizationType.Organization,
                ParentId = Guid.Parse("11111111-1111-1111-1111-111111111000"),
            });
            domainDbContext.Sys_Organizations.Add(new API.Model.Sys_Organization()
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111011"),
                Code = "Org0.1.1",
                Name = "Org0.1.1",
                Type = Network.Core.Enums.OrganizationType.Department,
                ParentId = Guid.Parse("11111111-1111-1111-1111-111111111001"),
            });
            domainDbContext.Sys_Organizations.Add(new API.Model.Sys_Organization()
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111012"),                
                Code = "Org0.1.2",
                Name = "Org0.1.2",
                Type = Network.Core.Enums.OrganizationType.Department,
                ParentId = Guid.Parse("11111111-1111-1111-1111-111111111001"),
            });

            domainDbContext.Sys_Organizations.Add(new API.Model.Sys_Organization()
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111002"),
                Code = "Org0.2",
                Name = "Org0.2",
                Type = Network.Core.Enums.OrganizationType.Organization,
                ParentId = Guid.Parse("11111111-1111-1111-1111-111111111000"),
            });
            domainDbContext.Sys_Organizations.Add(new API.Model.Sys_Organization()
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111021"),
                Code = "Org0.2.1",
                Name = "Org0.2.1",
                Type = Network.Core.Enums.OrganizationType.Department,
                ParentId = Guid.Parse("11111111-1111-1111-1111-111111111002"),
            });
            domainDbContext.Sys_Organizations.Add(new API.Model.Sys_Organization()
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111022"),
                Code = "Org0.2.2",
                Name = "Org0.2.2",
                Type = Network.Core.Enums.OrganizationType.Department,
                ParentId = Guid.Parse("11111111-1111-1111-1111-111111111002"),
            });
            domainDbContext.SaveChanges();
        }
        public DomainDbContext Db { get; private set; }
    }
}
