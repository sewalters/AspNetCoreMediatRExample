using System;
using System.Diagnostics;
using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesLab.Pages.AddressBook;

public class EditModel : PageModel
{
	private readonly IMediator _mediator;
	private readonly IRepo<AddressBookEntry> _repo;

	public EditModel(IRepo<AddressBookEntry> repo, IMediator mediator)
	{
		_repo = repo;
		_mediator = mediator;
	}

	[BindProperty]
	public UpdateAddressRequest UpdateAddressRequest { get; set; }

	public void OnGet(Guid id)
	{
		//First, access entry in AddressBook.
		var entry = _repo.Find(new EntryByIdSpecification(id)).FirstOrDefault();
		
		//If entry exists, we can now update it.
		UpdateAddressRequest = new UpdateAddressRequest
		{
			Id = entry.Id,
			Line1 = entry.Line1,
			Line2 = entry.Line2,
			City = entry.City,
			State = entry.State,
			PostalCode = entry.PostalCode,
		};
	}

	public ActionResult OnPost()
	{
		//First check that the request is properly set up.
		if (ModelState.IsValid) {
			//If properly set up, send request to Update entry and return to Index page.
			_mediator.Send(UpdateAddressRequest);
			return RedirectToPage("Index");
		}

		return Page();
	}
}