using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using DGZ_WEB_API.Models;
using DGZ_WEB_API.Utils;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DGZ_WEB_API.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AnalisingServicesController : ControllerBase
    {
        private readonly IOptions<AppSettings> _appSettings;
        private readonly EFDbContext _context;
        public AnalisingServicesController(IOptions<AppSettings> appSettings, EFDbContext context)
        {
            _context = context;
            _appSettings = appSettings;
        }

        [HttpGet]
        public ActionResult UpdateSODData()
        {
            //1 - высчетать налоговые регистрацию

            var suppliers = _context.suppliers.Take(10).ToList();


            //2 - обновить информацию руководителям
            //Паспорт
            //СФ
            //МТСР

            //
            //3 - таблица измененных значений (анализ изменения)

            foreach (var item in suppliers)
            {
                var r = get_tpb_usiness_activity_date_by_inn_response(item.inn);
                _context.tpb_usiness_activity_date_by_inn_responses.Add(r);
            }

            _context.SaveChanges();


            return Ok();
        }
        private void calcSti()
        {
            //add some
            //aaaaaa
        }


        private tpb_usiness_activity_date_by_inn_response get_tpb_usiness_activity_date_by_inn_response(string inn)
        {
            tpb_usiness_activity_date_by_inn_response obj = null;
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(
                    new
                    {
                        clientId = "d967b6bf-2c0b-469b-a501-d64f7e348cb8",
                        orgName = "ПОРТАЛ",
                        request = new
                        {
                            TPBusinessActivityDateByInn = new
                            {
                                request = new
                                {
                                    INN = inn
                                }
                            }
                        }
                    });

                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var url = "http://" + _appSettings.Value.SODHost + "/ServiceConstructor/SoapClient/SendRequest2";

                var response = client.PostAsync(url, data).GetAwaiter().GetResult();

                string result = response.Content.ReadAsStringAsync().Result;

                var j = JObject.Parse(result);
                if (j["response"]["TPBusinessActivityDateByInnResponse"]["response"] != null)
                {
                    var s = j["response"]["TPBusinessActivityDateByInnResponse"]["response"];
                    obj = new tpb_usiness_activity_date_by_inn_response
                        {
                            created_at = DateTime.Now,
                            updated_at = DateTime.Now,
                            legalAddress = s["FullAddress"].ToString(),
                            name = s["FullName"].ToString(),
                            tin = s["TIN"].ToString(),
                            rayonCode = s["RayonCode"].ToString(),
                            taxActiveDate = DateTime.Parse(s["TaxActiveDate"].ToString()),
                            rayonName = s["RayonName"].ToString(),
                            taxTypeCode = s["TaxTypeCode"].ToString().Trim()
                        };
                    if (!string.IsNullOrEmpty(obj.taxTypeCode))
                    {
                        var codeName = _context.taxe_codes.FirstOrDefault(x => x.code.Trim() == obj.taxTypeCode.Trim());
                        if (codeName != null) obj.taxTypeName = codeName.name;
                    }
                    _context.tpb_usiness_activity_date_by_inn_responses.AddRange(obj);
                    _context.SaveChanges();
                }
                return obj;
            }
        }

        [HttpGet]
        public ActionResult GetSources()
        {
            var model = new List<_sourceItem>();

            model.Add(new _sourceItem
            {
                name = "supplier",
                caption = "Поставщик"
            });
            model.Add(new _sourceItem
            {
                name = "counterpart",
                caption = ""
            });
            model.Add(new _sourceItem
            {
                name = "counterpart_type",
                caption = ""
            });
            model.Add(new _sourceItem
            {
                name = "country",
                caption = ""
            });
            model.Add(new _sourceItem
            {
                name = "currency",
                caption = ""
            });
            model.Add(new _sourceItem
            {
                name = "msec_detail",
                caption = ""
            });
            model.Add(new _sourceItem
            {
                name = "ownership_type",
                caption = ""
            });
            model.Add(new _sourceItem
            {
                name = "procuring_entity",
                caption = ""
            });
            model.Add(new _sourceItem
            {
                name = "supplier_member",
                caption = ""
            });

            return Ok(model);
        }
        public class _sourceItem
        {
            public string name { get; set; }
            public string caption { get; set; }
        }


        // GET: api/suppliers
        [HttpPost]
        public ActionResult GetSuppliersByPage([FromBody]FilterModel filter, int page, int size, string order = "desc", string sort = "Id")
        {
            if (page <= 0) page = 1;
            var TopNo = size;
            var SkipNo = (page - 1) * size;

            var query = _context.suppliers.Include(x => x._ownership_type).AsQueryable();


            if(filter != null && filter.conditions != null && filter.conditions.Length > 0)
            {
                foreach (var condition in filter.conditions)
                {
                    if (condition.val != null)
                        query = query.Where(condition.field_name, condition.operation, condition.val);
                }
            }


            query = query.OrderBy(new SortModel[] { new SortModel { ColId = sort, Sort = order } });

            var list = query.Skip(SkipNo).Take(TopNo);


            return Ok(new { items = list.ToList(), total_count = query.Count() });
        }

        [HttpGet]
        public ActionResult GetSupplierDetails(int id)
        {
            var model = _context.suppliers.Include(x => x._ownership_type).FirstOrDefault(x => x.id == id);
            return Ok(model);
        }

        [HttpGet]
        public ActionResult GetSupplierMembers(int supplierId)
        {
            var model = _context.supplier_members.Include(x => x._member_type).Where(x => x.supplier == supplierId);
            return Ok(model.ToArray());
        }

        [HttpGet]
        public ActionResult<string[]> SearchByName(string src)
        {
            src = string.IsNullOrEmpty(src) ? "" : src.Trim();
            if (src.Length > 2)
            {
                return _context.suppliers.Where(x => x.name.ToLower().Contains(src.ToLower())).Select(x => x.name).Distinct().ToArray();
            }
            return new string[] { };
        }
    }
    public class FilterModel
    {
        public _condition[] conditions { get; set; }
        public class _condition
        {
            public string field_name { get; set; }
            public string operation { get; set; }
            public object val { get; set; }
        }
    }
    public class SortModel
    {
        public string ColId { get; set; }
        public string Sort { get; set; }

        public string PairAsSqlExpression
        {
            get
            {
                return $"{ColId} {Sort}";
            }
        }
    }
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, IEnumerable<SortModel> sortModels)
        {
            var expression = source.Expression;
            int count = 0;
            foreach (var item in sortModels)
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var selector = Expression.PropertyOrField(parameter, item.ColId);
                var method = string.Equals(item.Sort, "desc", StringComparison.OrdinalIgnoreCase) ?
                    (count == 0 ? "OrderByDescending" : "ThenByDescending") :
                    (count == 0 ? "OrderBy" : "ThenBy");
                expression = Expression.Call(typeof(Queryable), method,
                    new Type[] { source.ElementType, selector.Type },
                    expression, Expression.Quote(Expression.Lambda(selector, parameter)));
                count++;
            }
            return count > 0 ? source.Provider.CreateQuery<T>(expression) : source;
        }
        public static IQueryable<T> Where<T>(this IQueryable<T> source, string propertyName, string comparison, object value)
        {
            return source.Where(ExpressionUtils.BuildPredicate<T>(propertyName, comparison, value));
        }
    }
    public static class ExpressionUtils
    {
        public static Expression<Func<T, bool>> BuildPredicate<T>(string propertyName, string comparison, object value)
        {
            var parameter = Expression.Parameter(typeof(T));
            var left = propertyName.Split('.').Aggregate((Expression)parameter, Expression.PropertyOrField);
            var body = MakeComparison(left, comparison, value);
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        static Expression MakeComparison(Expression left, string comparison, object value)
        {
            if (left.Type == typeof(int?))
            {
                value = (int?)int.Parse(value.ToString());
            }
            var constant = Expression.Constant(value, left.Type);
            switch (comparison)
            {
                case "==":
                    return Expression.MakeBinary(ExpressionType.Equal, left, constant);
                case "!=":
                    return Expression.MakeBinary(ExpressionType.NotEqual, left, constant);
                case ">":
                    return Expression.MakeBinary(ExpressionType.GreaterThan, left, constant);
                case ">=":
                    return Expression.MakeBinary(ExpressionType.GreaterThanOrEqual, left, constant);
                case "<":
                    return Expression.MakeBinary(ExpressionType.LessThan, left, constant);
                case "<=":
                    return Expression.MakeBinary(ExpressionType.LessThanOrEqual, left, constant);
                case "Contains":
                case "StartsWith":
                case "EndsWith":
                    if (value is string)
                    {
                        return Expression.Call(left, comparison, Type.EmptyTypes, constant);
                    }
                    throw new NotSupportedException($"Comparison operator '{comparison}' only supported on string.");
                default:
                    throw new NotSupportedException($"Invalid comparison operator '{comparison}'.");
            }
        }
    }

    [EnableCors("_myAllowSpecificOrigins")]
    [ApiController]
    public class OverrideController : ControllerBase
    {
        [Route("api/export/json")]
        [HttpGet]
        public ActionResult export_json()
        {
            var json = System.IO.File.ReadAllText("C:\\distr\\export-json.json");

            return Content(json, "application/json");
        }
        [Route("api/tendering")]
        [HttpGet]
        public ActionResult tendering()
        {
            var json = System.IO.File.ReadAllText("C:\\distr\\tendering.json");

            return Content(json, "application/json");
        }
    }
}