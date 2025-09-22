namespace RiderGo.API.Common.Api
{
    public static class AppExtension
    {
        public static void ConfigureDevEnvironment(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        public static void UseSecurity(this WebApplication app)
        {

        }
    }
}
