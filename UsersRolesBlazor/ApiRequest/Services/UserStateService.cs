namespace UsersRolesBlazor.ApiRequest.Services
{
    public class UserStateService
    {
        public string Role { get; private set; }
        public string Email { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsAuthenticated { get; private set; } = false;

        public void SetUser(string role, string email, string name, string description)
        {
            UserStateService UserState = new UserStateService
            {
                Role = role,
                Email = email,
                Name = name,
                Description = description,
                IsAuthenticated = true,
            };
        }

        public void ClearUser()
        {
            Role = null;
            Email = null;
            Name = null;
            Description = null;
        }
    }
}
