using SD_340_W22SD_Final_Project_Group6.DAL;
using SD_340_W22SD_Final_Project_Group6.Models;

namespace SD_340_W22SD_Final_Project_Group6.BLL
{
    public class TicketBusinessLogic
    {
        private IRepository<Ticket> _repo;
        public TicketBusinessLogic(IRepository<Ticket> repository)
        {
            _repo = repository;
        }

        public void AddTicket(Ticket ticket)
        {
            _repo.Add(ticket);
        }

        public void DelelteTicket(Ticket ticket)
        {
            _repo.Delete(ticket);
        }

        public Ticket GetTicket(int id)
        {
            return _repo.Get(id);
        }

        public Ticket GetTicket(Func<Ticket, bool> predicate)
        {
            return _repo.Get(predicate);
        }

        public List<Ticket> GetTickets()
        {
            return (List<Ticket>)_repo.GetAll();
        }

        public List<Ticket> GetTickets(Func<Ticket, bool> predicate)
        {
            return (List<Ticket>)_repo.GetList(predicate);
        }

        public Ticket UpdateTicket(Ticket ticket)
        {
            return _repo.Update(ticket);
        }

        public void SaveChanges()
        {
            _repo.Save();
        }
    }
}
