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
    public class GovIntegrationServiceController : ControllerBase
    {
        private readonly IOptions<AppSettings> _appSettings;
        private readonly EFDbContext _context;
        public GovIntegrationServiceController(IOptions<AppSettings> appSettings, EFDbContext context)
        {
            _context = context;
            _appSettings = appSettings;
        }


        [HttpGet("{inn}")]
        public async Task<ActionResult<supplier[]>> GetOrganizationByInn(string inn)
        {
            var supplierObj = _context.suppliers.FirstOrDefault(x => x.inn == inn);

            if (supplierObj != null) return Ok(new[] { supplierObj });
            else
            {
                using (var client = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(
                        new
                        {
                            clientId = "6bfc28f5-0793-4727-b347-a8486a80673b",
                            orgName = "ПОРТАЛ",
                            request = new
                            {
                                tpDataByINNforBusinessActivity = new
                                {
                                    request = new
                                    {
                                        inn = inn
                                    }
                                }
                            }
                        });

                    var data = new StringContent(json, Encoding.UTF8, "application/json");

                    var url = "http://" + _appSettings.Value.SODHost + "/ServiceConstructor/SoapClient/SendRequest2";

                    var response = await client.PostAsync(url, data);

                    string result = response.Content.ReadAsStringAsync().Result;

                    var j = JObject.Parse(result);
                    if (j["response"]["tpDataByINNforBusinessActivityResponse"]["response"] != null)
                    {
                        var s = j["response"]["tpDataByINNforBusinessActivityResponse"]["response"];
                        var tpObj = new tp_data_by_inn_for_business_activity_response
                        {
                            created_at = DateTime.Now,
                            updated_at = DateTime.Now,
                            FullAddress = s["FullAddress"].ToString(),
                            FullName = s["FullName"].ToString(),
                            inn = s["inn"].ToString(),
                            RayonCode = s["RayonCode"].ToString(),
                            ZIP = s["ZIP"].ToString()
                        };
                        _context.tp_data_by_inn_for_business_activity_responses.Add(tpObj);

                        supplierObj = new supplier
                        {
                            name = tpObj.FullName,
                            legalAddress = tpObj.FullAddress,
                            inn = inn,
                            zip = tpObj.ZIP,
                            rayonCode = tpObj.RayonCode,
                            created_at = tpObj.created_at,
                            updated_at = tpObj.updated_at
                        };

                        //_context.suppliers.Add(supplierObj);

                        _context.SaveChanges();
                        return Ok(new[] { supplierObj });
                    }
                    else return NotFound();
                }

            }
        }

        [HttpGet("{inn}")]
        public async Task<ActionResult<tpb_usiness_activity_date_by_inn_response[]>> GetIPByInn(string inn)
        {
            var obj = _context.tpb_usiness_activity_date_by_inn_responses.FirstOrDefault(x => x.TIN == inn);

            if (obj != null) return Ok(new[] { obj });
            else
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

                    var response = await client.PostAsync(url, data);

                    string result = response.Content.ReadAsStringAsync().Result;

                    var j = JObject.Parse(result);
                    if (j["response"]["TPBusinessActivityDateByInnResponse"]["response"] != null)
                    {
                        var s = j["response"]["TPBusinessActivityDateByInnResponse"]["response"];
                        obj = new tpb_usiness_activity_date_by_inn_response
                        {
                            created_at = DateTime.Now,
                            updated_at = DateTime.Now,
                            LegalAddress = s["FullAddress"].ToString(),
                            Name = s["FullName"].ToString(),
                            TIN = s["TIN"].ToString(),
                            RayonCode = s["RayonCode"].ToString(),
                            TaxActiveDate = DateTime.Parse(s["TaxActiveDate"].ToString()),
                            RayonName = s["RayonName"].ToString(),
                            TaxTypeCode = s["TaxTypeCode"].ToString().Trim()
                        };
                        if (!string.IsNullOrEmpty(obj.TaxTypeCode))
                        {
                            var codeName = _context.taxe_codes.FirstOrDefault(x => x.Code.Trim() == obj.TaxTypeCode.Trim());
                            if (codeName != null) obj.TaxTypeName = codeName.Name;
                        }
                        _context.tpb_usiness_activity_date_by_inn_responses.Add(obj);
                        _context.SaveChanges();
                        return Ok(new[] { obj });
                    }
                    else return NotFound();
                }

            }
        }

        [HttpGet("{inn}")]
        public async Task<ActionResult<supplier_member[]>> GetSupplierMembers(string inn)
        {
            var supplierObj = _context.suppliers.FirstOrDefault(x => x.inn == inn);
            if(supplierObj != null)
            {
                return _context.supplier_members.Where(x => x.supplier == supplierObj.id).ToArray();
            }
            return new supplier_member[] { };
        }

        [HttpGet("{inn}")]
        public async Task<ActionResult<tp_data_by_inn_for_business_activity_response[]>> GetTp_data_by_inn_for_business_activity_response(string inn)
        {
            return _context.tp_data_by_inn_for_business_activity_responses.Where(x => x.inn == inn).ToArray();
        }

        [HttpGet("{inn}")]
        public async Task<ActionResult<tpb_usiness_activity_date_by_inn_response[]>> GetTpb_usiness_activity_date_by_inn_response(string inn)
        {
            return _context.tpb_usiness_activity_date_by_inn_responses.Where(x => x.TIN == inn).ToArray();
        }

        [HttpGet]
        public async Task<ActionResult<ReferenceDescriptionItem[]>> GetReferenceDescriptionItems()
        {
            return new ReferenceDescriptionItem[]
            {
                new ReferenceDescriptionItem
                {
                    Caption = "Тип контрагента",
                    ApiName = "counterpart_type"
                },
                new ReferenceDescriptionItem
                {
                    Caption = "Контрагент",
                    ApiName = "counterparts"
                },
                new ReferenceDescriptionItem
                {
                    Caption = "Валюта",
                    ApiName = "currencies"
                },
                new ReferenceDescriptionItem
                {
                    Caption = "Страны",
                    ApiName = "countries"
                },
            };
        }

        [HttpGet]
        public async Task<ActionResult<bool>> SendUserEmail(string email, string username, string password)
        {
            return _SendEmail(email, username, password);
        }

        private bool _SendEmail(string email, string username, string password)
        {
            if (string.IsNullOrEmpty(email)) return false;
            MailMessage mail = new MailMessage();
            /*foreach (var email in payment.PABank.email.Split(','))
            {
                mail.To.Add(email.Trim());
            }*/
            mail.To.Add(email);
            mail.From = new MailAddress("intersoftkgz@gmail.com");
            mail.Subject = "Уведомление логин-пароль Департамент государственных закупок пр МФ КР";
            string Body = "<h4>Уважаемый пользователь. Ваши учетные данные успешно зарегистрированы в систему со след. параметрами:</h4>"
                + "<p>Логин: <b>" + username + "</b></p>"
                + "<p>Пароль: <b>" + password + "</b></p>";
            mail.Body = Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("intersoftkgz@gmail.com", "N8SFKjbZqGyzpJM"); // Enter seders User name and password  
            smtp.EnableSsl = true;
            smtp.Send(mail);

            return true;
        }

        public class ReferenceDescriptionItem
        {
            public string Caption { get; set; }
            public string ApiName { get; set; }
        }
    }
}