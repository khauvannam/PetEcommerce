using BasedDomain.Results;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Identity.API.Domains.Users;
using Identity.API.Errors;
using Identity.API.Features.Users;
using Identity.API.Interfaces;
using Moq;

namespace Identity_Test.Handlers;

[TestFixture]
public class RegisterTests
{
    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _validatorMock = new Mock<IValidator<Register.Command>>();
        _handler = new Register.Handler(_userRepositoryMock.Object, _validatorMock.Object);
    }

    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IValidator<Register.Command>> _validatorMock;
    private Register.Handler _handler;

    [Test]
    public async Task Handle_ShouldRegisterUser_WhenValidCommand()
    {
        // Arrange
        var command = new Register.Command
        {
            Email = "test@example.com",
            Username = "testuser",
            Password = "password123",
            PhoneNumber = "1234567890",
            Address = Address.Create("123 Main St", "Testville", "12345"),
        };

        var validateResult = new ValidationResult();

        _validatorMock
            .Setup(v => v.ValidateAsync(command, CancellationToken.None))
            .ReturnsAsync(validateResult);

        _userRepositoryMock
            .Setup(repo => repo.Register(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(Result.Success);

        // Act

        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeFalse();

        _validatorMock.Verify(
            v => v.ValidateAsync(It.IsAny<Register.Command>(), CancellationToken.None),
            Times.Once
        );

        _userRepositoryMock.Verify(r => r.Register(It.IsAny<User>(), command.Password), Times.Once);
    }

    [Test]
    public async Task Handle_ShouldFail_WhenEmailIsAlreadyContained()
    {
        // Arrange
        var command = new Register.Command
        {
            Email = "duplicate@example.com",
            Username = "testuser",
            Password = "password123",
            PhoneNumber = "1234567890",
        };

        var validateResult = new ValidationResult();
        _validatorMock
            .Setup(v => v.ValidateAsync(command, CancellationToken.None))
            .ReturnsAsync(validateResult);

        // Simulate email already existing in the system
        _userRepositoryMock
            .Setup(repo => repo.Register(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(Result.Failure(UserErrors.WentWrong));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _userRepositoryMock.Verify(
            repo => repo.Register(It.IsAny<User>(), command.Password),
            Times.Once
        );

        result.IsFailure.Should().BeTrue();
    }
}
