using HotDeskApp.Server.Models.Desk.Dtos;
using HotDeskApp.Server.Models.Desk.Services;
using HotDeskApp.Server.Models.Reservation.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HotDeskApp.Server.Controllers.Desk;

public class DeskControllerTests
{
    private readonly DeskController _controller;
    private readonly Mock<IDeskService> _mockDeskService;
    private readonly Mock<IReservationService> _mockReservationService;

    public DeskControllerTests()
    {
        _mockDeskService = new Mock<IDeskService>();
        _mockReservationService = new Mock<IReservationService>();
        _controller = new DeskController(_mockDeskService.Object, _mockReservationService.Object);
    }

    [Fact]
    public async Task GetAllDesks_NoDesks_ReturnsNotFound()
    {
        // Arrange
        _mockDeskService.Setup(s => s.GetAllDesksInfo())
            .ReturnsAsync(new List<DeskDto>());

        // Act
        var result = await _controller.GetAllDesks();

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetAllDesks_DesksExist_ReturnsOkWithDesks()
    {
        // Arrange
        var desks = new List<DeskDto>
        {
            new DeskDto( Guid.NewGuid(),Guid.NewGuid(), "Place", "Desk 1","Desc",true),
            new DeskDto( Guid.NewGuid(),Guid.NewGuid(), "Place", "Desk 2","Desc",false),
            new DeskDto( Guid.NewGuid(),Guid.NewGuid(), "Place", "Desk 3","Desc",true),
        };
        _mockDeskService.Setup(s => s.GetAllDesksInfo())
            .ReturnsAsync(desks);

        // Act
        var result = await _controller.GetAllDesks();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedDesks = Assert.IsAssignableFrom<IEnumerable<DeskDto>>(okResult.Value);
        Assert.Equal(desks.Count, returnedDesks.Count());
    }

    [Fact]
    public async Task GetDesksByLocation_NoDesksForLocation_ReturnsNotFound()
    {
        // Arrange
        var locationId = Guid.NewGuid();
        _mockDeskService.Setup(s => s.GetAllDesksInfoByLocation(locationId))
            .ReturnsAsync(new List<DeskDto>());

        // Act
        var result = await _controller.GetDesksByLocation(locationId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetDesksByLocation_DesksExist_ReturnsOkWithDesks()
    {
        // Arrange
        var locationId = Guid.NewGuid();
        var desks = new List<DeskDto>
        {
            new DeskDto( Guid.NewGuid(),Guid.NewGuid(), "Place", "Desk 1","Desc",true),
            new DeskDto( Guid.NewGuid(),Guid.NewGuid(), "Place", "Desk 2","Desc",false),
        };
        _mockDeskService.Setup(s => s.GetAllDesksInfoByLocation(locationId))
            .ReturnsAsync(desks);

        // Act
        var result = await _controller.GetDesksByLocation(locationId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedDesks = Assert.IsAssignableFrom<IEnumerable<DeskDto>>(okResult.Value);
        Assert.Equal(desks.Count, returnedDesks.Count());
    }

    [Fact]
    public async Task GetDeskById_DeskExists_ReturnsDesk()
    {
        // Arrange
        var deskId = Guid.NewGuid();
        var desk = new DeskDto(deskId, Guid.NewGuid(), "Place", "Desk 1", "Desc", true);
        _mockDeskService.Setup(s => s.GetDeskInfo(deskId))
            .ReturnsAsync(desk);

        // Act
        var result = await _controller.GetDeskById(deskId);

        // Assert
        var returnedDesk = Assert.IsType<DeskDto>(result);
        Assert.Equal(deskId, returnedDesk.Id);
    }

    [Fact]
    public async Task GetDeskById_DeskDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var deskId = Guid.NewGuid();
        _mockDeskService.Setup(s => s.GetDeskInfo(deskId))
            .ReturnsAsync((DeskDto)null);

        // Act
        var result = await _controller.GetDeskById(deskId);

        // Assert
        Assert.Null(result);
    }
}