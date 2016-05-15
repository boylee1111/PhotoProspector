using System.IO;
using System.Web.Mvc;
using PhotoProspector.Services;
using PhotoProspector.ViewModels;

namespace PhotoProspector.Controllers
{
    public class SignUpController : Controller
    {
        private readonly IImageService imageService;
        private readonly IFileService fileService;
        private readonly IUserService userService;
        private readonly IMessageService messageService;
        private readonly ITrainingService trainingService;

        public SignUpController(
            IImageService imageService,
            IFileService fileService,
            IUserService userService,
            IMessageService messageService,
            ITrainingService trainingService)
        {
            this.imageService = imageService;
            this.fileService = fileService;
            this.userService = userService;
            this.messageService = messageService;
            this.trainingService = trainingService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new SignUpViewModel());
        }

        public ActionResult Index(SignUpViewModel signUpViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var filename = Path.GetFileName(signUpViewModel.photoPath.FileName);
                    var tempFolder = Server.MapPath("~/TempImg/");
                    if (!Directory.Exists(tempFolder))
                    {
                        Directory.CreateDirectory(tempFolder);
                    }
                    var savedFullPaht = tempFolder + fileService.filenameHashed(filename);
                    signUpViewModel.photoPath.SaveAs(savedFullPaht);
                    if (userService.InsertUserToSQL(savedFullPaht, signUpViewModel, Server.MapPath("~/ImageSource/")))
                    {
                        string trainpath = Server.MapPath("~/ImageSource/");
                        trainingService.StartTraining(trainpath);
                        return View(new SignUpViewModel(SignUpViewModel.SignUpStatus.SignUpSucceed));
                    }
                    else
                    {
                        return View(new SignUpViewModel(SignUpViewModel.SignUpStatus.SignUpFailed, "Sign up failed. Please try again later."));
                    }
                }
            }
            catch
            {
                return View(new SignUpViewModel(SignUpViewModel.SignUpStatus.SignUpFailed, "Sign up failed. Please try again later."));
            }

            return View(signUpViewModel);
        }

        [HttpGet]
        public ActionResult DeleteUser()
        {
            return View(new DeleteUserViewModel());
        }

        [HttpPost]
        public ActionResult DeleteUser(DeleteUserViewModel deleteUserViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (userService.DeleteUserFromSQL(deleteUserViewModel.alias, Server.MapPath("~/ImageSource/")))
                    {
                        return View(new DeleteUserViewModel(DeleteUserViewModel.DeleteUserStatus.DeleteUserSucceed));
                    }
                    else
                    {
                        return View(new DeleteUserViewModel(DeleteUserViewModel.DeleteUserStatus.DeleteUserFailed, "Delete user failed. User email doesn't exist."));
                    }
                }
            }
            catch
            {
                return View(new DeleteUserViewModel(DeleteUserViewModel.DeleteUserStatus.DeleteUserFailed, "Delete user failed. Please try again later."));
            }

            return View(deleteUserViewModel);
        }

        [HttpPost]
        public ActionResult SendInvitationCode(string email)
        {
            try
            {
                messageService.SendInvitationCode(email);
            }
            catch
            {
                return Json(new { success = false, errorMessage = "Send invitation code failed, please try later." });
            }
            return Json(new { success = true });
        }
    }
}