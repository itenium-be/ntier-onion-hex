namespace Acerola.Domain.Tests
{
    using Acerola.Domain.ValueObjects;
    using Xunit;

    public class NameTests
    {
        [Fact]
        public void Empty_Name_Should_Be_Created()
        {
            //
            // Arrange
            string empty = string.Empty;

            //
            // Act and Assert
            Assert.Throws<NameShouldNotBeEmptyException>(
                () => new Name(empty));
        }

        [Fact]
        public void Full_Name_Shoud_Be_Created()
        {
            //
            // Arrange
            string valid = "Ivan Paulovich";

            //
            // Act
            Name name = new Name(valid);

            //
            // Assert
            Assert.Equal(new Name(valid), name);
        }
    }
}
