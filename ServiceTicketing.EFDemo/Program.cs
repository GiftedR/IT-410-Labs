using ServiceTicketing.EFDemo.Data;
using ServiceTicketing.EFDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace ServiceTicketing.EFDemo
{
	internal class Program
	{
		static void	Main(string[] args)
		{
			Console.WriteLine("Starting Week 7 EF Core CRUD Demo...");

			using var db = new ServiceTicketingContext();

			Console.WriteLine("DbContext created successfully.");

			// Step 4: Ensure seed data exists so the demo always works.
			EnsureSeedData(db);

			// Step 5–10: Demonstrate EF Core behavior.
			Step5_DeferredExecution_ReadTickets(db);
			Step6_TrackedVsNoTracking(db);
			Step7_CreateTicket(db);
			Step8_UpdateTicket_Tracked(db);
			Step9_UpdateTicket_UntrackedAttempt(db);
			Step10_DeleteTicket(db);

			Console.WriteLine("Week 7 demo completed.");
			Console.WriteLine("Done.");

			// ---------------------------
			// Step 4: Seed helper methods
			// ---------------------------
			static void EnsureSeedData(ServiceTicketingContext db)
			{
				Console.WriteLine("Step 4: Ensuring seed data exists...");

				// If ANY table is empty, we will insert minimal seed data.
				bool needsUsers = !db.Users.Any();
				bool needsCategories = !db.Categories.Any();
				bool needsTickets = !db.Tickets.Any();

				if (!needsUsers && !needsCategories && !needsTickets)
				{
					Console.WriteLine("Seed data already exists. Skipping inserts.");
					return;
				}

				Console.WriteLine("Seeding minimal data so the demo can run reliably...");

				if (needsUsers)
				{
					db.Users.AddRange(
						new User { FullName = "Alex Rivera", Email = "alex.rivera@example.com", CreatedOn = DateTime.Now },
						new User { FullName = "Jordan Lee", Email = "jordan.lee@example.com", CreatedOn = DateTime.Now }
					);
				}

				if (needsCategories)
				{
					db.Categories.AddRange(
						new Category { Name = "Hardware", Description = "Device and equipment issues" },
						new Category { Name = "Software", Description = "Application and login issues" }
					);
				}

				// Save Users/Categories first (even though we have no relationships yet, this keeps the flow clean).
				db.SaveChanges();

				if (needsTickets)
				{
					db.Tickets.AddRange(
						new Ticket
						{
							Title = "Laptop will not power on",
							Description = "User reports the laptop does not start after charging overnight.",
							Status = "Open",
							Priority = "High",
							CreatedOn = DateTime.Now
						},
						new Ticket
						{
							Title = "Unable to access email",
							Description = "User cannot log in to email account after password reset attempt.",
							Status = "Open",
							Priority = "Medium",
							CreatedOn = DateTime.Now
						}
					);

					db.SaveChanges();
				}

				Console.WriteLine("Seed data inserted (if needed).");
			}

			// -------------------------------------
			// Step 5: Deferred execution demonstration
			// -------------------------------------
			static void Step5_DeferredExecution_ReadTickets(ServiceTicketingContext db)
			{
				Console.WriteLine("Step 5: Deferred execution (READ tickets)");

				// Query is defined here, but NOT executed yet.
				var query = db.Tickets.Where(t => t.Status == "Open");

				Console.WriteLine("Query defined. No SQL should have executed yet.");

				// SQL executes when the query is enumerated (materialized).
				var openTickets = query.ToList();

				Console.WriteLine($"Tickets loaded: {openTickets.Count}");
			}

			// -------------------------------------
			// Step 6: Tracked vs NoTracking
			// -------------------------------------
			static void Step6_TrackedVsNoTracking(ServiceTicketingContext db)
			{
				Console.WriteLine("Step 6: Tracked vs NoTracking comparison");

				var tracked = db.Tickets.ToList();
				Console.WriteLine($"Tracked query returned: {tracked.Count} tickets");

				var untracked = db.Tickets.AsNoTracking().ToList();
				Console.WriteLine($"NoTracking query returned: {untracked.Count} tickets");
			}

			// -------------------------------------
			// Step 7: CREATE (INSERT)
			// -------------------------------------
			static void Step7_CreateTicket(ServiceTicketingContext db)
			{
				Console.WriteLine("Step 7: CREATE (INSERT) a new Ticket");

				var newTicket = new Ticket
				{
					Title = "Printer jam on floor 2",
					Description = "Paper jam occurs repeatedly after clearing; needs inspection.",
					Status = "Open",
					Priority = "Low",
					CreatedOn = DateTime.Now
				};

				db.Tickets.Add(newTicket);

				Console.WriteLine("Ticket added to DbContext. Database has NOT changed yet.");

				db.SaveChanges();

				Console.WriteLine($"SaveChanges executed. New TicketId assigned: {newTicket.TicketId}");
			}

			// -------------------------------------
			// Step 8: UPDATE (tracked workflow)
			// -------------------------------------
			static void Step8_UpdateTicket_Tracked(ServiceTicketingContext db)
			{
				Console.WriteLine("Step 8: UPDATE (tracked) - load, modify, SaveChanges");

				var ticket = db.Tickets
					.OrderByDescending(t => t.TicketId)
					.FirstOrDefault();

				if (ticket == null)
				{
					Console.WriteLine("No ticket found to update. Stop and verify seed/insert steps.");
					return;
				}

				Console.WriteLine($"Loaded TicketId={ticket.TicketId} (tracked). Current Status={ticket.Status}");

				// Modify in memory
				ticket.Status = "In Progress";

				Console.WriteLine("Status changed in memory to 'In Progress'. Database has NOT changed yet.");

				db.SaveChanges();

				Console.WriteLine("SaveChanges executed. Ticket should now be updated in the database.");
			}

			// -------------------------------------
			// Step 9: UPDATE attempt on untracked entity
			// -------------------------------------
			static void Step9_UpdateTicket_UntrackedAttempt(ServiceTicketingContext db)
			{
				Console.WriteLine("Step 9: UPDATE attempt on an untracked entity (AsNoTracking)");

				var untrackedTicket = db.Tickets
					.AsNoTracking()
					.OrderByDescending(t => t.TicketId)
					.FirstOrDefault();

				if (untrackedTicket == null)
				{
					Console.WriteLine("No ticket found. Stop and verify seed/insert steps.");
					return;
				}

				Console.WriteLine($"Loaded TicketId={untrackedTicket.TicketId} (NOT tracked).");

				// Modify in memory
				untrackedTicket.Priority = "High";

				Console.WriteLine("Priority changed in memory to 'High' on an untracked entity.");

				// SaveChanges will NOT persist this change because EF is not tracking this entity instance.
				db.SaveChanges();

				Console.WriteLine("SaveChanges executed. Expected behavior: NO UPDATE should occur for this change.");
			}

			// -------------------------------------
			// Step 10: DELETE
			// -------------------------------------
			static void Step10_DeleteTicket(ServiceTicketingContext db)
			{
				Console.WriteLine("Step 10: DELETE - remove a ticket and SaveChanges");

				var ticketToDelete = db.Tickets
					.OrderByDescending(t => t.TicketId)
					.FirstOrDefault();

				if (ticketToDelete == null)
				{
					Console.WriteLine("No ticket found to delete. Stop and verify seed/insert steps.");
					return;
				}

				Console.WriteLine($"Loaded TicketId={ticketToDelete.TicketId} for deletion.");

				db.Tickets.Remove(ticketToDelete);

				Console.WriteLine("Ticket marked for deletion. Database has NOT changed yet.");

				db.SaveChanges();

				Console.WriteLine("SaveChanges executed. Ticket should now be deleted from the database.");
			}
		}
	}
}
