using FunqTypes;
using NSubstitute;
using PastaFit.Core.Domain;
using PastaFit.Features.Booking.CreateBooking;
using PastaFit.Features.Booking.Ports;

namespace PastaFit.UnitTests;

public class CreateBookingHandlerTests
{
  [Fact]
  public async Task Fails_If_Member_Not_Found()
  {
    var memberId = Guid.NewGuid();
    var repository = Substitute.For<ICreateBookingRepository>();
    
    repository.GetMember(memberId)
      .Returns(Task.FromResult(Result<Member, BookingError>.Fail(new BookingError.MemberNotFound())));

    var result = await CreateBookingHandler.Handle(memberId, Guid.NewGuid(), repository);

    Assert.False(result.IsSuccess);
    Assert.Contains(result.Errors, e => e is BookingError.MemberNotFound);
  }

  [Fact]
  public async Task Fails_If_Class_Is_Full()
  {
    var memberId = Guid.NewGuid();
    var classId = Guid.NewGuid();
    var repository = Substitute.For<ICreateBookingRepository>();
    repository.HasExistingBooking(memberId, classId).Returns(Task.FromResult(false));
    repository.IsClassFull(classId).Returns(Task.FromResult(true));
    repository.GetMember(memberId).Returns(Task.FromResult(Result<Member, BookingError>.Ok(new Member(Guid.NewGuid(), "Alice", true))));
    repository.GetClass(classId).Returns(Task.FromResult(Result<Class, BookingError>.Ok(new Class(Guid.NewGuid(), "Yoga", 5))));

    var result = await CreateBookingHandler.Handle(memberId, classId, repository);

    Assert.False(result.IsSuccess);
    Assert.Contains(result.Errors, e => e is BookingError.ClassFull);
  }

  [Fact]
  public async Task Fails_If_Member_Is_Inactive()
  {
    var repo = Substitute.For<ICreateBookingRepository>();
    var memberId = Guid.NewGuid();

    repo.GetMember(memberId).Returns(Task.FromResult(Result<Member, BookingError>.Ok(new Member(Guid.NewGuid(), "Bob", false))));

    var result = await CreateBookingHandler.Handle(memberId, Guid.NewGuid(), repo);

    Assert.False(result.IsSuccess);
    Assert.Contains(result.Errors, e => e is BookingError.MemberInactive);
  }

  [Fact]
  public async Task Fails_If_Already_Booked()
  {
    var memberId = Guid.NewGuid();
    var classId = Guid.NewGuid();
    var repo = Substitute.For<ICreateBookingRepository>();
    repo.HasExistingBooking(memberId, classId).Returns(Task.FromResult(true));
    repo.GetMember(memberId).Returns(Task.FromResult(Result<Member, BookingError>.Ok(new Member(Guid.NewGuid(), "Charlie", true))));
    repo.GetClass(classId).Returns(Task.FromResult(Result<Class, BookingError>.Ok(new Class(Guid.NewGuid(), "Yoga", 5))));

    var result = await CreateBookingHandler.Handle(memberId, classId, repo);

    Assert.False(result.IsSuccess);
    Assert.Contains(result.Errors, e => e is BookingError.AlreadyBooked);
  }
}