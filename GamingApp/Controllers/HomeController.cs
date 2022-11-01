using GamingApp.Entity;
using GamingApp.Entity.Context;
using GamingApp.Hubs;
using GamingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GamingApp.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHubContext<MyHub> _hubContext;

        GamingAppContext db = new GamingAppContext();

        public HomeController(ILogger<HomeController> logger, IHubContext<MyHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        public string GetLocalIPAddress()
        {

            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            return remoteIpAddress.ToString();

        }

        public IActionResult Index()
        {

            var games = db.Game.OrderByDescending(x => x.CreateDate).ToList();

            return View(games);
        }

        public IActionResult YeniOrganizasyon()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult YeniOrganizasyon(Game organizasyon)
        {
            if (ModelState.IsValid)
            {
                organizasyon.CreateDate = DateTime.Now;
                organizasyon.GameAdminUserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                db.Game.Add(organizasyon);
                var kayit = db.SaveChanges();
                if (kayit > 0)
                {
                    Logs newLog = new Logs();
                    newLog.LogDate = DateTime.Now;
                    newLog.LogDesc = $"{organizasyon.GameName} isimli oyun {User.Identity.Name} tarafından oluşturuldu.";
                    newLog.InsertTable = "YeniOrganizasyon";
                    newLog.UserName = User.Identity.Name;
                    newLog.GameId = organizasyon.GameId;
                    newLog.IpAddress = GetLocalIPAddress();
                    db.Logs.Add(newLog);
                    db.SaveChanges();
                    _logger.LogInformation($"oyun {User.Identity.Name} tarafından oluşturuldu.");

                }
                else
                {

                    TempData.Put("KayitBasarili", new ResultMessage
                    {
                        Title = "Kayıt Başarısız",
                        Message = organizasyon.GameName + " isimli kaydınız kaydedilemedi. ",
                        Css = "danger"
                    });
                }
                return RedirectToAction("KullaniciEkle", new { id = organizasyon.GameId });
            }

            return View(organizasyon);
        }


        public ActionResult KullaniciEkle(int id)
        {
            var organizasyon = db.Game.Find(id);
            if (id < 1)
            {
                TempData.Put("KayitBasarili", new ResultMessage
                {
                    Title = "Organizasyon Bulunamadı !",
                    Message = "Aradağınız Organizasyonu Bulamadık :(. ",
                    Css = "danger"
                });
                return RedirectToAction("Index");
            }

            var model = new OrganizasyonUserModel()
            {
                SeciliOyuncular = db.GameUser.Where(x => x.GameId == id).Select(x => x.User).ToList()
            };

            ViewBag.Oyuncular = db.User.ToList();

            ViewData["ORG_ID"] = organizasyon.GameId;



            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> KullaniciEkle(OrganizasyonUserModel model, int[] oyuncuIds)
        {
            if (ModelState.IsValid)
            {
                var entity = db.Game.Find(model.Id);
                if (entity == null)
                {
                    TempData.Put("KayitBasarili", new ResultMessage
                    {
                        Title = "Organizasyon Bulunamadı !",
                        Message = "Aradağınız Organizasyonu Bulamadık :(. ",
                        Css = "danger"
                    });
                    return RedirectToAction("Index");
                }

                var turnuva = db.Game.Include("GameUser").Include("GameUser.User").FirstOrDefault(x => x.GameId == entity.GameId);
                var gameUser = db.GameUser.Where(x => x.GameId == turnuva.GameId).ToList();

                if (turnuva.GameClosed)
                {
                    TempData.Put("KayitBasarili", new ResultMessage
                    {
                        Title = "Oyun Sonlandırılmış !",
                        Message = "Oyun Sonlandırılmış !!! ",
                        Css = "danger"
                    });
                    return RedirectToAction("OrganizasyonDetay", new { id = turnuva.GameId });
                }

                if (turnuva != null)
                {
                    turnuva.GameUser = oyuncuIds.Select(uId => new GameUser()
                    {
                        UserId = uId,
                        GameId = entity.GameId
                    }).ToList();

                    var kayit = db.SaveChanges();
                    if (kayit > 0)
                    {

                        var session = db.Session.Where(x => x.GameId == turnuva.GameId && x.SessionClosed).ToList();

                        foreach (var item in gameUser)
                        {
                            oyuncuIds = oyuncuIds.Where(val => val != item.UserId).ToArray();
                        }


                        foreach (var item in session)
                        {
                            item.SessionUser = oyuncuIds.Select(uId => new SessionUser()
                            {
                                UserId = uId,
                                SessionId = item.SessionId,
                                Cash = 0,
                                CashOut = 0,
                                TotalCash = 0
                            }).ToList();
                        }
                        db.SaveChanges();

                        List<string> oyuncular = new List<string>();

                        foreach (var item in oyuncuIds)
                        {
                            var oyunc = db.User.Find(Convert.ToInt32(item));
                            oyuncular.Add(oyunc.UserNameSurname);
                        }

                        Logs newLog = new Logs();
                        newLog.LogDate = DateTime.Now;
                        newLog.LogDesc = $"{string.Join(",", oyuncular)} isimli oyuncular {User.Identity.Name} tarafından eklenmiştir.";
                        newLog.InsertTable = "KullaniciEkle";
                        newLog.UserName = User.Identity.Name;
                        newLog.GameId = turnuva.GameId;
                        newLog.IpAddress = GetLocalIPAddress();
                        db.Logs.Add(newLog);
                        db.SaveChanges();


                        _logger.LogInformation($"{string.Join(",", oyuncular)} isimli oyuncular {User.Identity.Name} tarafından eklenmiştir.");
                        await _hubContext.Clients.All.SendAsync("ReceiveMasaOrderCount", 1);

                    }
                    else
                    {
                        TempData.Put("KayitBasarili", new ResultMessage
                        {
                            Title = "Kayıt Başarısız",
                            Message = "kaydınız kaydedilemedi. ",
                            Css = "danger"
                        });
                    }
                }
                return RedirectToAction("OrganizasyonDetay", new { id = model.Id });
            }
            ViewBag.Oyuncular = db.User.ToList();
            return View(model);
        }

        public async Task<ActionResult> YeniSession(int id)
        {
            var turnuva = db.Game.Include("GameUser").Include("GameUser.User").FirstOrDefault(x => x.GameId == id);
            var session = db.Session.Where(x => x.GameId == turnuva.GameId).ToList();


            if (turnuva.GameClosed)
            {
                TempData.Put("KayitBasarili", new ResultMessage
                {
                    Title = "Oyun Sonlandırılmış !",
                    Message = "Oyun Sonlandırılmış !!! ",
                    Css = "danger"
                });
                return RedirectToAction("OrganizasyonDetay", new { id = id });
            }

            if (session.Where(x => !x.SessionClosed).Count() > 0)
            {

                TempData.Put("KayitBasarili", new ResultMessage
                {
                    Title = "Kayıt Başarılı",
                    Message = "Lütfen Önce Mevcut Seansı Sonlandırın",
                    Css = "success"
                });

                return RedirectToAction("OrganizasyonDetay", new { id = id });
            }

            var max = 1;
            if (session.Count > 0)
            {
                max = session.Max(x => Convert.ToInt32(x.SessionName)) + 1;
            }

            Session newSession = new Session();
            newSession.GameId = turnuva.GameId;
            newSession.SessionName = max.ToString();
            newSession.SessionClosed = false;
            newSession.SessionTime = DateTime.Now;
            db.Session.Add(newSession);

            var sessionKayit = db.SaveChanges();

            if (sessionKayit > 0)
            {
                foreach (var item in turnuva.GameUser)
                {
                    SessionUser su = new SessionUser();
                    su.SessionId = newSession.SessionId;
                    su.UserId = item.UserId;
                    su.Cash = 0;
                    su.CashOut = 0;
                    su.TotalCash = 0;
                    db.SessionUser.Add(su);

                }
                var suCount = db.SaveChanges();

                if (suCount > 0)
                {

                    Logs newLog = new Logs();
                    newLog.LogDate = DateTime.Now;
                    newLog.LogDesc = $"{newSession.SessionName}. seans {User.Identity.Name} tarafından eklenmiştir.";
                    newLog.InsertTable = "YeniSession";
                    newLog.UserName = User.Identity.Name;
                    newLog.GameId = turnuva.GameId;
                    newLog.IpAddress = GetLocalIPAddress();
                    db.Logs.Add(newLog);
                    db.SaveChanges();

                    _logger.LogInformation($"{newSession.SessionName}. seans {User.Identity.Name} tarafından eklenmiştir.");

                    await _hubContext.Clients.All.SendAsync("ReceiveMasaOrderCount", 1);

                }
                else
                {
                    TempData.Put("KayitBasarili", new ResultMessage
                    {
                        Title = "Kayıt Başarısız",
                        Message = "Bir Hata Meydana Geldi",
                        Css = "success"
                    });
                }
            }
            return RedirectToAction("OrganizasyonDetay", new { id = id });
        }

        public IActionResult GameScreen(int id)
        {
            var game = db.Game.Find(id);
            return View();
        }


        public ActionResult OrganizasyonDetay(int id)
        {
            var organizasyon = db.Game.Find(id);
            if (organizasyon == null)
            {
                TempData.Put("KayitBasarili", new ResultMessage
                {
                    Title = "Organizasyon Bulunamadı !",
                    Message = "Aradağınız Organizasyonu Bulamadık :(. ",
                    Css = "danger"
                });
                return RedirectToAction("Index");
            }


            var sess = db.Session.Where(x => x.GameId == organizasyon.GameId && !x.SessionClosed).FirstOrDefault();

            ViewData["ORG_ID"] = organizasyon.GameId;

            if (sess != null)
            {
                ViewData["SESSION_ID"] = sess.SessionId;
                ViewData["SESSION_NAME"] = sess.SessionName;
                ViewData["SESSION_TIME"] = sess.SessionTime.ToString("yyyy/MM/dd HH:mm:ss");
            }

            var harcama = db.Game.Include(x => x.GameUser).ThenInclude(x => x.User).Include(x => x.Session).ThenInclude(x => x.SessionUser).Where(x => x.GameId == id).FirstOrDefault();

            return View(harcama);

        }

        public string HesapGetir (int id)
        {
            var veriListResponse = db.SessionUser.Include(x => x.Session).ThenInclude(x => x.Game).Include(x => x.User).Where(x => x.Session.GameId == id).ToList();
            var data = veriListResponse.Select(k => new { k.TotalCash, k.User }).GroupBy(x => new { x.User }, (key, group) => new
            {
                xx = key.User.UserNameSurname,
                foto = key.User.UserPhoto,
                tCharge = Math.Round(group.Sum(k => Convert.ToDecimal(k.TotalCash)), 1)
            }).OrderByDescending(x => x.tCharge).ToList();

            List<HesapModel> artilar = new List<HesapModel>();
            List<HesapModel> eksiler = new List<HesapModel>();

            foreach (var item in data)
            {

                HesapModel hesap = new HesapModel();

                if (item.tCharge > 0)
                {
                    hesap.Ucret = item.tCharge;
                    hesap.İsim = item.xx;

                    artilar.Add(hesap);
                }
                else if (item.tCharge < 0)
                {
                    hesap.Ucret = item.tCharge;
                    hesap.İsim = item.xx;

                    eksiler.Add(hesap);
                }
            }

            string borc = "";
            foreach (var eksiOlan in eksiler.OrderBy(x => x.Ucret))
            {

                //decimal kalanborc = 0;

                foreach (var artiOlan in artilar.OrderByDescending(x => x.Ucret))
                {
                    if (Math.Abs(eksiOlan.Ucret) >= Math.Abs(artiOlan.Ucret) && Math.Abs(eksiOlan.Ucret) > 0 && Math.Abs(artiOlan.Ucret) > 0)
                    {
                        eksiOlan.Ucret = Math.Abs(eksiOlan.Ucret) - Math.Abs(artiOlan.Ucret);
                        borc += $"{eksiOlan.İsim}  {artiOlan.İsim}'e  {artiOlan.Ucret} TL verecek <br/>";
                        artiOlan.Ucret = 0;
                    }
                    else if (Math.Abs(eksiOlan.Ucret) < Math.Abs(artiOlan.Ucret) && Math.Abs(eksiOlan.Ucret) > 0 && Math.Abs(artiOlan.Ucret) > 0)
                    {
                        artiOlan.Ucret = Math.Abs(artiOlan.Ucret) - Math.Abs(eksiOlan.Ucret);
                        borc += $"{eksiOlan.İsim}  {artiOlan.İsim}'e {Math.Abs(eksiOlan.Ucret)} TL verecek <br/>";
                        eksiOlan.Ucret = 0;

                    }

                }
            }
            return borc;
        }



        public JsonResult Hesapla(int id)
        {
            var borc = HesapGetir(id);

           // await _hubContext.Clients.All.SendAsync("ReceiveMasaOrderCounts", 1);

            return Json(new
            {
                sonuc_kod = 1,
                sonuc_mesaj = borc
            });
        }

        public async Task<JsonResult> HesapPaylas(int id)
        {

             await _hubContext.Clients.All.SendAsync("ReceiveMasaOrderCounts", 1);
            return Json(new
            {
                sonuc_kod = 1,
                sonuc_mesaj = "asd"
            });
        }


        public ActionResult HarcamaEkle(int UserId, int SessionId)
        {
            if (UserId < 1 || SessionId < 1)
            {
                return BadRequest();
            }

            var sessionuser = db.SessionUser.Include(x => x.User).Where(x => x.SessionId == SessionId && x.UserId == UserId).FirstOrDefault();
            if (sessionuser == null)
            {
                return BadRequest();
            }
            ViewBag.User = sessionuser.User.UserNameSurname.ToString();
            return PartialView(sessionuser);
        }

        public async Task<JsonResult> HarcamaKaydet(int id, decimal harcama)
        {
            var sessionuser = db.SessionUser.Include(x => x.Session).Include(x => x.User).Where(x => x.SessionUserId == id).FirstOrDefault();
            if (sessionuser == null)
            {
                return Json(new
                {
                    sonuc_kod = 0,
                    sonuc_mesaj = "Böyle bir id yok"
                });
            }

            var gameid = sessionuser.Session.GameId;
            var game = db.Game.Find(gameid);

            if (game.GameClosed)
            {
                return Json(new
                {
                    sonuc_kod = 0,
                    sonuc_mesaj = "Oyun Sonlandırılmış !!!"
                });
            }

            if (game.GameAdminUserId != Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return Json(new
                {
                    sonuc_kod = 0,
                    sonuc_mesaj = "Bu turnuvada harcama eklemek için yetkiniz yok !!!"
                });
            }

            sessionuser.Cash = sessionuser.Cash - Convert.ToDecimal(harcama);
            db.SessionUser.Update(sessionuser);
            db.SaveChanges();




            Logs newLog = new Logs();
            newLog.LogDate = DateTime.Now;
            newLog.LogDesc = $"{sessionuser.User.UserNameSurname} isimli kullanıcıya {User.Identity.Name} tarafından {harcama} TL eklenmiştir.";
            newLog.InsertTable = "HarcamaKaydet";
            newLog.UserName = User.Identity.Name;
            newLog.GameId = gameid;
            newLog.IpAddress = GetLocalIPAddress();
            db.Logs.Add(newLog);
            db.SaveChanges();

            _logger.LogInformation($"{sessionuser.User.UserNameSurname} isimli kullanıcıya {User.Identity.Name} tarafından {harcama} TL eklenmiştir.");

            await _hubContext.Clients.All.SendAsync("ReceiveMasaOrderCount", 1);

            return Json(new
            {
                sonuc_kod = 1,
                sonuc_mesaj = "Başarılı"
            });
        }


        public ActionResult SeansBitir(int id)
        {
            var sessionuser = db.SessionUser.Include(x => x.User).Where(x => x.SessionId == id).ToList();
            if (sessionuser == null)
            {
                return BadRequest();
            }

            ViewData["SESSION_ID"] = id;
            ViewData["TOPLAM_ALINAN"] = (sessionuser.Sum(x => x.Cash) * -1).ToString("0.##");

            return PartialView(sessionuser);
        }

        public async Task<JsonResult> SeansKapatBitir(SeansModel parameters)
        {
            try
            {
                var session = db.Session.Find(parameters.SessionId);

                var gameid = session.GameId;
                var game = db.Game.Find(gameid);

                if (game.GameClosed)
                {
                    return Json(new
                    {
                        sonuc_kod = 0,
                        sonuc_mesaj = "Oyun Sonlandırılmış !!!"
                    });
                }


                if (game.GameAdminUserId != Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)))
                {
                    return Json(new
                    {
                        sonuc_kod = 0,
                        sonuc_mesaj = "Bu turnuvada seans kapatmak için yetkiniz yok !!!"
                    });
                }

                foreach (var item in parameters.UrunList)
                {
                    var su = db.SessionUser.Find(item.SESSIONUSERID);
                    su.CashOut = item.ARTITUTARI;
                    su.TotalCash = item.ARTITUTARI + su.Cash;
                    db.SessionUser.Update(su);
                }


                session.SessionClosed = true;
                db.Session.Update(session);

                var dbtest = db.SaveChanges();

                if (dbtest > 0)
                {

                    Logs newLog = new Logs();
                    newLog.LogDate = DateTime.Now;
                    newLog.LogDesc = $"{session.SessionName}. seans {User.Identity.Name} tarafından bitirilmiştir.";
                    newLog.InsertTable = "SeansKapatBitir";
                    newLog.UserName = User.Identity.Name;
                    newLog.GameId = session.GameId;
                    newLog.IpAddress = GetLocalIPAddress();
                    db.Logs.Add(newLog);
                    db.SaveChanges();

                    _logger.LogInformation($"{session.SessionName}. seans {User.Identity.Name} tarafından bitirilmiştir.");

                    await _hubContext.Clients.All.SendAsync("ReceiveMasaOrderCount", 1);
                    return Json(new
                    {
                        sonuc_kod = 1,
                        sonuc_mesaj = "Başarılı"
                    });

                }
                else
                {
                    return Json(new
                    {
                        sonuc_kod = 0,
                        sonuc_mesaj = "Hata Meydana Geldi"
                    });
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //return BadRequest("Hata : Beklenmedik bi durum oluştu " + ex.Message);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult LogsByGame(int id)
        {

            var logs = db.Logs.Include(x => x.Game).Where(x => (x.LogDate.Date == DateTime.Now.Date || x.LogDate.Date == DateTime.Now.AddDays(-1).Date) && x.GameId == id).OrderByDescending(x => x.LogDate).ToList();

            return View("Loglar", logs);
        }

        public ActionResult Loglar()
        {
            var logs = db.Logs.Include(x => x.Game).Where(x => x.LogDate.Date == DateTime.Now.Date || x.LogDate.Date == DateTime.Now.AddDays(-1).Date).OrderByDescending(x => x.LogDate).ToList();

            return View(logs);
        }

        public ActionResult AdminDegistir(int id)
        {
            var game = db.Game.Find(id);
            var list = db.User.ToList();
            ViewData["UserId"] = new SelectList(list, "UserId", "UserNameSurname");
            return View(game);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminDegistir(Game game)
        {
            if (ModelState.IsValid)
            {

                db.Game.Update(game);
                db.SaveChanges();
                return RedirectToAction("OrganizasyonDetay", new { id = game.GameId });

            }
            var list = db.User.ToList();
            ViewData["UserId"] = new SelectList(list, "UserId", "UserNameSurname");
            return View(game);
        }

        public async Task<JsonResult> OyunuBitir(int id)
        {
            var game = db.Game.Find(id);
            game.GameClosed = true;
            db.Game.Update(game);


            var dbtest = db.SaveChanges();

            if (dbtest > 0)
            {

                Logs newLog = new Logs();
                newLog.LogDate = DateTime.Now;
                newLog.LogDesc = $"{game.GameName} isimli oyun {User.Identity.Name} tarafından bitirilmiştir.";
                newLog.InsertTable = "OyunuBitir";
                newLog.UserName = User.Identity.Name;
                newLog.GameId = game.GameId;
                newLog.IpAddress = GetLocalIPAddress();
                db.Logs.Add(newLog);
                db.SaveChanges();


                _logger.LogInformation($"{game.GameName} isimli oyun {User.Identity.Name} tarafından bitirilmiştir.");

                await _hubContext.Clients.All.SendAsync("ReceiveMasaOrderCounts", 1);


                return Json(new
                {
                    sonuc_kod = 1,
                    sonuc_mesaj = "bitti"
                });

            }
            else
            {
                return Json(new
                {
                    sonuc_kod = 0,
                    sonuc_mesaj = "HATAA"
                });
            }

        }





        public IActionResult GrafikIndex()
        {
            return View();
        }

        public async Task<IActionResult> TumZamanlar()
        {
            try
            {
                var veriListResponse = db.SessionUser.Include(x => x.Session).ThenInclude(x => x.Game).Include(x => x.User).Where(x=>x.Session.Game.OyunTuruId != 2).ToList();
                if (veriListResponse != null)
                {
                    var data = veriListResponse.Select(k => new { k.TotalCash, k.User }).GroupBy(x => new { x.User }, (key, group) => new
                    {
                        xx = key.User.UserNameSurname,
                        foto = key.User.UserPhoto,
                        tCharge = group.Sum(k => Convert.ToDouble(k.TotalCash))
                    }).OrderBy(x => x.tCharge).ToList();
                    List<GrafikModel> y = new List<GrafikModel>();
                    foreach (var item in data)
                    {
                        GrafikModel d = new GrafikModel();
                        d.DataTipi = item.xx.ToString();
                        d.Foto = item.foto != null ? item.foto : "defaultpp.png";
                        d.Toplam = Math.Round(item.tCharge, 2, MidpointRounding.ToEven);
                        y.Add(d);
                    }
                    return Ok(y);
                }
                else
                {
                    TempData.Put("message", new ResultMessage
                    {
                        Title = "Bir Hata Oluştu.",
                        Message = "Bir Hata Oluştu.",
                        Css = "danger"
                    });
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                TempData.Put("message", new ResultMessage
                {
                    Title = "Bir Hata Oluştu.",
                    Message = ex.Message + " " + ex.InnerException.Message,
                    Css = "danger"
                });
                throw new Exception(ex.Message + " " + ex.InnerException.Message);

                //return BadRequest("Hata : Beklenmedik bi durum oluştu " + ex.Message);
            }
        }


    }
}
