using Microsoft.AspNetCore.Mvc;

namespace APIMain.Authentication.Jwt {
    public static class ControllerExtensions {
        public static bool VerifyTokenUser(this ControllerBase controller, int userId) {
            var userIdClaim = controller.User.FindFirst("userid")?.Value;

            if (!int.TryParse(userIdClaim, out int jwtUserId) || userId != jwtUserId) {
                return false;
            }

            return true;
        }
    }
}
