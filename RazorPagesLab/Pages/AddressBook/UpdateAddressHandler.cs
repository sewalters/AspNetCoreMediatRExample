using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace RazorPagesLab.Pages.AddressBook;

public class UpdateAddressHandler : IRequestHandler<UpdateAddressRequest, Guid>
{
    private readonly IRepo<AddressBookEntry> _repo;

    public UpdateAddressHandler(IRepo<AddressBookEntry> repo)
    {
        _repo = repo;
    }

    /// <summary>
    /// This function is an overriden Update proicedure for the Handle function call. It takes a request object with an id and attempts to
    /// update an entry existing in AddressBook with the matching id.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///  entry.Id is returned upon successful edit of an entry.
    ///  Guid.Empty is returned if the request.Id does not map to an entry in AddressBook.
    /// </returns>
    public async Task<Guid> Handle(UpdateAddressRequest request, CancellationToken cancellationToken)
    {
        // First aquire entry present at requested ID.
        var entry = _repo.Find(new EntryByIdSpecification(request.Id)).FirstOrDefault();

        if (entry == null)
        {
            //If entry does not exist, return Guid.Empty to indicate value not found.
            return await Task.FromResult(Guid.Empty);
        }


        //Update entry if it exists.
        entry.Update(
            request.Line1,
            request.Line2,
            request.City,
            request.State,
            request.PostalCode
        );
        return await Task.FromResult(entry.Id);
    }
}
