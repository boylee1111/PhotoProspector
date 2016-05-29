using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using PhotoProspector.Helpers;
using PhotoProspector.Models;
using PhotoProspector.Services;
using PhotoProspector.ViewModels;

namespace PhotoProspector.Controllers
{
    public class OneDriveSearchController : Controller
    {
        private const int UploadScreenWidth = 600;  // Change the value of the width of the image on the screen
        private const int UploadScreenHeight = 900;
        private const string OneDriveSearchResultImagePath = "/ImageSource/SearchImage/";

        private readonly IOneDrivePhotoService oneDrivePhotoService;

        public OneDriveSearchController(IOneDrivePhotoService oneDrivePhotoService)
        {
            this.oneDrivePhotoService = oneDrivePhotoService;
        }

        private string RedirectUri
        {
            get
            {
                return Url.Action("Redirect", "OneDriveSearch", null, Request.Url.Scheme);
            }
        }

        [HttpGet]
        public ActionResult Index(bool isAuthSucceed = true, string errorMsg = "")
        {
            OneDriveUser user = OneDriveUser.UserForRequest(Request);
            if (isAuthSucceed && (user != null)) // if authentication passed and user is cache in session, directly go to ready page.
            {
                return Redirect(Url.Action("ReadyToSearch", "OneDriveSearch"));
            }

            ViewBag.isAuthSucceed = isAuthSucceed; // determine whether error message showing.
            return View();
        }

        [HttpGet]
        public ActionResult ReadyToSearch()
        {
            OneDriveUser user = OneDriveUser.UserForRequest(Request);
            if (null == user)
            {
                return Redirect(Url.Action("Index", "OneDriveSearch"));
            }

            return View();
        }

        [HttpPost]
        public ActionResult OneDriveSearchByAlias(string alias)
        {
            try
            {
                var savedDirectoryRootPath = OneDriveSearchResultImagePath + alias + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "/";
                savedDirectoryRootPath = OneDriveSearchResultImagePath + "onedrive_test/";
                var savedDirectoryPath = Server.MapPath("~" + savedDirectoryRootPath);

                var searchResultViewModel = new SearchResultViewModel(alias, savedDirectoryPath, savedDirectoryRootPath);

                return PartialView("~/Views/Search/SearchResultByAlias.cshtml", searchResultViewModel);
            }
            catch
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                throw new Exception();
            }
        }

        [HttpGet]
        public ActionResult SignInPersonal()
        {
            return Redirect(oneDrivePhotoService.GenerateOneDrivePersonalAuthUrl(RedirectUri));
        }

        [HttpGet]
        public ActionResult SignInBusiness()
        {
            return Redirect(oneDrivePhotoService.GenerateOneDriveForBizAuthUrl(RedirectUri));
        }

        [HttpGet]
        public ActionResult SignOut()
        {
            OneDriveUser.ClearResponseCookie(this.Response);
            return Redirect(Url.Action("Index", "OneDriveSearch"));
        }

        public async Task<ActionResult> Redirect(string code, string state)
        {
            OAuthHelper helper;
            try
            {
                helper = OAuthHelper.HelperForService(state, this.RedirectUri);
            }
            catch
            {
                return Redirect(Url.Action("Index", "OneDriveSearch", new { isAuthSucceed = false, errorMsg = "Server is busy. Please try again later." }));
            }

            string discoveryResource = "https://api.office.com/discovery/";
            var token = await helper.RedeemAuthorizationCodeAsync(code, discoveryResource);
            if (null == token)
            {
                return Redirect(Url.Action("Index", "OneDriveSearch", new { isAuthSucceed = false, errorMsg = "Invalid response from token service. Unable to login. Please try again later." }));
            }

            OneDriveUser user = new OneDriveUser(token, helper, discoveryResource);
            user.SetResponseCookie(this.Response);

            return Redirect(Url.Action("Index", "OneDriveSearch", new { isAuthSucceed = true }));
        }
    }
}