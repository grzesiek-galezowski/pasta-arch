using FunqTypes;
using NSubstitute;
using PastaFit.Core.Domain;
using PastaFit.Features.Booking.CreateBooking;
using PastaFit.Features.Booking.Ports;
using TddXt.XNSubstitute;

namespace PastaFit.UnitTests;

public class CreateBookingHandlerTests
{
  [Fact]
  public async Task Fails_If_Member_Not_Found()
  {
    var memberId = Guid.NewGuid();
    var repository = Substitute.For<ICreateBookingRepository>();
    var response = Substitute.For<ICreateBookingResponseInProgress>();

    var fail = Result<Member, BookingError>.Fail(new BookingError.MemberNotFound());
    repository.GetMember(memberId).Returns(fail);

    await CreateBookingHandler.Handle(
      memberId,
      Guid.NewGuid(),
      repository,
      response);

    response.ReceivedOnly(1).CouldNotFindMember(fail);
  }

  [Fact]
  public async Task Fails_If_Class_Is_Full()
  {
    var memberId = Guid.NewGuid();
    var classId = Guid.NewGuid();
    var response = Substitute.For<ICreateBookingResponseInProgress>();
    var repository = Substitute.For<ICreateBookingRepository>();
    
    repository.HasExistingBooking(memberId, classId).Returns(false);
    repository.IsClassFull(classId).Returns(true);
    repository.GetMember(memberId).Returns(
      Result<Member, BookingError>.Ok(new Member(Guid.NewGuid(), "Alice", true)));
    repository.GetClass(classId)
      .Returns(Result<Class, BookingError>.Ok(new Class(Guid.NewGuid(), "Yoga", 5)));

    await CreateBookingHandler.Handle(
      memberId,
      classId,
      repository,
      response);
    
    response.ReceivedOnly(1).ClassFull();
  }

  [Fact]
  public async Task Fails_If_Member_Is_Inactive()
  {
    var repo = Substitute.For<ICreateBookingRepository>();
    var memberId = Guid.NewGuid();
    var response = Substitute.For<ICreateBookingResponseInProgress>();

    repo.GetMember(memberId).Returns(
      Result<Member, BookingError>.Ok(new Member(Guid.NewGuid(), "Bob", false)));

    await CreateBookingHandler.Handle(
      memberId,
      Guid.NewGuid(),
      repo,
      response);

    response.ReceivedOnly(1).MemberInactive();
  }

  [Fact]
  public async Task Fails_If_Already_Booked()
  {
    var memberId = Guid.NewGuid();
    var classId = Guid.NewGuid();
    var response = Substitute.For<ICreateBookingResponseInProgress>();
    var repo = Substitute.For<ICreateBookingRepository>();
    
    repo.HasExistingBooking(memberId, classId).Returns(true);
    repo.GetMember(memberId).Returns(
      Result<Member, BookingError>.Ok(new Member(Guid.NewGuid(), "Charlie", true)));
    repo.GetClass(classId)
      .Returns(Result<Class, BookingError>.Ok(new Class(Guid.NewGuid(), "Yoga", 5)));

    await CreateBookingHandler.Handle(
      memberId,
      classId,
      repo,
      response);

    response.ReceivedOnly(1).AlreadyBooked();
  }

  [Fact]
  public async Task Fails_If_Class_Not_Found()
  {
    var memberId = Guid.NewGuid();
    var classId = Guid.NewGuid();
    var repo = Substitute.For<ICreateBookingRepository>();
    var response = Substitute.For<ICreateBookingResponseInProgress>();
    var getClassResult = Result<Class, BookingError>.Fail(new BookingError.ClassNotFound());

    repo.GetMember(memberId).Returns(Result<Member, BookingError>.Ok(new Member(Guid.NewGuid(), "David", true)));
    repo.GetClass(classId).Returns(getClassResult);

    await CreateBookingHandler.Handle(
      memberId,
      classId,
      repo,
      response);

    response.ReceivedOnly(1).CouldNotGetClass(getClassResult);
  }
}