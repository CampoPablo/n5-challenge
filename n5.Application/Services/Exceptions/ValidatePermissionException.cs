namespace n5.Application.Services.Exceptions
{
    public static class ValidatePermissionException
    {
        /// <summary>
        /// If not exists permissions throw exception
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="Exception"></exception>
        static public void IfNotExistsPemissionsThrowException(object data)
        {
            if (data == null)
            {
                throw new Exception("Not exists permissions for this employee");
            }
        }
    }
}
