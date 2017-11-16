using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevenNote.Data;
using ElevenNote.Models;
using ElevenNote.Web.Data;

namespace ElevenNote.Services
{
    public class NoteService
    {
        private readonly Guid _userId;
        private string v;

        public Guid OwnerId { get; private set; }

        public NoteService(Guid userId)
        {
            _userId = userId;
        }

     
        public IEnumerable<NoteListItemModel> GetNotes()
        {

            using (var ctx = new ElevenNoteDbContext())
            {
                return
                    ctx
                        .Notes
                        .Where(e => OwnerId == _userId)
                        .Select(
                            e =>
                                new NoteListItemModel
                                {
                                    NoteId = e.NoteId,
                                    Title = e.Title,
                                    CreatedUtc = e.CreatedUtc,
                                    ModifiedUtc = e.ModifiedUtc
                                })

                       .ToArray();

            }
        }
        public bool CreateNote(NoteCreateModel model)
        {
            using (var ctx = new ElevenNoteDbContext())
        {

                var entity =
                        new NoteEntity
                        {
                            OwnerId = _userId,
                            Title = model.Title,
                            Content = model.Content,
                            CreatedUtc = DateTime.UtcNow
                        };

                ctx.Notes.Add(entity);

               return ctx.SaveChanges() == 1;

            }
        }
        NoteEntity entity;

        public NoteDetailModel GetNoteById(int id)
        {
            using (var ctx = new ElevenNoteDbContext())
            {
                ctx
                    .Notes
                    .SingleOrDefault(e => e.NoteId == id && e.OwnerId == _userId);
            }

            if (entity == null) return new NoteDetailModel();

            return
                new NoteDetailModel
                {
                    NoteId = entity.NoteId,
                    Title = entity.Title,
                    Content = entity.Content,
                    CreatedUtc = entity.CreatedUtc,
                    ModifiedUtc = entity.ModifiedUtc
                };
        }
    }
}
