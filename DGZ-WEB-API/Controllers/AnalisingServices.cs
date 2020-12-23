using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
using OfficeOpenXml;

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
        static bool isLoading = false;
        public ActionResult IsLoading()
        {
            return Ok(isLoading);
        }
        [HttpGet]
        public ActionResult UpdateSODData()
        {
            isLoading = true;
            int ip_infos = 0, pension_infos = 0;

            //1 - высчетать налоговые регистрацию

            var suppliers = _context.suppliers.Where(x => x.inn.Length == 14).ToList();

            foreach (var item in suppliers)
            {
                var r = get_tpb_usiness_activity_date_by_inn_response(item.inn);
                if (r != null)
                {
                    _context.tpb_usiness_activity_date_by_inn_responses.Add(r);
                    _context.SaveChanges();
                    ip_infos++;
                }
                    
            }
            //2 - обновить информацию руководителям
            //СФ
            foreach (var item in _context.supplier_members.Where(x => x.pin.Length == 14).ToList())
            {
                var pension_infoObj = get_pension_info(item.pin);
                if(pension_infoObj != null)
                {
                    pension_infoObj.supplier_member = item.id;
                    _context.pension_infos.Add(pension_infoObj);
                    _context.SaveChanges();
                    pension_infos++;
                }
            }
            //МТСР

            //
            //3 - таблица измененных значений (анализ изменения)

            


            return Ok(new { ip_infos, pension_infos });
        }
        private void calcSti()
        {
            //add some
            //aaaaaa
        }

        [HttpGet]
        public ActionResult MigrateAllSuppliers()
        {
            try
            {
                foreach (var s in _context.suppliers)
                {
                    var _s = new _supplier
                    {
                        created_at = s.created_at,
                        updated_at = s.updated_at,
                        bankAccount = s.bankAccount,
                        factAddress = s.factAddress,
                        legalAddress = s.legalAddress,
                        bankName = s.bankName,
                        bic = s.bic,
                        id = s.id,
                        inn = s.inn,
                        isBlack = s.isBlack,
                        isResident = s.isResident,
                        name = s.name,
                        ownership_type = s.ownership_type,
                        rayonCode = s.rayonCode,
                        telephone = s.telephone,
                        zip = s.zip,
                        supplier_members = _context.supplier_members.Where(x => x.supplier == s.id).ToArray(),
                        ip_items = _context.tpb_usiness_activity_date_by_inn_responses.Where(x => x.tin == s.inn).ToArray()
                    };
                    migrateSupplier(_s);
                }
            }
            catch (Exception e)
            {

                return Ok(new { result = false, error = e.Message, trace = e.StackTrace });
            }
            return Ok(new { result = true });
        }
        class _supplier : supplier
        {
            public supplier_member[] supplier_members { get; set; }
            public tpb_usiness_activity_date_by_inn_response[] ip_items { get; set; }
        }
        private void migrateSupplier(_supplier document)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(document);

                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var url = "http://192.168.2.150/dgz-cissa-rest-api/api/DgzImport/CreateSupplier";

                var response = client.PostAsync(url, data).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                string result = response.Content.ReadAsStringAsync().Result;
            }
        }

        private tpb_usiness_activity_date_by_inn_response get_tpb_usiness_activity_date_by_inn_response(string inn)
        {
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
                    var obj = new tpb_usiness_activity_date_by_inn_response
                        {
                        id = 0,
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
                    //_context.tpb_usiness_activity_date_by_inn_responses.Add(obj);
                    //_context.SaveChanges();
                    return obj;
                }
                return null;
            }
        }

        private pension_info get_pension_info(string pin)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(
                    new
                    {
                        clientId = "1da179ae-3d5d-40f0-80c0-15de49f44001",
                        orgName = "ПОРТАЛ",
                        request = new
                        {
                            GetPensionInfoWithSum = new
                            {
                                PIN = pin,
                                RequestOrg = "mlsd",
                                RequestPerson = "system"
                            }
                        }
                    });

                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var url = "http://" + _appSettings.Value.SODHost + "/ServiceConstructor/SoapClient/SendRequest2";

                var response = client.PostAsync(url, data).GetAwaiter().GetResult();

                string result = response.Content.ReadAsStringAsync().Result;

                var j = JObject.Parse(result);
                if (j["response"]["GetPensionInfoWithSumResponse"]["DossierInfoes"] != null)
                {
                    var s = j["response"]["GetPensionInfoWithSumResponse"]["DossierInfoes"]["DossierInfoWithSum"];
                    if(s != null)
                    {
                        var obj = new pension_info
                        {
                            rusf = s["RUSF"].ToString(),
                            numDossier = s["NumDossier"].ToString(),
                            pinPensioner = s["PINPensioner"].ToString(),
                            pinRecipient = s["PINRecipient"].ToString(),
                            dateFromInitial = s["DateFromInitial"].ToString(),
                            sum = s["Sum"].ToString(),
                            kindOfPension = s["KindOfPension"].ToString(),
                            categoryPension = s["CategoryPension"].ToString(),
                            created_at = DateTime.Now,
                            updated_at = DateTime.Now
                        };
                        return obj;
                    }
                }
                return null;
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
                name = "license_type",
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

            var query = _context.suppliers.Include(x => x._ownership_type).Include(x => x._industry)/*.Include(x => x.licenses)*/.AsQueryable();


            if(filter != null && filter.conditions != null && filter.conditions.Length > 0)
            {
                foreach (var condition in filter.conditions)
                {
                    if (condition.val != null)
                    {
                        if (condition.field_name == "license__license_type")
                        {
                            var subQuery = _context.licenses.Where(x => x.license_type == (int)condition.val).Select(x1 => x1.supplier ?? 0).Distinct().ToArray();
                            query = query.Where(x => subQuery.Contains(x.id));
                        }
                        else
                            query = query.Where(condition.field_name, condition.operation, condition.val);
                    }
                }
            }


            query = query.OrderBy(new SortModel[] { new SortModel { ColId = sort, Sort = order } });

            var list = query.Skip(SkipNo).Take(TopNo);


            return Ok(new { items = list.ToList(), total_count = query.Count() });
        }

        [HttpGet]
        public ActionResult GetSupplierDetails(int id)
        {
            var s = _context.suppliers.Include(x => x._ownership_type).Include(x => x.licenses).ThenInclude(x => x._license_type).FirstOrDefault(x => x.id == id);
            var model = new _supplier
            {
                created_at = s.created_at,
                updated_at = s.updated_at,
                bankAccount = s.bankAccount,
                factAddress = s.factAddress,
                legalAddress = s.legalAddress,
                bankName = s.bankName,
                bic = s.bic,
                id = s.id,
                inn = s.inn,
                isBlack = s.isBlack,
                isResident = s.isResident,
                name = s.name,
                ownership_type = s.ownership_type,
                rayonCode = s.rayonCode,
                telephone = s.telephone,
                zip = s.zip,
                industry = s.industry,
                licenses = s.licenses,
                _industry = s._industry,
                _ownership_type = s._ownership_type,
                supplier_members = _context.supplier_members.Where(x => x.supplier == s.id).ToArray(),
                ip_items = _context.tpb_usiness_activity_date_by_inn_responses.Where(x => x.tin == s.inn).ToArray()
            };
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
        [HttpPost, DisableRequestSizeLimit]
        public ActionResult Upload()
        {
            try
            {
                var formCollection = Request.ReadFormAsync().GetAwaiter().GetResult();
                var file = formCollection.Files.First();
                var pathToSave = "c:\\distr";
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    using (ExcelPackage package = new ExcelPackage(file.OpenReadStream()))
                    {
                        StringBuilder sb = new StringBuilder();
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                        int rowCount = worksheet.Dimension.Rows;
                        int ColCount = worksheet.Dimension.Columns;
                        int innCol = 1,
                            nameCol = 2,
                            ownership_typeCol = 3,
                            legalAddressCol = 4,
                            factAddressCol = 5,
                            telephoneCol = 6,
                            bankNameCol = 7,
                            bankAccountCol = 8,
                            bicCol = 9,
                            zipCol = 10,
                            rayonCodeCol = 11,
                            isResidentCol = 12,
                            isBlackCol = 13;
                        string errors = "";
                        for (int row = 2; row <= rowCount; row++)
                        {
                            try
                            {
                                var inn = "";
                                if (worksheet.Cells[row, innCol].Value != null) inn = worksheet.Cells[row, innCol].Value.ToString();
                                var name = "";
                                if (worksheet.Cells[row, nameCol].Value != null) name = worksheet.Cells[row, nameCol].Value.ToString();
                                int? ownership_type = null;
                                if (worksheet.Cells[row, ownership_typeCol].Value != null && worksheet.Cells[row, ownership_typeCol].Value.ToString().Trim() != "") ownership_type = int.Parse(worksheet.Cells[row, ownership_typeCol].Value.ToString());
                                var legalAddress = "";
                                if (worksheet.Cells[row, legalAddressCol].Value != null) legalAddress = worksheet.Cells[row, legalAddressCol].Value.ToString();
                                var factAddress = "";
                                if (worksheet.Cells[row, factAddressCol].Value != null) factAddress = worksheet.Cells[row, factAddressCol].Value.ToString();
                                var telephone = "";
                                if (worksheet.Cells[row, telephoneCol].Value != null) telephone = worksheet.Cells[row, telephoneCol].Value.ToString();
                                var bankName = "";
                                if (worksheet.Cells[row, bankNameCol].Value != null) bankName = worksheet.Cells[row, bankNameCol].Value.ToString();
                                var bankAccount = "";
                                if (worksheet.Cells[row, bankAccountCol].Value != null) bankAccount = worksheet.Cells[row, bankAccountCol].Value.ToString();
                                var bic = "";
                                if (worksheet.Cells[row, bicCol].Value != null) bic = worksheet.Cells[row, bicCol].Value.ToString();
                                var zip = "";
                                if (worksheet.Cells[row, zipCol].Value != null) zip = worksheet.Cells[row, zipCol].Value.ToString();
                                var rayonCode = "";
                                if (worksheet.Cells[row, rayonCodeCol].Value != null) rayonCode = worksheet.Cells[row, rayonCodeCol].Value.ToString();
                                bool isResident = true;
                                if (worksheet.Cells[row, isResidentCol].Value != null && worksheet.Cells[row, isResidentCol].Value.ToString().Trim() != "")
                                {
                                    int c = int.Parse(worksheet.Cells[row, isResidentCol].Value.ToString());
                                    isResident = c == 1;
                                }
                                bool isBlack = false;
                                if (worksheet.Cells[row, isBlackCol].Value != null)
                                {
                                    int c = int.Parse(worksheet.Cells[row, isBlackCol].Value.ToString());
                                    isBlack = c == 1;
                                }
                                _context.suppliers.Add(new supplier
                                {
                                    inn = inn,
                                    created_at = DateTime.Now,
                                    updated_at = DateTime.Now,
                                    bankAccount = bankAccount,
                                    factAddress = factAddress,
                                    legalAddress = legalAddress,
                                    bankName = bankName,
                                    bic = bic,
                                    isBlack = isBlack,
                                    isResident = isResident,
                                    name = name,
                                    ownership_type = ownership_type,
                                    rayonCode = rayonCode,
                                    telephone = telephone,
                                    zip = zip
                                });
                            }
                            catch (Exception e)
                            {
                                errors += "; error: " + e.Message + ", trace: " + e.StackTrace;
                                continue;
                                //throw;
                            }
                        }
                        _context.SaveChanges();
                        return Ok(new { result = true, fullPath, errors });
                    }
                }
                else
                {
                    return Ok(new { result = false, error = "Файл не загружен!" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { result = false, error = ex.Message, trace = ex.StackTrace });
            }
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
        private readonly IOptions<AppSettings> _appSettings;
        private readonly EFDbContext _context;
        public OverrideController(IOptions<AppSettings> appSettings, EFDbContext context)
        {
            _context = context;
            _appSettings = appSettings;
        }
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
        [Route("api/sod/sf")]
        [HttpPost]
        public ActionResult sf(object jsonObj)
        {
            if (fetchSOD(jsonObj, out string result))
            {
                var res = JObject.Parse(result);
                if (res["response"] != null)
                {
                    if (res["response"]["GetWorkPeriodInfoResponse"] != null)
                    {
                        if (res["response"]["GetWorkPeriodInfoResponse"]["WorkPeriods"] != null)
                        {
                            if(res["response"]["GetWorkPeriodInfoResponse"]["WorkPeriods"]["WorkPeriod"] != null)
                            {
                                if (res["response"]["GetWorkPeriodInfoResponse"]["WorkPeriods"]["WorkPeriod"] is JArray)
                                {
                                    return Ok(res);
                                }
                                else if(res["response"]["GetWorkPeriodInfoResponse"]["WorkPeriods"]["WorkPeriod"] is JObject)
                                {
                                    return Ok(JObject.FromObject(
                    new
                    {
                        response = new
                        {
                            GetWorkPeriodInfoResponse = new
                            {
                                WorkPeriods = new
                                {
                                    WorkPeriod = new object[]
                            {
                                res["response"]["GetWorkPeriodInfoResponse"]["WorkPeriods"]["WorkPeriod"]
                            }
                                }
                            }
                        }
                    }
                    ));
                                }
                            }
                        }
                    }
                }
            }
            return Ok(JObject.FromObject(
                    new
                    {
                        response = new
                        {
                            GetWorkPeriodInfoResponse = new
                            {
                                WorkPeriods = new
                                {
                                    WorkPeriod = new object[]
                            {

                            }
                                }
                            }
                        }
                    }
                    ));
        }

        [Route("api/sod/kadastr")]
        [HttpPost]
        public ActionResult kadastr(object jsonObj)
        {
            if(fetchSOD(jsonObj, out string result))
            {
                var res = JObject.Parse(result);
                //return Ok(res);
                if (res["response"] != null)
                {
                    if (res["response"]["Searchpin_allResponse"] != null && !(res["response"]["Searchpin_allResponse"] is JValue))
                    {
                        if (res["response"]["Searchpin_allResponse"]["Searchpin_allResult"] != null)
                        {
                            if(res["response"]["Searchpin_allResponse"]["Searchpin_allResult"]["SFP"] != null)
                            {
                                if (res["response"]["Searchpin_allResponse"]["Searchpin_allResult"]["SFP"] is JObject)
                                {
                                    return Ok(JObject.FromObject(
                    new
                    {
                        response = new
                        {
                            Searchpin_allResponse = new
                            {
                                Searchpin_allResult = new
                                {
                                    SFP = new object[] { res["response"]["Searchpin_allResponse"]["Searchpin_allResult"]["SFP"] }
                                }
                            }
                        }
                    }
                    ));
                                }
                                else
                                {
                                    return Ok(res);
                                }
                            }
                        }
                    }
                }
            }
            return Ok(JObject.FromObject(
                    new
                    {
                        response = new
                        {
                            Searchpin_allResponse = new
                            {
                                Searchpin_allResult = new 
                            {
                                    SFP = new object[] { }
                                }
                            }
                        }
                    }
                    ));
        }

        [Route("api/sod/mtsr")]
        [HttpPost]
        public ActionResult mtsr(object jsonObj)
        {
            if (fetchSOD(jsonObj, out string result))
            {
                var res = JObject.Parse(result);
                //return Ok(res);
                if (res["response"] != null)
                {
                    if (res["response"]["GetActivePaymentsByPINResponse"] != null)
                    {
                        if (res["response"]["GetActivePaymentsByPINResponse"]["response"] != null)
                        {
                            return Ok(res);
                        }
                    }
                }
            }
            return Ok(JObject.FromObject(
                    new
                    {
                        response = new
                        {
                            GetActivePaymentsByPINResponse = new
                            {
                                response = new
                                {
                                    StartDate = "",
                                    EndDate = "",
                                    PaymentTypeName = "",
                                    PaymentSize = 0,
                                    OrganizationName = "",
                                    DocumentNo = "",
                                    DocumentName = "",
                                    IsActive = false
                                }
                            }
                        }
                    }
                    ));
        }

        private bool fetchSOD(object jsonObj, out string result)
        {
            result = "";
            try
            {
                using (var client = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(jsonObj);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");

                    var url = "http://" + _appSettings.Value.SODHost + "/ServiceConstructor/SoapClient/SendRequest2";

                    var response = client.PostAsync(url, data).GetAwaiter().GetResult();

                    result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}