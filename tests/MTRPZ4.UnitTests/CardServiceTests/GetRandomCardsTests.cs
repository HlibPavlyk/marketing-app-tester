using Moq;
using MTRPZ4.Infrastructure.Repository.IRepository;
using MTRPZ4.Application.Services;
using MTRPZ4.CoreDomain.Entities;
using MTRPZ4.Application.DTO;

namespace MTRPZ4.UnitTests.CardServiceTests
{
    public class GetRandomCardsTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public GetRandomCardsTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
        }

        [Theory]
        [InlineData(null)]
        public async Task GetRandomCards_ShouldThrowArgumentNullException_WhenUsernameIsNull(string? username)
        {
            var cardService = new CardService(_mockUnitOfWork.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => cardService.GetRandomCards(username));
        }

        [Theory]
        [InlineData("testuser")]
        public async Task GetRandomCards_ShouldThrowInvalidOperationException_WhenColorIsNull(string? username)
        {

            _mockUnitOfWork.Setup(u => u.Colors.GetById(It.IsAny<int>())).ReturnsAsync((Color?)null);

            var cardService = new CardService(_mockUnitOfWork.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() => cardService.GetRandomCards(username));
        }

        [Theory]
        [InlineData("testuser")]
        public async Task GetRandomCards_ShouldThrowInvalidOperationException_WhenFontIsNull(string? username)
        {
            _mockUnitOfWork.Setup(u => u.Colors.GetById(It.IsAny<int>())).ReturnsAsync(new Color { Id = 1, Pigment = "Red" });
            _mockUnitOfWork.Setup(u => u.Fonts.GetById(It.IsAny<int>())).ReturnsAsync((Font?)null);

            var cardService = new CardService(_mockUnitOfWork.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() => cardService.GetRandomCards(username));
        }

        [Theory]
        [InlineData("testuser")]
        public async Task GetRandomCards_ShouldThrowInvalidOperationException_WhenPriceIsNull(string? username)
        {
            _mockUnitOfWork.Setup(u => u.Colors.GetById(It.IsAny<int>())).ReturnsAsync(new Color { Id = 1, Pigment = "Red" });
            _mockUnitOfWork.Setup(u => u.Fonts.GetById(It.IsAny<int>())).ReturnsAsync(new Font { Id = 1, Type = "Arial" });
            _mockUnitOfWork.Setup(u => u.Prices.GetById(It.IsAny<int>())).ReturnsAsync((Price?)null);

            var cardService = new CardService(_mockUnitOfWork.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() => cardService.GetRandomCards(username));
        }

        [Theory]
        [InlineData("testuser")]
        public async Task GetRandomCards_ShouldWorkFine_WhenAllIsCorrect(string? username)
        {
            _mockUnitOfWork.Setup(u => u.Colors.GetById(It.IsAny<int>())).ReturnsAsync(new Color { Id = 1, Pigment = "Red" });
            _mockUnitOfWork.Setup(u => u.Fonts.GetById(It.IsAny<int>())).ReturnsAsync(new Font { Id = 1, Type = "Arial" });
            _mockUnitOfWork.Setup(u => u.Prices.GetById(It.IsAny<int>())).ReturnsAsync(new Price { Id = 1, Value = 777.77M });

            var cardService = new CardService(_mockUnitOfWork.Object);

            var result = await cardService.GetRandomCards(username);

            Assert.NotNull(result);
            var cardsList = Assert.IsAssignableFrom<IEnumerable<CardDTO>>(result);
            var cards = new List<CardDTO>(cardsList);
            Assert.Equal(6, cards.Count);
            foreach (var card in cards)
            {
                Assert.NotNull(card);
                Assert.NotNull(card.Color);
                Assert.NotNull(card.Font);
                Assert.NotNull(card.Price);
            }

        }
    }
}