using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using DGZ_WEB_API.Models;
using DGZ_WEB_API.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DGZ_WEB_API.Controllers
{
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

        
    }
}