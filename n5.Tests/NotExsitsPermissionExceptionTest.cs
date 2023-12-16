using n5.Application.Services.Exceptions;

namespace n5.Tests
{
    public class NotExistsPermissionExceptionTests
    {
        [Fact]
        public void InNotExistsPemissionsThrowException_CountIsZero_ThrowsException()
        {
            // Arrange
             object? data = null;

            // Act & Assert
            Assert.Throws<Exception>(() => ValidatePermissionException.IfNotExistsPemissionsThrowException(data));
        }

        [Fact]
        public void InNotExistsPemissionsThrowException_CountIsNotZero_DoesNotThrowException()
        {
            // Arrange`
            object data = new object();

            // Act
            var exception = Record.Exception(() => ValidatePermissionException.IfNotExistsPemissionsThrowException(data));

            // Assert
            Assert.Null(exception);
        }
    }
}