using CRM.Application.DTOs.Notes;
using CRM.Application.Services;
using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace CRM.Tests.Services
{
    public class NoteServiceTests
    {
        private readonly Mock<INoteRepository> _noteRepositoryMock;
        private readonly NoteService _noteService;

        public NoteServiceTests()
        {
            _noteRepositoryMock = new Mock<INoteRepository>();
            _noteService = new NoteService(_noteRepositoryMock.Object);
        }

        [Fact]
        public async Task GetByContactIdAsync_ShouldReturnNotes_ForGivenContact()
        {
            // Arrange
            var contactId = 1;
            var notes = new List<Note>
            {
                new Note
                {
                    Id = 1,
                    Content = "First note",
                    ContactId = contactId,
                    Contact = new Contact
                    {
                        FirstName = "Jane",
                        LastName = "Smith"
                    }
                }
            };

            _noteRepositoryMock
                .Setup(r => r.GetByContactIdAsync(contactId))
                .ReturnsAsync(notes);

            // Act
            var result = await _noteService.GetByContactIdAsync(contactId);

            // Assert
            result.Should().HaveCount(1);
            result.First().Content.Should().Be("First note");
            result.First().ContactFullName.Should().Be("Jane Smith");
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedNote()
        {
            // Arrange
            var userId = 1;
            var request = new NoteRequestDto
            {
                Content = "Follow up call scheduled",
                ContactId = 1
            };

            _noteRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Note>()))
                .ReturnsAsync((Note n) => n);

            // Act
            var result = await _noteService.CreateAsync(request, userId);

            // Assert
            result.Should().NotBeNull();
            result.Content.Should().Be("Follow up call scheduled");
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenNoteNotFound()
        {
            // Arrange
            _noteRepositoryMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Note?)null);

            // Act
            var result = await _noteService.DeleteAsync(99);

            // Assert
            result.Should().BeFalse();
        }
    }
}