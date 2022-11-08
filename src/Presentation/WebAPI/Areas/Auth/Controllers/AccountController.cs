//using System.Threading;
//using System.Threading.Tasks;
//using Application.Common.Interfaces;
//using Application.CQRS.Users;
//using Application.CQRS.Users.ConfirmEmail;
//using Application.CQRS.Users.CreateUser;
//using Application.CQRS.Users.ForgotPasword;
//using Application.CQRS.Users.ResetPassword;
//using MediatR;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using WebAPI.Areas.Auth.Contracts.RequestModel;
//using WebAPI.Areas.Auth.Contracts.ResponseModels;
//using WebAPI.Controllers;
//using WebAPI.Services;

//namespace WebAPI.Areas.Auth.Controllers
//{
//    [Area("Auth")]
//    [Route("[Area]/[Action]")]
//    public class AccountController : BaseController
//    {
//        private readonly IUserManager _userManager;
//        private readonly IAuthTokenProvider _tokenProvider;
//        private readonly IHttpContextAccessor _httpContextAccessor;

//        public AccountController(
//            IUserManager userManager,
//            IAuthTokenProvider tokenProvider,
//            IHttpContextAccessor httpContextAccessor)
//        {
//            _userManager = userManager;
//            _tokenProvider = tokenProvider;
//            _httpContextAccessor = httpContextAccessor;
//        }

//        [HttpPost]
//        public async Task<IActionResult> Login([FromBody] LoginRequestModel model, CancellationToken cancellationToken)
//        {
//            var response = await _userManager.LoginWithUserName(userName: model.UserName, password: model.Password);
//            if (response.Result.Succeeded)
//            {
//                return Ok(await GenerateLoginResponseModel(response.User.Id, cancellationToken));
//            }
//            return BadRequest(response.Result.Errors);
//        }
//        [HttpPost]
//        public async Task<IActionResult> ResendConfirmationCode([FromBody] string userId, CancellationToken cancellationToken)
//        {
//            await SendConfirmationCode(userId, cancellationToken);
//            return Ok();

//        }
//        [HttpPost]
//        public async Task<IActionResult> ConfirmEmailAsync([FromBody] ConfirmEmailRequestModel model, CancellationToken cancellationToken)
//        {
//            var command = new ConfirmEmailCommand { UserId = model.UserId, Token = model.Code };
//            var response = await Mediator.Send(command);
//            return Ok();
//        }

//        [HttpPost]
//        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command, CancellationToken cancellationToken)
//        {
//            var response = await Mediator.Send(command, cancellationToken);
//            return Ok();
//        }

//        [HttpPost]
//        public async Task<IActionResult> ResetPasword([FromBody] ResetPasswordCommand command, CancellationToken cancellationToken)
//        {
//            var response = await Mediator.Send(command, cancellationToken);
//            return Ok();
//        }

//        [HttpPost]
//        public async Task<IActionResult> RefreshLoginAsync([FromBody] RefreshLoginRequestModel model, CancellationToken cancellationToken)
//        {
//            var accessToken = await GetAccessTooken();
//            var accessTokenId = _tokenProvider.GetTokenId(accessToken);
//            var userId = _tokenProvider.GetUserId(accessToken);

//            var refreshToken = await _userManager.UpdateRefreshTokenAsync(model.RefreshToken, accessTokenId, cancellationToken);
//            return Ok(new AuthResponseModel(
//                refreshToken: refreshToken.Token,
//                accessToken: await generateAccessToken(userId, refreshToken.AccessTokenId, cancellationToken)
//            ));
//        }
//        [Authorize]
//        [HttpPost]
//        public async Task<IActionResult> LogOut(CancellationToken cancellationToken)
//        {
//            var accessTokenId = _tokenProvider.GetTokenId(await GetAccessTooken());
//            var isLoggedOut = await _userManager.InvalidateRefreshTokenAsync(accessTokenId, cancellationToken);
//            return isLoggedOut ? Ok() : BadRequest();
//        }

//        #region helpers
//        private Task<string> GetAccessTooken()
//        {
//            return _httpContextAccessor.HttpContext.GetTokenAsync("Bearer", "access_token");
//        }
//        private async Task<string> generateAccessToken(string userId, string accessTokenId, CancellationToken cancellationToken)
//        {
//            var claims = await _userManager.GetUserClaimsAsync(userId);
//            var jwt = _tokenProvider.CreateToken(userId, claims, accessTokenId);
//            return jwt;
//        }

//        private Task SendConfirmationCode(string userId, CancellationToken cancellationToken)
//        {
//            return Mediator.Publish(new SendEmailConfirmationCode { UserId = userId }, cancellationToken);
//        }

//        private async Task<AuthResponseModel> GenerateLoginResponseModel(string userId, CancellationToken cancellationToken)
//        {
//            var refreshToken = await _userManager.CreateRefreshTokenAsync(userId, cancellationToken);
//            return new AuthResponseModel(
//                refreshToken: refreshToken.Token,
//                accessToken: await generateAccessToken(userId, refreshToken.AccessTokenId, cancellationToken));
//        }
//        #endregion
//    }
//}
