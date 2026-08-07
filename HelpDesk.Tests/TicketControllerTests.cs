using HelpDesk.Api.Controllers;
using HelpDesk.Api.Models;
using HelpDesk.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HelpDesk.Tests
{
    public class TicketControllerTests
    {
        private readonly Mock<ITicketRepository> _mockRepo;
        private readonly TicketController _controller;

        public TicketControllerTests()
        {
            _mockRepo = new Mock<ITicketRepository>();
            _controller = new TicketController(_mockRepo.Object);
        }

        #region Mandatory Test Cases

        [Fact]
        public async Task GetAllTickets_ReturnsOkResult_WhenTicketExist()
        {
            // Arrange
            var tickets = new List<Ticket>
            {
                new Ticket { Id = 1, Title = "Issue 1", Description = "Desc 1", Priority = "High", Status = "Open", RaisedBy = "User 1" },
                new Ticket { Id = 2, Title = "Issue 2", Description = "Desc 2", Priority = "Medium", Status = "In Progress", RaisedBy = "User 2" }
            };
            _mockRepo.Setup(repo => repo.GetAllTicketsAsync()).ReturnsAsync(tickets);

            // Act
            var result = await _controller.GetAllTickets();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnTickets = Assert.IsType<List<Ticket>>(okResult.Value);
            Assert.Equal(2, returnTickets.Count);
        }

        [Fact]
        public async Task GetTicketById_ReturnsOkResult_WhenTicketExists()
        {
            // Arrange
            int testId = 1;
            var ticket = new Ticket { Id = testId, Title = "Issue 1", Description = "Desc 1", Priority = "High", Status = "Open", RaisedBy = "User 1" };
            _mockRepo.Setup(repo => repo.GetTicketByIdAsync(testId)).ReturnsAsync(ticket);

            // Act
            var result = await _controller.GetTicketById(testId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnTicket = Assert.IsType<Ticket>(okResult.Value);
            Assert.Equal(testId, returnTicket.Id);
            Assert.Equal("Issue 1", returnTicket.Title);
        }

        [Fact]
        public async Task GetTicketById_ReturnsNotFound_WhenTicketDoesNotExist()
        {
            // Arrange
            int testId = 999;
            _mockRepo.Setup(repo => repo.GetTicketByIdAsync(testId)).ReturnsAsync((Ticket?)null);

            // Act
            var result = await _controller.GetTicketById(testId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateTicket_ReturnsOkResult_WhenTicketIsCreatedSuccessfully()
        {
            // Arrange
            var newTicket = new Ticket { Title = "New Issue", Description = "New Desc", Priority = "High", Status = "Open", RaisedBy = "User A" };
            _mockRepo.Setup(repo => repo.CreateTicketAsync(It.IsAny<Ticket>())).ReturnsAsync(10);

            // Act
            var result = await _controller.CreateTicket(newTicket);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnTicket = Assert.IsType<Ticket>(okResult.Value);
            Assert.Equal(10, returnTicket.Id);
        }

        [Fact]
        public async Task CreateTicket_ReturnsBadRequest_WhenTicketIsNull()
        {
            // Act
            var result = await _controller.CreateTicket(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetTicketsByStatus_ReturnsOkResult_WhenMatchingTicketsExist()
        {
            // Arrange
            string status = "Open";
            var openTickets = new List<Ticket>
            {
                new Ticket { Id = 1, Title = "Open Issue 1", Status = "Open" },
                new Ticket { Id = 2, Title = "Open Issue 2", Status = "Open" }
            };
            _mockRepo.Setup(repo => repo.GetTicketsByStatusAsync(status)).ReturnsAsync(openTickets);

            // Act
            var result = await _controller.GetTicketsByStatus(status);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnTickets = Assert.IsType<List<Ticket>>(okResult.Value);
            Assert.Equal(2, returnTickets.Count);
            Assert.All(returnTickets, t => Assert.Equal("Open", t.Status));
        }

        #endregion

        #region Optional Test Cases

        [Fact]
        public async Task UpdateTicket_ReturnsOkResult_WhenUpdateIsSuccessful()
        {
            // Arrange
            int id = 1;
            var existingTicket = new Ticket { Id = id, Title = "Old Title", Status = "Open" };
            var updatedTicket = new Ticket { Id = id, Title = "New Title", Status = "In Progress" };

            _mockRepo.Setup(repo => repo.GetTicketByIdAsync(id)).ReturnsAsync(existingTicket);
            _mockRepo.Setup(repo => repo.UpdateTicketAsync(It.IsAny<Ticket>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateTicket(id, updatedTicket);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnTicket = Assert.IsType<Ticket>(okResult.Value);
            Assert.Equal("New Title", returnTicket.Title);
        }

        [Fact]
        public async Task UpdateTicket_ReturnsNotFound_WhenTicketDoesNotExist()
        {
            // Arrange
            int id = 999;
            var updatedTicket = new Ticket { Id = id, Title = "New Title" };
            _mockRepo.Setup(repo => repo.GetTicketByIdAsync(id)).ReturnsAsync((Ticket?)null);

            // Act
            var result = await _controller.UpdateTicket(id, updatedTicket);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteTicket_ReturnsOkResult_WhenTicketIsDeletedSuccessfully()
        {
            // Arrange
            int id = 1;
            var existingTicket = new Ticket { Id = id, Title = "To Delete" };
            _mockRepo.Setup(repo => repo.GetTicketByIdAsync(id)).ReturnsAsync(existingTicket);
            _mockRepo.Setup(repo => repo.DeleteTicketAsync(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteTicket(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteTicket_ReturnsNotFound_WhenTicketDoesNotExist()
        {
            // Arrange
            int id = 999;
            _mockRepo.Setup(repo => repo.GetTicketByIdAsync(id)).ReturnsAsync((Ticket?)null);

            // Act
            var result = await _controller.DeleteTicket(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetAllTickets_ReturnsEmptyList_WhenNoTicketsExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllTicketsAsync()).ReturnsAsync(new List<Ticket>());

            // Act
            var result = await _controller.GetAllTickets();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnTickets = Assert.IsType<List<Ticket>>(okResult.Value);
            Assert.Empty(returnTickets);
        }

        [Fact]
        public async Task GetTicketsByStatus_ReturnsEmptyList_WhenNoMatchingTicketsExist()
        {
            // Arrange
            string status = "NonExistentStatus";
            _mockRepo.Setup(repo => repo.GetTicketsByStatusAsync(status)).ReturnsAsync(new List<Ticket>());

            // Act
            var result = await _controller.GetTicketsByStatus(status);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnTickets = Assert.IsType<List<Ticket>>(okResult.Value);
            Assert.Empty(returnTickets);
        }

        #endregion
    }
}
