
using Demo.Api.Data;
using Demo.Api.Models;
using Microsoft.AspNet.OData;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace Demo.Api.Controllers
{
    public class PerformanceIndicatorController : ODataController
    {
        readonly PerformanceIndicatorQueryService _qs = new PerformanceIndicatorQueryService();

        [EnableQuery]
        public IQueryable<PerformanceIndicator> Get()
        {
            return _qs.GetPerformanceIndicators().AsQueryable();
        }

        [EnableQuery]
        public SingleResult<PerformanceIndicator> Get([FromODataUri] int key)
        {
            IQueryable<PerformanceIndicator> result = _qs.GetPerformanceIndicators().Where(p => p.Id == key).AsQueryable();
            return SingleResult.Create(result);
        }
    }
}
