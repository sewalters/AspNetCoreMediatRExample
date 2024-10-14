using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace RazorPagesLab.Pages.AddressBook;

public class DeleteAddressHandler
    : IRequestHandler<DeleteAddressRequest, Guid>
{
    private readonly IRepo<AddressBookEntry> _repo;

    public DeleteAddressHandler(IRepo<AddressBookEntry> repo)
    {
        _repo = repo;
    }

    
    /// <summary>
    ///  Function deletes entry in Address Book using a requested id.
    ///  If the id maps to an existing entity in the Address Book we detele it and return its ID for confirmation.
    ///  Otherwise, Guid.Empty is returned to indicate no entity was found with provided ID.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///  entity.Id is returned upon successful deletion of item.
    ///  Guid.Empty is returned upon failure to find entity with ID.
    /// </returns>
    public async Task<Guid> Handle(DeleteAddressRequest request, CancellationToken cancellationToken)
    {
        //Attempt to access the entry associated with the requested Id.
        var entry = _repo.Find(new EntryByIdSpecification(request.Id)).FirstOrDefault();

        if (entry == null)
        {
            Console.WriteLine("Entry not found in repo");
            return await Task.FromResult(Guid.Empty);
        }

        //if entry exists, we remove it from repo.
        _repo.Remove(entry);

        //return requested Id upon successful deletion.
        return await Task.FromResult(request.Id);
    }
}
