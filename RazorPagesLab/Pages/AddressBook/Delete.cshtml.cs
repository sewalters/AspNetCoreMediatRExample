using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesLab.Pages.AddressBook;

public class DeleteModel : PageModel
{
    private readonly IMediator _mediator;
    private readonly IRepo<AddressBookEntry> _repo;
    public DeleteModel(IRepo<AddressBookEntry> repo, IMediator mediator)
    {
        _repo = repo;
        _mediator = mediator;
    }

    [BindProperty]
    public DeleteAddressRequest DeleteAddressRequest { get; set; }

    public void OnGet(Guid id)
    {
        // Use ID to find entry in repo.
        var entry = _repo.Find(new EntryByIdSpecification(id)).FirstOrDefault();

        // Call for entry Deletion, passing in entry data for display to user.
        DeleteAddressRequest = new DeleteAddressRequest
        {
            Id = id,
            Line1 = entry.Line1,
            Line2 = entry.Line2,
            City = entry.City,
            State = entry.State,
            PostalCode = entry.PostalCode
        };

    }

    public async Task<IActionResult> OnPost()
    {
        // Todo: Use mediator to send a "command" to update the address book entry, redirect to entry list.i
        if (ModelState.IsValid)
        {
            await _mediator.Send(DeleteAddressRequest);
            return RedirectToPage("Index");
        }

        return Page();
    }
}
