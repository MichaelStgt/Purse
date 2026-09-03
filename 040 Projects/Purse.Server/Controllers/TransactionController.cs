using CommunityToolkit.Datasync.Server;
using CommunityToolkit.Datasync.Server.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Purse.Server.Data;
using Purse.Server.Model;

namespace Purse.Server.Controllers
{
    [Route("tables/transaction")]
    public class TransactionController : TableController<Transaction>
    {
        public TransactionController(AppDbContext context)
            : base(new EntityTableRepository<Transaction>(context))
        {
        }
    }
}